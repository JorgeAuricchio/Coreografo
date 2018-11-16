using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticket
{
    public class Ticket
    {
        public string Agencia;
        public string Conta;
        public double Saldo;

        public Ticket(string pAgencia, string pConta, double pSaldo)
        {
            Agencia = pAgencia;
            Conta = pConta;
            Saldo = pSaldo;
        }
    }
}
