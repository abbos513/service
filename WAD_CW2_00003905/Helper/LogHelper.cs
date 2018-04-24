using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WAD_CW2_00003905;
using WAD_CW2_00003905.Models;
using WAD_CW2_00003905.DAL.Entities;
using WAD_CW2_00003905.DAL.Repositories;

namespace WAD_CW2_00003905.Helper
{
    public class LogHelper
    {

        public string clientIp { get; set; }
        public string username { get; set; }
        public string actionName { get; set; }
        public string controllerName { get; set; }
        public string httpRequestType { get; set; }
        public string httpParameters { get; set; }

        public void getParams(Controllers.ServiceController serviceController, int entityId)
        {
            clientIp = (serviceController.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? serviceController.Request.ServerVariables["REMOTE_ADDR"]).Split(',')[0].Trim();
            username = (string)(serviceController.Session["User"] ?? "Anonymous");
            actionName = serviceController.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = serviceController.ControllerContext.RouteData.Values["controller"].ToString();
            httpRequestType = serviceController.ControllerContext.HttpContext.Request.HttpMethod;
            httpParameters = "";

            if (serviceController.ModelState.ContainsKey("Id") && serviceController.ModelState.Keys.Count > 1)
            {
                foreach (var modelStateKey in serviceController.ModelState.Keys)
                {
                    string key = modelStateKey;

                    var rawValue = new string[1];
                    var value = "";

                    if (actionName == "Delete")
                    {
                        if (key == "Id")
                            value = serviceController.ModelState[modelStateKey].Value.RawValue.ToString();
                    }
                    else
                    {
                        rawValue = (string[])serviceController.ModelState[modelStateKey].Value.RawValue;
                        value = rawValue[0];
                    }

                    if (!string.IsNullOrEmpty(value))
                    {
                        httpParameters += key + " = " + value + "\n";
                    }
                    else
                    {
                        httpParameters += key + " = NOT PROVIDED\n";
                    }

                }
            }
            else if (serviceController.ModelState.Keys.Count > 1)
            {
                httpParameters = "EntityId = " + entityId + "\n";
                foreach (var modelStateKey in serviceController.ModelState.Keys)
                {
                    string key = modelStateKey;
                    var rawValue = (string[])serviceController.ModelState[modelStateKey].Value.RawValue;
                    var value = rawValue[0];
                    if (!string.IsNullOrEmpty(value))
                    {
                        httpParameters += key + " = " + value + "\n";
                    }
                    else
                    {
                        httpParameters += key + " = NOT PROVIDED\n";
                    }

                }
            }
            else
            {
                httpParameters = "EntityId = " + entityId + "\n";
            }
            httpParameters = httpParameters.Remove(httpParameters.Length - 1);
        }

        public void getParams(Controllers.ServiceController serviceControler,
                              string name,
                              SortDetail? criteria,
                              SortOrder? order,
                              int? page)
        {
            clientIp = (serviceControler.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? serviceControler.Request.ServerVariables["REMOTE_ADDR"]).Split(',')[0].Trim();
            username = (string)(serviceControler.Session["User"] ?? "Anonymous");
            username = username.Trim();
            actionName = serviceControler.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = serviceControler.ControllerContext.RouteData.Values["controller"].ToString();
            httpRequestType = serviceControler.ControllerContext.HttpContext.Request.HttpMethod;
            string searchName = name;
            if (string.IsNullOrEmpty(name))
            {
                searchName = "NOT PROVIDED\n";
            }
            httpParameters = "Search Name = " + searchName + "Sort Criteria = " + criteria + "\nSort Order = " + order + "\nPage Number = " + page;

        }

        public void getParams(Controllers.ServiceController bookController, string format)
        {
            clientIp = (bookController.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? bookController.Request.ServerVariables["REMOTE_ADDR"]).Split(',')[0].Trim();
            username = (string)(bookController.Session["User"] ?? "Anonymous");
            actionName = bookController.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = bookController.ControllerContext.RouteData.Values["controller"].ToString();
            httpRequestType = bookController.ControllerContext.HttpContext.Request.HttpMethod;
            httpParameters = "Format = " + format ?? "";
        }

        public void getParams(Controllers.ServiceController bookController)
        {
            clientIp = (bookController.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? bookController.Request.ServerVariables["REMOTE_ADDR"]).Split(',')[0].Trim();
            username = (string)(bookController.Session["User"] ?? "Anonymous");
            actionName = bookController.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = bookController.ControllerContext.RouteData.Values["controller"].ToString();
            httpRequestType = bookController.ControllerContext.HttpContext.Request.HttpMethod;
            httpParameters = "NO PARAMETERS";
        }

        public void getParams(Controllers.ServiceController bookController, ServiceViewModel model)
        {
            clientIp = (bookController.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? bookController.Request.ServerVariables["REMOTE_ADDR"]).Split(',')[0].Trim();
            username = (string)(bookController.Session["User"] ?? "Anonymous");
            actionName = bookController.ControllerContext.RouteData.Values["action"].ToString();
            controllerName = bookController.ControllerContext.RouteData.Values["controller"].ToString();
            httpRequestType = bookController.ControllerContext.HttpContext.Request.HttpMethod;
            httpParameters = "NO PARAMETERS";
            httpParameters = "Id = " + model.Id +
                             "\nName = " + model.Name +
                             "\nType = " + model.Type +
                             "\nDescription = " + model.Description +
                             "\nPrice = " + model.Price;
        }

        public void logToDatabase(LogHelper logHelper)
        {
            Log log = new Log();
            log.actionDate = DateTime.Now;
            log.userName = logHelper.username;
            log.actionName = logHelper.actionName;
            log.controllerName = logHelper.controllerName;
            log.requestIP = logHelper.clientIp;
            log.requestParams = logHelper.httpParameters;
            log.requestType = logHelper.httpRequestType;
            new LoggingRepository().Create(log);
        }
    }
}