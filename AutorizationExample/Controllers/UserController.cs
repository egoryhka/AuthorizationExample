using AutorizationExample.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationExample.Controllers
{
    public class UserController : Controller
    {


        public IActionResult Main()
        {
           // if (!Request.Cookies.Keys.Contains("Login") || !Request.Cookies.Keys.Contains("Password")) return RedirectToAction("Index", "Home"); 


            // СПЕШАЛ ФОР    ВАНЯ    ^ вот эту проверку надо делать по сути для любых переходов на сайте

            return View();
        }
    }
}
