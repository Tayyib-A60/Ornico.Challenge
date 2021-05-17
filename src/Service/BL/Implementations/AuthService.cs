using Core.Constants;
using Core.Helpers;
using System;
using Models.Entities;
using Models.ViewModels;
using Repository.Commands.Interfaces;
using Repository.Queries.Interfaces;
using Service.BL.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Models.DTOs;

namespace Service.BL.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IDapperCommandRepository<User> _dCRUser;
        private readonly IDapperQueryRepository<User> _dQRUser;
        private readonly IDapperCommandRepository<EmailVerificationLog> _dCREmailVerificationLog;
        private readonly IDapperQueryRepository<EmailVerificationLog> _dQREmailVerificationLog;
        private readonly IUtilities _utilities;
        public readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        private readonly string _appSecret;
        public AuthService(IDapperCommandRepository<User> dCRUser, IDapperQueryRepository<User> dQRUser, IDapperCommandRepository<EmailVerificationLog> dCREmailVerificationLog, IDapperQueryRepository<EmailVerificationLog> dQREmailVerificationLog, IConfiguration configuration, IUtilities utilities, IEmailService emailService)
        {
            _dCRUser = dCRUser;
            _dQRUser = dQRUser;
            _dCREmailVerificationLog = dCREmailVerificationLog;
            _dQREmailVerificationLog = dQREmailVerificationLog;
            _configuration = configuration;
            _appSecret = _configuration.GetValue<string>("AppSettings:AppSecret");
            _utilities = utilities;
            _emailService = emailService;
        }

        public async Task<BaseResponse<UserLoginResponse>> Authenticate(AuthUserDTO authUser)
        {
            var response = new BaseResponse<UserLoginResponse>();
            try
            {
                var user = await GetUser(authUser.Email);

                if (user == null)
                {
                    response.Message = $"User with email {authUser.Email} does not exists";
                    return response;
                }

                var validPassword = VerifyPasswordHash(authUser.Password, user.PasswordHash, user.PasswordSalt);

                if(validPassword)
                {
                    response.Data = new UserLoginResponse
                    {
                        Email = authUser.Email,
                        Token = CreateToken(user),
                        Role = user.Role,
                        IsVerfied = user.EmailVerified
                    };
                    response.Status = true;
                    response.Message = ResponseMessages.OperationSuccessful;
                }
                else
                {
                    response.Message = "Invalid Password";
                }

                return response;
            }
            catch(Exception ex)
            {
                return response;
            }
        }

        public async Task<BaseResponse<EmailVerificationResponse>> VerifyEmail(EmailVerificationDTO emailVerificationDTO, string userID)
        {
            var response = new BaseResponse<EmailVerificationResponse>();

            try
            {
                var query = new Dictionary<string, string>() { { "ID", userID } };
                var userQR = await _dQRUser.GetByAsync(query);

                var user = userQR.FirstOrDefault();

                if(user != null)
                {
                    var emailVerificationLog = await GetEmailVerificationLog(user.Email);

                    // Do a time check here later

                    if(emailVerificationLog != null)
                    {
                        if(emailVerificationLog.Code == emailVerificationDTO.Code)
                        {
                            emailVerificationLog.Used = true;
                            await _dCREmailVerificationLog.UpdateAsync(emailVerificationLog);
                            
                            if(user.EmailVerified)
                            {
                                response.Message = "Your account has already been verified";
                                return response;
                            }

                            user.EmailVerified = true;

                            await _dCRUser.UpdateAsync(user);



                            response.Message = ResponseMessages.OperationSuccessful;
                            response.Status = true;
                        
                        }
                        else
                        {
                            response.Message = "Invalid verification code";
                        }
                    }
                    else
                    {
                        response.Message = "Unable to verify user account";
                    }
                }
                else
                {
                    response.Message = "User account does not exist";
                }

                return response;
            }
            catch(Exception ex)
            {
                return response;
            }
        }

        public async Task<BaseResponse<UserLoginResponse>> CreateUser(CreateUserDTO createUserDTO)
        {
            var response = new BaseResponse<UserLoginResponse>();
            try
            {
                var user = await GetUser(createUserDTO.Email);

                if(user != null)
                {
                    if(user.EmailVerified)
                    {
                        response.Message = $"User with email {createUserDTO.Email} already exists";
                        return response;
                    }

                    var timeDiff = DateTime.Now - user.CreationDate;

                    if (timeDiff.TotalHours >= 24)
                    {
                        await _dCRUser.DeleteAsync(user.ID);
                    }
                    else
                    {
                        response.Message = $"User with email {createUserDTO.Email} already exists";
                        return response;
                    }
                }
                else
                {
                    byte[] passwordHash, passwordSalt;

                    CreatePasswordHash(createUserDTO.Password, out passwordHash, out passwordSalt);

                    user = new User
                    {
                        ID = Guid.NewGuid(),
                        CreationDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                        Email = createUserDTO.Email,
                        FullName = createUserDTO.FullName,
                        Instagram = createUserDTO.Instagram,
                        Twitter = createUserDTO.Twitter,
                        Role = createUserDTO.Role,
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt
                    };

                    await _dCRUser.AddAsync(user);

                    // Send Email with Verification Code to user
                    var code = _utilities.GenerateNumericKey(6);

                    var emailVerificationLog = await GetEmailVerificationLog(user.Email);

                    if(emailVerificationLog != null)
                    {
                        emailVerificationLog.Code = code;
                        emailVerificationLog.LastModifiedDate = DateTime.Now;

                        await _dCREmailVerificationLog.UpdateAsync(emailVerificationLog);
                    }
                    else
                    {
                        emailVerificationLog = new EmailVerificationLog
                        {
                            ID = Guid.NewGuid(),
                            Code = code,
                            CreationDate = DateTime.Now,
                            LastModifiedDate = DateTime.Now,
                            Email = user.Email
                        };

                        await _dCREmailVerificationLog.AddAsync(emailVerificationLog);
                    }


                    var status = await _emailService.SendEmail(user.Email, user.FullName, $"Your verification code is \n <b>{code}</b> it expires in 15 minutes", "Email Verification");

                    // Set to false because sending email with both Sendgrid and mailchimp now requires a subscription account
                    if (!status)
                    {
                        response.Message = ResponseMessages.OperationSuccessful + ", Please check your email for your verification code, it expires in 15 minutes and you have 24 hours to verify your account or it'll be deleted";
                        response.Status = true;
                        response.Data = new UserLoginResponse
                        {
                            Token = CreateToken(user),
                            Email = user.Email,
                            Role = user.Role,
                            IsVerfied = user.EmailVerified
                        };
                        return response;
                    }
                    else
                    {
                        await _dCRUser.DeleteAsync(user.ID);

                        await _dCREmailVerificationLog.DeleteAsync(emailVerificationLog.ID);

                        response.Message = "Unable to send user verification email at the moment, please try signing up later";
                        return response;
                    }

                }

                return response;
            }
            catch(Exception ex)
            {
                return response;
            }
        }

        private async Task<User> GetUser(string email)
        {
            try
            {
                var query = new Dictionary<string, string>() { { "Email", email } };

                var userQueryResult = await _dQRUser.GetByAsync(query);

                var user = userQueryResult.FirstOrDefault();
                return user;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private async Task<EmailVerificationLog> GetEmailVerificationLog(string email)
        {
            try
            {
                var query = new Dictionary<string, string>() { { "Email", email } };

                var evQueryResult = await _dQREmailVerificationLog.GetByAsync(query);

                var emailVerificationLog = evQueryResult.FirstOrDefault();

                return emailVerificationLog;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string CreateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                new Claim (ClaimTypes.Email, user.Email),
                new Claim (ClaimTypes.Role, user.Role.ToString ()),
                new Claim (ClaimTypes.NameIdentifier, user.ID.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("value cannot be empty or whitespace, on string is allowed ", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("value cannot be empty or whitespace, only string is allowed ", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }
            return true;
        }

        // Doesn't work because of the subscription plan, check the db and get the Code to use for Verification
        public async Task<BaseResponse<EmailVerificationResponse>> SendVerificationEmail(string userID)
        {
            var response = new BaseResponse<EmailVerificationResponse>();
            try
            {
                var userQR = await _dQRUser.GetByAsync(new Dictionary<string, string>() { { "ID", userID } });
                var user = userQR.FirstOrDefault();

                if(user != null)
                {
                    if(user.EmailVerified)
                    {
                        response.Message = "Account already verified";
                        return response;
                    }
                    var code = _utilities.GenerateNumericKey(6);

                    var emailVerificationLog = await GetEmailVerificationLog(user.Email);

                    if (emailVerificationLog != null)
                    {
                        emailVerificationLog.Code = code;
                        emailVerificationLog.LastModifiedDate = DateTime.Now;

                        await _dCREmailVerificationLog.UpdateAsync(emailVerificationLog);
                    }
                    else
                    {
                        emailVerificationLog = new EmailVerificationLog
                        {
                            ID = Guid.NewGuid(),
                            Code = code,
                            CreationDate = DateTime.Now,
                            LastModifiedDate = DateTime.Now,
                            Email = user.Email
                        };

                        await _dCREmailVerificationLog.AddAsync(emailVerificationLog);
                    }
                    var status = await _emailService.SendEmail(user.Email, user.FullName, $"Your verification code is \n <b>{code}</b>", "Email Verification");
                    if(status)
                    {
                        response.Status = true;
                        response.Message = ResponseMessages.OperationSuccessful;
                        response.Data = new EmailVerificationResponse
                        {
                            Email = user.Email
                        };
                        return response;
                    }
                    else
                    {
                        response.Message = "Unable to send verification email";
                        return response;
                    }
                }
                else
                {
                    response.Message = "User account not found";
                    return response;
                }
            }
            catch(Exception ex)
            {
                return response;
            }
        }
    }
}
