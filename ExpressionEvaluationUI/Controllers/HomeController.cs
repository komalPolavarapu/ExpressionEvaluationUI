using ExpressionEvaluationUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ExpressionEvaluationUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ExpressionEvaluationModel model)
        {
            var client = new RestClient(Environment.GetEnvironmentVariable("EvaluationAPI"));
            var req = new RestRequest(Method.GET);
            req.AddParameter("input", model.Expression);
            IRestResponse response = await client.ExecuteAsync(req);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {               
                model.Output = Convert.ToDouble(response.Content);
            }
            else if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {                
                model.ErrorMessage = response.Content;
            }
            else
            {
                model.ErrorMessage = "Error in processing the expression, please contact the administrator.";
            }
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
