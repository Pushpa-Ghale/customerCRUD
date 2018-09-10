using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace customerCRUD
{
    public static class customerCRUD
    {
        [FunctionName("Function1")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string fname = req.Query["first name"];
            string lname = req.Query["last name"];

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            fname = fname ?? data?.fname;
            lname = lname ?? data?.lname;
            string action = data?.action;
            int id = data?.id;
            string phnum = data?.phnum;

            Customer customerInfo = new Customer();
            customerInfo.id = id;
            customerInfo.fName = fname;
            customerInfo.lName = lname;
            customerInfo.phoneNum = phnum;
            customerInfo.address = "address";

            string results = string.Empty;
            if (action == "Insert")
            {
                results = InsertRecord(customerInfo, log);
            }
            else if (action == "Update")
            {
                results = UpdateRecord(customerInfo, log);
            }
            else if (action == "Delete")
            {
                results = DeleteRecord(customerInfo, log);
            }
            else
            {
                results = "Action not recognized";
            }

            return results != null
                ? (ActionResult)new OkObjectResult(results)
                : new BadRequestObjectResult("Something went wrong!!");
        }

        public static string InsertRecord(Customer customerInfo,TraceWriter log)
        {
            //preparing sql cmd to insert the customer data to database
            string cmdStr = $"insert into customer(Id, Fname, Lname, Address, Phnum ) values ('{customerInfo.id}' , '{customerInfo.fName}', '{customerInfo.lName}', '{customerInfo.address}', {customerInfo.phoneNum})";
            log.Info(cmdStr);

            string ExecuteSqlResult = ExecuteSqlCmd(cmdStr , log);
            return $"Performed {cmdStr} in the database. {ExecuteSqlResult}";

        }

        public static string UpdateRecord(Customer customerInfo,TraceWriter log)
        {
            //preparing sql cmd to update the customer data to database. Id is the primary key
            string cmdStr = $"update customer SET Fname = '{customerInfo.fName}', Lname = '{customerInfo.lName}', Address = '{customerInfo.address}', Phnum = {customerInfo.phoneNum} where Id = {customerInfo.id}";
            log.Info(cmdStr);

            string ExecuteSqlResult = ExecuteSqlCmd(cmdStr, log);
            return $"Performed {cmdStr} in the database. {ExecuteSqlResult}";
        }

        public static string DeleteRecord(Customer customerInfo,TraceWriter log)
        {
            //preparing sql cmd to delete the customer data to database. Id is the primary key
            string cmdStr = $"Delete from customer where Id = {customerInfo.id}";
            log.Info(cmdStr);
            string ExecuteSqlResult = ExecuteSqlCmd(cmdStr, log);
            return $"Performed {cmdStr} in the database. {ExecuteSqlResult}";
        }

        public static string ExecuteSqlCmd(string cmdStr, TraceWriter log)
        {
            string ExecuteSqlResult = string.Empty;
            try
            {
                SqlConnection con = new SqlConnection("Server=tcp:*****.database.windows.net,1433;Initial Catalog=pushpaDB;Persist Security Info=False;User ID=*****;Password=*****;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
                SqlCommand cmd = new SqlCommand();

                con.Open();
                SqlCommand myCommand = new SqlCommand(cmdStr, con);
                myCommand.Connection = con;
                myCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                log.Info(ex.Message);
                ExecuteSqlResult = ex.Message;
            }
            return ExecuteSqlResult;
        }
    }
    
    public class Customer
    {
        public int id { get; set; }
        public string fName { get; set; }
        public string lName { get; set; }
        public string phoneNum { get; set; }
        public string address { get; set; }
    }
}
