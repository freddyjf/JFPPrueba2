using System;
using System.Collections.Generic;
using System.Web;
using JFP.Core.Authentication;

namespace JFP.WEB.Framework.WebContext
{
    public class CurrentContext : IWebContext
    {

        #region Fileds
        public const string CURRENT_SESION_SUSCRIPTOR = "current.context.suscriptor";
        public const string SessionAuthSuscriptor = "SESSION_AUTH_SUSCRIPTOR";

        #endregion

       
        //private CurrentContext _ContextBySesion = null;
        private readonly HttpSessionStateBase _httpSesion;


        public CurrentContext(HttpSessionStateBase sesion)
        {

            _httpSesion = sesion;
          

        }

       

       
        public void removeSuscriptor()
        {
            throw new NotImplementedException();
        }
    }
}
