using CustomerCrud.Core;
using CustomerCrud.Repositories;
using CustomerCrud.Requests;

namespace CustomerCrud.Test;

public class CustomersControllerTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly Mock<ICustomerRepository> _repositoryMock;

    public CustomersControllerTest(WebApplicationFactory<Program> factory)
    {
        _repositoryMock = new Mock<ICustomerRepository>();

        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddScoped<ICustomerRepository>(st => _repositoryMock.Object);
            });
        }).CreateClient();
    }

    [Fact]
    public async Task GetAllTest()
    {

        // Arrange
        //  Aqui, dados falsos são gerados com o AutoFaker para criar uma lista de 3 objetos do tipo Customer. O AutoFaker é uma biblioteca que gera dados falsos automaticamente para testes unitários.
        var customers = AutoFaker.Generate<Customer>(3);

        // Um mock do método GetAll() do repositório, configurado usando o _repositoryMock
        _repositoryMock.Setup(r => r.GetAll()).Returns(customers);

        //Act
        //Uma chamada GET para a rota "/customers" utilizando o _client
        var response = await _client.GetAsync("/customers");

        //Assert
        var content = await response.Content.ReadFromJsonAsync<IEnumerable<Customer>>();

        //verifica que a resposta retornada pela chamada ao cliente é do tipo 200 (Ok)
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        //  response.EnsureSuccessStatusCode();

        //verifica que o conteúdo da resposta é equivalente ao objeto retornado pelo mock
        content.Should().BeEquivalentTo(customers);

        // verifica que o método mockado foi chamado uma única vez
        _repositoryMock.Verify(r => r.GetAll(), Times.Once);

    }

    [Fact]
    public async Task GetByIdTest()
    {
        var customers = AutoFaker.Generate<Customer>();  

        _repositoryMock.Setup(r => r.GetById(1)).Returns(customers);

        var response = await _client.GetAsync("/customers/1");
        
        var content = await response.Content.ReadFromJsonAsync<Customer>(); 
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        content.Should().BeEquivalentTo(customers);
        _repositoryMock.Verify(r => r.GetById(1), Times.Once);
    }

    [Fact]
    public async Task CreateTest()
    {
        var request = AutoFaker.Generate<CustomerRequest>(); 

        _repositoryMock.Setup(r => r.GetNextIdValue()).Returns(1);

        _repositoryMock.Setup(r => r.Create(It.Is<Customer>(r => r.Id == 1))).Returns(true); 

        var response = await _client.PostAsJsonAsync("/customers", request); 
        var content = await response.Content.ReadFromJsonAsync<Customer>(); 

        response.StatusCode.Should().Be(HttpStatusCode.Created); 

        content!.Id.Should().Be(1);
        content.Name.Should().Be(request.Name);
        content.CPF.Should().Be(request.CPF);
        content.Transactions.Should().BeEquivalentTo(request.Transactions); 
        content.CreatedAt.Should().BeCloseTo(content.UpdatedAt, TimeSpan.FromMilliseconds(100));

        _repositoryMock.Verify(r => r.GetNextIdValue(), Times.Once);
        _repositoryMock.Verify(r => r.Create(It.Is<Customer>(r => r.Id == 1)), Times.Once); 
    }

    [Fact]
    public async Task UpdateTest()
    {
        var request = AutoFaker.Generate<CustomerRequest>();

        _repositoryMock.Setup(r => r.Update(It.Is<int>(id => id == 1), It.IsAny<object>())).Returns(true);

        var response = await _client.PutAsJsonAsync("/customers/1", request);
        var content = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        content.Should().Be("Customer 1 updated");
        _repositoryMock.Verify(r => r.Update(It.Is<int>(id => id == 1), It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task DeleteTest()
    {
        throw new NotImplementedException();
    }
}