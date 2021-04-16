using AuthorizationExample;
using AutorizationExample.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace AutorizationExample.Controllers
{
    public class AuthorizationController : Controller
    {

        private readonly ProjectActivityDbContext _context;

        public AuthorizationController(ProjectActivityDbContext context)
        {
            _context = context;
        }


        private User TryAuthenticateUser(string Login, string Password)
        {
            return _context.Users.FirstOrDefault(x => x.Login == Login && x.Password == Password);
        }
        
        private User FindUserByLogin(string Login)
        {
            return _context.Users.FirstOrDefault(x => x.Login == Login);
        }

        private void SaveCookies(User user)
        {
            Response.Cookies.Append("Login", Convert.ToBase64String(Encoding.ASCII.GetBytes(user.Login)));
            Response.Cookies.Append("Password", Convert.ToBase64String(Encoding.ASCII.GetBytes(user.Password)));
        }

        public ActionResult Authorizate(User user)
        {
            if (user.Admin)
            {
                SaveCookies(user);
                return RedirectToAction("Main", "Admin"/*, user*/);
            }
            SaveCookies(user);
            return RedirectToAction("Main", "User"/*, user*/);
            
        }

        private void SaveInput(string l,string p, string ac)
        {
            ViewBag.Login = l;
            ViewBag.Password = p;
            ViewBag.AdminCode = ac;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(string Login, string Password, string AdminCode)
        {
            if (Login == null || Password == null)
            {
                SaveInput(Login, Password, AdminCode);
                ViewBag.Error += "\nПожалуйста введите логин и пароль";
                return View();
            }

            if (FindUserByLogin(Login)!=null)
            {
                SaveInput(Login, Password, AdminCode);
                ViewBag.Error += "\nЛогин занят";
                return View();
            }

            if (TryAuthenticateUser(Login, Password) == null)
            {
                User user = new User { Login = Login, Password = Password, Admin = AdminCode != null ? true : false };

                if (user.Admin)
                {
                    if (AdminCode == "A1B2C3")
                    {
                        _context.Users.Add(user);
                        _context.SaveChanges();
                        return Authorizate(user);
                    }
                    else
                    {
                        SaveInput(Login, Password, AdminCode);
                        ViewBag.Error += "\nПроверочный код неправильный";
                        return View();
                    }
                }
                _context.Users.Add(user);
                _context.SaveChanges();
                return Authorizate(user);
            }

            SaveInput(Login, Password, AdminCode);
            ViewBag.Error += "\nНекорректные данные";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string Login, string Password)
        {
            User user = TryAuthenticateUser(Login, Password);
            if (user == null)
            {
                SaveInput(Login, Password, "");
                ViewBag.Error += "\nНекорректные данные";
                return View();
            }
            else
            {
                return Authorizate(user);
            }
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

       
        public ActionResult Start()
        {
            if(!Request.Cookies.Keys.Contains("Login") || !Request.Cookies.Keys.Contains("Password")) return RedirectToAction("Index", "Home");


            string login = Encoding.ASCII.GetString(Convert.FromBase64String(Request.Cookies["Login"]));
            string password = Encoding.ASCII.GetString(Convert.FromBase64String(Request.Cookies["Password"]));

            User user = TryAuthenticateUser(login, password);

            if (user != null)
            {
                return Authorizate(user);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
