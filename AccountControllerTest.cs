/*using System.Security.Principal;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NSubstitute;
using Restul_Web_Assessment.Controllers;
using Restul_Web_Assessment.Interfaces;
using Restul_Web_Assessment.Repository.Models;
using Xunit;

namespace Tests
{
    [TestClass]
    public class AccountControllerTest
    {
        private readonly AccountController _controller;
        private readonly IRun _mockAccount;
        public AccountControllerTest()
        {
            _mockAccount = Substitute.For<IRun>();
            _controller = new AccountController(_mockAccount);
            
        }

        [Fact]
        public void GetAccount_WhenAccountNumberValid_ReturnOkObjectResultWithAccount()
        {
            //Arrange
            int accountNumber = 1;
            var accountMock = new Account
            {
                AccountNumber = accountNumber,
                AccountHolder = "Simphiwe",
                AccountType = "Savings",
                IsActive = "True",
                Balance = 100,
                DateModified = DateTime.UtcNow,
                UserId = 3
            };
            _mockAccount.GetAccount(Arg.Any<int>()).Returns(accountMock);

            //Act
            var result = _controller.GetAccount(accountNumber);

            //Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedAccount = okResult.Value.Should().BeOfType<Account>().Subject;
            returnedAccount.AccountNumber.Should().Be(accountNumber);
        }
    }
}*/

using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Restul_Web_Assessment.Controllers;
using Restul_Web_Assessment.Interfaces;
using Restul_Web_Assessment.Repository.Models;
using Restul_Web_Assessment.Repository.PostModels;
using System;
using Xunit;

namespace Tests
{
    public class AccountControllerTest
    {
        private readonly AccountController _controller;
        private readonly IAccount _mockAccount;

        public AccountControllerTest()
        {
            _mockAccount = Substitute.For<IAccount>();
            _controller = new AccountController(_mockAccount);
        }

        [Fact]
        public void GetAccount_WhenAccountNumberValid_ReturnOkObjectResultWithAccount()
        {
            // Arrange
            int accountNumber = 1;
            var accountMock = new Account
            {
                AccountNumber = accountNumber,
                AccountHolder = "Simphiwe",
                AccountType = "Savings",
                IsActive = "True",
                Balance = 100,
                DateModified = DateTime.UtcNow,
                UserId = 3
            };
            _mockAccount.GetAccount(Arg.Any<int>()).Returns(accountMock);

            // Act
            var result = _controller.GetAccount(accountNumber);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedAccount = okResult.Value.Should().BeOfType<Account>().Subject;
            returnedAccount.AccountNumber.Should().Be(accountNumber);
        }

        [Fact]
        public void GetAccount_WhenAccountNumberInvalid_ReturnNotFoundResult()
        {
            // Arrange
            int accountNumber = 1;
            _mockAccount.GetAccount(Arg.Any<int>()).Returns((Account)null);

            // Act
            var result = _controller.GetAccount(accountNumber);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetUserAccounts_WhenUserIDIsValid_ReturnsOkObjectResultWithAccounts()
        {
            // Arrange
            int userID = 3;
            var accounts = new List<Account>
            {
                new Account
                {
                    AccountNumber = 5,
                    AccountHolder = "Simphiwe",
                    AccountType = "Savings",
                    IsActive = "True",
                    Balance = 100,
                    DateModified = DateTime.UtcNow,
                    UserId = 3
                }
            };
            _mockAccount.GetUserAccounts(userID).Returns(accounts);

            // Act
            var result = _controller.GetUserAccounts(userID);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedAccounts = okResult.Value.Should().BeOfType<List<Account>>().Subject;
            returnedAccounts.Should().BeEquivalentTo(accounts);
        }

        [Fact]
        public void GetUserAccounts_WhenUserIDIsInvalid_ReturnsNotFoundResult()
        {
            // Arrange
            int userID = 3;
            _mockAccount.GetUserAccounts(userID).Returns(new List<Account>());

            // Act
            var result = _controller.GetUserAccounts(userID);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }


        [Fact]
        public void PostAccount_WhenAccountIsValid_ReturnOkObjectResult()
        {
            // Arrange
            var accountPost = new AccountDTO
            {
                AccountHolder = "Simphiwe",
                AccountType = "Savings",
                UserId = 3
            };
            var accountMock = new Account
            {
                AccountNumber = 1,
                AccountHolder = "Simphiwe",
                AccountType = "Savings",
                IsActive = "True",
                Balance = 100,
                DateModified = DateTime.UtcNow,
                UserId = 3
            };
            _mockAccount.PostAccount(Arg.Any<AccountDTO>()).Returns(accountMock);

            // Act
            var result = _controller.PostAccount(accountPost);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedAccount = okResult.Value.Should().BeOfType<Account>().Subject;
            returnedAccount.AccountHolder.Should().Be(accountPost.AccountHolder);
        }

        [Fact]
        public void PostAccount_WhenAccountIsInvalid_ReturnBadRequestResult()
        {
            // Arrange
            var accountPost = new AccountDTO
            {
                AccountHolder = "Simphiwe",
                AccountType = "Savings",
                UserId = 3
            };
            _mockAccount.PostAccount(Arg.Any<AccountDTO>()).Returns((Account)null);

            // Act
            var result = _controller.PostAccount(accountPost);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void Withdraw_WhenWithdrawalIsValid_ReturnOkResult()
        {
            // Arrange
            int accountNumber = 1;
            int withdrawalAmount = 50;
            _mockAccount.Withdraw(Arg.Any<int>(), Arg.Any<int>()).Returns(true);

            // Act
            var result = _controller.Withdraw(accountNumber, withdrawalAmount);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public void Withdraw_WhenWithdrawalIsInvalid_ReturnBadRequestResult()
        {
            // Arrange
            int accountNumber = 1;
            int withdrawalAmount = 50;
            _mockAccount.Withdraw(Arg.Any<int>(), Arg.Any<int>()).Returns(false);

            // Act
            var result = _controller.Withdraw(accountNumber, withdrawalAmount);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void Deposit_WhenDepositIsValid_ReturnOkResult()
        {
            // Arrange
            int accountNumber = 1;
            int depositAmount = 50;
            _mockAccount.Deposit(Arg.Any<int>(), Arg.Any<int>()).Returns(true);

            // Act
            var result = _controller.Deposit(accountNumber, depositAmount);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public void Deposit_WhenDepositIsInvalid_ReturnBadRequestResult()
        {
            // Arrange
            int accountNumber = 1;
            int depositAmount = 50;
            _mockAccount.Deposit(Arg.Any<int>(), Arg.Any<int>()).Returns(false);

            // Act
            var result = _controller.Deposit(accountNumber, depositAmount);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }
    }
}
