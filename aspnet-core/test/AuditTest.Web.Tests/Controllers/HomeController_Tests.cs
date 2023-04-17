using System.Threading.Tasks;
using AuditTest.Models.TokenAuth;
using AuditTest.Web.Controllers;
using Shouldly;
using Xunit;

namespace AuditTest.Web.Tests.Controllers
{
    public class HomeController_Tests: AuditTestWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}