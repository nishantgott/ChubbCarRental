using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ChubbCarRental.Services
{
    public class MailgunEmailService
    {
        private readonly string _apiKey;
        private readonly string _domain;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public MailgunEmailService(IConfiguration configuration)
        {
            _apiKey = configuration["Mailgun:ApiKey"];
            _domain = configuration["Mailgun:Domain"];
            _fromEmail = configuration["Mailgun:FromEmail"];
            _fromName = configuration["Mailgun:FromName"];
        }

        public async Task<bool> SendBookingConfirmationEmail(string toEmail, string userName, string carMake, string carModel)
        {
            var client = new RestClient(new RestClientOptions($"https://api.mailgun.net/v3/{_domain}/messages")
            {
                Authenticator = new HttpBasicAuthenticator("api", _apiKey)
            });

            var request = new RestRequest();
            request.AddParameter("from", $"{_fromName} <{_fromEmail}>");
            request.AddParameter("to", toEmail);
            request.AddParameter("subject", "Booking Confirmation - Car Rental Service");
            request.AddParameter(
                "text",
                $"Dear {userName},\n\nYour booking for the car {carMake} {carModel} has been confirmed.\n\nThank you for choosing our service."
            );

            request.AddParameter(
                "html",
                $"<p>Dear {userName},</p><p>Your booking for the car <strong>{carMake} {carModel}</strong> has been confirmed.</p><p>Thank you for choosing our service.</p>"
            );

            var response = await client.ExecutePostAsync(request);

            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}
