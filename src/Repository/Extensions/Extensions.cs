using Model.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Extensions
{
    public static class Extensions
    {
        public static bool IgnoreProperty<T>(this Type typeObject, string propertyName)
        {
            return Attribute.IsDefined(typeObject.GetProperty(propertyName), typeof(IgnoreDuringInsertOrUpdateAttribute), false);
        }

        public static bool IgnoreTrailProperty<T>(this Type typeObject, string propertyName)
        {
            return Attribute.IsDefined(typeObject.GetProperty(propertyName), typeof(IgnoreDuringInsertOrUpdateAttribute), false);
        }
        
        public static string GetTableName<T>(this Type entity)
        {
            if (!Attribute.IsDefined(entity, typeof(TableNameAttribute), false))
            {
                if (Attribute.IsDefined(entity, typeof(TableNameAttribute), false))
                {
                    TableNameAttribute tableNameAttribute1 = (TableNameAttribute)Attribute.GetCustomAttribute(entity, typeof(TableNameAttribute));
                    return tableNameAttribute1.Name;
                }
                return null;
            }

            TableNameAttribute tableNameAttribute = (TableNameAttribute)Attribute.GetCustomAttribute(entity, typeof(TableNameAttribute));
            return tableNameAttribute.Name;
        }
    }
}
