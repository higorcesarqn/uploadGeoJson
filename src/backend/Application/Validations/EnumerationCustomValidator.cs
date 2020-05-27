﻿using Core.Models;
using FluentValidation;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Application.Validations
{
    public static class EnumerationCustomValidator
    {
        public static IRuleBuilderInitial<T, int?> IsEnumeration<T, TEnumeration>(this IRuleBuilder<T, int?> ruleBuilder, string errorMessage, bool isRequired = true)
            where TEnumeration : Enumeration
        {
            return ruleBuilder.Custom((id, context) =>
            {
                if (!isRequired && !id.HasValue)
                {
                    return;
                }

                if (isRequired && !id.HasValue)
                {
                    context.AddFailure(errorMessage);
                }

                var flag = Enumeration.GetAll<TEnumeration>().Any(x => x.Id == id);
                if (!flag)
                {
                    context.AddFailure(errorMessage);
                }
            });
        }

        public static IRuleBuilderInitial<T, int> IsEnumeration<T, TEnumeration>(this IRuleBuilder<T, int> ruleBuilder, string errorMessage)
            where TEnumeration : Enumeration
        {
            return ruleBuilder.Custom((id, context) =>
            {
                var flag = Enumeration.GetAll<TEnumeration>().Any(x => x.Id == id);
                if (!flag)
                {
                    context.AddFailure(errorMessage);
                }
            });
        }

        public static IRuleBuilderInitial<T, int> IsEnumeration<T, TEnumeration>(this IRuleBuilder<T, int> ruleBuilder, Expression<Func<TEnumeration, bool>> expression, string errorMessage)
            where TEnumeration : Enumeration
        {
            return ruleBuilder.Custom((id, context) =>
            {
                var flag = Enumeration.GetAll<TEnumeration>().Any(x => x.Id == id);

                if (!flag)
                {
                    context.AddFailure(errorMessage);
                }
            });
        }
    }
}
