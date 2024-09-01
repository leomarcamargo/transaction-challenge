
# Consolidation-Worker

O **Consolidation-Worker** é um serviço que processa transações financeiras em tempo real utilizando o Kafka. Ele foi projetado para lidar com grandes volumes de dados de forma eficiente, garantindo que cada transação seja processada e armazenada corretamente. O principal objetivo do serviço é consolidar o saldo diário de transações financeiras. Isso garante que os dados estejam sempre atualizados para consulta e permite a geração de relatórios detalhados sobre o fluxo de caixa e a situação financeira da organização.

## Documentação técnica

Dentro da pasta `docs`, encontra-se um [README](./docs/README.md) com as especificações técnicas e os diagramas de sequência para cada fluxo que a aplicação executa. Esses diagramas fornecem uma visão clara dos processos internos e da arquitetura do sistema.

## Pré-requisitos
Antes de executar a aplicação, certifique-se de que os seguintes pré-requisitos estão instalados e configurados:

- .NET 8
- SQL Server
- Redis
- Kafka

Caso prefira, você pode usar o SQL Server, Redis e Kafka em um ambiente Docker.

## Configuração
As seguintes configurações devem ser ajustadas no arquivo `appsettings.json` para que o projeto funcione corretamente:

- **ConnectionStrings**: Defina a string de conexão para o banco de dados SQL Server.
  ```json
  "ConnectionStrings": {
    "ConsolidationConnection": "Server=localhost;Database=Consolidation;Integrated Security=SSPI;TrustServerCertificate=true;"
  }
  ```
- **KafkaSettings**: Configure os detalhes do servidor Kafka.
  ```json
  "KafkaSettings": {
    "BootstrapServers": "localhost:9094",
    "GroupId": "transactions-group"
  }
  ```
- **RedisSettings**: Ajuste as configurações para conexão com o Redis.
  ```json
  "RedisSettings": {
    "Host": "localhost",
    "Port": 6380
  }
  ```

## Execução
Certifique-se de que o Redis, SQL Server e Kafka estejam configurados e em execução. Após isso, siga os passos abaixo para executar o projeto:

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
   dotnet run --project src/Consolidations.Worker/Consolidations.Worker.csproj
   ```

Após esses passos, o serviço estará em execução, pronto para processar transações e realizar a consolidação dos dados financeiros.