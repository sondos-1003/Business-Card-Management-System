using Microsoft.AspNetCore.Hosting;
using Moq;
using NeoRxTask.DTOs;
using NeoRxTask.Entities;
using NeoRxTask.Repositories.IRepositories;
using NeoRxTask.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BusinessCardApp.Tests
{
    public class BusinessCardServiceTests
    {
        private readonly Mock<IBusinessCardRepository> _mockRepo;
        private readonly Mock<IWebHostEnvironment> _mockEnv;
        private readonly BusinessCardService _service;

        public BusinessCardServiceTests()
        {
            _mockRepo = new Mock<IBusinessCardRepository>();
            _mockEnv = new Mock<IWebHostEnvironment>();

            _mockEnv.Setup(e => e.WebRootPath)
                    .Returns("C:\\FakePath");

            _service = new BusinessCardService(
                _mockRepo.Object,
                _mockEnv.Object
            );
        }

        // ============================
        // A) CREATE TESTS
        // ============================

        [Fact]
        public async Task CreateAsync_ShouldReturnBusinessCard_WhenValidData()
        {
            var dto = new CreateBusinessCardDto
            {
                Name = "Sarah",
                Email = "sarah@gmail.com"
            };

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<BusinessCard>()))
                     .Returns(Task.CompletedTask);

            var result = await _service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("Sarah", result.Name);

            _mockRepo.Verify(r => r.AddAsync(It.IsAny<BusinessCard>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_WhenDtoIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _service.CreateAsync(null));
        }

        // ============================
        // B) GET BY ID TESTS
        // ============================

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCard_WhenExists()
        {
            var card = new BusinessCard
            {
                Id = 1,
                Name = "Sara"
            };

            _mockRepo.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync(card);

            var result = await _service.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Sara", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync((BusinessCard)null);

            var result = await _service.GetByIdAsync(1);

            Assert.Null(result);
        }

        // ============================
        // C) UPDATE TESTS
        // ============================

        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedCard_WhenCardExists()
        {
            // Arrange
            var existingCard = new BusinessCard
            {
                Id = 1,
                Name = "Old Name",
                Gender = "Female",
                Email = "old@gmail.com"
            };

            var updateDto = new UpdateBusinessCardDto
            {
                Name = "New Name",
                Gender = "Male",
                DOB = new DateTime(2000, 1, 1),
                Email = "new@gmail.com",
                PhoneNumber = "0799999999",
                Address = "Amman"
            };

            _mockRepo.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync(existingCard);

            _mockRepo.Setup(r => r.UpdateAsync(1, It.IsAny<BusinessCard>()))
                     .Returns(Task.CompletedTask);

            // Act
            var result = await _service.UpdateAsync(1, updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Name", result.Name);
            Assert.Equal("Male", result.Gender);
            Assert.Equal("new@gmail.com", result.Email);
            Assert.Equal("0799999999", result.PhoneNumber);
            Assert.Equal("Amman", result.Address);

            _mockRepo.Verify(r => r.UpdateAsync(1, It.IsAny<BusinessCard>()), Times.Once);
        }
        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenCardNotFound()
        {
            // Arrange
            var updateDto = new UpdateBusinessCardDto
            {
                Name = "New Name"
            };

            _mockRepo.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync((BusinessCard)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() =>
                _service.UpdateAsync(1, updateDto));

            Assert.Equal("Business card not found", exception.Message);

            _mockRepo.Verify(r => r.UpdateAsync(It.IsAny<int>(), It.IsAny<BusinessCard>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReplaceNullStringsWithEmpty()
        {
            var existingCard = new BusinessCard
            {
                Id = 1,
                Name = "Old"
            };

            var updateDto = new UpdateBusinessCardDto
            {
                Name = null,
                Gender = null,
                Email = null,
                PhoneNumber = null,
                Address = null
            };

            _mockRepo.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync(existingCard);

            _mockRepo.Setup(r => r.UpdateAsync(1, It.IsAny<BusinessCard>()))
                     .Returns(Task.CompletedTask);

            var result = await _service.UpdateAsync(1, updateDto);

            Assert.Equal(string.Empty, result.Name);
            Assert.Equal(string.Empty, result.Gender);
            Assert.Equal(string.Empty, result.Email);
            Assert.Equal(string.Empty, result.PhoneNumber);
            Assert.Equal(string.Empty, result.Address);
        }

        // ============================
        // D) DELETE TESTS
        // ============================

        [Fact]
        public async Task DeleteAsync_ShouldReturnTrue_WhenCardExists()
        {
            var existingCard = new BusinessCard
            {
                Id = 1,
                Name = "Sara"
            };

            _mockRepo.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync(existingCard);

            _mockRepo.Setup(r => r.DeleteAsync(existingCard))
                     .Returns(Task.CompletedTask);

            var result = await _service.DeleteAsync(1);

            Assert.True(result);
            _mockRepo.Verify(r => r.DeleteAsync(existingCard), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenCardNotFound()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync((BusinessCard)null);

            var result = await _service.DeleteAsync(1);

            Assert.False(result);
            _mockRepo.Verify(r => r.DeleteAsync(It.IsAny<BusinessCard>()), Times.Never);
        }
    }
}