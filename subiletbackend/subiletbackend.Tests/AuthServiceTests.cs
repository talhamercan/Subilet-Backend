using SubiletBackend.Infrastructure;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace subiletbackend.Tests
{
    public class AuthServiceTests
    {
        [Fact]
        public void HashPassword_And_VerifyPassword_Works()
        {
            var config = new ConfigurationBuilder().AddInMemoryCollection().Build();
            var service = new AuthService(config);
            var password = "test123";
            var hash = service.HashPassword(password);
            Assert.True(service.VerifyPassword(hash, password));
            Assert.False(service.VerifyPassword(hash, "wrongpass"));
        }
    }
} 