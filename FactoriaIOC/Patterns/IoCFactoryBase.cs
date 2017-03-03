using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;
using FactoriaIOC.Trace;

namespace FactoriaIOC.Patterns
{
    /*/// <summary>
    /// Inversion of Control factory implementation.
    /// This is a simple factory build with Microsoft Unity
    /// </summary>*/
    /// <summary>
    /// <para>Implementación de una factoría de Inversión de Control (IoC)</para>
    /// <para>Esta es una factoría sencilla construida utilizando Microsoft Unity</para>
    /// </summary>
    public abstract class IoCFactoryBase
    {
        #region Members

        /// <summary>
        /// Objeto utilizado para bloqueos por multithreading
        /// </summary>
        private static object _lockObj = new object();

        /// <summary>
        /// Indica si se ha iniciado ya la factoría IoC
        /// </summary>
        private static bool _configurado = false;

        /// <summary>
        /// Sección de configuración de Unity en el *.config correspondiente
        /// </summary>
        private static UnityConfigurationSection _unityConfigSection;

        /// <summary>
        /// Diccionario cuyos elementos son los pares {[Nombre contenedor], [Contenedor de Unity]}
        /// </summary>
        private static IDictionary<string, IUnityContainer> _containersDictionary;

        /// <summary>
        /// Diccionario cuyos elementos son los pares {[Nombre contenedor], [Contenedor de Unity]}
        /// </summary>
        public static IDictionary<string, IUnityContainer> ContainersDictionary
        {
            get
            {
                return IoCFactoryBase._containersDictionary;
            }
            set
            {
                lock (_lockObj)
                {
                    IoCFactoryBase._containersDictionary = value;
                }
            }
        }

        #endregion Members



        #region Métodos privados

        /*/// <summary>
        /// wires up the ioc containers
        /// <remarks>
        /// This static constructor remove 'beforeFieldInit' IL annotation
        /// </remarks>
        /// </summary>*/
        /// <summary>
        /// <para>Inicializa y configura los contenedores de IoC</para>
        /// <para><remarks>Se desactiva la anotación de IL de "beforeFieldInit" para este método</remarks></para>
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        private static void InitializeAndConfigureContainers()
        {
            // Get Unity Configuration Section

            IoCFactoryBase._unityConfigSection = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");


            // Main Containers

            // Root Container for general objetcs                        
            IoCFactoryBase.ContainersDictionary = new Dictionary<string, IUnityContainer>();

            IUnityContainer rootContainer = new UnityContainer();
            IoCFactoryBase.ContainersDictionary.Add("RootContext", rootContainer);

            // Configure Container
            /* IoCFactoryBase._unityConfigSection.Containers["RootContext"].Configure(rootContainer); // Obsolete. Replaced by: */
            IoCFactoryBase._unityConfigSection.Configure(rootContainer, "RootContext");

            // Application container for real app's objects
            // His parent is RootContainer
            IUnityContainer realAppContainer = rootContainer.CreateChildContainer();
            IoCFactoryBase.ContainersDictionary.Add("RealAppContext", realAppContainer);

            // Configure Container
            /* IoCFactoryBase._unityConfigSection.Containers["RealAppContext"].Configure(realAppContainer); // Obsolete. Replaced by: */
            IoCFactoryBase._unityConfigSection.Configure(realAppContainer, "RealAppContext");


            // Application container for real app's objects
            // His parent is RootContainer
            IUnityContainer fakeAppContainer = rootContainer.CreateChildContainer();
            IoCFactoryBase.ContainersDictionary.Add("FakeAppContext", fakeAppContainer);

            // Configure Container
            /* IoCFactoryBase._unityConfigSection.Containers["FakeAppContext"].Configure(fakeAppContainer); // Obsolete. Replaced by: */
            IoCFactoryBase._unityConfigSection.Configure(fakeAppContainer, "FakeAppContext");

            // (CDLTLL) Dynamically create all child containers (Stubbing Containers, etc.)
            // All are childs from RealAppContainer

            for (int i = 0; i < _unityConfigSection.Containers.Count; i++)
            {
                ContainerElement containerElement = IoCFactoryBase._unityConfigSection.Containers[i];
                String containerElementName = containerElement.Name;

                if ((containerElementName != "RootContext")
                 && (containerElementName != "RealAppContext")
                 && (containerElementName != "FakeAppContext"))
                {
                    //Create Container
                    IUnityContainer childContainer = realAppContainer.CreateChildContainer();
                    IoCFactoryBase.ContainersDictionary.Add(containerElementName, childContainer);

                    //Configure Container
                    /* IoCFactoryBase._unityConfigSection.Containers[i].Configure(childContainer); // Obsolete. Replaced by: */
                    IoCFactoryBase._unityConfigSection.Configure(childContainer, containerElementName);
                }
            }


            // CONFIGURE CONTAINERS
            IoCFactoryBase.ConfigureRootContainer(rootContainer);
            IoCFactoryBase.ConfigureRealContainer(realAppContainer);
            IoCFactoryBase.ConfigureFakeContainer(fakeAppContainer);


            lock (IoCFactoryBase._lockObj) { IoCFactoryBase._configurado = true; }
        }



