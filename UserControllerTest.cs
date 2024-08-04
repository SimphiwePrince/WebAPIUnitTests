using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Restul_Web_Assessment.Controllers;
using Restul_Web_Assessment.Interfaces;
using Restul_Web_Assessment.Repository.Models;
using Restul_Web_Assessment.Repository.PostModels;
using System.Collections.Generic;
using Xunit;

namespace Tests
{
    public class UserControllerTests
    {
        private readonly UserController _controller;
        private readonly IUser _mockUser;

        public UserControllerTests()
        {
            _mockUser = Substitute.For<IUser>();
            _controller = new UserController(_mockUser);
        }

        [Fact]
        public void Login_WhenCredentialsAreValid_ReturnsOkObjectResultWithToken()
        {
            // Arrange
            string idNumber = "384829";
            string password = "@passy";
            string token = "valid_token";
            _mockUser.LoginAuthorize(idNumber, password).Returns(token);

            // Act
            var result = _controller.Login(idNumber, password);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedToken = okResult.Value.Should().BeOfType<string>().Subject;
            returnedToken.Should().Be(token);
        }

        [Fact]
        public void Login_WhenCredentialsAreInvalid_ReturnsNotFoundResult()
        {
            // Arrange
            string idNumber = "384829";
            string password = "@passy";
            _mockUser.LoginAuthorize(idNumber, password).Returns(string.Empty);

            // Act
            var result = _controller.Login(idNumber, password);

            // Assert
            var notFoundResult = result.Should().BeOfType<NotFoundObjectResult>().Subject;
            var returnedMessage = notFoundResult.Value.Should().BeOfType<string>().Subject;
            returnedMessage.Should().Be("Invalid login credentials, or user does not exist");
        }

        [Fact]
        public void RegisterUser_WhenUserIsValid_ReturnsOkObjectResult()
        {
            // Arrange
            var user = new UserDTO
            {
                FirstName = "Thando",
                LastName = "Ndlovu",
                Idnumber = "384829",
                ResidentialAddress = "Joburg",
                MobileNumber = "042786",
                EmailAddress = "s@phiwe",
                Password = "@passy"
            };

            var createdUser = new UserModel
            {
                FirstName = "Thando",
                LastName = "Ndlovu",
                DateOfBirth = DateOnly.Parse("2005-04-22"),
                Idnumber = "384829",
                ResidentialAddress = "Joburg",
                MobileNumber = "042786",
                EmailAddress = "s@phiwe",
                Password = "@passy"
            };

            _mockUser.PostUser(user).Returns(createdUser);

            // Act
            var result = _controller.RegisterUser(user);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedUser = okResult.Value.Should().BeOfType<UserModel>().Subject;
            returnedUser.Should().BeEquivalentTo(createdUser);
        }

        [Fact]
        public void RegisterUser_WhenUserIsInvalid_ReturnsBadRequestResult()
        {
            // Arrange
            var user = new UserDTO
            {
                FirstName = "Thando",
                LastName = "Ndlovu",
                Idnumber = "384829",
                ResidentialAddress = "Joburg",
                MobileNumber = "042786",
                EmailAddress = "s@phiwe",
                Password = "@passy"
            };

            _mockUser.PostUser(user).Returns((UserModel)null);

            // Act
            var result = _controller.RegisterUser(user);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
    }
}
