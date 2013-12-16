using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Xml;
using System;
using System.Collections;
using System.Data.Entity.Infrastructure;

namespace PixelMEDIA.PixelCore.Helpers
{
	/// <summary>
	/// Helper library for Entity-related mundane/verbose functionality simplification.
	/// </summary>
	public static class EntityHelper
	{
		/// <summary>
		/// Set the entity's EntityState.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="context"></param>
		/// <param name="entity"></param>
		/// <param name="state"></param>
		public static void SetState<T>(this DbContext context, T entity, EntityState state) where T : class
		{
			context.Entry(entity).State = state;
		}

		/// <summary>
		/// Set the entity to the Modified EntityState.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="context"></param>
		/// <param name="entity"></param>
		public static void SetModified<T>(this DbContext context, T entity) where T : class
		{
			SetState(context, entity, EntityState.Modified);
		}

		/// <summary>
		/// Attach a modified entity to the collection and flag it as modified.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="context"></param>
		/// <param name="collection"></param>
		/// <param name="entity"></param>
		public static void AttachModified<T>(this DbContext context, DbSet<T> collection, T entity) where T : class
		{
			collection.Attach(entity);
			SetModified(context, entity);
		}


		/*
		public static IEnumerable<string> GetEntityFields<T>(this DbContext context, T entity) where T : class
		{
			var query = context.Entry(entity).GetType().GetInterfaces().Single(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>)).GetGenericArguments().Single();
			var columns = query.GetProperties();
			return columns.Select(column => column.Name);
		}
		*/

		/*
		public static XmlDocument GetEntityXml(this DbContext context, object entity) 
		{
			var props = ReflectionHelper.GetGetableProperties(entity.GetType());
			var doc = XmlHelper.CreateDocument("entity");

			foreach (var prop in props)
			{
				if (prop.GetAccessors().Where(p => p.IsVirtual).Count() == 0) //The virtual properties in DbContext entities point to the objects (don't want). WRONG.
				{
					doc.DocumentElement.SetAttribute(prop.Name, Convert.ToString(prop.GetValue(entity, null)));
				}
			}

			return doc;
		}
		 */

	}
}