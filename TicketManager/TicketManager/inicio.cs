using Amqp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace TicketManager
{
    public class inicio
    {
        const string NOME_FILA_CONSUMIDOR_MQ = "consumidor";
        const string MIME_TYPE_JSON = "application/json";

        public void monitorar()
        {
            Console.WriteLine("INICIO DA OPERACAO");
            Connection connection = null;
            try
            {
                Settings configs = new Settings();
                ConfiguracoesValores config = configs.ResgataParametro();
                string EndpointElasticSearchOK = config.Parametros.EndpointElasticSearch.DoctoOK;
                string enderecoFila = config.Parametros.EnderecoFila;
                string topicoEntrada = config.Parametros.Inicio.TopicoEntrada;
                string topicoSaida = config.Parametros.Inicio.TopicoSaida;

                Address address = new Address(enderecoFila);
                connection = new Connection(address);
                Session session = new Session(connection);
                ReceiverLink receiver = new ReceiverLink(session, NOME_FILA_CONSUMIDOR_MQ, topicoEntrada);

                while (true)
                {
                    Message request = receiver.Receive(new TimeSpan(0, 30, 0));
                    string replyTo = request.Properties.ReplyTo;
                    if (null != request)
                    {
                        Console.WriteLine(request.Body);
                        string stringData = request.Body.ToString();

                        dynamic results = JsonConvert.DeserializeObject<dynamic>(stringData);

                        results.fila = enderecoFila;
                        stringData = JsonConvert.SerializeObject(results);
                        using (var client = new HttpClient())
                        {
                            var contentDataES = new StringContent(stringData, System.Text.Encoding.UTF8, MIME_TYPE_JSON);

                            HttpResponseMessage responseES = client.PostAsync(EndpointElasticSearchOK, contentDataES).Result;
                        }
                        AMQ.AMQ gravaMensagem = new AMQ.AMQ();

                        gravaMensagem.executa(enderecoFila, topicoSaida, stringData, "", "");
                        receiver.Accept(request);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Exception {0}.", e);
                if (null != connection)
                {
                    connection.Close();
                }
            }
        }
    }
}