using Core.Helpers;
using Models.ViewModels;
using Service.BL.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;

namespace Service.BL.Implementations
{
    public class EmailService: IEmailService
    {
        private readonly IUtilities _utilities;
        private readonly IConfiguration _configuration;
        private readonly string _messagingKey;


        public EmailService(IUtilities utilities, IConfiguration configuration)
        {
            _utilities = utilities;
            _configuration = configuration;
            _messagingKey = _configuration.GetValue<string>("AppSettings:MessagingKey");
        }
        

        public async Task<bool> SendEmail(string email, string name, string message, string subject)
        {
            // Doesn't work because of the subscription plan
            var to = new [] { new { email = email, name = name, type = "to" } };
            var request = new
            {
                key = _messagingKey,
                message = new
                {
                    html = "",
                    subject = subject,
                    text = message,
                    from_name = "Ornico Story",
                    from_email = "noreply@ornico.com",
                    to = to,
                    headers = new {}
                }
            };

            var httpResponse = await _utilities.MakeHttpRequest(request, "https://mandrillapp.com/api/1.0/messages/send", "", HttpMethod.Post, null);

            if (httpResponse != null)
            {
                if (httpResponse.IsSuccessStatusCode)
                {
                    string resp = await httpResponse.Content.ReadAsStringAsync();
                    return false;
                }
                else
                {
                    string failureResponse = await httpResponse.Content.ReadAsStringAsync();
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