        /*/// <summary>
        /// Configure root container.Register types and life time managers for unity builder process
        /// </summary>
        /// <param name="container">Container to configure</param>*/
        /// <summary>
        /// <para>Configura el contenedor raíz (root container)</para>
        /// <para>Registra los tipos junto a sus gestores de tiempo de vida para los procesos de construcción de Unity</para>
        /// </summary>
        /// <param name="container">Contenedor a configurar</param>
        private static void ConfigureRootContainer(IUnityContainer container)
        {
            /*
            ENG:
            Take into account that Types and Mappings registration could be also done using the UNITY XML configuration
            But we prefer doing it here (C# code) because we'll catch errors at compiling time instead execution time, if any type has been written wrong.
            */
            /*
            ESP:
            Los tipos y mapeos también se pueden registrar utilizando la sección de configuración XML de UNITY (Unity.config)
            La ventaja de definirlos en código es capturar los errores durante la compilación en lugar de durante el tiempo de ejecución, por ejemplo
            si se ha escrito mal un tipo; la desventaja es que se hace necesario recompilar en caso de modificar alguna asociación.
            */


            //Register crosscuting mappings
            container.RegisterType<ITraceManager, TraceManager>(new TransientLifetimeManager());
        }



        /*/// <summary>
        /// Configure real container. Register types and life time managers for unity builder process
        /// </summary>
        /// <param name="container">Container to configure</param>*/
        /// <summary>
        /// <para>Configura el contenedor real (real container)</para>
        /// <para>Registra los tipos junto a sus gestores de tiempo de vida para los procesos de construcción de Unity</para>
        /// </summary>
        /// <param name="container">Contenedor a configurar</param>
        private static void ConfigureRealContainer(IUnityContainer container)
        {
            //container.RegisterType<IMainModuleContext, MainModuleContext>(new PerExecutionContextLifetimeManager(Guid.NewGuid()));
            container.RegisterType<ITraceManager, TraceManager>(new TransientLifetimeManager());
        }



        /*/// <summary>
        /// Configure fake container.Register types and life time managers for unity builder process
        /// </summary>
        /// <param name="container">Container to configure</param>*/
        /// <summary>
        /// <para>Configura el contenedor falso (fake container)</para>
        /// <para>Registra los tipos junto a sus gestores de tiempo de vida para los procesos de construcción de Unity</para>
        /// </summary>
        /// <param name="container">Contenedor a configurar</param>
        private static void ConfigureFakeContainer(IUnityContainer container)
        {
            //Note: Generic register type method cannot be used here, 
            //MainModuleFakeContext cannot have implicit conversion to IMainModuleContext

            //container.RegisterType(typeof(IMainModuleContext), typeof(MainModuleFakeContext), new PerExecutionContextLifetimeManager(Guid.NewGuid()));
            container.RegisterType<ITraceManager, TraceManager>(new TransientLifetimeManager());
        }

        #endregion Métodos privados



        #region Public Methods

        /// <summary>
        /// Inicializa y configura los contenedores de IoC (Inversión de control)
        /// </summary>        
        public static void Initialize()
        {
            IoCFactoryBase.InitializeAndConfigureContainers();
        }



