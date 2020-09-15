using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NewsCoreApp.Application;
using NewsCoreApp.Data.Entities;
using NewsCoreApp.Data.Enums;
using Newtonsoft.Json.Linq;
using PaulMiami.AspNetCore.Mvc.Recaptcha;

namespace NewsCoreApp.Controllers
{
    public class ContactController : Controller
    {
        FeedbackService _feedbackService;
        ContactService _contactService;

        public ContactController()
        {
            _feedbackService = new FeedbackService();
            _contactService = new ContactService();
        }

        public IActionResult Index()
        {
            var model = _contactService.GetByStatus(Status.Active);
            return View(model);
        }

        [HttpPost]
        [ValidateRecaptcha]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitContact(Feedback feedback)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Success"] = false;
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return View();
            }
            else
            {
                feedback.Status = Status.Active;
                feedback.CreatedDate = DateTime.Now;
                feedback.ModifiedDate = DateTime.Now;
                _feedbackService.Add(feedback);
                _feedbackService.Save();

                ViewData["Success"] = true;
            }
            return View("Index", feedback);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitContact_RecapchatV3(Feedback feedback)
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