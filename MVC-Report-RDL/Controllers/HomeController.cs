using Microsoft.Reporting.WebForms;
using MVC_Report_RDL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Report_RDL.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
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

        public ActionResult ReportePaises()
        {
            using (DatosEntities d = new DatosEntities())
            {
                var v = d.Paises.ToList();
                return View(v);
            }
        }

        public ActionResult VerReporte(string id)
        {
            LocalReport lr = new LocalReport();
            string rutaReporte = Path.Combine(Server.MapPath("~/Reportes"), "Report1.rdlc");
            if (System.IO.File.Exists(rutaReporte))
            {
                lr.ReportPath = rutaReporte;
            }
            else
            {
                return View("Index");
            }

            List<Paises> pa = new List<Paises>();
            using (DatosEntities d = new DatosEntities())
            {

                pa = d.Paises.ToList();
            }

            ReportDataSource rd = new ReportDataSource("DataSet1", pa);
            lr.DataSources.Add(rd);
            string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;


            Warning[] warnings;
            string[] streams;
            byte[] renderBytes;

            string deviceInfo =
                "<DeviceInfo>" +
                " <OutputFormat>" + id + "</OutputFormat>" +
                " <PageWidth>8.5in</PageWidth>" +
                " <PageHeight>11in</PageHeight>" +
                " <MarginTop>0.5</MarginTop>" +
                " <MarginLeft>1in</MarginLeft>" +
                " <MarginRight>1in</MarginRight>" +
                " <MarginBottom>0.5in</MarginBottom>" +
                " </DeviceInfo>";

            renderBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings
                );

            return File(renderBytes, mimeType);
        }
    }
}