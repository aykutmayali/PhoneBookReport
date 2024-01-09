using ContactService.Entities;

namespace ContactService.UnitTests;

public class ContactEntityTests
{
    [Fact]
    public void HasName_NameNotNull_True()
    {
        //arrange
        var contact = new Contact { Id = Guid.NewGuid(), Name = "John"};
        //act
        var result = contact.Name;
        //assert
        Assert.True(result !=null);
    }

    [Fact]
    public void HasName_NameNotNull_False()
    {
        //arrange
        var contact = new Contact { Id = Guid.NewGuid(), Name = null};
        //act
        var result = contact.Name;
        //assert
        Assert.True(result ==null);
    }
}