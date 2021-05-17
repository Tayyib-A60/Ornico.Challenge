using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Attributes
{
    /// <summary>
    /// Applied to properties which should be ignored during Insert or Update Operations
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IgnoreDuringInsertOrUpdateAttribute : Attribute { }


    /// <summary>
    /// Applied to classes mapped to database tables.
    /// <para>Name: Name of the table mapped to the class</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TableNameAttribute : Attribute
    {
        public string Name { get; set; }
        public TableNameAttribute(string name)
        {
            Name = name;
        }
    }
}
