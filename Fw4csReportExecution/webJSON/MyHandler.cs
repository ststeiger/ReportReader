using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AnySqlWebAdmin
{
    class MyHandler
        : System.Web.IHttpHandler
    {


        public class MS_SQL_Service 
            : SqlService
        {

        }


        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            try
            {
                System.Collections.Generic.Dictionary<string, object> pars = SqlServiceHelper.GetParameters(context);

                if (!pars.ContainsKey("sql"))
                    throw new System.Exception("Parameter sql not provided....");


                string sql = System.Convert.ToString(pars["sql"]);
                sql = System.IO.Path.Combine("SQL", sql);
                sql = System.IO.File.ReadAllText(sql, System.Text.Encoding.UTF8);


                RenderType_t format = RenderType_t.Array;

                if (pars.ContainsKey("format"))
                {
                    string form = System.Convert.ToString(pars["format"]);
                    int renderType = 1;
                    int.TryParse(form, out renderType);

                    format = (RenderType_t)renderType;
                } // End if (pars.ContainsKey("format")) 

                SqlServiceJsonHelper.AnyDataReaderToAnyJson(sql, new MS_SQL_Service(), pars, context, format);

                // throw new Exception("SQL error");
                // await context.Response.WriteAsync("Howdy");
                // context.Response.StatusCode = 500;
            } // End Try 
            catch (System.Exception ex)
            {
                // header($_SERVER['SERVER_PROTOCOL'] . ' 500 Internal Server Error', true, 500);
                // header("HTTP/1.0 500 Internal Server Error");
                // header('HTTP/1.1 200 OK');

                // context.Response.Headers["HTTP/1.0 500 Internal Server Error"] = "";
                context.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                context.Response.Headers["X-Error-Message"] = "Incorrect username or password";

                context.Response.ContentType = "text/plain";

                context.Response.Write(ex.Message);
                context.Response.Write(System.Environment.NewLine);
                context.Response.Write(System.Environment.NewLine);
                context.Response.Write(ex.StackTrace);
                System.Console.WriteLine();
            } // End Catch 
        }


        bool IHttpHandler.IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}
