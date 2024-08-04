using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Restul_Web_Assessment.Repository;
using Restul_Web_Assessment.Repository.Models;
using Restul_Web_Assessment.Repository.PostModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Tests
{
    public class AccountTests
    {
        private readonly Restul_Web_Assessment.Repository.Account _run;
        private readonly BankingDbContext _mockDbContext;

        public AccountTests()
        {
            _mockDbContext = Substitute.For<BankingDbContext>();
            _run = new Restul_Web_Assessment.Repository.Account(_mockDbContext);
        }

        [Fact]
        public void GetAccount_WhenAccountNumberValid_ReturnsAccount()
        {
            // Arrange
            int accountNumber = 1;
            var accountMock = new Restul_Web_Assessment.Repository.Models.Account
            {
                AccountNumber = accountNumber,
                AccountHolder = "Simphiwe",
                AccountType = "Savings",
                IsActive = "True",
                Balance = 100,
                DateModified = DateTime.UtcNow,
                UserId = 3
            };

            var mockDbSet = CreateMockDbSet(new List<Restul_Web_Assessment.Repository.Models.Account> { accountMock }.AsQueryable());
            _mockDbContext.Accounts.Returns(mockDbSet);

            // Act
            var result = _run.GetAccount(accountNumber);

            // Assert
            result.Should().NotBeNull();
            result.AccountNumber.Should().Be(accountNumber);
        }

        [Fact]
        public void GetAccount_WhenAccountNumberInvalid_ReturnsNull()
        {
            // Arrange
            int accountNumber = 1;
            var mockDbSet = CreateMockDbSet(new List<Restul_Web_Assessment.Repository.Models.Account>().AsQueryable());
            _mockDbContext.Accounts.Returns(mockDbSet);

            // Act
            var result = _run.GetAccount(accountNumber);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void PostAccount_WhenAccountInfoValid_ReturnsAccount()
        {
            // Arrange
            var accountPost = new AccountDTO
            {
                AccountHolder = "Simphiwe",
                AccountType = "Savings",
                UserId = 3
            };

            var mockDbSet = CreateMockDbSet(new List<Restul_Web_Assessment.Repository.Models.Account>().AsQueryable());
            _mockDbContext.Accounts.Returns(mockDbSet);

            // Act
            var result = _run.PostAccount(accountPost);

            // Assert
            result.Should().NotBeNull();
            result.AccountHolder.Should().Be(accountPost.AccountHolder);
        }
        
        [Fact]
        public void Withdraw_WhenWithdrawalValid_ReturnsTrue()
        {
            // Arrange
            int accountNumber = 1;
            int withdrawalAmount = 50;
            var accountMock = new Restul_Web_Assessment.Repository.Models.Account
            {
                AccountNumber = accountNumber,
                AccountHolder = "Simphiwe",
                AccountType = "Savings",
                IsActive = "True",
                Balance = 100,
                DateModified = DateTime.UtcNow,
                UserId = 3
            };

            var mockDbSet = CreateMockDbSet(new List<Restul_Web_Assessment.Repository.Models.Account> { accountMock }.AsQueryable());
            _mockDbContext.Accounts.Returns(mockDbSet);

            // Act
            var result = _run.Withdraw(accountNumber, withdrawalAmount);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Withdraw_WhenWithdrawalInvalid_ReturnsFalse()
        {
            // Arrange
            int accountNumber = 1;
            int withdrawalAmount = 50;
            var mockDbSet = CreateMockDbSet(new List<Restul_Web_Assessment.Repository.Models.Account>().AsQueryable());
            _mockDbContext.Accounts.Returns(mockDbSet);

            // Act
            var result = _run.Withdraw(accountNumber, withdrawalAmount);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Deposit_WhenDepositValid_ReturnsTrue()
        {
            // Arrange
            int accountNumber = 1;
            int depositAmount = 50;
            var accountMock = new Restul_Web_Assessment.Repository.Models.Account
            {
                AccountNumber = accountNumber,
                AccountHolder = "Simphiwe",
                AccountType = "Savings",
                IsActive = "True",
                Balance = 100,
                DateModified = DateTime.UtcNow,
                UserId = 3
            };

            var mockDbSet = CreateMockDbSet(new List<Restul_Web_Assessment.Repository.Models.Account> { accountMock }.AsQueryable());
            _mockDbContext.Accounts.Returns(mockDbSet);

            // Act
            var result = _run.Deposit(accountNumber, depositAmount);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Deposit_WhenDepositInvalid_ReturnsFalse()
        {
            // Arrange
            int accountNumber = 1;
            int depositAmount = 50;
            var mockDbSet = CreateMockDbSet(new List<Restul_Web_Assessment.Repository.Models.Account>().AsQueryable());
            _mockDbContext.Accounts.Returns(mockDbSet);

            // Act
            var result = _run.Deposit(accountNumber, depositAmount);

            // Assert
            result.Should().BeFalse();
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
