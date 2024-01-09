using AutoFixture;
using AutoMapper;
using ContactService.Controllers;
using ContactService.Data;
using ContactService.RequestHelpers;
using MassTransit;
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
}