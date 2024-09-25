using System.Runtime.InteropServices;

public class Cartinha
{
    public string Nome { get; set; }
    public Endereco Endereco { get; set; }
    public int Idade { get; set; }
    public string Texto { get; set; }
}

public class Endereco
{
    public string Rua { get; set; }
    public string Numero { get; set; }
    public string Bairro { get; set; }
    public string Cidade { get; set; }
    public string Estado { get; set; }
}
public interface ICartinhaRepository
{
    void AdicionarCartinha(Cartinha cartinha);
    List<Cartinha> ListarCartinhas();
}
public class CartinhaRepository : ICartinhaRepository
{
    private static List<Cartinha> _cartinhas = new List<Cartinha>();

    public void AdicionarCartinha(Cartinha cartinha)
    {
        _cartinhas.Add(cartinha);
    }

    public List<Cartinha> ListarCartinhas()
    {
        return _cartinhas;
    }
}
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class CartinhaController : ControllerBase
{
    private readonly ICartinhaRepository _cartinhaRepository;

    public CartinhaController(ICartinhaRepository cartinhaRepository)
    {
        _cartinhaRepository = cartinhaRepository;
    }

    [HttpPost]
    public IActionResult AdicionarCartinha([FromBody] Cartinha cartinha)
    {
        if (cartinha == null)
        {
            return BadRequest("Dados da cartinha não podem ser nulos.");
        }

        // Validações
        if (string.IsNullOrWhiteSpace(cartinha.Nome) || cartinha.Nome.Length < 3 || cartinha.Nome.Length > 255)
            return BadRequest("Nome inválido.");

        if (cartinha.Endereco == null || string.IsNullOrWhiteSpace(cartinha.Endereco.Rua) ||
            string.IsNullOrWhiteSpace(cartinha.Endereco.Numero) ||
            string.IsNullOrWhiteSpace(cartinha.Endereco.Bairro) ||
            string.IsNullOrWhiteSpace(cartinha.Endereco.Cidade) ||
            string.IsNullOrWhiteSpace(cartinha.Endereco.Estado))
            return BadRequest("Endereço inválido.");

        if (cartinha.Idade >= 15)
            return BadRequest("A idade deve ser menor que 15 anos.");

        if (string.IsNullOrWhiteSpace(cartinha.Texto) || cartinha.Texto.Length > 500)
            return BadRequest("Texto da carta inválido.");

        _cartinhaRepository.AdicionarCartinha(cartinha);
        return Ok("Cartinha enviada com sucesso!");
    }

    [HttpGet]
    public IActionResult ListarCartinhas()
    {
        var cartinhas = _cartinhaRepository.ListarCartinhas();
        return Ok(cartinhas);
    }
}
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSingleton<ICartinhaRepository, CartinhaRepository>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
dotnet new webapi - n CartasPapaiNoel

dotnet run

