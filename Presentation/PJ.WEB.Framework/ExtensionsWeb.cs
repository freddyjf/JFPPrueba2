using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using JFP.Core.Config;


namespace System.Web
{
    public static class Extensiones_SystemWeb
    {
        const string LOCALHOST_IP = "127.0.0.1";

        /// <summary>
        /// Retorna el valor string de un campo en un Reader.
        /// </summary>
        /// <param name="lector"></param>
        /// <param name="nombreCampo"></param>
        /// <returns></returns>
        public static string getIP(this HttpRequest Request)
        {
            string IP = string.Empty;

            if (Config.LlaveBoolean("SimularAccesoIP"))
            {
                IP = Config.LlaveString("IPSimulada");

            }
            else
            {


                //Esto es debido a que el nuevo balanceador reescribe la nueva IP
                IP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (String.IsNullOrWhiteSpace(IP))
                    IP = Request.UserHostAddress;
                else
                {
                    if (IP.Contains(","))
                        IP = IP.Split(',').Last().Trim();

                }

                if (IP == "::1")
                    IP = LOCALHOST_IP;
            }

            return IP;
        }

        /// <summary>
        /// Retorna la URL referida
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public static string getHTTP_REFERER(this HttpRequest Request)
        {

            string UrlRef = Convert.ToString(Request.QueryString["HTTP_REFERER"]);

            if (((UrlRef == string.Empty) || (UrlRef == null)) && Request.ServerVariables["HTTP_REFERER"] != null)
                UrlRef = Request.ServerVariables["HTTP_REFERER"].ToString();

            return UrlRef ?? string.Empty;
        }
    }
}