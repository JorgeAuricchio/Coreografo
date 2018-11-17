using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace Canal
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var client = new HttpClient())
            {
                for (int x = 0; x < 1; x++)
                {
                    Thread.Sleep(1000);
                    string Cliente = "Jorge";
                    string Refeicao = "Salada";
                    string Bebida = "Agua com Gas";

                    Ticket contaConsulta = new Ticket(Cliente, Refeicao, Bebida);
                    string stringData = JsonConvert.SerializeObject(contaConsulta);

                    var contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");

                    Console.WriteLine("abrindo ticket");
                    Console.WriteLine(stringData);

                    HttpResponseMessage response1 = client.PostAsync("http://localhost:9000/api/values", contentData).Result;

                    string retorno = response1.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(retorno);
                 //   Console.ReadKey();
                }
            }
        }
    }

    public class Ticket
    {
        public string Cliente;
        public string Refeicao;
        public string Bebida;

        public Ticket(string pCliente, string pRefeicao, string pBebida)
        {
            Cliente = pCliente;
            Refeicao = pRefeicao;
            Bebida = pBebida;
        }
    }
}
