using Microsoft.VisualStudio.TestTools.UnitTesting;
using ISE182_project.Layers.LoggingLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Tester.Layers.BusinessLogic
{
    [TestClass()]
    class LoggerTests
    {
        #region Developer

        [TestMethod()]
        public void Developer_Positive()
        {
            string s = RandomString(120);
            string withReference = Logger.Developer(s);

            Assert.AreEqual("Developer : " + s, Logger.Developer(s));
        }

        [TestMethod()]
        public void Developer_Negative()
        {
            //None
        }

        #endregion

        #region MethodStart

        [TestMethod()]
        public void Server_Positive()
        {
            string s = RandomString(120);
            string withReference = Logger.Developer(s);

            Assert.AreEqual("Server : " + s, Logger.Developer(s));
        }

        [TestMethod()]
        public void Server_Negative()
        {
            //None
        }

        #endregion


        #region Healpers

        private static string RandomString(int length)
        {
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        #endregion
    }
}