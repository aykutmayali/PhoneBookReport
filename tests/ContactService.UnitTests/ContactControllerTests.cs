using AutoFixture;
using AutoMapper;
using ContactService.Controllers;
using ContactService.Data;
using ContactService.DTOs;
using ContactService.Entities;
using ContactService.RequestHelpers;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ContactService.UnitTests;

public class ContactControllerTests
{
    private readonly Mock<IContactRepository> _contactRepo;
    private readonly Mock<IPublishEndpoint> _publishEndpoint;
    private readonly Fixture _fixture;
    private readonly ContactsControllerForMock _controller;
    private readonly IMapper _mapper;
    public ContactControllerTests()
    {
        _fixture = new Fixture();
        _contactRepo = new Mock<IContactRepository>();
        _publishEndpoint = new Mock<IPublishEndpoint>();

        var mockMapper = new MapperConfiguration(mc => {
            mc.AddMaps(typeof(MappingProfiles).Assembly);
        }).CreateMapper().ConfigurationProvider;

        _mapper = new Mapper(mockMapper);
        _controller = new ContactsControllerForMock(_contactRepo.Object, _mapper, _publishEndpoint.Object);
    }

    [Fact]
    public async Task GetContacts_WithNoParams_Returns10Contatcs()
    {
        //arrange
        var contacts = _fixture.CreateMany<ContactDto>(10).ToList();
        _contactRepo.Setup(repo => repo.GetContactsAsync()).ReturnsAsync(contacts);
        //act
        var result = await _controller.GetAllContacts();
        //assert
        Assert.Equal(10, result.Value.Count);
        Assert.IsType<ActionResult<List<ContactDto>>>(result);

    }

    [Fact]
    public async Task GetContactById_WithValidGuid_ReturnsContact()
    {
        //arrange
        var contact = _fixture.Create<ContactDto>();
        _contactRepo.Setup(repo => repo.GetContactByIdAsync(It.IsAny<Guid>())).ReturnsAsync(contact);
        //act
        var result = await _controller.GetContactById(contact.Id);
        //assert
        Assert.Equal(contact.Name, result.Value.Name);
        Assert.IsType<ActionResult<ContactDto>>(result);

    }

    [Fact]
    public async Task GetContactById_WithInValidGuid_ReturnsNotFound()
    {
        //arrange
        _contactRepo.Setup(repo => repo.GetContactByIdAsync(It.IsAny<Guid>())).ReturnsAsync(value:null);
        //act
        var result = await _controller.GetContactById(Guid.NewGuid());
        //assert
        Assert.IsType<NotFoundResult>(result.Result);

    }

}