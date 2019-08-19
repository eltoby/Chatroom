namespace ChatroomApi.Tests
{
    using System.Linq;
    using ChatroomApi.Controllers;
    using ChatroomApi.Domain;
    using ChatroomApi.Service;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class MessagesControllerTests
    {
        private MessagesController sut;
        private IHubContext<ChatHub> chatHub;
        private IMessageService messageService;

        [TestInitialize]
        public void Setup()
        {
            this.chatHub = Mock.Of<IHubContext<ChatHub>>();
            this.messageService = Mock.Of<IMessageService>();
            this.sut = new MessagesController(this.chatHub, this.messageService);
        }

        [TestMethod]
        public void GetLastMessagesTest()
        {
            var messages = new[]
            {
                new Message { Nick = "Pablo", Text = "Message 1", Timestamp = 1},
                new Message { Nick = "Cris", Text = "Message 2", Timestamp = 2}
            };

            Mock.Get(this.messageService).Setup(x => x.GetLastMessages()).Returns(messages);

            var result = this.sut.GetLastMessages();

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Pablo", result.First().Nick);
            Assert.AreEqual("Message 2", result.Last().Message);
        }
    }
}
