using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Exia.Utils {
    public static class ExpressionExtension {
        /// <summary>
        /// Get the name of a specified property lambda expression
        /// </summary>
        /// <param name="propertyExpression">A lambda expression</param>
        /// <returns>A string containing the name of the property</returns>
        public static string GetPropertyName(LambdaExpression propertyExpression) {
            if (propertyExpression == null) {
                throw new ArgumentNullException("propertyExpression");
            }

            if (!(propertyExpression.Body is MemberExpression body)) {
                throw new ArgumentException("Invalid argument", "propertyExpression");
            }

            if (!(body.Member is PropertyInfo member)) {
                throw new ArgumentException("Argument is not a property", "propertyExpression");
            }

            return member.Name;
        }

        /// <summary>
        /// Get the name of a specified property lambda expression
        /// </summary>
        /// <typeparam name="T">Type of the property</typeparam>
        /// <param name="propertyExpression">A lambda expression</param>
        /// <returns>A string containing the name of the property</returns>
        public static string GetPropertyName<T>(Expression<Func<T>> propertyExpression) {
            return ExpressionExtension.GetPropertyName(propertyExpression as LambdaExpression);
        }

        /// <summary>
        /// Compare a string to a property expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="propertyExpression"></param>
        /// <returns></returns>
        public static bool EqualToPropertyName<T>(this string source, Expression<Func<T>> propertyExpression) {
            if (string.IsNullOrEmpty(source)) {
                throw new ArgumentNullException(nameof(source));
            }

            if (propertyExpression == null) {
                throw new ArgumentNullException(nameof(propertyExpression));
            }

            return source == ExpressionExtension.GetPropertyName(propertyExpression);
        }
    }
}