        /*/// <summary>
        /// Returns an injected implementation for the requested interface
        /// It uses default IoC Container defined in AppSettings
        /// </summary>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        /// defaultIocContainer AppSetting key not found
        /// </exception>*/
        /// <summary>
        /// <para>Retorna una implementación para la interfaz solicitada</para>
        /// <para>La implementación puede contener adicionalmente código inyectado</para>
        /// <para>Se utiliza el contenedor de IoC por defecto definido en los AppSettings del *.config</para>
        /// </summary>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        /// No se ha encontrado la clave "defaultIocContainer" en los AppSettings
        /// </exception>
        /// <typeparam name="T">Interfaz de la cual se solicita una implementación</typeparam>
        public static T Resolve<T>()
        {
            //We use the default container specified in AppSettings
            string containerName = ConfigurationManager.AppSettings["defaultIoCContainer"];

            if (String.IsNullOrEmpty(containerName) ||
                String.IsNullOrWhiteSpace(containerName))
            {
                throw new ArgumentNullException("No containerName defined");
            }

           return IoCFactoryBase.Resolve<T>(containerName);

        }



        /// <summary>
        /// <para>Retorna una implementación para la interfaz solicitada</para>
        /// <para>La implementación puede contener adicionalmente código inyectado</para>
        /// <para>Se utiliza el contenedor de IoC por defecto definido en los AppSettings del *.config</para>
        /// </summary>
        /// <param name="overrides">Parámetros adicionales para la construcción del objeto que implemente la interfaz</param>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">
        /// No se ha encontrado la clave "defaultIocContainer" en los AppSettings
        /// </exception>
        /// <typeparam name="T">Interfaz de la cual se solicita una implementación</typeparam>
        public static T Resolve<T>(params ParameterOverride[] overrides)
        {

            //We use the default container specified in AppSettings
            string containerName = ConfigurationManager.AppSettings["defaultIoCContainer"];

            if (String.IsNullOrEmpty(containerName) ||
                String.IsNullOrWhiteSpace(containerName))
            {
                throw new ArgumentNullException("No containerName defined");
            }

            return IoCFactoryBase.Resolve<T>(containerName, overrides);

        }



        /*/// <summary>
        /// Returns an injected implementation for the requested interface
        /// It uses provided IoC Container passed as parameter
        /// </summary>*/
        /// <summary>
        /// <para>Retorna una implementación para la interfaz solicitada</para>
        /// <para>La implementación puede contener adicionalmente código inyectado</para>
        /// <para>Se utiliza el contenedor de IoC indicado</para>
        /// </summary>
        /// <param name="containerName">Nombre del contenedor de IoC a utilizar</param>
        /// <typeparam name="T">Interfaz de la cual se solicita una implementación</typeparam>
        public static T Resolve<T>(string containerName)
        {

            //check preconditions
            if (String.IsNullOrEmpty(containerName)
               ||
               String.IsNullOrWhiteSpace(containerName))
            {
                throw new ArgumentNullException("No containerName defined");
            }

            if (!IoCFactoryBase._configurado)
                IoCFactoryBase.Initialize();

            if (!ContainersDictionary.ContainsKey(containerName))
                throw new InvalidOperationException(String.Format("container '{0}' not found", containerName));

            IUnityContainer container = IoCFactoryBase.ContainersDictionary[containerName];

            return container.Resolve<T>();

        }



        /// <summary>
        /// <para>Retorna una implementación para la interfaz solicitada</para>
        /// <para>La implementación puede contener adicionalmente código inyectado</para>
        /// <para>Se utiliza el contenedor de IoC indicado</para>
        /// </summary>
        /// <param name="containerName">Nombre del contenedor de IoC a utilizar</param>
        /// <param name="overrides">Parámetros adicionales para la construcción del objeto que implemente la interfaz</param>
        /// <typeparam name="T">Interfaz de la cual se solicita una implementación</typeparam>
        public static T Resolve<T>(string containerName, params ParameterOverride[] overrides)
        {

            //check preconditions
            if (String.IsNullOrEmpty(containerName)
               ||
               String.IsNullOrWhiteSpace(containerName))
            {
                throw new ArgumentNullException("No containerName defined");
            }

            if (!IoCFactoryBase._configurado)
                IoCFactoryBase.Initialize();

            if (!ContainersDictionary.ContainsKey(containerName))
                throw new InvalidOperationException(String.Format("container '{0}' not found", containerName));

            IUnityContainer container = IoCFactoryBase.ContainersDictionary[containerName];

            return container.Resolve<T>(overrides);

        }

        #endregion Métodos públicos
    }
}
