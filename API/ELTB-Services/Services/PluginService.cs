using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Composition.Convention;
using System.IO;
using System.Composition.Hosting;
using System.Runtime.Loader;
using System.Reflection;
using SharedObjects.Esoterica;

namespace ELTB.Services {

    public class PluginService : IPluginService {

        private static CompositionHost Container { get; set; }

        public IEsotericInterpreter InterpreterFor(string language) {
            return Container.GetExports<IEsotericInterpreter>().FirstOrDefault(interp => interp.Language == language);
        }

        public IEnumerable<IEsotericInterpreter> RegisteredInterpreters {
            get {
                return Container.GetExports<IEsotericInterpreter>();
            }
        }

        internal static void Configure() {
            try {
                var conventions = new ConventionBuilder();
                conventions
                    .ForTypesDerivedFrom<IEsotericInterpreter>()
                    .Export<IEsotericInterpreter>()
                    .Shared();
                var binDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                var configuration = new ContainerConfiguration().WithAssembliesInPath(binDirectory, conventions);
                Container = configuration.CreateContainer();
            }
            catch (Exception ex) {
                if (ex is ReflectionTypeLoadException) {
                    var typeLoadException = ex as ReflectionTypeLoadException;
                    var loaderExceptions = typeLoadException.LoaderExceptions;
                }
            }
        }

    }

    public static class ContainerConfigurationExtensions {
        public static ContainerConfiguration WithAssembliesInPath(this ContainerConfiguration configuration, string path, SearchOption searchOption = SearchOption.TopDirectoryOnly) {
            return WithAssembliesInPath(configuration, path, null, searchOption);
        }

        public static ContainerConfiguration WithAssembliesInPath(this ContainerConfiguration configuration, string path, AttributedModelProvider conventions, SearchOption searchOption = SearchOption.TopDirectoryOnly) {
            //var files = Directory.GetFiles(path, "WARP.Language.Standard.dll", searchOption);
            var files = Directory.GetFiles(path, "*.dll", searchOption);
            //var names = files.Select(AssemblyLoadContext.GetAssemblyName);
            var assemblies = files.Select(AssemblyLoadContext.Default.LoadFromAssemblyPath).ToList();
            //var assemblies = names.Select(Assembly.Load).ToArray();

            configuration = configuration.WithAssemblies(assemblies, conventions);

            return configuration;
        }
    }
}
