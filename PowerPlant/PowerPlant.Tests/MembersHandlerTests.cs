using Moq;
using NUnit.Framework;
using PowerPlant.Domain;
using PowerPlant.Domain.Models;
using System.Collections.Generic;

namespace PowerPlant.Tests
{
    [TestFixture]
    public class MembersHandlerTests
    {
        private Mock<ICliHelper> _cliHelperMock;
        private Mock<IMembersService> _membersServiceMock;
        private Mock<IConsoleManager> _consoleManagerMock;

        private MembersHandler _sut;

        private Member _loggedMember;

        [SetUp]
        public void Setup()
        {
            _cliHelperMock = new Mock<ICliHelper>();
            _membersServiceMock = new Mock<IMembersService>();
            _consoleManagerMock = new Mock<IConsoleManager>();

            _sut = new MembersHandler(
                _cliHelperMock.Object,
                _membersServiceMock.Object,
                _consoleManagerMock.Object);

            _loggedMember = new Member { Id = 1, Login = "admin", Password = "admin", Function = MemberFunction.Admin };
        }

        [Test]
        public void CreateNewMember_UserRequestedAction_RefusalMessageWrittenCorrectly()
        {
            //Arrange
            _loggedMember.Function = MemberFunction.User;

            //Act
            _sut.CreateNew(_loggedMember);

            //Assert
            _membersServiceMock
                .Verify(x => x.CreateAsync(It.IsAny<Member>()).Result, Times.Never);

            _consoleManagerMock
                .Verify(x => x.WriteLine("(!) Only for admins"), Times.Once);

        }

        [Test]
        public void CreateNewMember_AdminRequestedAction_ActionCalledCorrectly()
        {
            //Arrange
            _cliHelperMock
                .Setup(x => x.GetString("Login"))
                .Returns("user");

            _cliHelperMock
                .Setup(x => x.GetString("Password"))
                .Returns("user");

            _cliHelperMock
                .Setup(x => x.GetMemberFunction())
                .Returns(MemberFunction.User);

            _membersServiceMock
                .Setup(x => x.CreateAsync(It.IsAny<Member>()).Result)
                .Returns(true);

            //Act
            _sut.CreateNew(_loggedMember);

            //Assert
            _membersServiceMock
                .Verify(x => x.CreateAsync(It.IsAny<Member>()).Result, Times.Once);

            _consoleManagerMock
                .Verify(x => x.WriteLine("Member created successfully"));
        }

        [Test]
        public void DeleteMember_UserRequestedAction_RefusalMessageWrittenCorrectly()
        {
            //Arrange
            _loggedMember.Function = MemberFunction.User;

            //Act
            _sut.Delete(_loggedMember);

            //Assert
            _membersServiceMock
                .Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Never);

            _consoleManagerMock
                .Verify(x => x.WriteLine("(!) Only for admins"), Times.Once);
        }

        [Test]
        public void DeleteMember_AdminProvidedWrongPassword_RefusalMessageWrittenCorrectly()
        {
            //Arrange
            _cliHelperMock
                .Setup(x => x.GetString("Password"))
                .Returns("dupa");

            //Act
            _sut.Delete(_loggedMember);

            //Assert
            _membersServiceMock
                .Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Never);

            _consoleManagerMock
                .Verify(x => x.WriteLine("(!) Wrong password. \nRedirecting to menu..."), Times.Once);
        }

        [Test]
        public void DeleteMember_AdminProvidedWrongIdToDelete_RefusalMessageWrittenCorrectly()
        {
            //Arrange
            _cliHelperMock
                .Setup(x => x.GetString("Password"))
                .Returns("admin");

            _membersServiceMock
                .Setup(x => x.GetAllAsync().Result)
                .Returns(new Dictionary<int, Member>
                {
                    { 2, new Member { Id = 2, Login = "user", Password = "user", Function = MemberFunction.User } }
                });

            _cliHelperMock
                .Setup(x => x.GetInt("ID"))
                .Returns(1);

            _membersServiceMock
                .Setup(x => x.DeleteAsync(It.IsAny<int>()).Result)
                .Returns(false);

            //Act
            _sut.Delete(_loggedMember);

            //Assert
            _membersServiceMock
                .Verify(x => x.DeleteAsync(1), Times.Never);

            _consoleManagerMock
                .Verify(x => x.WriteLine("Pick member to delete from database:\n"), Times.Once);

            _consoleManagerMock
                .Verify(x => x.WriteLine("(!) There is no member under given id [1], or it's yours. \nRedirecting to menu..."), Times.Once);
        }

        [Test]
        public void DeleteMember_AdminRequestedAction_ActionCalledCorrectly()
        {
            //Arrange
            _cliHelperMock
                .Setup(x => x.GetString("Password"))
                .Returns("admin");

            _membersServiceMock
                .Setup(x => x.GetAllAsync().Result)
                .Returns(new Dictionary<int, Member>
                {
                    { 2, new Member { Id = 2, Login = "user", Password = "user", Function = MemberFunction.User } }
                });

            _cliHelperMock
                .Setup(x => x.GetInt("ID"))
                .Returns(2);

            _membersServiceMock
                .Setup(x => x.DeleteAsync(It.IsAny<int>()).Result)
                .Returns(true);

            //Act
            _sut.Delete(_loggedMember);

            //Assert
            _membersServiceMock
                .Verify(x => x.DeleteAsync(2), Times.Once);

            _consoleManagerMock
                .Verify(x => x.WriteLine("Pick member to delete from database:\n"), Times.Once);

            _consoleManagerMock
                .Verify(x => x.WriteLine("Member deleted successfully"), Times.Once);
        }
    }
}