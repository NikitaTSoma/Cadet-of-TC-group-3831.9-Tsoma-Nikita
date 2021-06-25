using Microsoft.VisualStudio.TestTools.UnitTesting;
using KursValut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Configuration;
using System.Data.SqlClient;

namespace KursValut.Tests
{
    [TestClass()]
    public class Form1Tests
    {
        public SqlConnection sqlConnection = null;


        [TestMethod()]
        public void Form1_LoadTest()
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DBKValuts"].ConnectionString);
            sqlConnection.Open();
            Assert.IsTrue(sqlConnection.State == ConnectionState.Open);
        }

    }
}