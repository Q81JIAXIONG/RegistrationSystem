using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Data.SqlClient;

namespace RegistrationSystem.Models.Tests
{
    [TestClass()]
    public class MSDatabaseCommunicationTests
    {
        [TestMethod()]
        public void GetDBDataResultDataTableTest()
        {
            //arrange
            DatabaseCommunicationFactory DBF = new DatabaseCommunicationFactory();
            MSDatabaseCommunication DB = DBF.GetDatabaseCommunication(DatabaseEnum.Test30);
            SqlCommand Command = new SqlCommand();
            Command.CommandText = "select '123'";
            bool bo = false;

            //act
            DataTable dt;
            string expected;
            string actual = "123";

            dt = DB.GetDBDataResultDataTable(Command, out bo);
            expected = dt.Rows[0][0].ToString();

            //assert
            Assert.AreEqual(true, bo);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetDBDataResultDataSetTest()
        {
            //arrange
            DatabaseCommunicationFactory DBF = new DatabaseCommunicationFactory();
            MSDatabaseCommunication DB = DBF.GetDatabaseCommunication(DatabaseEnum.Test30);
            SqlCommand Command = new SqlCommand();
            Command.CommandText = "select '123'";
            bool bo = false;

            //act
            DataSet ds;
            string expected;
            string actual = "123";

            ds = DB.GetDBDataResultDataSet(Command, out bo);
            expected = ds.Tables[0].Rows[0][0].ToString();

            //assert
            Assert.AreEqual(true, bo);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetDBDataResultDataReaderTest()
        {
            //arrange
            DatabaseCommunicationFactory DBF = new DatabaseCommunicationFactory();
            MSDatabaseCommunication DB = DBF.GetDatabaseCommunication(DatabaseEnum.Test30);
            SqlCommand Command = new SqlCommand();
            Command.CommandText = "select '123'";
            bool bo = false;

            //act
            string expected = "";
            string actual = "123";

            using (SqlConnection conn = new SqlConnection())
            {
                using (SqlDataReader dr = DB.GetDBDataResultDataReader(conn, Command, out bo))
                {
                    Console.WriteLine(conn.State);
                    while (dr.Read())
                    {
                        expected = dr[0].ToString();
                    }
                }
            }

            //assert
            Assert.AreEqual(true, bo);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetDBDataResultTest()
        {
            //arrange
            DatabaseCommunicationFactory DBF = new DatabaseCommunicationFactory();
            MSDatabaseCommunication DB = DBF.GetDatabaseCommunication(DatabaseEnum.Test30);
            SqlCommand Command = new SqlCommand();
            Command.CommandText = "select '123'";
            bool bo = false;

            //act
            object expected;
            string actual = "123";

            expected = DB.GetDBDataResult(Command, out bo);

            //assert
            Assert.AreEqual(true, bo);
            Assert.AreEqual(expected.ToString(), actual);
        }

        [TestMethod()]
        public void ExecuteNonQueryTest()
        {
            //arrange
            DatabaseCommunicationFactory DBF = new DatabaseCommunicationFactory();
            MSDatabaseCommunication DB = DBF.GetDatabaseCommunication(DatabaseEnum.Test30);
            SqlCommand Command = new SqlCommand();
            Command.CommandText =
                @"  declare @t table(a int)
                    insert into @t
                    values (1),(1)";
            bool bo = false;

            //act
            int expected;
            int actual = 2;

            expected = DB.ExecuteNonQuery(Command, out bo);

            //assert
            Assert.AreEqual(true, bo);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void ExecuteSQLBulkCopyTest()
        {
            //arrange
            DatabaseCommunicationFactory DBF = new DatabaseCommunicationFactory();
            MSDatabaseCommunication DB = DBF.GetDatabaseCommunication(DatabaseEnum.Test30);
            DataTable dt = new DataTable("BulkCopy");
            dt.Columns.Add("TestingTime", typeof(DateTime));
            DataRow dr = dt.NewRow();
            dr["TestingTime"] = DateTime.Now;
            dt.Rows.Add(dr);

            //act
            bool bo = false;
            bo = DB.ExecuteSQLBulkCopy(dt);

            //assert
            Assert.AreEqual(true, bo);
        }
    }
}