using PagedList;
using System;//
using System.Collections.Generic;//
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using System.Xml.Schema;
using WAD_CW2_00003905.DAL.Entities;//
using WAD_CW2_00003905.DAL.Repositories;//
using WAD_CW2_00003905.Helper;
using WAD_CW2_00003905.Models;//

namespace WAD_CW2_00003905.Controllers
{
    [Authorize]
    public class ServiceController : Controller
    {
        // GET: Service.
        public ActionResult Index(string name, SortDetail? sortDetails, SortOrder? sortOrder, int? page)
        {
            List<ServiceViewModel> model = new List<ServiceViewModel>();
            var services = new ServiceRepository().GetAll();

            List<SelectListItem> Items = new List<SelectListItem>();
            //List<DAL.Entities.Type> categories = new ServiceTypeRepository().GetAll().ToList();

            Items.Add(new SelectListItem() { Text = "Select Category", Value = "", Selected = false });
            //foreach (var item in categories)
            //{
            //    Items.Add(new SelectListItem() { Text = item.TypeName, Value = item.Id.ToString(), Selected = false });
            //}

            ViewBag.ListItems = Items;
            ViewBag.Name = name ?? "";
            //ViewBag.Category = type ?? "";
            ViewBag.SortDetail = sortDetails ?? SortDetail.Type;
            ViewBag.Order = sortOrder ?? SortOrder.ASC;

            services = sortFilter(services, name, sortDetails, sortOrder);

            model = services.Select(MapToModel).ToList();

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            LogHelper logging = new LogHelper();
            logging.getParams(this, name, sortDetails, sortOrder, pageNumber);
            logging.logToDatabase(logging);

            return View(model.ToPagedList(pageNumber, pageSize));

        }


        [HttpGet]
        public ActionResult Logging()
        {
            var logsData = new LoggingRepository().GetAll().ToList();
            var logs = new List<LoggingViewModel>();

            foreach (var item in logsData)
            {
                var log = new LoggingViewModel();
                log.date = item.actionDate.ToString();
                log.username = item.userName;
                log.ip = item.requestIP;
                log.controller = item.controllerName;
                log.action = item.actionName;
                log.httpRequestType = item.requestType;
                log.httpParameters = item.requestParams;

                logs.Add(log);
            }

            LogHelper logHelper = new LogHelper();
            logHelper.getParams(this, 0);
            logHelper.logToDatabase(logHelper);

            return View(logs);
        }

        private Service MapFromModel(ServiceViewModel model)
        {
            return new Service()
            {
                Id = model.Id,
                Name = model.Name,
                Type = model.Type,
                Description = model.Description,
                Price = model.Price
                //Type1 = model.typeId
            };
        }

        private ServiceViewModel MapToModel(Service service)
        {
            return new ServiceViewModel()
            {
                Id = service.Id,
                Name = service.Name,
                Type = service.Type,
                Description = service.Description,
                Price = service.Price
                //typeId = service.Type1
            };
        }


        [HttpGet]
        public ActionResult Create()
        {
            List<SelectListItem> services = new List<SelectListItem>();
            //List<DAL.Entities.Type> types = new ServiceTypeRepository().GetAll().ToList();
            //foreach (var item in types)
            //{
            //    services.Add(new SelectListItem() { Text = item.TypeName, Value = item.Id.ToString(), Selected = false });
            //}
            ViewBag.ListServices = services;

            LogHelper logHelper = new LogHelper();
            logHelper.getParams(this, 0);
            logHelper.logToDatabase(logHelper);

            return View(new ServiceViewModel());
        }

        [HttpPost]
        public ActionResult Create(ServiceViewModel model)
        {
            new ServiceRepository().Create(MapFromModel(model));

            LogHelper logHelper = new LogHelper();
            logHelper.getParams(this, 0);
            logHelper.logToDatabase(logHelper);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            return View(MapToModel(new ServiceRepository().GetById(id)));
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            List<SelectListItem> Services = new List<SelectListItem>();
            //List<DAL.Entities.Type> type = new ServiceTypeRepository().GetAll().ToList();
            //foreach (var service in type)
            //{
            //    Services.Add(new SelectListItem() { Text = service.TypeName, Value = service.Id.ToString(), Selected = false });
            //}
            ViewBag.ListServices = Services;

            LogHelper logHelper = new LogHelper();
            logHelper.getParams(this, 0);
            logHelper.logToDatabase(logHelper);

            return View(MapToModel(new ServiceRepository().GetById(id)));
        }

