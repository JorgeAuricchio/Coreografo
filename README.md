# Coreografo
Exemplo de Coreografia de Serviços via Broker

subir ElasticSearch
docker pull docker.elastic.co/elasticsearch/elasticsearch:6.4.3
docker run -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" docker.elastic.co/elasticsearch/elasticsearch:6.4.3

subir ActiveMQ
docker pull webcenter/activemq
docker run --name='activemq' -it --rm -e 'ACTIVEMQ_CONFIG_MINMEMORY=512' -e 'ACTIVEMQ_CONFIG_MAXMEMORY=2048' -P webcenter/activemq:latest

executar robo PSHub
cd "C:\Users\Jla\Documents\Visual Studio 2017\Projects\PSHub\PSHub\bin\Debug\netcoreapp2.1"
dotnet .\PSHub.dll

executar robo TicketManager
cd "C:\Users\Jla\Documents\Visual Studio 2017\Projects\TicketManager\TicketManager\bin\Debug\netcoreapp2.1"
dotnet .\TicketManager.dll

executar api de Ticket
Porta 9000
cd "C:\Users\Jla\Documents\Visual Studio 2017\Projects\Ticket\Ticket\bin\Debug\netcoreapp2.1"
dotnet .\Ticket.dll

executar api de OrderService
Porta 9001
cd "C:\Users\Jla\Documents\Visual Studio 2017\Projects\OrderService\OrderService\bin\Debug\netcoreapp2.1"
dotnet .\OrderService.dll

executar api de ConsumerService
Porta 9002
cd "C:\Users\Jla\Documents\Visual Studio 2017\Projects\ConsumerService\ConsumerService\bin\Debug\netcoreapp2.1"
dotnet .\ConsumerService.dll

executar api de KitchenService
Porta 9003
cd "C:\Users\Jla\Documents\Visual Studio 2017\Projects\KitchenService\KitchenService\bin\Debug\netcoreapp2.1"
dotnet .\KitchenService.dll

executar api AccountingService
Porta 9004
cd "C:\Users\Jla\Documents\Visual Studio 2017\Projects\AccountingService\AccountingService\bin\Debug\netcoreapp2.1"
dotnet .\AccountingService.dll

Para ver os logs dos serviços basta acessar o ElasticSearch
   http://localhost:9200/coreografado/doc/_search

Post: 
{
  "query": { 
    "bool": { 
      "must": [
        { "match": { "codigoTicket": "UID DO TICKET" }}  
      ]
    }
  }
}

Próximos passos
   criar classes dos serviços e regras de negócio (incluir casos de erro)
   criar monitor grafico para ver os servicos atuando
   criar conteineres de todos os servicos e consoles   
   criar testes unitarios
