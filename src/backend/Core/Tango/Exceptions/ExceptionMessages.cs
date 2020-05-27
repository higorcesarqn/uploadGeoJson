using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Tango.Types.Extensions;

namespace Core.Tango.Exceptions
{
    internal enum ExceptionMessages
    {
        [Description("The input \"{0}\" must be positive")]
        MustBePositive
    }

    internal static class ExceptionMessagesMethods
    {
        internal static string GetMessage(this ExceptionMessages message, params string[] args)
            => string.Format(message.GetDescription(), args);
        
    }
}
