using Amqp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace TicketManager
{
    public class termino
    {
        const string NOME_FILA_CONSUMIDOR_MQ = "consumidor";

        public void monitorar()
        {
            Console.WriteLine("TERMINO DA OPERACAO");
            Connection connection = null;
            try
            {
                Settings configs = new Settings();
                ConfiguracoesValores config = configs.ResgataParametro();
                string EndpointElasticSearchOK = config.Parametros.EndpointElasticSearch.DoctoOK;
                string enderecoFila = config.Parametros.EnderecoFila;

                Address address = new Address(enderecoFila);
                connection = new Connection(address);
                Session session = new Session(connection);
                string topicoEntrada = config.Parametros.Termino.TopicoEntrada;

                ReceiverLink receiver = new ReceiverLink(session, NOME_FILA_CONSUMIDOR_MQ, topicoEntrada);

                while (true)
                {
                    Message request = receiver.Receive(new TimeSpan(0, 30, 0));
                    string replyTo = request.Properties.ReplyTo;
                    if (null != request)
                    {
                        Console.WriteLine(request.Body);
                        string stringData = request.Body.ToString();

                        Console.WriteLine("FIM de operacao");
                        Console.WriteLine(stringData);
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
