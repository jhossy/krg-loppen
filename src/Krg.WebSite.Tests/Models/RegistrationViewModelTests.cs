using AutoFixture;
using Krg.Domain.Models;
using Krg.Website.Models;

namespace Krg.WebSite.Tests.Models;

[TestClass]
public class RegistrationViewModelTests
{
    private readonly IFixture _fixture = new Fixture();
    
    [DataTestMethod]
    [DataRow("")]
    [DataRow("Short note")]
    [DataRow(" Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque rhoncus elit in sagittis rhoncus. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Maecenas ultricies est purus, non commodo neque egestas sit amet. Nunc id porta lorem, eu semper nibh. Nullam ac placerat justo. Pellentesque eleifend dolor nec dolor egestas euismod. Integer dolor turpis, auctor a ligula a, tempus maximus massa. Donec ac tellus rhoncus felis varius tristique. Quisque malesuada vehicula leo, eu pharetra arcu luctus quis. Etiam ornare ullamcorper tincidunt. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Phasellus rhoncus nisl et nisi volutpat, vel rutrum massa venenatis. Cras eleifend elit et mauris luctus placerat. Mauris turpis orci, sollicitudin maximus pulvinar at, eleifend eget mi. Integer accumsan sed ipsum a condimentum. Curabitur efficitur augue sit amet egestas volutpat.\n\nNunc finibus, enim in efficitur sagittis, nibh ante iaculis turpis, non facilisis lorem nisl in mi. Integer eu fringilla elit, eu dapibus arcu. Donec turpis sem, elementum in purus quis, fringilla lacinia turpis. Praesent finibus, massa facilisis viverra tincidunt, diam ligula vulputate felis, ut congue dui augue vel massa. Vivamus convallis justo lacus, vitae laoreet velit fringilla vel. Nulla sollicitudin posuere massa, quis tincidunt neque dapibus a. Vestibulum sit amet sapien elementum, commodo ante non, pharetra lectus. In quis imperdiet est, ac efficitur ipsum. Duis mollis erat quis arcu mattis mollis et eget enim. Interdum et malesuada fames ac ante ipsum primis in faucibus. Mauris ut risus mi.\n\nQuisque vehicula risus auctor, vehicula elit eget, euismod purus. Cras in tempus turpis, sed semper lectus. Phasellus ut turpis condimentum metus tempus consequat. Fusce viverra ultricies congue. Sed at velit sapien. Morbi et nisl feugiat nisl vehicula viverra. Vivamus nec libero lacus. Proin non neque sem. Cras non nibh turpis. Suspendisse porttitor nunc in felis porta, ut consequat metus dictum. Nullam a elit in massa volutpat pretium. Duis maximus ipsum velit, sit amet tempus magna suscipit ut.")]
    [DataRow(null)]
    public void RegistrationViewModel_Ctor_WillCreate(string note)
    {
        //Arrange
        EventDate evenDate = _fixture
            .Build<EventDate>()
            .With(x => x.Note, note)
            .Create();
        
        List<Registration> registrations = _fixture.Create<List<Registration>>();
            
        //Act
        RegistrationViewModel sut = new RegistrationViewModel(evenDate, registrations);
        
        //Assert
        Assert.IsTrue(sut != null);
    }
}