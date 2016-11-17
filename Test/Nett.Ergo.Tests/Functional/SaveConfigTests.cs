using FluentAssertions;
using Nett.Coma.Tests.TestData;
using Nett.UnitTests.Util;
using Nett.UnitTests.Util.Scenarios;
using System.Linq;

namespace Nett.Ergo.Tests.Functional
{
    public class SaveConfigTests
    {
        private const string Func = "Save Ergo config";
        private const string FuncSaveSetting = "Save setting";

        [FFact(Func, "When saved on root level, that setting will get saved")]
        public void Save_WhenIsSettingOnRootLevel_ThatSettingGetsSaved()
        {
            using (var scenario = SingleManagedScenario.Setup(nameof(Save_WhenIsSettingOnRootLevel_ThatSettingGetsSaved)))
            {
                // Arrange
                const int newValue = SingleManagedScenario.IntSettingDefaultValue + 1;
                var ep = scenario.Cofiguration.CreateErgoProxy();

                // Act
                ep.IntSetting = newValue;

                // Assert
                scenario.ReadFromDisk().IntSetting.Should().Be(newValue);
            }
        }

        [FFact(Func, "When saved without scope gets saved in originator scope")]
        public void Save_WhenSavedWithoutScope_GetsSavedInOriginiatorScope()
        {
            using (var scenario = GitScenario.Setup(nameof(Save_WhenSavedWithoutScope_GetsSavedInOriginiatorScope)))
            {
                // Arrange
                var coma = scenario.CreateMergedFromDefaults();
                var ergo = coma.CreateErgoProxy();
                var updated = !coma.Get(c => c.Core.AutoClrf);

                // Act
                ergo.Core.AutoClrf = updated;

                // Assert
                var onDisk = coma.Get(c => c.Core.AutoClrf);
                onDisk.Should().Be(updated);
            }
        }

        [FFact(FuncSaveSetting, "Saves it in the correct file.")]
        public void Save_WhenScopeIsSpecified_SettingIsSavedToThatScope()
        {
            using (var scenario = GitScenario.Setup(nameof(Save_WhenScopeIsSpecified_SettingIsSavedToThatScope)))
            {
                // Arrange
                var cfg = scenario.CreateMergedFromDefaults().CreateErgoProxy();

                // Act
                using (scenario.SystemFileSource.MakeCurrent())
                {
                    cfg.User.EMail = "new@email.at";
                }

                // Assert
                var tbl = Toml.ReadFile(scenario.SystemFile);
                tbl.Rows.Count().Should().Be(2);
            }
        }
    }
}
