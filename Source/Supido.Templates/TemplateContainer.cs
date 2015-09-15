using Supido.Core.Proxy;
using System.Collections.Generic;

namespace Supido.Templates
{
    /// <summary>
    /// Class for a template container, tree structured.
    /// </summary>
    public class TemplateContainer
    {
        #region - Properties -

        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public TemplateContainer Parent { get; private set; }

        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public Dictionary<string, object> Attributes { get; private set; }

        /// <summary>
        /// Gets the array values.
        /// </summary>
        /// <value>
        /// The array values.
        /// </value>
        public List<TemplateContainer> ArrayValues { get; private set; }

        /// <summary>
        /// Gets the array values map.
        /// </summary>
        /// <value>
        /// The array values map.
        /// </value>
        public Dictionary<string, TemplateContainer> ArrayValuesMap { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is list.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is list; otherwise, <c>false</c>.
        /// </value>
        public bool IsList
        {
            get
            {
                return this.ArrayValues.Count > 0;
            }
        }

        /// <summary>
        /// Gets the root.
        /// </summary>
        /// <value>
        /// The root.
        /// </value>
        public TemplateContainer Root
        {
            get
            {
                TemplateContainer container = this;
                while (container.Parent != null)
                {
                    container = container.Parent;
                }
                return container;
            }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateContainer"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public TemplateContainer(TemplateContainer parent)
        {
            this.Parent = parent;
            this.Attributes = new Dictionary<string, object>();
            this.ArrayValues = new List<TemplateContainer>();
            this.ArrayValuesMap = new Dictionary<string, TemplateContainer>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateContainer"/> class.
        /// </summary>
        public TemplateContainer()
            : this(null)
        {

        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Adds from object.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public void AddFromObject(object instance)
        {
            if (instance != null)
            {
                IObjectProxy proxy = ObjectProxyFactory.Get(instance);
                foreach (string propertyName in proxy.PropertyNames)
                {
                    object value = proxy.GetValue(instance, propertyName);
                    if (value != null)
                    {
                        this.AddAttribute(propertyName, value);
                    }
                }
            }
        }

        /// <summary>
        /// Adds the attribute.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void AddAttribute(string name, object value)
        {
            if (this.Attributes.ContainsKey(name))
            {
                this.Attributes[name] = value;
            }
            else
            {
                this.Attributes.Add(name, value);
            }
        }

        /// <summary>
        /// Removes the attribute.
        /// </summary>
        /// <param name="name">The name.</param>
        public void RemoveAttribute(string name)
        {
            this.Attributes.Remove(name);
        }

        /// <summary>
        /// Gets the attribute.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public object GetAttribute(string name)
        {
            if (this.Attributes.ContainsKey(name))
            {
                return this.Attributes[name];
            }
            return null;
        }


        public TemplateLink AddLink(string name, TemplateLink link)
        {
            this.AddAttribute(name, link);
            return link;
        }

        public TemplateLink AddLink(string name, string link)
        {
            return this.AddLink(name, new TemplateLink(this, link));
        }

        public TemplateLink AddListLink(string name, string link)
        {
            TemplateLink result = this.AddLink(name, new TemplateLink(this, link));
            result.IsList = true;
            return result;
        }

        /// <summary>
        /// Adds the child.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public TemplateContainer AddChild(string name)
        {
            return this.AddChild(name, new TemplateContainer(this));
        }

        public TemplateContainer AddChild(string name, TemplateContainer container)
        {
            this.AddAttribute(name, container);
            return container;
        }

        /// <summary>
        /// Removes the child.
        /// </summary>
        /// <param name="name">The name.</param>
        public void RemoveChild(string name)
        {
            if (this.GetChild(name) != null)
            {
                this.RemoveAttribute(name);
            }
        }

        /// <summary>
        /// Gets the child.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public TemplateContainer GetChild(string name)
        {
            object obj = this.GetAttribute(name);
            if ((obj == null) || !(obj is TemplateContainer))
            {
                return null;
            }
            return obj as TemplateContainer;
        }

        /// <summary>
        /// Adds the array value.
        /// </summary>
        /// <returns></returns>
        public TemplateContainer AddArrayValue()
        {
            return this.AddArrayValue(new TemplateContainer(this));
        }

        /// <summary>
        /// Adds the array value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public TemplateContainer AddArrayValue(string key)
        {
            return this.AddArrayValue(key, new TemplateContainer(this));
        }

        /// <summary>
        /// Adds the array value.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public TemplateContainer AddArrayValue(TemplateContainer item)
        {
            return this.AddArrayValue(this.ArrayValues.Count.ToString(), item);
        }

        /// <summary>
        /// Adds the array value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public TemplateContainer AddArrayValue(string key, TemplateContainer item)
        {
            this.ArrayValuesMap.Add(key, item);
            this.ArrayValues.Add(item);
            return item;
        }

        /// <summary>
        /// Gets the key from array value.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        private string GetKeyFromArrayValue(TemplateContainer item)
        {
            foreach (KeyValuePair<string, TemplateContainer> kvp in this.ArrayValuesMap)
            {
                if (kvp.Value == item)
                {
                    return kvp.Key;
                }
            }
            return null;
        }

        /// <summary>
        /// Removes the array value.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveArrayValue(int index)
        {
            TemplateContainer item = this.ArrayValues[index];
            this.ArrayValues.RemoveAt(index);
            string key = this.GetKeyFromArrayValue(item);
            if (!string.IsNullOrEmpty(key))
            {
                this.ArrayValuesMap.Remove(key);
            }
        }

        /// <summary>
        /// Gets the array value.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public TemplateContainer GetArrayValue(int index)
        {
            return this.ArrayValues[index];
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            this.Attributes.Clear();
            this.ArrayValues.Clear();
        }

        /// <summary>
        /// Given a path string, pops the next token to be iterated.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        protected string Pop(ref string path)
        {
            int indexDot = path.IndexOf(".");
            int indexC = path.IndexOf("[");
            if (indexDot == -1)
            {
                if (indexC == -1)
                {
                    string result = path;
                    path = "";
                    return result;
                }
                else if (indexC == 0)
                {
                    int indexC2 = path.IndexOf("]");
                    string result = path.Substring(0, indexC2 + 1);
                    path = path.Substring(indexC2 + 1);
                    if (path.StartsWith("."))
                    {
                        path = path.Substring(1);
                    }
                    return result;
                }
                else
                {
                    string result = path.Substring(0, indexC);
                    path = path.Substring(indexC);
                    if (path.StartsWith("."))
                    {
                        path = path.Substring(1);
                    }
                    return result;
                }
            }
            else
            {
                if ((indexC > -1) && (indexC < indexDot))
                {
                    if (indexC == 0)
                    {
                        int indexC2 = path.IndexOf("]");
                        string result = path.Substring(0, indexC2 + 1);
                        path = path.Substring(indexC2 + 1);
                        if (path.StartsWith("."))
                        {
                            path = path.Substring(1);
                        }
                        return result;
                    }
                    else
                    {
                        string result = path.Substring(0, indexC);
                        path = path.Substring(indexC);
                        if (path.StartsWith("."))
                        {
                            path = path.Substring(1);
                        }
                        return result;
                    }
                }
                else
                {
                    string result = path.Substring(0, indexDot);
                    path = path.Substring(indexDot + 1);
                    return result;
                }
            }
        }

        /// <summary>
        /// Gets an object by path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public object GetByPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return this;
            }
            if (path.StartsWith("/"))
            {
                return this.Root.GetByPath(path.Substring(1));
            }
            if (path.StartsWith("../"))
            {
                if (this.Parent == null)
                {
                    return null;
                }
                return this.Parent.GetByPath(path.Substring(3));
            }
            string currentPath = this.Pop(ref path);
            object result = null;
            if (currentPath.StartsWith("["))
            {
                if (!this.IsList)
                {
                    return null;
                }
                string arrayPos = currentPath.Substring(1, currentPath.Length - 2);
                if (arrayPos.StartsWith("*"))
                {
                    arrayPos = arrayPos.Substring(1);
                    int iarrayPos;
                    if (!int.TryParse(arrayPos, out iarrayPos))
                    {
                        return null;
                    }
                    if ((iarrayPos < 0) || (iarrayPos >= this.ArrayValues.Count))
                    {
                        return null;
                    }
                    result = this.ArrayValues[iarrayPos];
                }
                else
                {
                    if (this.ArrayValuesMap.ContainsKey(arrayPos))
                    {
                        result = this.ArrayValuesMap[arrayPos];
                    }
                }
            }
            else
            {
                if (!this.Attributes.ContainsKey(currentPath))
                {
                    return null;
                }
                result = this.Attributes[currentPath];
            }
            while (result is TemplateLink)
            {
                if (string.IsNullOrEmpty(path))
                {
                    return result;
                }
                TemplateLink link = (result as TemplateLink);
                if (link.IsList)
                {
                    if (path.StartsWith("["))
                    {
                        currentPath = this.Pop(ref path);
                        string arrayPos = currentPath.Substring(1, currentPath.Length - 2);
                        string convertedPos = link.ListValues[int.Parse(arrayPos)];
                        object obj = this.GetByPath(link.Link);
                        if (obj is TemplateContainer)
                        {
                            if ((obj as TemplateContainer).IsList)
                            {
                                result = (obj as TemplateContainer).ArrayValuesMap[convertedPos];
                            }
                            else
                            {
                                return null;
                            }
                        }

                    }
                    else
                    {
                        result = null;
                    }
                }
                else
                {
                    result = this.GetByPath((result as TemplateLink).Link);
                }
            }
            if (string.IsNullOrEmpty(path))
            {
                return result;
            }
            if (result is TemplateContainer)
            {
                return (result as TemplateContainer).GetByPath(path);
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
