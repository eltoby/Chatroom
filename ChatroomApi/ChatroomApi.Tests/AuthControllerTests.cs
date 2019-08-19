namespace ChatroomApi.Tests
{
    using ChatroomApi.Controllers;
    using ChatroomApi.Domain;
    using ChatroomApi.Models;
    using ChatroomApi.Service;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class AuthControllerTests
    {
        private AuthController sut;
        private IUserService userService;
        private IOptions<AppSettings> appSettings;

        [TestInitialize]
        public void Setup()
        {
            this.userService = Mock.Of<IUserService>();
            this.appSettings = Mock.Of<IOptions<AppSettings>>();

            this.sut = new AuthController(this.userService, this.appSettings);
        }

        [TestMethod]
        public void LoginFailsIfNoUsernameSpecified()
        {
            var login = new LoginModel
            {
                User = string.Empty
            };

            var result = this.sut.Login(login) as BadRequestObjectResult;

            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("Invalid client request", result.Value);
        }

        [TestMethod]
        public void LoginUnauthorizedIfCredentialsFails()
        {
            var login = new LoginModel() { User = "Pablo", Password = "pass" };
            Mock.Get(this.userService).Setup(x => x.IsValidUser("Pablo", "pass")).Returns(false);

            var result = this.sut.Login(login) as UnauthorizedResult;

            Assert.AreEqual(401, result.StatusCode);
        }

        [TestMethod]
        public void LoginAuthorized()
        {
            var login = new LoginModel() { User = "Pablo", Password = "pass" };
            Mock.Get(this.userService).Setup(x => x.IsValidUser("Pablo", "pass")).Returns(true);
            var fakeAppSettings = new AppSettings
            {
                BaseUrl = "http://localhost"
            };

            Mock.Get(this.appSettings).Setup(x => x.Value).Returns(fakeAppSettings);
            var result = this.sut.Login(login) as OkObjectResult;

            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public void AddExistingUserFails()
        {
            var login = new LoginModel
            {
                User = "Pablo",
                Password = "pass"
            };

            Mock.Get(this.userService).Setup(x => x.AddUser(It.IsAny<User>())).Returns(false);
            var result = this.sut.AddUser(login) as BadRequestObjectResult;

            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("Username already exists", result.Value);
        }

        [TestMethod]
        public void AddUserTest()
        {
            var login = new LoginModel
            {
                User = "Pablo",
                Password = "pass"
            };

            Mock.Get(this.userService).Setup(x => x.AddUser(It.IsAny<User>())).Returns(true);
            Mock.Get(this.userService).Setup(x => x.IsValidUser("Pablo", "pass")).Returns(true);
            var fakeAppSettings = new AppSettings
            {
                BaseUrl = "http://localhost"
            };

            Mock.Get(this.appSettings).Setup(x => x.Value).Returns(fakeAppSettings);

            var result = this.sut.AddUser(login) as OkObjectResult;

            Assert.AreEqual(200, result.StatusCode);
        }
    }
}
