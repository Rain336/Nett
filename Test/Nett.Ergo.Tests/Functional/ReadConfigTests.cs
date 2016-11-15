using FluentAssertions;
using Nett.Coma.Tests.TestData;
using Nett.UnitTests.Util;
using Nett.UnitTests.Util.Scenarios;

namespace Nett.Ergo.Tests.Functional
{
    public sealed class ReadConfigTests
    {
        private const string ReadConfigFunc = "Read Ergo Config";

        [FFact(ReadConfigFunc, "When reading root level setting, correct value is read from file")]
        public void ReadConfig_WhenRootSettingIsRead_CorrectValueIsRead()
        {
            using (var scenario = SingleManagedScenario.Setup(nameof(ReadConfig_WhenRootSettingIsRead_CorrectValueIsRead)))
            {
                // Arrange
                var ep = scenario.Cofiguration.CreateErgoProxy();

                // Act
                var read = ep.IntSetting;

                // Assert
                read.Should().Be(SingleManagedScenario.IntSettingDefaultValue);
            }
        }

        [FFact(ReadConfigFunc, "When reading sub table value, value is read correctly")]
        public void ReadConfig_WhenSubTableValueIsRead_ThatValueIsReadCorrectly()
        {
            using (var scenario = GitScenario.Setup(nameof(ReadConfig_WhenSubTableValueIsRead_ThatValueIsReadCorrectly)))
            {
                // Arrange
                var cfg = scenario.CreateMergedFromDefaults().CreateErgoProxy();

                // Act
                var read = cfg.User.EMail;

                // Assert
                read.Should().Be(GitScenario.DefaultEMail);
            }
        }
    }
}
