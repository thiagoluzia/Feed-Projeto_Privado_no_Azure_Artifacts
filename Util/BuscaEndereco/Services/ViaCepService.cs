using BuscaEndereco.Model;
using System.Text.Json;

namespace BuscaEndereco.Services
{
    /// <summary>
    /// Serviço para consulta de endereço utilizando a API ViaCEP.
    /// </summary>
    public static class ViaCepService
    {
        /// <summary>
        /// Instância de HttpClient usada para realizar requisições HTTP.
        /// </summary>
        private static readonly HttpClient _httpClient = new HttpClient();


        /// <summary>
        /// Consulta um endereço com base no CEP fornecido utilizando a API ViaCEP.
        /// </summary>
        /// <param name="inputCep">CEP a ser consultado.</param>
        /// <returns>Objeto <see cref="Endereco"/> contendo os dados do endereço, ou null se a consulta falhar.</returns>
        /// <exception cref="HttpRequestException">Lançada quando a solicitação HTTP falhar.</exception>
        /// <exception cref="JsonException">Lançada quando ocorrer um erro na desserialização do JSON.</exception>
        /// <exception cref="NotSupportedException">Lançada quando o tipo de dado não for suportado na desserialização.</exception>
        /// <exception cref="ArgumentNullException">Lançada quando o argumento nulo for passado para a desserialização.</exception>
        /// <exception cref="Exception">Lançada quando ocorrer um erro desconhecido.</exception>
        public static async Task<Endereco?> ConsultarCepAsync(string inputCep)
        {

            try
            {
                var cep = FormatarCep(inputCep);
                ValidaCep(cep);

                var requestUrl = @$"https://viacep.com.br/ws/{cep}/json";
                var response = await _httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    var contentStream = await response.Content.ReadAsStreamAsync();
                    var endereco = await JsonSerializer.DeserializeAsync<Endereco>(contentStream);
                    return endereco;
                }
                else
                {
                    throw new HttpRequestException("Erro ao consultar o CEP no ViaCEP.");
                }
            }
            catch (JsonException ex)
            {
                throw new JsonException("Erro ao desserializar resposta do ViaCEP.", ex);
            }
            catch (NotSupportedException ex)
            {
                throw new NotSupportedException("Erro de desserialização: tipo não suportado.", ex);
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException("Erro de desserialização: argumento nulo.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro desconhecido ao consultar endereço no ViaCEP.", ex);
            }

        }

        /// <summary>
        /// Valida o formato do CEP.
        /// </summary>
        /// <param name="cep">CEP a ser validado.</param>
        /// <exception cref="ArgumentException">Lançada quando o CEP for nulo, vazio ou não tiver 8 dígitos.</exception>
        /// <exception cref="ArgumentException"></exception>
        private static void ValidaCep(string cep)
        {

            if (string.IsNullOrWhiteSpace(cep))
                throw new ArgumentException("O CEP não pode ser nulo ou vazio.");

            if (cep.Length != 8)
                throw new ArgumentException("O CEP deve ter 8 dígitos.");

        }

        /// <summary>
        /// Formata o CEP removendo caracteres especiais.
        /// </summary>
        /// <param name="cep">CEP a ser formatado.</param>
        /// <returns>CEP formatado.</returns>
        private static string FormatarCep(string cep)
        {

            return cep.Trim().Replace("-", "").Replace(".", "");

        }
    }
}
