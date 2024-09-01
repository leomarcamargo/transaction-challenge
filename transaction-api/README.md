# Transaction-API

A **Transaction-API** é uma aplicação desenvolvida em .NET 8, que expõe uma interface RESTful para o gerenciamento de transações financeiras. O principal objetivo deste serviço é receber e armazenar dados de transações financeiras, além de notificar outros serviços sobre novas transações por meio de mensagens Kafka. A API também fornece funcionalidades para listar todas as transações cadastradas e buscar uma transação específica por seu ID.

## Documentação técnica

Dentro da pasta `docs`, encontra-se um [README](./docs/README.md) com as especificações técnicas e os diagramas de sequência para cada fluxo que a aplicação executa. Esses diagramas fornecem uma visão clara dos processos internos e da arquitetura do sistema.

## Pré-requisitos
Antes de executar a aplicação, certifique-se de que os seguintes pré-requisitos estão instalados e configurados:

- .NET 8
- SQL Server
- Kafka

Caso prefira, você pode usar o SQL Server e Kafka em um ambiente Docker.

## Configuração
As seguintes configurações devem ser ajustadas no arquivo `appsettings.json` para que o projeto funcione corretamente:

- **ConnectionStrings**: Defina a string de conexão para o banco de dados SQL Server.
  ```json
  "ConnectionStrings": {
    "TransactionDbConnection": "Server=localhost;Database=Transactions;Integrated Security=SSPI;TrustServerCertificate=true;"
  }

- **KafkaSettings**: Configure os detalhes do servidor Kafka.
  ```json
  "KafkaSettings": {
    "BootstrapServers": "localhost:9094"
  }
  ```

## Execução
Certifique-se de que o SQL Server e Kafka estejam configurados e em execução. Após isso, siga os passos abaixo para executar o projeto:

1. Restaure as dependências do projeto:
   ```bash
   dotnet restore
   ```

2. Compile o projeto:
   ```bash
   dotnet build
   ```

3. Inicie o serviço:
   ```bash
   dotnet run --project src/Transactions.API/Transactions.API.csproj
   ```

Após esses passos, a API estará em execução, pronta para receber e processar transações financeiras. A documentação da API pode ser acessada via Swagger na rota `/swagger/index.html`.