using System;
using Services.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Api.Exceptions;
using Datadog.Trace;
using Datadog.Trace.Configuration;
using Api.Helpers;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            HostEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // setup datadog
            var settings = TracerSettings.FromDefaultSources();
            settings.AnalyticsEnabled = true;
            settings.TracerMetricsEnabled = true;
            settings.LogsInjectionEnabled = true;
            Tracer.Instance = new Tracer(settings);
            services.AddSingleton(Tracer.Instance);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            .ConfigureApiBehaviorOptions(o =>
            {
                o.InvalidModelStateResponseFactory = context =>
                {
                    return new ValidationFailedResult(context.ModelState);
                };
            });

            services.AddCors(options =>
            {
                options.AddPolicy("pol",
                builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });
            services.AddControllers();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                            .GetBytes(Configuration.GetValue<string>("AppSettings:AppSecret"))),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            //services.AddCors();

            services.AddAutoMapper(typeof(AutomapperProfile));
            services.AddHttpClient();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ornico Story API", Version = "v1" });
            });

            services.AddApplicationInsightsTelemetry();
            if (!HostEnvironment.IsDevelopment())
            {
                services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddSqlServer(Configuration["ConnectionStrings:DbConnectionString"], name: "sqlserver")
                .AddDatadogPublisher(Configuration["HealthCheckMonitor"], Environment.GetEnvironmentVariable("DD_AGENT_HOST"), new string[] { $"service:{Environment.GetEnvironmentVariable("DD_SERVICE")}", $"environment:{Environment.GetEnvironmentVariable("DD_ENV")}", $"env:{Environment.GetEnvironmentVariable("DD_ENV")}", $"version:{Environment.GetEnvironmentVariable("DD_VERSION")}" });
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ConfigureExceptionHandler();
            app.UseCors(options =>
            {
                options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ornico Story API");
            });

            var supportedCultures = new[] { new CultureInfo("en-GB"), new CultureInfo("en-US") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-GB"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("pol");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                if (!env.IsDevelopment())
                {
                    endpoints.MapHealthChecks("/healthz");
                }
            });

        }
    }
}
