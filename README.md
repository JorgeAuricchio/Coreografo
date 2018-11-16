# Coreografo
Exemplo de Coreografia de Serviços via Broker

### Desenvolvido em .NetCore 2.1

### subir ElasticSearch<br/>
docker pull docker.elastic.co/elasticsearch/elasticsearch:6.4.3<br/>
docker run -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" docker.elastic.co/elasticsearch/elasticsearch:6.4.3

### subir ActiveMQ<br/>
docker pull webcenter/activemq<br/>
docker run --name='activemq' -it --rm -e 'ACTIVEMQ_CONFIG_MINMEMORY=512' -e 'ACTIVEMQ_CONFIG_MAXMEMORY=2048' -P webcenter/activemq:latest

### executar robo PSHub<br/>
cd "C:\Users\Jla\Documents\Visual Studio 2017\Projects\PSHub\PSHub\bin\Debug\netcoreapp2.1"<br/>
dotnet .\PSHub.dll

### executar robo TicketManager<br/>
cd "C:\Users\Jla\Documents\Visual Studio 2017\Projects\TicketManager\TicketManager\bin\Debug\netcoreapp2.1"<br/>
dotnet .\TicketManager.dll

### executar api de Ticket<br/>
Porta 9000<br/>
cd "C:\Users\Jla\Documents\Visual Studio 2017\Projects\Ticket\Ticket\bin\Debug\netcoreapp2.1"<br/>
dotnet .\Ticket.dll

### executar api de OrderService<br/>
Porta 9001<br/>
cd "C:\Users\Jla\Documents\Visual Studio 2017\Projects\OrderService\OrderService\bin\Debug\netcoreapp2.1"<br/>
dotnet .\OrderService.dll

### executar api de ConsumerService<br/>
Porta 9002<br/>
cd "C:\Users\Jla\Documents\Visual Studio 2017\Projects\ConsumerService\ConsumerService\bin\Debug\netcoreapp2.1"<br/>
dotnet .\ConsumerService.dll

### executar api de KitchenService<br/>
Porta 9003<br/>
cd "C:\Users\Jla\Documents\Visual Studio 2017\Projects\KitchenService\KitchenService\bin\Debug\netcoreapp2.1"<br/>
dotnet .\KitchenService.dll

### executar api AccountingService<br/>
Porta 9004<br/>
cd "C:\Users\Jla\Documents\Visual Studio 2017\Projects\AccountingService\AccountingService\bin\Debug\netcoreapp2.1"<br/>
dotnet .\AccountingService.dll

### Para ver os logs dos serviços basta acessar o ElasticSearch<br/>
   http://localhost:9200/coreografado/doc/_search<br/>

Post: <br/>
{<br/>
  "query": { <br/>
    "bool": { <br/>
      "must": [<br/>
        { "match": { "codigoTicket": "UID DO TICKET" }}  <br/>
      ]<br/>
    }<br/>
  }<br/>
}<br/>

### Próximos passos<br/>
   criar classes dos serviços e regras de negócio (incluir casos de erro)<br/>
   criar monitor grafico para ver os servicos atuando<br/>
   criar conteineres de todos os servicos e consoles   <br/>
   criar testes unitarios