        [HttpPost]
        public ActionResult Edit(ServiceViewModel model)
        {
            new ServiceRepository().Update(MapFromModel(model));

            LogHelper logHelper = new LogHelper();
            logHelper.getParams(this, model.Id);
            logHelper.logToDatabase(logHelper);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            LogHelper logHelper = new LogHelper();
            logHelper.getParams(this, 0);
            logHelper.logToDatabase(logHelper);

            return View(MapToModel(new ServiceRepository().GetById(id)));
        }

        [HttpPost]
        public ActionResult Delete(ServiceViewModel model)
        {
            new ServiceRepository().Delete(model.Id);

            LogHelper logHelper = new LogHelper();
            logHelper.getParams(this, model.Id);
            logHelper.logToDatabase(logHelper);

            return RedirectToAction("Index");
        }


        public ActionResult Export(string format)
        {
            var services = new ServiceRepository().GetAll().ToList();

            var xDoc = new XDocument();
            xDoc.Declaration = new XDeclaration("1.0", "utf-8", "no");

            if (format == "html")
            {
                xDoc.Add(new XProcessingInstruction("xml-stylesheet", "type='text/xsl' href='/XML/ServicesToHtml.xslt'"));
            }

            if (format == "csv")
            {
                xDoc.Add(new XProcessingInstruction("xml-stylesheet", "type='text/xsl' href='/XML/ServiceToCSV.csv'"));
            }

            xDoc.Add(new XElement("Services", services.Select(s =>
                                                                    new XElement("Service", new XAttribute("Id", s.Id),
                                                                    new XElement("Name", s.Name),
                                                                    new XElement("Type", s.Type),
                                                                    new XElement("Description", s.Description),
                                                                    new XElement("Price", s.Price)
                                                                    ))));
                
            var schemas = new XmlSchemaSet();
            schemas.Add("", "http://" + System.Web.HttpContext.Current.Request.Url.Authority + "/XML/ServiceSchema.xsd");

            var isValid = true;
            var errorMessage = "";

            xDoc.Validate(schemas, (o, e) =>
            {
                isValid = false;
                errorMessage = e.Message;
            }
            , true);

            if (!isValid)
            {
                xDoc = new XDocument();
                xDoc.Declaration = new XDeclaration("1.0", "utf-8", "no");
                xDoc.Add(new XElement("Error", errorMessage));
            }



            var sw = new StringWriter();
            xDoc.Save(sw);

            LogHelper logHelper = new LogHelper();
            logHelper.getParams(this, 0);
            logHelper.logToDatabase(logHelper);

            return Content(sw.ToString(), "text/xml");
        }

        public IQueryable<Service> sortFilter(IQueryable<Service> services, string name, SortDetail? detail, SortOrder? order)
        {
            //Filter
            //if (!string.IsNullOrEmpty(type))
            //{
            //    int filter = int.Parse(type);
            //    services = services.Where(p => p.Type == filter);
            //}
            ////Search
            if (!string.IsNullOrEmpty(name))
                services = services.Where(p => p.Name.ToLower().Contains(name.ToLower()));
            //Sort
            if (detail == SortDetail.Price)
            {
                if (order == SortOrder.DESC)
                    services = services.OrderByDescending(p => p.Price);
                else
                    services = services.OrderBy(p => p.Price);
            }
            else if (detail == SortDetail.Name)
            {javascript:;
                if (order == SortOrder.DESC)
                    services = services.OrderByDescending(p => p.Name);
                else
                    services = services.OrderBy(p => p.Name);
            }
            //else if (detail == SortDetail.Category)
            //{
            //    if (order == SortOrder.DESC)
            //        services = services.OrderByDescending(p => p.Type1.TypeName);
            //    else
            //        services = services.OrderBy(p => p.Type1.TypeName);
            //}
            return services;
        }

    }
}