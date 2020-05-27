﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Egl.Sit.Api.Infrastructure.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class ValidateModelStateFilter : ActionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var validationErrors = context.ModelState
                .Keys
                .ToDictionary(
                    key => key.ToLower(),
                    value => context.ModelState[value]
                        .Errors
                        .Select(s => s.ErrorMessage));

                context.Result = new BadRequestObjectResult(validationErrors);
            }
        }
    }
}