using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApiServiceGeneral;
using WebApiServiceGeneral.Controllers;

namespace WebApiServiceGeneral.Tests.Controllers
{
    [TestClass]
    public class UserControllerTest
    {
        [TestMethod]
        public void UserGetTest()
        {
            // Disponer
            UserController controller = new UserController();

            // Actuar
            IEnumerable<DataAccessINNOCV.Users> result = controller.Get();

            // Declarar
            if (result != null)
            {
                Assert.AreEqual(1, result.ElementAt(0).Id);
                Assert.AreEqual(2, result.ElementAt(1).Id);
            }
                
        }

        [TestMethod]
        public void UserGetByIdTest()
        {
            // Disponer
            UserController controller = new UserController();

            // Actuar
            DataAccessINNOCV.Users result = controller.Get(5);

            // Declarar
            if (result != null)
                Assert.AreEqual(5, result.Id);
            
        }

        [TestMethod]
        public void UserPostTest()
        {
            // Disponer
            UserController controller = new UserController();

            DataAccessINNOCV.Users user = new DataAccessINNOCV.Users();

            // Actuar
            string result = controller.Create(user);

            
            // Declarar
            Assert.AreNotEqual(string.Empty, result);

        }

        [TestMethod]
        public void UserPutTest()
        {
            // Disponer
            UserController controller = new UserController();

            DataAccessINNOCV.Users user = new DataAccessINNOCV.Users();
            
            // Actuar
            string result = controller.Put(5, user);

            
            // Declarar
            Assert.AreNotEqual(string.Empty, result);
            Assert.AreEqual("5", result);
        }

        [TestMethod]
        public void UserDeleteTest()
        {
            // Disponer
            UserController controller = new UserController();

            // Actuar
            string result = controller.Delete(5);

            // Declarar
            Assert.AreNotEqual(string.Empty, result);
            Assert.AreEqual("5", result);

        }
    }
}
