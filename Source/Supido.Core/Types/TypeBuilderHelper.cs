using Supido.Core.Proxy;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Supido.Core.Types
{
    /// <summary>
    /// Helper for building types.
    /// </summary>
    public class TypeBuilderHelper
    {
        #region - Static Methods -

        /// <summary>
        /// Creates a new TypeBuilder in the given domain
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="assemblyNameStr">The assembly name string.</param>
        /// <param name="dynamicModuleName">Name of the dynamic module.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <returns></returns>
        public static TypeBuilder GetClassTypeBuilder(AppDomain domain, string assemblyNameStr, string dynamicModuleName, string typeName)
        {
            AssemblyName assemblyName = new AssemblyName(assemblyNameStr);
            AssemblyBuilder assemblyBuilder = domain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder module = assemblyBuilder.DefineDynamicModule(dynamicModuleName);
            return module.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Class);
        }

        /// <summary>
        /// Adds a new property to the type builder, with getter and setter to a generated field.
        /// </summary>
        /// <param name="typeBuilder">The type builder.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyType">Type of the property.</param>
        public static void AddProperty(TypeBuilder typeBuilder, string propertyName, Type propertyType)
        {
            const MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.HideBySig;
            FieldBuilder field = typeBuilder.DefineField("_" + propertyName, typeof(string), FieldAttributes.Private);
            PropertyBuilder property = typeBuilder.DefineProperty(propertyName, PropertyAttributes.None, propertyType, new[] { propertyType });
            MethodBuilder getMethodBuilder = typeBuilder.DefineMethod("get_value", getSetAttr, propertyType, Type.EmptyTypes);
            ILGenerator getIl = getMethodBuilder.GetILGenerator();
            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, field);
            getIl.Emit(OpCodes.Ret);
            MethodBuilder setMethodBuilder = typeBuilder.DefineMethod("set_value", getSetAttr, null, new[] { propertyType });
            ILGenerator setIl = setMethodBuilder.GetILGenerator();
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, field);
            setIl.Emit(OpCodes.Ret);
            property.SetGetMethod(getMethodBuilder);
            property.SetSetMethod(setMethodBuilder);
        }

        /// <summary>
        /// Given a source type, clones the type but only the common type properties.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="domain">The domain.</param>
        /// <param name="assemblyNameStr">The assembly name string.</param>
        /// <param name="dynamicModuleName">Name of the dynamic module.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <returns></returns>
        public static Type CloneCommonType(Type sourceType, AppDomain domain, string assemblyNameStr, string dynamicModuleName, string typeName)
        {
            TypeBuilder typeBuilder = TypeBuilderHelper.GetClassTypeBuilder(domain, assemblyNameStr, dynamicModuleName, typeName);
            IObjectProxy entityProxy = ObjectProxyFactory.GetByType(sourceType);
            foreach (PropertyInfo propertyInfo in entityProxy.Properties)
            {
                if (TypesManager.IsCommonType(propertyInfo.PropertyType))
                {
                    TypeBuilderHelper.AddProperty(typeBuilder, propertyInfo.Name, propertyInfo.PropertyType);
                }
            }
            return typeBuilder.CreateType();
        }

        #endregion
    }
}
