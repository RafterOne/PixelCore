using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace PixelMEDIA.PixelCore.Helpers
{
    /// <summary>
    /// Reflection utilities.
    /// </summary>
	public static class ReflectionHelper
	{
		/// <summary>
		/// See if a property has an attribute.
		/// </summary>
		/// <param name="prop"></param>
		/// <param name="attributeType"></param>
		/// <returns></returns>
		public static bool HasAttribute(this ICustomAttributeProvider prop, Type attributeType)
		{
			return prop.IsDefined(attributeType, true);
		}

		/// <summary>
		/// See if a property has an attribute (Generic version).
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="prop"></param>
		/// <returns></returns>
		public static bool HasAttribute<T>(this ICustomAttributeProvider prop) where T : Attribute
		{
			return prop.IsDefined(typeof(T), true);
		}

		/// <summary>
		/// Check if a property has a public setter.
		/// </summary>
		/// <param name="prop"></param>
		/// <returns></returns>
		public static bool HasPublicSetter(this PropertyInfo prop)
		{
			return prop.GetSetMethod() != null;
		}

		/// <summary>
		/// Copies every common property with the same name, same type, and a public setter from the first object to the second object.
		/// </summary>
		/// <param name="copyFrom"></param>
		/// <param name="copyTo"></param>
		public static void CopyProperties(object copyFrom, object copyTo)
		{
			var tfrom = copyFrom.GetType();
			var tto = copyTo.GetType();

			foreach (var propFrom in tfrom.GetProperties())
			{
				var propTo = tto.GetProperty(propFrom.Name);

				if (propTo != null
					&& propTo.HasPublicSetter()
					&& propTo.PropertyType == propFrom.PropertyType)
				{
					var propertyValue = propFrom.GetValue(copyFrom, null);
					propTo.SetValue(copyTo, propertyValue, null);
				}
			}
		}


		/// <summary>
		/// Gets a named property from an object.
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="o"></param>
		/// <returns></returns>
		public static object GetProperty(string propertyName, object o)
		{
			var otype = o.GetType();
			var propTo = otype.GetProperty(propertyName);
			return propTo.GetValue(o, null);
		}

		/// <summary>
		/// Creates a new object of the generic type argument with a copy of all common properties of the passed in object.
		/// </summary>
		/// <typeparam name="TTo"></typeparam>
		/// <param name="copyFrom"></param>
		/// <returns></returns>
        public static TTo Transmute<TTo>(object copyFrom)
		{
			var copyTo = Activator.CreateInstance<TTo>();
			CopyProperties(copyFrom, copyTo);
			return copyTo;
		}

		/// <summary>
		/// Return a collection of properties that are publicly getable.
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		public static IEnumerable<PropertyInfo> GetGetableProperties(Type t)
		{
			var getableProps = new List<PropertyInfo>();
			foreach (var prop in t.GetProperties())
			{
				if (prop != null)
				{
					var getMethod = prop.GetGetMethod();
					if (prop.CanRead && getMethod != null && getMethod.IsPublic)
					{
						getableProps.Add(prop);
					}
				}
			}
			return getableProps;
		}

		/// <summary>
		/// Gets a collection of properties in the order in which they appear in the fieldNames array. If no fieldNames are passed, all properties are returnee.
		/// </summary>
		/// <param name="t"></param>
		/// <param name="fieldNames"></param>
		/// <returns></returns>
		public static IEnumerable<PropertyInfo> GetGetableProperties(Type t, params string[] fieldNames)
		{
			return GetSpecifiedPropertiesInFieldNameOrder(GetGetableProperties(t), fieldNames);
		}

		/// <summary>
		/// This is for sequencing fields in a collection for export purposes.
		/// </summary>
		/// <param name="props"></param>
		/// <param name="fieldNames"></param>
		/// <returns></returns>
		public static IEnumerable<PropertyInfo> GetSpecifiedPropertiesInFieldNameOrder(IEnumerable<PropertyInfo> props, params string[] fieldNames)
		{
			if (fieldNames.Length > 0)
			{
				var propList = new List<PropertyInfo>();
				foreach (var fieldName in fieldNames)
				{
					var prop = (from foundProp in props where foundProp.Name == fieldName select foundProp).FirstOrDefault();
					if (prop != null)
					{
						propList.Add(prop);
					}
				}
				return  propList;
			}
			return props;
		}

		/// <summary>
		/// Compares the public properties of two objects.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="object1"></param>
		/// <param name="object2"></param>
		/// <returns></returns>
		public static bool ShallowCompare<T>(T object1, T object2)
		{
			var ttype = typeof(T);

			foreach (var prop in GetGetableProperties(ttype))
			{
				var value1 = prop.GetValue(object1, null);
				var value2 = prop.GetValue(object2, null);

				if (!Object.Equals(value1, value2))
				{
					return false;
				}
			}

			return true;
		}

        /// <summary>
        /// Set the property value of the given name on an object..
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="propertyValue">The value of the property.</param>
        public static void SetPropertyValue(object obj, string propertyName, object propertyValue)
        {
            if ((obj != null) && (!string.IsNullOrEmpty(propertyName)))
            {
                try
                {
                    var prop = obj.GetType().GetProperty(propertyName);
                    var propertyType = prop.GetType();
                    var propertyTypeValue = Convert.ChangeType(propertyValue, prop.PropertyType);
                    prop.SetValue(obj, propertyTypeValue, null);
                }
                catch (Exception)
                {
                    return;
                }
            }
        }
	}
}
