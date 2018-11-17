# Coreografo
Exemplo de Coreografia de Serviços via Broker

### Desenvolvido em .NetCore 2.1

### Para rodar precisamos:
Ter um equipamento com Windows ou Mac<br/>
Visual Studio Community<br/>
Docker

## Componentes<br/>
### ActiveMQ<br/>
Responsavel pela fila utilizada no Hub<br/>
para subir:<br/>
docker pull webcenter/activemq<br/>
docker run --name='activemq' -it --rm -e 'ACTIVEMQ_CONFIG_MINMEMORY=512' -e 'ACTIVEMQ_CONFIG_MAXMEMORY=2048' -P webcenter/activemq:latest<br/>
Url: http://localhost:32771/admin/<br/>
Usuário: admin<br/>
Senha: admin

### ElasticSearch<br/>
Responsavel por armazenar logs das execucoes dos servicos/hub/ticketmanager<br/>
para subir:<br/>
docker pull docker.elastic.co/elasticsearch/elasticsearch:6.4.3<br/>
docker run -p 9200:9200 -p 9300:9300 -e "discovery.type=single-node" docker.elastic.co/elasticsearch/elasticsearch:6.4.3

### Robô PSHub<br/>
Responsavel por rotear os topicos assinados e acionar os respectivos servicos (APIs)<br/>
para subir:<br/>
cd "PSHub\PSHub\bin\Debug\netcoreapp2.1"<br/>
dotnet .\PSHub.dll

### Robô TicketManager<br/>
Responsavel por orquestrar caso necessario algum servico<br/>
para subir:<br/>
cd "TicketManager\TicketManager\bin\Debug\netcoreapp2.1"<br/>
dotnet .\TicketManager.dll

### API de Ticket<br/>
API responsavel por criar ticket<br/>
Porta 9000<br/>
para subir:<br/>
cd "Ticket\Ticket\bin\Debug\netcoreapp2.1"<br/>
dotnet .\Ticket.dll

### API de OrderService<br/>
API/Servico para demonstracao de coreografia, ainda nao tem regras de negocio<br/>
Porta 9001<br/>
para subir:<br/>
cd "OrderService\OrderService\bin\Debug\netcoreapp2.1"<br/>
dotnet .\OrderService.dll

### API de ConsumerService<br/>
API/Servico para demonstracao de coreografia, ainda nao tem regras de negocio<br/>
Porta 9002<br/>
para subir:<br/>
cd "ConsumerService\ConsumerService\bin\Debug\netcoreapp2.1"<br/>
dotnet .\ConsumerService.dll

### API de KitchenService<br/>
API/Servico para demonstracao de coreografia, ainda nao tem regras de negocio<br/>
Porta 9003<br/>
para subir:<br/>
cd "KitchenService\KitchenService\bin\Debug\netcoreapp2.1"<br/>
dotnet .\KitchenService.dll

### API AccountingService<br/>
API/Servico para demonstracao de coreografia, ainda nao tem regras de negocio<br/>
Porta 9004<br/>
para subir:<br/>
cd "AccountingService\AccountingService\bin\Debug\netcoreapp2.1"<br/>
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

Para visualizar use: https://www.getpostman.com/apps <br/>

### Próximos passos<br/>
   criar classes dos serviços e regras de negócio (incluir casos de erro)<br/>
   criar monitor grafico para ver os servicos atuando<br/>
   criar conteineres de todos os servicos e consoles   <br/>
   criar testes unitarios
