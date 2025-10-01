using AppBoot.AssemblyLoad;

namespace AppBoot.UnitTests
{
    public class BreadcrumbNameConventionPathBuilderTests
    {
        private readonly string[] topDirs = ["UI", "Modules", "Infra"];
        private readonly string pluginsDir = "Modules";

        [Fact]
        public void GetPluginFullPath_EntryAssemblyIsDeepAndPluginIsDeep_PathIsBuilt()
        {
            string hostAssemblyLocation = @"someRoot\UI\ConsoleUi\bin\Debug\net10.0\ConsoleUi.exe";
            string plugin = "Notifications.Services";
            string expected = @"someRoot\Modules\Notifications\Notifications.Services\bin\Debug\net10.0\Notifications.Services.dll";

            Test(hostAssemblyLocation, plugin, expected);
        }

        private void Test(string hostAssemblyLocation, string plugin, string expected)
        {
            var builder = new BreadcrumbNameConventionPathBuilder(hostAssemblyLocation, pluginsDir, topDirs);
            string actual = builder.GetPluginFullPath(plugin);
            Assert.Equal(expected, actual);
        }

    }
}