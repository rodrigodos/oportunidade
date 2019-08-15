    using RodrigoDiasMostUsedWords.Models;
    using System;
    using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Mvc;
    using System.Xml.Serialization;

    namespace RodrigoDiasMostUsedWords.Controllers
    {
        public class HomeController : Controller
        {
            public ActionResult Index()
            {


            ViewBag.ErrorMessage = "";

            TrendWords trw = new TrendWords();
            
            TopicWords tow = new TopicWords();

            List<TrendWords> trwl = new List<TrendWords>();
            List<TopicWords> towl = new List<TopicWords>();
            try
            {

            
            trwl = trw.ListTrendWords();
            towl = tow.ListTopicWords();
            }
            catch(Exception ex)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry(ex.Message + " "+ ex.StackTrace, EventLogEntryType.Error, 101, 1);
                }

                //throw ex;
            }


            var objTuple = new Tuple<List<TrendWords>, List<TopicWords>>(trwl,towl);


            return View(objTuple);
            }

            public ActionResult About()
            {
                ViewBag.Message = "Your application description page.";

                return View();
            }

            public ActionResult Contact()
            {
                ViewBag.Message = "Your contact page.";

                return View();
            }
        }
    }