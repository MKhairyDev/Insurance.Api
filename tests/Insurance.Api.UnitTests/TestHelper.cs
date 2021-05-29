using Microsoft.Extensions.Configuration;

namespace Insurance.Api.UnitTests
{
    public static class TestHelper
    {
        public static IConfigurationRoot BuildConfiguration(string testDirectory)
        {
            return new ConfigurationBuilder()
                .SetBasePath(basePath: testDirectory)
                .AddJsonFile("appsettings.json", optional: true)
                .Build();
        }
    }
}