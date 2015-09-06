using Supido.Business.Attributes;
using Supido.Business.Meta;
using Supido.Core.Types;
using Supido.Core.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using Telerik.OpenAccess.Metadata;
using Telerik.OpenAccess.Metadata.Relational;

namespace Supido.Business.Security
{
    /// <summary>
    /// Security scanner for assemblies
    /// </summary>
    public class SecurityScanner
    {
        #region - Fields -

        /// <summary>
        /// The metatables
        /// </summary>
        private Dictionary<string, MetaPersistentType> metatables;

        /// <summary>
        /// The metatables, but the string index is only class name without namespace.
        /// </summary>
        private Dictionary<string, MetaPersistentType> typeMetatables = new Dictionary<string, MetaPersistentType>();

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets the parent security manager.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public ISecurityManager Parent { get; private set; }

        /// <summary>
        /// Gets or sets the entity preffix.
        /// </summary>
        /// <value>
        /// The entity preffix.
        /// </value>
        public string EntityPreffix { get; set; }

        /// <summary>
        /// Gets or sets the entity suffix.
        /// </summary>
        /// <value>
        /// The entity suffix.
        /// </value>
        public string EntitySuffix { get; set; }

        /// <summary>
        /// Gets or sets the dto preffix.
        /// </summary>
        /// <value>
        /// The dto preffix.
        /// </value>
        public string DtoPreffix { get; set; }

        /// <summary>
        /// Gets or sets the dto suffix.
        /// </summary>
        /// <value>
        /// The dto suffix.
        /// </value>
        public string DtoSuffix { get; set; }

        /// <summary>
        /// Gets or sets the filter preffix.
        /// </summary>
        /// <value>
        /// The filter preffix.
        /// </value>
        public string FilterPreffix { get; set; }

        /// <summary>
        /// Gets or sets the filter suffix.
        /// </summary>
        /// <value>
        /// The filter suffix.
        /// </value>
        public string FilterSuffix { get; set; }

        /// <summary>
        /// Gets or sets the bo preffix.
        /// </summary>
        /// <value>
        /// The bo preffix.
        /// </value>
        public string BOPreffix { get; set; }

        /// <summary>
        /// Gets or sets the bo suffix.
        /// </summary>
        /// <value>
        /// The bo suffix.
        /// </value>
        public string BOSuffix { get; set; }

