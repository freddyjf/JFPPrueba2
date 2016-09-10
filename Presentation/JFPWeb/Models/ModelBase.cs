using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FluentValidation.Attributes;
using JFP.WEB.Framework;

namespace Precedente.Models
{

    public enum etypeModelOperator
    {
        Create,
        Insert,
        Update,
        Delete,
        Browser
    }
    public class ModelBase
    {

        /// <summary>
        /// Determina la operacion que se esta realizando con el Modelo
        /// 1 -> Insert
        /// 2 -> Update
        /// 3 -> Delete
        /// 4 -> Browser
        /// </summary>
        public etypeModelOperator typeModelState { get; set; }

        
        public string textBySearch { get; set; }
    }
}