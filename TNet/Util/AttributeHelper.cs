using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace TNet.Util
{
    /// <summary>
    /// 特性辅助类
    /// </summary>
    public class AttributeHelper
    {

        public static string GetValueOfNamePropertyOnDisplayAttribute<TModel, TValue>(Expression<Func<TModel, TValue>> expression) {
           return GetPropertyOnAttributeOfModelProperty<TModel,TValue,DisplayAttribute,string>(expression,"Name");
        }

        private static TAttributeProperty GetPropertyOnAttributeOfModelProperty<TModel, TValue,TAttribute,TAttributeProperty>(Expression<Func<TModel, TValue>> expression,string propertyNameOfAttribute) where TAttribute : Attribute
        {
            var memberExpression = GetMemberInfo(expression);
            TAttribute tat = memberExpression.Member.GetCustomAttribute<TAttribute>();
            Type attrType = typeof(TAttribute);
            TAttributeProperty attributePropertyValue=(TAttributeProperty) attrType.GetProperty(propertyNameOfAttribute).GetValue(tat);
            
            return attributePropertyValue;
        }

        private static MemberExpression GetMemberInfo(Expression method)
        {
            LambdaExpression lambda = method as LambdaExpression;
            if (lambda == null)
                throw new ArgumentNullException("method");

            MemberExpression memberExpr = null;

            if (lambda.Body.NodeType == ExpressionType.Convert)
            {
                memberExpr =
                    ((UnaryExpression)lambda.Body).Operand as MemberExpression;
            }
            else if (lambda.Body.NodeType == ExpressionType.MemberAccess)
            {
                memberExpr = lambda.Body as MemberExpression;
            }

            if (memberExpr == null)
                throw new ArgumentException("method");

            return memberExpr;
        }
    }
}