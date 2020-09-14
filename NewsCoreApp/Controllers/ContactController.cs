using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsCoreApp.Data.Entities;
using Newtonsoft.Json.Linq;

namespace NewsCoreApp.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitContact(Feedback feedback)
        {
            string recaptchaResponse = this.Request.Form["g-recaptcha-response"];

            var client = HttpClientFactory.Create();
            try
            {
                var parameters = new Dictionary<string, string>
                {
                    {"secret", "6LfS9csZAAAAAKUkGrvBW69PyD9Ud36JibDqbJ-2"},
                    {"response", recaptchaResponse},
                    {"remoteip", this.HttpContext.Connection.RemoteIpAddress.ToString()}
                };
                
                HttpResponseMessage response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", new FormUrlEncodedContent(parameters));
                response.EnsureSuccessStatusCode();

                string apiResponse = await response.Content.ReadAsStringAsync();
                dynamic apiJson = JObject.Parse(apiResponse);
                if (apiJson.success != true)
                {
                    feedback.Title = "Not Ok";
                    this.ModelState.AddModelError(string.Empty, "There was an unexpected problem processing this request. Please try again.");
                }
            }
            catch (HttpRequestException ex)
            {
                // Something went wrong with the API. Let the request through.
                //this.logger.LogError(ex, "Unexpected error calling reCAPTCHA api.");
            }
            feedback.Title = "Sure Ok";
            return new OkObjectResult(feedback);
        }
    }
}