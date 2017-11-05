using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Schedule.Entities;
using Schedule.Web.Models;
using Schedule.Web.Filters;
using Microsoft.Extensions.Options;
using Schedule.Web.ViewModels;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Schedule.Web.Controllers
{
    [AuthenticateAttribute]
    public class EditarDBController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
