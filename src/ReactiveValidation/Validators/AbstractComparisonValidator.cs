﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using ReactiveValidation.Helpers;

namespace ReactiveValidation.Validators
{
    public abstract class AbstractComparisonValidator <TObject, TProp> : PropertyValidator<TObject, TProp>
        where TObject : IValidatableObject
        where TProp : IComparable
    {
        private readonly ParameterInfo<TObject, TProp> _valueToCompare;

        protected AbstractComparisonValidator(
            IStringSource stringSource,
            Expression<Func<TObject, TProp>> valueToCompareExpression,
            ValidationMessageType validationMessageType)
            : base(stringSource, validationMessageType, valueToCompareExpression)
        {
            _valueToCompare = valueToCompareExpression.GetParameterInfo();
        }

        protected sealed override bool IsValid(ValidationContext<TObject, TProp> context)
        {
            var paramValue = context.GetParamValue(_valueToCompare);
            var comparationResult = Comparer<TProp>.Default.Compare(context.PropertyValue, paramValue);

            if (IsValid(comparationResult) == false) {
                context.RegisterMessageArgument("ValueToCompare", _valueToCompare, paramValue);
                return false;
            }

            return true;
        }

        protected abstract bool IsValid(int comparationResult);
    }
}
