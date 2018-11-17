using System;
using System.Net.Http;

namespace ElasticSearch
{
    public class ElasticSearch
    {
        public (int, string) executa(string endereco, StringContent conteudo)
        {
            const int ERROR_SUCCESS = 0;
            const int ERROR_OTHER = 2;

            int exitCode = ERROR_SUCCESS;
            string resposta = "";

            try
            {
                using (var client = new HttpClient())
                {

                    HttpResponseMessage responseES = client.PostAsync(endereco, conteudo).Result;

                    resposta = responseES.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception e)
            {
                resposta = e.Message;
                Console.Error.WriteLine("Exception {0}.", e);
                exitCode = ERROR_OTHER;
            }
            return (exitCode, resposta);
        }
    }
}
