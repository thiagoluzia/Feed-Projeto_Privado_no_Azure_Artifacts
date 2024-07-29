using BuscaEndereco.Model;
using BuscaEndereco.Services;

namespace ConsoleApp
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            System.Console.Title = "BUSCAR POR CEP";

            Console.Write("Digite seu CEP: ");
            var cep = Console.ReadLine();
            
            var endereco =  await EnderecoCompleto(cep);

            Console.WriteLine("\n|***************ENDEREÇO****************|");
            Console.WriteLine($"LOGRADOURO: {endereco.Logradouro}");
            Console.WriteLine($"CIDADE: {endereco.Cidade}");
            Console.WriteLine($"BAIRRO: {endereco.Bairro}");
            Console.WriteLine($"UF: {endereco.Uf}");

            Console.ReadKey(); 

        }

        public static async Task<Endereco?> EnderecoCompleto(string cep)
        {
            var endereco  =await  ViaCepService.ConsultarCepAsync(cep);

            return endereco;
        }
    }
}
