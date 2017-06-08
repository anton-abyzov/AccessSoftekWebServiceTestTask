using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Services;
using log4net.Core;
using log4net.Repository;

namespace AccessSoftekTestTask
{
    /// <summary>
    /// Summary description for AmazonService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class AmazonService : System.Web.Services.WebService
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        // AA: this is useless as instance of webservice is created each time it's executed
        //private IDictionary<string, Order> cache = new Dictionary<string, Order>(); // 
        private object lockObj = new Object();
        private ICustomLogger logger = new CustomLogger();
        [WebMethod]
        public Order LoadOrderInfo(string orderCode)
        {
            if (string.IsNullOrEmpty(orderCode))
            {
                Context.Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
                return null;
            }

            try
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                lock (lockObj)
                {
                    if (HttpRuntime.Cache.Get(orderCode) != null)
                    {
                        stopWatch.Stop();
                        logger.Log("INFO", "Elapsed - {0}", stopWatch.Elapsed);
                        return HttpRuntime.Cache.Get(orderCode) as Order;
                    }
                }
                string queryTemplate =
                  "SELECT OrderID, CustomerID, TotalMoney" +
                  "  FROM dbo.Orders where OrderCode='{0}'";
                string query = string.Format(queryTemplate, orderCode);
                SqlConnection connection =
                  new SqlConnection(this.connectionString);
                SqlCommand command =
                  new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    //int.TryParse could be added for with custom handling when type cast fails
                    var orderId = (int)reader[0];
                    var customerId = (int)reader[1];
                    var totalMoney = (int)reader[2];
                    Order order = new Order(orderId, customerId, totalMoney);
                    lock (lockObj)
                    {
                        if (HttpRuntime.Cache.Get(orderCode) == null)
                            HttpRuntime.Cache.Insert(orderCode, order);
                    }
                    stopWatch.Stop();
                    logger.Log("INFO", "Elapsed - {0}", stopWatch.Elapsed);
                    return order;
                }
                stopWatch.Stop();
                logger.Log("INFO", "Elapsed - {0}", stopWatch.Elapsed);
                Context.Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
                return null;
            }
            catch (SqlException ex)
            {
                logger.Log("ERROR", ex.Message);
                throw new ApplicationException("Error");
            }
        }
    }

    public class Order
    {
        public Order(){ }
        public Order(int orderId, int customerId, int totalMoney)
        {
            OrderID = orderId;
            CustomerID = customerId;
            TotalMoney = totalMoney;
        }
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public int TotalMoney { get; set; }
    }

    public class OrderItem
    {
        public int OrderItemID { get; set; }
        public int OrderID { get; set; }
        public int TotalMoney { get; set; }
    }

    public interface ICustomLogger : ILogger
    {
        void Log(string severity, string erroMessage, TimeSpan timeElapsed);
        void Log(string severity, string erroMessage);
    }

    public class CustomLogger : ICustomLogger
    {
        public void Log(Type callerStackBoundaryDeclaringType, Level level, object message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void Log(LoggingEvent logEvent)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabledFor(Level level)
        {
            throw new NotImplementedException();
        }

        public string Name { get; }
        public ILoggerRepository Repository { get; }
        public void Log(string severity, string erroMessage, TimeSpan timeElapsed)
        {
            Debug.Write($"{severity}, {String.Format(erroMessage, timeElapsed)}");
        }

        public void Log(string severity, string erroMessage)
        {
            Debug.Write($"{severity}, {erroMessage}");
        }
    }
}
