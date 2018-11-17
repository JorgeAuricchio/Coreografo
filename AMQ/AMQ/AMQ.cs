using Amqp;
using Amqp.Framing;
using System;

namespace AMQ
{
    public class AMQ
    {
        public int executa(string endereco, string topico, string mensagem, string correlationID)
        {
            const int ERROR_SUCCESS = 0;
            const int ERROR_OTHER = 2;

            int exitCode = ERROR_SUCCESS;
            Connection connection = null;
            try
            {
                Address address = new Address(endereco);
                connection = new Connection(address);
                Session session = new Session(connection);
                SenderLink sender = new SenderLink(session, "envioGenerico", topico);
                // TODO: ReplyTo
               
                Message message = new Message(mensagem);

                //message.Properties = new Properties() { CorrelationId = correlationID , ReplyTo= replyTo };
                message.Properties = new Properties() { CorrelationId = correlationID };
                //                message.Properties = new Properties() { MessageId = id };
                sender.Send(message);

                sender.Close();
                session.Close();
                connection.Close();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Exception {0}.", e);
                if (null != connection)
                    connection.Close();
                exitCode = ERROR_OTHER;
            }
            return exitCode;
        }
    }
}
