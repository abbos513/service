using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WAD_CW2_00003905.DAL.Entities;
using WAD_CW2_00003905.DAL.Repositories;
using WAD_CW2_00003905.Models;

namespace WAD_CW2_00003905.Controllers
{
    public class FileUploadController : Controller
    {
        // GET: FileUpload
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection formCollection)
        {
            var file = Request.Files["fileToImport"];

            if(file == null)
            {
                ViewBag.Result = "File is missing";
                return View();
            }

            var services = new List<ServiceImportModel>();
            using (var reader = new StreamReader(file.InputStream))
            {
                string line;
                while((line = reader.ReadLine()) != null)
                {
                    var tokens = line.Split('|');

                    var service = new ServiceImportModel();

                    service.Name = tokens[0];
                    service.Type = tokens[1];
                    service.Price = decimal.Parse(tokens[2]);
                    service.Description = tokens[3];

                    services.Add(service);
                    new ServiceRepository().Create(MapFromModel(service));
                }
            }
            ViewBag.Services = services;

            return View();
        }


        private Service MapFromModel(ServiceImportModel model)
        {
            return new Service()
            {
                Name = model.Name,
                Type = model.Type,
                Description = model.Description,
                Price = model.Price
            };
        }
    }
}