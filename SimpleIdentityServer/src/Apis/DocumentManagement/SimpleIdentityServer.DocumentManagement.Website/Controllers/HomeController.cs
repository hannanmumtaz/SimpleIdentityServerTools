﻿using Microsoft.AspNetCore.Mvc;

namespace SimpleIdentityServer.DocumentManagement.Website.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
