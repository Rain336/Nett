using FluentAssertions;
using Nett.Coma.Tests.TestData;
using Xunit;

namespace Nett.Ergo.Tests.Functional
{
    public class CreateProxyTests
    {
        [Fact]
        public void CreateProxy_ReturnsAProxy()
        {
            using (var scenario = GitScenario.Setup(nameof(CreateProxy_ReturnsAProxy)))
            {
                // Arrange
                var cfg = scenario.CreateMergedFromDefaults();

                // Act
                GitScenario.GitConfig proxy = cfg.CreateErgoProxy();

                // Assert
                cfg.Should().NotBeNull();
            }
        }

        [Fact]
        public void CreatedProxy_EMail_ReturnsCorrectEMail()
        {
            using (var scenario = GitScenario.Setup(nameof(CreatedProxy_EMail_ReturnsCorrectEMail)))
            {
                // Arrange
                var cfg = scenario.CreateMergedFromDefaults();

                // Act
                GitScenario.GitConfig proxy = cfg.CreateErgoProxy();

                // Assert
                proxy.User.EMail.Should().Be(GitScenario.DefaultEMail);
            }
        }
    }
}
