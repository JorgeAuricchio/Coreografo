using Amqp;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSHub
{
    public class ParametrosRotina
    {
        public string enderecoAMQ;
        public string topico;
        public List<Rota> rota = new List<Rota>();


        public ParametrosRotina(string penderecoAMQ, string ptopico, string penderecoRota, string pcallBack)
        {
            enderecoAMQ = penderecoAMQ;
            topico = ptopico;
            rota.Add(new Rota(penderecoRota, pcallBack));
        }
    }
    public class Rota
    {
        public string endereco;
        public string callback;

        public Rota(string pendereco, string pcallback)
        {
            endereco = pendereco;
            callback = pcallback;
        }
    }
}
