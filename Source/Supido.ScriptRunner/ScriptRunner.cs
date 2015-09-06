using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Supido.ScriptRunner
{
    /// <summary>
    /// Class for compiling C# scripts
    /// </summary>
    public class ScriptRunner
    {
        #region - Fields -

        /// <summary>
        /// The compiler provider for C#
        /// </summary>
        private CSharpCodeProvider compiler;

        /// <summary>
        /// The compiler options.
        /// </summary>
        private CompilerParameters compilerOptions;

        /// <summary>
        /// The compilation errors.
        /// </summary>
        private CompilerErrorCollection compilerErrors;

        /// <summary>
        /// The compiled script.
        /// </summary>
        private Assembly compiledScript;

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the script code.
        /// </summary>
        /// <value>
        /// The script code.
        /// </value>
        public string ScriptCode { get; set; }

        /// <summary>
        /// Gets the compiler options.
        /// </summary>
        /// <value>
        /// The compiler options.
        /// </value>
        public CompilerParameters CompilerOptions
        {
            get { return this.compilerOptions; }
        }

        /// <summary>
        /// Gets the compilation errors.
        /// </summary>
        /// <value>
        /// The compilation errors.
        /// </value>
        public CompilerErrorCollection CompilerErrors
        {
            get { return this.compilerErrors; }
        }

        #endregion

        #region - Constructors -

        /// <summary>
        /// Initializes a new instance of the <see cref="ScripterRunner"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public ScriptRunner(string name)
        {
            this.Name = name;
            InitCompiler();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScripterRunner"/> class.
        /// </summary>
        public ScriptRunner()
            : this(new Guid().ToString())
        {
        }

        #endregion

        #region - Methods -

        /// <summary>
        /// Initialization of the compiler, adding compiler options.
        /// </summary>
        private void InitCompiler()
        {
            Dictionary<string, string> compilerInfo = new Dictionary<string, string>();
            compilerInfo.Add("CompilerVersion", "v4.0");
            compiler = new CSharpCodeProvider(compilerInfo);
            this.compilerOptions = new CompilerParameters();
            this.compilerOptions.GenerateExecutable = false;
            this.compilerOptions.GenerateInMemory = true;
        }

        /// <summary>
        /// Adds an assembly to the referenced assemblies.
        /// </summary>
        /// <param name="name">The name.</param>
        public void AddAssembly(string name)
        {
            name = name.Trim();
            if (!string.IsNullOrEmpty(name))
            {
                if (!this.compilerOptions.ReferencedAssemblies.Contains(name))
                {
                    this.compilerOptions.ReferencedAssemblies.Add(name);
                }
            }
        }

        /// <summary>
        /// Adds an assembly list to the referenced assemblies.
        /// </summary>
        /// <param name="names">The names.</param>
        public void AddAssemblies(string[] names)
        {
            foreach (string name in names)
            {
                this.AddAssembly(name);
            }
        }

        /// <summary>
        /// Adds an assembly list to the referenced assemblies.
        /// </summary>
        /// <param name="names">The names.</param>
        public void AddAssemblies(IList<string> names)
        {
            this.AddAssemblies(names.ToArray());
        }

        /// <summary>
        /// Adds the default assemblies to the referenced assemblies.
        /// </summary>
        public void AddDefaultAssemblies()
        {
            this.compilerOptions.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);
            this.AddAssembly("System.dll");
            this.AddAssembly("System.Core.dll");
            this.AddAssembly("System.Xml.dll");
            this.AddAssembly("System.Data.dll");
        }

        /// <summary>
        /// Compiles the script.
        /// </summary>
        /// <returns></returns>
        public bool Compile()
        {
            CompilerResults result = this.compiler.CompileAssemblyFromSource(this.compilerOptions, this.ScriptCode);
            this.compilerErrors = result.Errors;
            if (result.Errors.HasErrors)
            {
                this.compiledScript = null;
                return false;
            }
            else
            {
                this.compiledScript = result.CompiledAssembly;
                return true;
            }
        }

        /// <summary>
        /// Loads the script from file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void LoadFromFile(string fileName)
        {
            StreamReader reader = new StreamReader(fileName);
            this.ScriptCode = reader.ReadToEnd();
            reader.Close();
        }

        /// <summary>
        /// Gets an interface from the compiled script.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetInterface<T>()
        {
            foreach (Type type in compiledScript.GetExportedTypes())
            {
                foreach (Type iface in type.GetInterfaces())
                {
                    if (iface == typeof(T))
                    {
                        ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                        if ((constructor != null) && (constructor.IsPublic))
                        {
                            T scriptObject = (T)constructor.Invoke(null);
                            return scriptObject;
                        }
                    }
                }
            }
            return default(T);
        }

        /// <summary>
        /// Gets a list of interfaces of the given type from the compiled script.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetInterfaces<T>()
        {
            List<T> list = new List<T>();
            foreach (Type type in compiledScript.GetExportedTypes())
            {
                foreach (Type iface in type.GetInterfaces())
                {
                    if (iface == typeof(T))
                    {
                        ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                        if ((constructor != null) && (constructor.IsPublic))
                        {
                            T scriptObject = (T)constructor.Invoke(null);
                            list.Add(scriptObject);
                        }
                    }
                }
            }
            return list;
        }

        public Type GetType(string name)
        {
            return this.compiledScript.GetType(name);
        }

        public MethodInfo GetMethod(string typeName, string methodName)
        {
            Type type = this.GetType(typeName);
            if (type == null)
            {
                return null;
            }
            return type.GetMethod(methodName);
        }

        #endregion
    }
}
