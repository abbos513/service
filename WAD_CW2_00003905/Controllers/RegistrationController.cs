using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WAD_CW2_00003905.DAL.Entities;
using WAD_CW2_00003905.Models;
using WAD_CW2_00003905.DAL.Repositories;
using System.Net.Mail;
using System.Net;
using System.Web.Security;
using Newtonsoft.Json;
using System.Configuration;

namespace WAD_CW2_00003905.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: Registration
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = MapFromModel(model);
                var repository = new UserRepository();
                repository.Create(user);

                try
                {
                    ViewBag.EmailSendingResult = "Email succesfully";
                    string emailBody = "This email address is registered with " + model.Username + " username " + model.Password + " password";
                    ////(string from, string to, string subject, string body);
                    //var mail = new MailMessage("abbos513@gmail.com", model.Email, "Registration", emailBody);
                    //var smtpClient = new SmtpClient("smtp.gmail.com", 587);

                    SmtpClient client = new SmtpClient("smtp.gmail.com", 25);
                    client.EnableSsl = true;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("forwadcw@gmail.com", "!qwertyui");
                    MailMessage msgobj = new MailMessage();
                    msgobj.To.Add(model.Email);
                    msgobj.From = new MailAddress("forwadcw@gmail.com");
                    msgobj.Subject = "Registration";
                    msgobj.Body = emailBody;
                    client.Send(msgobj);
                    Response.Write("Message has been send succesfully");
                    
                }
                catch (Exception)
                {
                    ViewBag.EmailSendingResult = "Failed. Try again";
                    ModelState.AddModelError("", "Something went wron, please try again. Check your network to get email");
                }
            }

            ModelState.AddModelError("", "Something went wron, please try again");
            return RedirectToAction("Login");
        }

        public User MapFromModel(RegistrationViewModel model)
        {
            return new User
            {
                Email = model.Email,
                Username = model.Username,
                Password = model.Password,
                FirstName = model.Firstname,
                LastName = model.LastName
            };
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (Session["User"] == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Service");
            }
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = UserExsists(model);

            if (user != null)
            {
                var ticket = new FormsAuthenticationTicket(
                    1,
                    string.Format(user.Username),
                    DateTime.Now,
                    DateTime.Now.Add(FormsAuthentication.Timeout),
                    false,
                    user.Id.ToString(),
                    FormsAuthentication.FormsCookiePath);

                //encrypt the ticket
                var encryptedTicket = FormsAuthentication.Encrypt(ticket);

                // create the cockie
                var authCookie = new HttpCookie("UserLoginData")
                {
                    Value = encryptedTicket,
                    Expires = DateTime.Now.Add(FormsAuthentication.Timeout)
                };
                HttpContext.Response.Cookies.Set(authCookie);
                return RedirectToAction("Index", "Service");
            }


            ModelState.AddModelError("", "User does not exsist");

            return View(model);
        }

        public ActionResult LogOut()
        {
            var httpCookie = HttpContext.Response.Cookies["UserLoginData"];
            if (httpCookie != null)
            {
                httpCookie.Value = string.Empty;
            }
            return RedirectToAction("Login");
        }

        private User UserExsists(LoginViewModel model)
        {
            var repository = new UserRepository();
            return repository.GetAll().FirstOrDefault(u => u.Username == model.Username || u.Email == model.Username && u.Password == model.Password);
        }

        private User UserExsists(RegistrationViewModel model)
        {
            var repository = new UserRepository();
            return repository.GetAll().FirstOrDefault(u => u.Username == model.Username || u.Email == model.Email);
        }

        private bool ValidateCaptcha()
        {
            var response = Request["g-recaptcha-response"];
            //secret that was generated in key value pair
            string secret = ConfigurationManager.AppSettings["reCAPTCHASecretKey"];
            var client = new WebClient();

            var reply =
                client.DownloadString(
                    string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));
            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);
            //when response is false check for the error message
            if (!captchaResponse.Success)
            {
                if (captchaResponse.ErrorCodes.Count <= 0) return false;
                var error = captchaResponse.ErrorCodes[0].ToLower();

                switch (error)
                {
                    case ("missing-input-secret"):
                        ViewBag.message = "The secret parameter is missing.";
                        break;
                    case ("invalid-input-secret"):
                        ViewBag.message = "The secret parameter is invalid or malformed.";
                        break;
                    case ("missing-input-response"):
                        ViewBag.message = "The response parameter is missing. Please, preceed with reCAPTCHA.";
                        break;
                    case ("invalid-input-response"):
                        ViewBag.message = "The response parameter is invalid or malformed.";
                        break;
                    default:
                        ViewBag.message = "Error occured. Please try again";
                        break;
                }
                return false;
            }
            else
            {
                ViewBag.message = "Valid";
            }
            return true;
        }

        public class CaptchaResponse// Recapcha did not a
        {
            [JsonProperty("success")]
            public bool Success { get; set; }

            [JsonProperty("error-codes")]
            public List<string> ErrorCodes { get; set; }
        }
    }
}