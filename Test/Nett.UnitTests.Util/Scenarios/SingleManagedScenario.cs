using System;
using Nett.Coma;

namespace Nett.UnitTests.Util.Scenarios
{
    public sealed class SingleManagedScenario : IDisposable
    {
        public const int IntSettingDefaultValue = 1;

        public const string DefaultFileContent = @"
IntSetting = 1
";

        public TestFileName File { get; }

        public IConfigSource Source { get; }

        public Config<Config> Cofiguration { get; set; }

        public Config ReadFromDisk() => Toml.ReadFile<Config>(this.File);

        public void Dispose()
        {
            this.File.Dispose();
        }

        public SingleManagedScenario(string testName)
        {
            this.File = TestFileName.Create(testName, "config", Toml.FileExtension);
            this.Source = ConfigSource.CreateFileSource(this.File);
        }

        public static SingleManagedScenario Setup(string testName)
        {
            var scenario = new SingleManagedScenario(testName);

            System.IO.File.WriteAllText(scenario.File, DefaultFileContent);

            scenario.Cofiguration = Coma.Config.Create(() => new Config(), scenario.Source);

            return scenario;
        }

        public class Config
        {
            public virtual int IntSetting { get; set; }
        }
    }
}
