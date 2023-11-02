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
        throw new NotImplementedException();
    }

    [Fact]
    public async Task CreateTest()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public async Task UpdateTest()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public async Task DeleteTest()
    {
        throw new NotImplementedException();
    }
}