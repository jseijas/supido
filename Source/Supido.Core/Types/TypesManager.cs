using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Supido.Core.Types
{
    /// <summary>
    /// Manager for common and db types
    /// </summary>
    public static class TypesManager
    {
        #region - Fields -

        /// <summary>
        /// The types resolver.
        /// </summary>
        private static readonly TypeResolver resolver = new TypeResolver();

        /// <summary>
        /// Common types
        /// </summary>
        public static Dictionary<string, Type> commonTypes = new Dictionary<string, Type>();

        /// <summary>
        /// Inverse common types
        /// </summary>
        private static readonly Dictionary<Type, string> commonTypeNames = new Dictionary<Type, string>();

        /// <summary>
        /// Database types
        /// </summary>
        public static Dictionary<string, DbType> dbTypes = new Dictionary<string, DbType>();

        /// <summary>
        /// Inverse database types
        /// </summary>
        private static readonly Dictionary<DbType, string> dbTypeNames = new Dictionary<DbType, string>();


        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes the <see cref="TypesManager"/> class.
        /// </summary>
        static TypesManager()
        {
            AddDefaultCommonTypes();
            AddDefaultDbTypes();
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Adds a comon type
        /// </summary>
        /// <param name="names">The names.</param>
        /// <param name="type">The type.</param>
        public static void AddCommonType(string[] names, Type type)
        {
            if ((names != null) && (names.Length > 0))
            {
                foreach (string name in names)
                {
                    commonTypes.Add(name, type);
                    resolver.AddType(name, type);
                }
                commonTypeNames.Add(type, names[0]);
            }
        }

        /// <summary>
        /// Adds a database type.
        /// </summary>
        /// <param name="names">The names.</param>
        /// <param name="type">The type.</param>
        public static void AddDbType(string[] names, DbType type)
        {
            if ((names != null) && (names.Length > 0))
            {
                foreach (string name in names)
                {
                    dbTypes.Add(name, type);
                }
                dbTypeNames.Add(type, names[0]);
            }
        }

        /// <summary>
        /// Adds the default common types.
        /// </summary>
        private static void AddDefaultCommonTypes()
        {
            AddCommonType(new string[] { "bool", "Bool", "BOOL", "boolean", "Boolean", "BOOLEAN", "bool?" }, typeof(bool));
            AddCommonType(new string[] { "byte", "Byte", "BYTE" }, typeof(byte));
            AddCommonType(new string[] { "short", "Short", "SHORT", "int16", "Int16", "INT16" }, typeof(short));
            AddCommonType(new string[] { "int", "Int", "INT", "int32", "Int32", "INT32" }, typeof(int));
            AddCommonType(new string[] { "long", "Long", "LONG", "int64", "Int64", "INT64" }, typeof(long));
            AddCommonType(new string[] { "float", "Float", "FLOAT", "single", "Single", "SINGLE" }, typeof(float));
            AddCommonType(new string[] { "double", "Double", "DOUBLE" }, typeof(double));
            AddCommonType(new string[] { "decimal", "Decimal", "DECIMAL" }, typeof(decimal));
            AddCommonType(new string[] { "byte[]", "Byte[]", "BYTE[]", "image", "Image", "IMAGE", "binary", "Binary", "BINARY" }, typeof(byte[]));
            AddCommonType(new string[] { "DateTime", "datetime", "dateTime", "DATETIME" }, typeof(DateTime));
            AddCommonType(new string[] { "Guid", "guid", "GUID" }, typeof(Guid));
            AddCommonType(new string[] { "string", "String", "STRING" }, typeof(string));
            AddCommonType(new string[] { "object", "Object", "OBJECT" }, typeof(object));
        }

        /// <summary>
        /// Adds the default db types.
        /// </summary>
        private static void AddDefaultDbTypes()
        {
            AddDbType(new string[] { "bool", "Bool", "BOOL", "boolean", "Boolean", "BOOLEAN", "bool?" }, DbType.Boolean);
            AddDbType(new string[] { "byte", "Byte", "BYTE" }, DbType.Byte);
            AddDbType(new string[] { "short", "Short", "SHORT", "int16", "Int16", "INT16" }, DbType.Int16);
            AddDbType(new string[] { "int", "Int", "INT", "int32", "Int32", "INT32" }, DbType.Int32);
            AddDbType(new string[] { "long", "Long", "LONG", "int64", "Int64", "INT64" }, DbType.Int64);
            AddDbType(new string[] { "float", "Float", "FLOAT", "single", "Single", "SINGLE" }, DbType.Single);
            AddDbType(new string[] { "double", "Double", "DOUBLE" }, DbType.Double);
            AddDbType(new string[] { "decimal", "Decimal", "DECIMAL" }, DbType.Decimal);
            AddDbType(new string[] { "byte[]", "Byte[]", "BYTE[]", "image", "Image", "IMAGE", "binary", "Binary", "BINARY" }, DbType.Binary);
            AddDbType(new string[] { "DateTime", "datetime", "dateTime", "DATETIME" }, DbType.DateTime);
            AddDbType(new string[] { "Guid", "guid", "GUID" }, DbType.Guid);
            AddDbType(new string[] { "string", "String", "STRING" }, DbType.String);
            AddDbType(new string[] { "object", "Object", "OBJECT" }, DbType.Object);
        }

        /// <summary>
        /// Gets a common type given its name.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns></returns>
        public static Type GetCommonType(string typeName)
        {
            typeName = typeName.Trim();
            if (commonTypes.ContainsKey(typeName))
            {
                return commonTypes[typeName];
            }
            return null;
        }

        /// <summary>
        /// Gets the name of the common type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string GetCommonTypeName(Type type)
        {
            if (commonTypeNames.ContainsKey(type))
            {
                return commonTypeNames[type];
            }
            return type.ToString();
        }

        /// <summary>
        /// Determines whether [is common type] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// 	<c>true</c> if [is common type] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsCommonType(Type type)
        {
            return (commonTypeNames.ContainsKey(type));
        }

        /// <summary>
        /// Gets a database type given its name.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns></returns>
        public static DbType GetDbType(string typeName)
        {
            typeName = typeName.Trim();
            if (dbTypes.ContainsKey(typeName))
            {
                return dbTypes[typeName];
            }
            throw new TypeLoadException(string.Format("TypesManager can not load the database type \"{0}\"", typeName));
        }

        /// <summary>
        /// Gets the name of the db type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string GetDbTypeName(DbType type)
        {
            if (dbTypeNames.ContainsKey(type))
            {
                return dbTypeNames[type];
            }
            return type.ToString();
        }

        /// <summary>
        /// Determines whether [is db type] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// 	<c>true</c> if [is db type] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDbType(DbType type)
        {
            return (dbTypeNames.ContainsKey(type));
        }

        /// <summary>
        /// Gets the default type of the db.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static DbType GetDefaultDbType(Type type)
        {
            return GetDbType(GetCommonTypeName(type));
        }

        /// <summary>
        /// Resolves the type.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns></returns>
        public static Type ResolveType(string typeName)
        {
            return resolver.Resolve(typeName);
        }

        /// <summary>
        /// Resolves the type.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static Type ResolveType(string typeName, Assembly assembly)
        {
            Type result = null;
            if (assembly != null)
            {
                result = assembly.GetType(typeName, true);
            }
            if (result == null)
            {
                result = ResolveType(typeName);
            }
            return result;
        }

        #endregion
    }
}
