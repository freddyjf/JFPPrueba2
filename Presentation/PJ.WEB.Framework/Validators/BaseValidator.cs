using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace JFP.WEB.Framework.Validators
{
    public class BaseValidator<T>: AbstractValidator<T> where T : class
    {
    
    protected BaseValidator()
    {
       PostInitialize();
    }

    protected virtual void PostInitialize()
    {

    }
    


    }
}
