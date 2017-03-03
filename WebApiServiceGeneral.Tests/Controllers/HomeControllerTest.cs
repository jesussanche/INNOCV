using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApiServiceGeneral;
using WebApiServiceGeneral.Controllers;

namespace WebApiServiceGeneral.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void HomeIndexTest()
        {
            // Disponer
            HomeController controller = new HomeController();

            // Actuar
            ViewResult result = controller.Index() as ViewResult;

            // Declarar
            Assert.IsNotNull(result);
            Assert.AreEqual("Home Page", result.ViewBag.Title);
        }
    }
}
