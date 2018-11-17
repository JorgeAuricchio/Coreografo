using Amqp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TicketManager
{
    public class erro
    {
        const string NOME_FILA_CONSUMIDOR_MQ = "consumidor";

        public void Monitorar(object parametros)
        {
            ErroParametros param = ((ErroParametros)parametros);
            string topico = param.Topico;
            string endrecoFila = param.EnderecoFila;

            Console.WriteLine("ERRO: " + topico);
            Connection connection = null;
            try
            {                
                Address address = new Address(endrecoFila);
                connection = new Connection(address);
                Session session = new Session(connection);
                ReceiverLink receiver = new ReceiverLink(session, NOME_FILA_CONSUMIDOR_MQ, topico);

                while (true)
                {
                    Message request = receiver.Receive(new TimeSpan(0, 30, 0));
                    string replyTo = request.Properties.ReplyTo;
                    string correlationID = request.Properties.CorrelationId;

                    if (null != request)
                    {
                        Console.WriteLine(request.Body);
                        string stringData = request.Body.ToString();

                        Console.WriteLine("FIM de operacao: {0}", correlationID);
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
    public class ErroParametros
    {
        public string Topico;
        public string EnderecoFila;
    }
}