        /// <summary>
        /// Gets or sets the metatables.
        /// </summary>
        /// <value>
        /// The metatables.
        /// </value>
        public Dictionary<string, MetaPersistentType> Metatables 
        {
            get
            {
                return this.metatables; 
            }
            set
            {
                this.metatables = value;
                this.typeMetatables.Clear();
                foreach (KeyValuePair<string, MetaPersistentType> kvp in this.metatables)
                {
                    this.typeMetatables.Add(this.GetClassName(kvp.Key), kvp.Value);
                }
            }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityScanner"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public SecurityScanner(ISecurityManager parent)
        {
            this.Parent = parent;
            this.EntityPreffix = string.Empty;
            this.EntitySuffix = string.Empty;
            this.DtoPreffix = string.Empty;
            this.DtoSuffix = string.Empty;
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Removes the preffix from a name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="preffix">The preffix.</param>
        /// <returns></returns>
        private string RemovePreffix(string name, string preffix)
        {
            if (!string.IsNullOrEmpty(preffix))
            {
                if (name.StartsWith(preffix))
                {
                    name = name.Substring(preffix.Length);
                }
            }
            return name;
        }

        /// <summary>
        /// Removes the suffix from a name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="suffix">The suffix.</param>
        /// <returns></returns>
        private string RemoveSuffix(string name, string suffix)
        {
            if (!string.IsNullOrEmpty(suffix))
            {
                if (name.EndsWith(suffix))
                {
                    name = name.Substring(0, name.Length - suffix.Length);
                }
            }
            return name;
        }

        /// <summary>
        /// Gets the name of the entity from a dto name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private string GetEntityName(string name)
        {
            name = this.RemovePreffix(name, this.DtoPreffix);
            name = this.RemoveSuffix(name, this.DtoSuffix);
            return this.EntityPreffix + name + this.EntitySuffix;
        }

        /// <summary>
        /// Gets the class name form a full name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private string GetClassName(string name)
        {
            int i = name.LastIndexOf('.');
            if (i > -1)
            {
                return name.Substring(i + 1, name.Length - i - 1);
            }
            else
            {
                return name;
            }
        }

        /// <summary>
        /// Determines whether the specified type is dto.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private bool IsDto(Type type)
        {
            return AttributeUtil.GetAttributeFrom<DtoAttribute>(type) != null;
        }

        /// <summary>
        /// Determines whether the specified type is filter.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private bool IsFilter(Type type)
        {
            return AttributeUtil.GetAttributeFrom<FilterAttribute>(type) != null;
        }

        /// <summary>
        /// Determines whether the specified type is bo.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private bool IsBO(Type type)
        {
            return AttributeUtil.GetAttributeFrom<BOAttribute>(type) != null;
        }

        /// <summary>
        /// Processes the type if it's a DTO
        /// </summary>
        /// <param name="type">The type.</param>
        private void ProcessDto(Type type)
        {
            DtoAttribute dtoAttribute = AttributeUtil.GetAttributeFrom<DtoAttribute>(type);
            if (dtoAttribute != null)
            {
                Type entityType = null;
                if (dtoAttribute.EntityType != null)
                {
                    entityType = dtoAttribute.EntityType;
                }
                else
                {
                    string entityName = dtoAttribute.EntityName;
                    if (string.IsNullOrEmpty(entityName))
                    {
                        entityName = this.GetEntityName(type.Name);
                    }
                    entityType = TypesManager.ResolveType(entityName);
                    if (entityType == null) 
                    {
                        if (this.typeMetatables.ContainsKey(entityName))
                        {
                            entityName = this.typeMetatables[entityName].FullName;
                            entityType = TypesManager.ResolveType(entityName);
                        }
                    }
                }
                if (entityType != null)
                {
                    IMetamodelEntity metaEntity = this.Parent.MetamodelManager.RegisterEntity(entityType, type);
                    if (this.Metatables.ContainsKey(entityType.FullName))
                    {
                        MetaPersistentType metaType = this.Metatables[entityType.FullName];
                        Dictionary<MetaColumn, MetaMember> mapColumns = new Dictionary<MetaColumn, MetaMember>();
                        foreach (MetaMember member in metaType.Members)
                        {
                            MetaPrimitiveMember primitiveMember = member as MetaPrimitiveMember;
                            if (primitiveMember != null)
                            {
                                mapColumns.Add(primitiveMember.Column, member);
                            }
                        }
                        MetaTable metaTable = metaType.Table;
                        foreach (MetaColumn metaColumn in metaTable.Columns)
                        {
                            MetaMember member = mapColumns[metaColumn];
                            metaEntity.AddField(member.Name, metaColumn.IsPrimaryKey, !metaColumn.IsPrimaryKey);
                        }

                    }
                }
            }
        }

        /// <summary>
        /// Processes a type if its a Filter
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private void ProcessFilter(Type type, Dictionary<string, Type> dtoMap)
        {
            FilterAttribute filterAttribute = AttributeUtil.GetAttributeFrom<FilterAttribute>(type);
            if (filterAttribute == null)
            {
                return;
            }
            if (filterAttribute.DtoType != null)
            {
                this.Parent.BOManager.AddFilterType(filterAttribute.DtoType, type);
                return;
            }
            string name = string.IsNullOrEmpty(filterAttribute.Name) ? type.Name : filterAttribute.Name;
            if (name.IndexOf('.') > -1)
            {
                Type dtoType = TypesManager.ResolveType(name);
                if (dtoType != null)
                {
                    this.Parent.BOManager.AddFilterType(dtoType, type);
                }
                return;
            }
            if (dtoMap.ContainsKey(name))
            {
                this.Parent.BOManager.AddFilterType(dtoMap[name], type);
                return;
            }
            string testname = this.DtoPreffix + name + this.DtoSuffix;
            if (dtoMap.ContainsKey(testname))
            {
                this.Parent.BOManager.AddFilterType(dtoMap[testname], type);
                return;
            }
            testname = name;
            testname = this.RemovePreffix(testname, this.FilterPreffix);
            testname = this.RemoveSuffix(testname, this.FilterSuffix);
            if (dtoMap.ContainsKey(testname))
            {
                this.Parent.BOManager.AddFilterType(dtoMap[testname], type);
                return;
            }
            testname = this.DtoPreffix + testname + this.DtoSuffix;
            if (dtoMap.ContainsKey(testname))
            {
                this.Parent.BOManager.AddFilterType(dtoMap[testname], type);
                return;
            }
        }

        /// <summary>
        /// Processes a type if its a BO
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private void ProcessBO(Type type, Dictionary<string, Type> dtoMap)
        {
            BOAttribute boAttribute = AttributeUtil.GetAttributeFrom<BOAttribute>(type);
            if (boAttribute == null)
            {
                return;
            }
            if (boAttribute.DtoType != null)
            {
                this.Parent.BOManager.AddBOType(boAttribute.DtoType, type);
                return;
            }
            string name = string.IsNullOrEmpty(boAttribute.Name) ? type.Name : boAttribute.Name;
            if (name.IndexOf('.') > -1)
            {
                Type dtoType = TypesManager.ResolveType(name);
                if (dtoType != null)
                {
                    this.Parent.BOManager.AddBOType(dtoType, type);
                }
                return;
            }
            if (dtoMap.ContainsKey(name))
            {
                this.Parent.BOManager.AddBOType(dtoMap[name], type);
                return;
            }
            string testname = this.DtoPreffix + name + this.DtoSuffix;
            if (dtoMap.ContainsKey(testname))
            {
                this.Parent.BOManager.AddBOType(dtoMap[testname], type);
                return;
            }
            testname = name;
            testname = this.RemovePreffix(testname, this.BOPreffix);
            testname = this.RemoveSuffix(testname, this.BOSuffix);
            if (dtoMap.ContainsKey(testname))
            {
                this.Parent.BOManager.AddBOType(dtoMap[testname], type);
                return;
            }
            testname = this.DtoPreffix + testname + this.DtoSuffix;
            if (dtoMap.ContainsKey(testname))
            {
                this.Parent.BOManager.AddBOType(dtoMap[testname], type);
                return;
            }
        }

        /// <summary>
        /// Scans the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        protected void Scan(Assembly assembly)
        {
            IList<Type> dtoTypes = new List<Type>();
            IList<Type> filterTypes = new List<Type>();
            IList<Type> boTypes = new List<Type>();
            IList<Type> serviceTypes = new List<Type>();
            foreach (Type type in assembly.GetTypes())
            {
                if (this.IsDto(type))
                {
                    dtoTypes.Add(type);
                }
                else if (this.IsFilter(type))
                {
                    filterTypes.Add(type);
                }
                else if (this.IsBO(type))
                {
                    boTypes.Add(type);
                }
            }
            // Process Dtos
            Dictionary<string, Type> dtoMap = new Dictionary<string, Type>();
            foreach (Type type in dtoTypes)
            {
                dtoMap.Add(GetClassName(type.Name), type);
                this.ProcessDto(type);
            }
            // Process filters
            foreach (Type type in filterTypes)
            {
                this.ProcessFilter(type, dtoMap);
            }
            // Process filters
            foreach (Type type in boTypes)
            {
                this.ProcessBO(type, dtoMap);
            }
        }

        /// <summary>
        /// Scans the specified assembly given its name.
        /// </summary>
        /// <param name="namespaceStr">The namespace string.</param>
        protected void Scan(string namespaceStr)
        {
            Assembly assembly = Assembly.Load(namespaceStr);
            if (assembly != null)
            {
                this.Scan(assembly);
            }
        }

        /// <summary>
        /// Scans several assemblies by the namespaces comma separated. If no assembly name its provided, then scan all the assemblies of the current domain.
        /// </summary>
        /// <param name="namespacesStr">The namespaces string.</param>
        public void ScanNamespace(string namespacesStr)
        {
            if (string.IsNullOrEmpty(namespacesStr))
            {
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    this.Scan(assembly);
                }
            }
            else
            {
                string[] namespaces = namespacesStr.Split(',');
                foreach (string namespaceStr in namespaces)
                {
                    this.Scan(namespacesStr.Trim());
                }
            }
        }

        #endregion
    }
}
