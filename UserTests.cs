using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSubstitute;
using Restul_Web_Assessment.Repository;
using Restul_Web_Assessment.Repository.Models;
using Restul_Web_Assessment.Repository.PostModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using Xunit;
using User = Restul_Web_Assessment.Repository.User;

namespace Tests
{
    public class UserTests
    {
        private readonly User _user;
        private readonly IConfiguration _mockConfig;
        private readonly BankingDbContext _mockDbContext;

        public UserTests()
        {
            _mockConfig = Substitute.For<IConfiguration>();
            _mockConfig["Jwt:Key"].Returns("test_key_12345678901234567890123456789012");
            _mockConfig["Jwt:Issuer"].Returns("test_issuer");
            _mockConfig["Jwt:Audience"].Returns("test_audience");

            _mockDbContext = Substitute.For<BankingDbContext>();
            _user = new User(_mockConfig, _mockDbContext);
        }

        [Fact]
        public void LoginAuthorize_WhenCredentialsAreValid_ReturnsToken()
        {
            // Arrange
            string idNumber = "384829";
            string password = "@passy";
            var mockUser = new UserModel
            {
                Idnumber = idNumber,
                Password = password
            };

            var mockDbSet = CreateMockDbSet(new List<UserModel> { mockUser }.AsQueryable());
            _mockDbContext.Users.Returns(mockDbSet);

            // Act
            var result = _user.LoginAuthorize(idNumber, password);

            // Assert
            result.Should().NotBeEmpty();
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(result) as JwtSecurityToken;
            jsonToken.Should().NotBeNull();
            jsonToken.Claims.First(claim => claim.Type == "IDNumber").Value.Should().Be(idNumber);
        }

        [Fact]
        public void LoginAuthorize_WhenCredentialsAreInvalid_ReturnsEmptyString()
        {
            // Arrange
            string idNumber = "384829";
            string password = "@passy";

            var mockDbSet = CreateMockDbSet(new List<UserModel>().AsQueryable());
            _mockDbContext.Users.Returns(mockDbSet);

            // Act
            var result = _user.LoginAuthorize(idNumber, password);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void PostUser_WhenUserDataIsValid_ReturnsUser()
        {
            // Arrange
            var userData = new UserDTO
            {
                FirstName = "Thando",
                LastName = "Ndlovu",
                Idnumber = "384829",
                ResidentialAddress = "Joburg",
                MobileNumber = "042786",
                EmailAddress = "s@phiwe",
                Password = "@passy"
            };

            var user = new UserModel
            {
                FirstName = "Thando",
                LastName = "Ndlovu",
                Idnumber = "384829",
                ResidentialAddress = "Joburg",
                MobileNumber = "042786",
                EmailAddress = "s@phiwe",
                Password = "@passy"
            };

            var mockDbSet = CreateMockDbSet(new List<UserModel>().AsQueryable());
            _mockDbContext.Users.Returns(mockDbSet);

            // Act
            var result = _user.PostUser(userData);

            // Assert
            result.Should().NotBeNull();
            result.FirstName.Should().Be(userData.FirstName);
        }

        private static DbSet<T> CreateMockDbSet<T>(IQueryable<T> data) where T : class
        {
            var mockSet = Substitute.For<DbSet<T>, IQueryable<T>>();
            ((IQueryable<T>)mockSet).Provider.Returns(data.Provider);
            ((IQueryable<T>)mockSet).Expression.Returns(data.Expression);
            ((IQueryable<T>)mockSet).ElementType.Returns(data.ElementType);
            ((IQueryable<T>)mockSet).GetEnumerator().Returns(data.GetEnumerator());
            return mockSet;
        }
    }
}
