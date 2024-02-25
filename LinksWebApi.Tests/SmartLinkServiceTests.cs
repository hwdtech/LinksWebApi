using AutoMapper;
using FluentAssertions;
using LinksWebApi.BL;
using LinksWebApi.BL.Dto;
using LinksWebApi.BL.Services;
using LinksWebApi.Data.Entities;
using LinksWebApi.Data.Interfaces;
using Moq;

namespace LinksWebApi.Tests;

public class SmartLinkServiceTests
{
    private readonly Mock<ISmartLinkRepository> _mockRepository;
    private readonly SmartLinkService _service;

    public SmartLinkServiceTests()
    {
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });
        var mapper = mappingConfig.CreateMapper();

        _mockRepository = new Mock<ISmartLinkRepository>();
        _service = new SmartLinkService(_mockRepository.Object, mapper);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedSmartLink_WhenDataIsValid()
    {
        // Arrange
        var generatedId = 123;
        var dto = new SmartLinkBaseDto("TestUrl", "TestName");

        _mockRepository.Setup(r => r.GetByRelativePathAsync(It.IsAny<string>())).ReturnsAsync((SmartLink?)null);
        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<SmartLink>()))
            .Callback<SmartLink>(entity =>
            {
                // Выполняем действия с entity, например, устанавливаем Id
                entity.Id = generatedId;
            })
            .ReturnsAsync((SmartLink entity) => entity); // Возвращает объект, переданный в метод

        // Act
        var result = await _service.Create(dto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(generatedId);
        result.Name.Should().Be(dto.Name);
        result.OriginRelativeUrl.Should().Be(dto.OriginRelativeUrl);
        result.RedirectionRules.Should().BeEmpty(); // Проверка, что правила перенаправления отсутствуют

        // Проверка вызова репозитория с правильными параметрами
        _mockRepository.Verify(x =>
            x.CreateAsync(It.Is<SmartLink>(y => y.Name == dto.Name && y.OriginRelativeUrl == dto.OriginRelativeUrl && y.NormalizedOriginRelativeUrl == "/TestUrl")), Times.Once());
    }

    [Fact]
    public async Task Create_ShouldThrowApplicationException_WhenUrlExists()
    {
        // Arrange
        var dto = new SmartLinkBaseDto("ExistingUrl", "TestName");
        var existingEntity = new SmartLink
        {
            Name = "Existing",
            OriginRelativeUrl = "ExistingUrl",
            NormalizedOriginRelativeUrl = "/ExistingUrl"
        };
        _mockRepository.Setup(r => r.GetByRelativePathAsync(existingEntity.NormalizedOriginRelativeUrl)).ReturnsAsync(existingEntity);

        // Act & Assert
        await Assert.ThrowsAsync<ApplicationException>(() => _service.Create(dto));
    }

    [Fact]
    public async Task GetById_ShouldReturnSmartLinkDto_WhenSmartLinkExists()
    {
        // Arrange
        var smartLinkId = 1;
        var smartLinkEntity = new SmartLink
        {
            Id = smartLinkId,
            Name = "Name",
            OriginRelativeUrl = "RelativeUrl",
            NormalizedOriginRelativeUrl = "/RelativeUrl"
        };
        _mockRepository.Setup(r => r.GetByIdAsync(smartLinkId)).ReturnsAsync(smartLinkEntity);

        // Act
        var result = await _service.GetById(smartLinkId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(smartLinkId);
        result.Name.Should().Be(smartLinkEntity.Name);
        result.OriginRelativeUrl.Should().Be(smartLinkEntity.OriginRelativeUrl);
        result.RedirectionRules.Should().BeEmpty(); // Проверка, что правила перенаправления отсутствуют

        // Проверка вызова репозитория с правильными параметрами
        _mockRepository.Verify(x => x.GetByIdAsync(It.Is<int>(id => id == smartLinkId)), Times.Once());
    }

    [Fact]
    public async Task GetById_ShouldReturnNull_WhenSmartLinkDoesNotExist()
    {
        // Arrange
        var smartLinkId = 1;
        _mockRepository.Setup(r => r.GetByIdAsync(smartLinkId)).ReturnsAsync((SmartLink?)null);

        // Act
        var result = await _service.GetById(smartLinkId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Update_ShouldReturnUpdatedSmartLinkDto_WhenUpdateIsSuccessful()
    {
        // Arrange
        var smartLinkId = 1;
        var dto = new SmartLinkBaseDto("UpdatedUrl", "UpdatedName");
        var smartLinkEntity = new SmartLink
        {
            Id = smartLinkId,
            Name = "Name",
            OriginRelativeUrl = "TestUrl",
            NormalizedOriginRelativeUrl = "/TestUrl"
        };
        _mockRepository.Setup(r => r.GetByIdAsync(smartLinkId)).ReturnsAsync(smartLinkEntity);
        _mockRepository.Setup(r => r.UpdateAsync(smartLinkEntity)).Returns(Task.CompletedTask);

        // Act
        var result = await _service.Update(smartLinkId, dto);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(smartLinkId);
        result.Name.Should().Be(dto.Name);
        result.OriginRelativeUrl.Should().Be(dto.OriginRelativeUrl);
        result.RedirectionRules.Should().BeEmpty();
    }

    [Fact]
    public async Task Update_ShouldReturnNull_WhenSmartLinkDoesNotExist()
    {
        // Arrange
        var smartLinkId = 1;
        var dto = new SmartLinkBaseDto("UpdatedUrl", "UpdatedName");
        _mockRepository.Setup(r => r.GetByIdAsync(smartLinkId)).ReturnsAsync((SmartLink?)null);

        // Act
        var result = await _service.Update(smartLinkId, dto);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Delete_ShouldReturnTrue_WhenDeleteIsSuccessful()
    {
        // Arrange
        var smartLinkId = 1;
        _mockRepository.Setup(r => r.DeleteAsync(smartLinkId)).ReturnsAsync(true);

        // Act
        var result = await _service.Delete(smartLinkId);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_ShouldReturnFalse_WhenSmartLinkDoesNotExist()
    {
        // Arrange
        var smartLinkId = 1;
        _mockRepository.Setup(r => r.DeleteAsync(smartLinkId)).ReturnsAsync(false);

        // Act
        var result = await _service.Delete(smartLinkId);

        // Assert
        result.Should().BeFalse();
    }
}