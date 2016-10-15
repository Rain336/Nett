using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Nett.Coma.Tests.TestData;
using NSubstitute;
using Xunit;

namespace Nett.Ergo.Tests.Functional
{
    public class CreateProxyTests
    {
        [Fact]
        public void A()
        {
            using (var scenario = GitScenario.Setup(nameof(A)))
            {
                // Arrange
                var cfg = scenario.CreateMergedFromDefaults();

                // Act
                GitScenario.GitConfig proxy = cfg.CreateErgoProxy();

                // Assert
                cfg.Should().NotBeNull();
            }
        }
    }
}
