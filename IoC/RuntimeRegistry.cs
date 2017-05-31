using Link.Data.File;
using Link.Data.Rest;
using Link.Domain.Contracts;
using StructureMap;
using System.Diagnostics;

namespace LogisticBot.IoC
{
    public class RuntimeRegistry : Registry
    {
        public RuntimeRegistry()
        {
            Scan(x => {
                x.AssembliesAndExecutablesFromApplicationBaseDirectory();
                x.WithDefaultConventions();
            });
            For<ISettingsReader>().Singleton().Use<SettingsReader>();
            For<ILogger>().Use<Logger>();

        }
    }
}