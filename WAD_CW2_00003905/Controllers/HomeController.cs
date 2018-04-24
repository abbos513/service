using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WAD_CW2_00003905.DAL.Entities;
using WAD_CW2_00003905.Models;

namespace WAD_CW2_00003905.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

    }
}