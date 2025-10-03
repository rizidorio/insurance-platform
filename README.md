# Sistema de Gerenciamento de Propostas de Seguro

Sistema de microserviços para gerenciar propostas e contratações de seguro, desenvolvido com .NET 8, seguindo os princípios de Arquitetura Hexagonal, DDD, Clean Code e SOLID.

## 🏗️ Arquitetura

O sistema é composto por dois microserviços principais:

### 1. PropostaService
Responsável pelo gerenciamento de propostas de seguro.

**Funcionalidades:**
- Criar proposta de seguro
- Listar propostas
- Consultar proposta por ID
- Alterar status da proposta (Em Análise → Aprovada/Rejeitada)

### 2. ContratacaoService
Responsável pela contratação de propostas aprovadas.

**Funcionalidades:**
- Contratar proposta (apenas se aprovada)
- Listar contratações
- Consultar contratação por ID
- Comunicação com PropostaService via HTTP

## 🎯 Padrões e Princípios Aplicados

### Arquitetura Hexagonal (Ports & Adapters)
Cada microserviço é organizado em camadas:
- **Domain**: Entidades, Value Objects, Enums, Interfaces (Ports)
- **Application**: Use Cases, DTOs
- **Infrastructure**: Implementações (Adapters) - Repositórios, HttpClients
- **API**: Controllers, Configuração

### Domain-Driven Design (DDD)
- Entidades ricas com lógica de negócio
- Value Objects para conceitos específicos
- Domain Services para regras complexas
- Linguagem ubíqua no código

### SOLID
- **S**ingle Responsibility: Cada classe tem uma única responsabilidade
- **O**pen/Closed: Extensível via interfaces
- **L**iskov Substitution: Implementações substituíveis
- **I**nterface Segregation: Interfaces específicas
- **D**ependency Inversion: Dependência de abstrações

### Clean Code
- Nomes significativos e descritivos
- Métodos pequenos e focados
- Comentários apenas quando necessário
- Tratamento adequado de exceções

## 🛠️ Tecnologias Utilizadas

- **.NET 8**: Framework principal
- **PostgreSQL**: Banco de dados relacional
- **Entity Framework Core**: ORM
- **Docker**: Containerização
- **Swagger/OpenAPI**: Documentação de APIs
- **xUnit**: Framework de testes
- **FluentAssertions**: Assertions fluentes
- **Moq**: Mock de dependências

## 📋 Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started) e Docker Compose
- [PostgreSQL](https://www.postgresql.org/download/) (opcional, se não usar Docker)

## 🚀 Como Executar

### Opção 1: Com Docker Compose (Recomendado)

1. Clone o repositório:
```bash
git clone https://github.com/seu-usuario/insurance-system.git
cd insurance-system
```

2. Execute com Docker Compose:
```bash
docker-compose up --build
```

3. Os serviços estarão disponíveis em:
   - PropostaService: http://localhost:5001
   - PropostaService Swagger: http://localhost:5001/swagger
   - ContratacaoService: http://localhost:5002
   - ContratacaoService Swagger: http://localhost:5002/swagger

### Opção 2: Execução Local

1. Inicie o PostgreSQL ou utilize Docker:
```bash
docker run --name postgres-propostas -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=propostas_db -p 5432:5432 -d postgres:15-alpine
docker run --name postgres-contratacoes -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=contratacoes_db -p 5433:5432 -d postgres:15-alpine
```

2. Execute as migrations:
```bash
cd PropostaService
dotnet ef database update

cd ../ContratacaoService
dotnet ef database update
```

3. Execute os serviços:
```bash
# Terminal 1 - PropostaService
cd PropostaService
dotnet run --urls "http://localhost:5001"

# Terminal 2 - ContratacaoService
cd ContratacaoService
dotnet run --urls "http://localhost:5002"
```

## 🧪 Executar Testes

```bash
# Todos os testes
dotnet test

# Testes do PropostaService
cd PropostaService.Tests
dotnet test

# Testes do ContratacaoService
cd ContratacaoService.Tests
dotnet test

# Com cobertura
dotnet test /p:CollectCoverage=true
```

## 📡 Exemplos de Uso da API

### PropostaService

#### Criar Proposta
```bash
curl -X POST http://localhost:5001/api/propostas \
  -H "Content-Type: application/json" \
  -d '{
    "nomeCliente": "João Silva",
    "cpfCliente": "12345678901",
    "valorPremio": 1500.00,
    "tipoSeguro": 1
  }'
```

**Tipos de Seguro:**
- 1: Vida
- 2: Auto
- 3: Residencial
- 4: Empresarial

#### Listar Propostas
```bash
curl http://localhost:5001/api/propostas
```

#### Consultar Proposta
```bash
curl http://localhost:5001/api/propostas/{id}
```

#### Aprovar Proposta
```bash
curl -X PATCH http://localhost:5001/api/propostas/{id}/status \
  -H "Content-Type: application/json" \
  -d '{"status": "aprovada"}'
```

#### Rejeitar Proposta
```bash
curl -X PATCH http://localhost:5001/api/propostas/{id}/status \
  -H "Content-Type: application/json" \
  -d '{"status": "rejeitada"}'
```

### ContratacaoService

#### Contratar Proposta
```bash
curl -X POST http://localhost:5002/api/contratacoes \
  -H "Content-Type: application/json" \
  -d '{
    "propostaId": "guid-da-proposta-aprovada"
  }'
```

#### Listar Contratações
```bash
curl http://localhost:5002/api/contratacoes
```

#### Consultar Contratação
```bash
curl http://localhost:5002/api/contratacoes/{id}
```

## 🗂️ Estrutura do Projeto

```
insurance-system/
├── PropostaService/
│   ├── Domain/
│   │   ├── Entities/
│   │   │   ├── Proposta.cs
│   │   │   ├── StatusProposta.cs
│   │   │   └── TipoSeguro.cs
│   │   ├── Ports/
│   │   │   └── IPropostaRepository.cs
│   │   └── Services/
│   │       └── PropostaDomainService.cs
│   ├── Application/
│   │   ├── DTOs/
│   │   └── UseCases/
│   ├── Infrastructure/
│   │   ├── Data/
│   │   ├── Repositories/
│   │   └── Migrations/
│   ├── API/
│   │   └── Controllers/
│   └── Dockerfile
├── ContratacaoService/
│   ├── Domain/
│   │   ├── Entities/
│   │   ├── Ports/
│   │   └── Models/
│   ├── Application/
│   │   ├── DTOs/
│   │   └── UseCases/
│   ├── Infrastructure/
│   │   ├── Data/
│   │   ├── Repositories/
│   │   ├── HttpClients/
│   │   └── Migrations/
│   ├── API/
│   │   └── Controllers/
│   └── Dockerfile
├── PropostaService.Tests/
├── ContratacaoService.Tests/
├── docker-compose.yml
└── README.md
```

## 🔄 Fluxo de Negócio

1. **Criar Proposta**: Cliente cria uma proposta de seguro (status: Em Análise)
2. **Análise**: Proposta é analisada e pode ser Aprovada ou Rejeitada
3. **Contratação**: Apenas propostas Aprovadas podem ser contratadas
4. **Validação**: ContratacaoService valida o status da proposta antes de contratar
5. **Geração**: Sistema gera número de apólice automaticamente

## 🔒 Regras de Negócio

### PropostaService
- CPF deve ter 11 dígitos e ser válido
- Valor do prêmio deve ser maior que zero
- Prêmio mínimo: R$ 100,00
- Prêmio máximo: R$ 100.000,00
- Apenas propostas "Em Análise" podem mudar de status
- Uma proposta não pode ser aprovada/rejeitada duas vezes

### ContratacaoService
- Apenas propostas com status "Aprovada" podem ser contratadas
- Uma proposta só pode ter uma contratação
- Número de apólice é gerado automaticamente e é único
- Comunica com PropostaService para validar status

## 🐛 Troubleshooting

### Erro de conexão com banco de dados
- Verifique se o PostgreSQL está rodando
- Confirme as credenciais no `appsettings.json`
- Execute as migrations: `dotnet ef database update`

### Erro de comunicação entre microserviços
- Verifique se ambos os serviços estão rodando
- Confirme a URL do PropostaService no `appsettings.json` do ContratacaoService
- No Docker, use o nome do serviço: `http://proposta-service:80`

### Porta já em uso
```bash
# Linux/Mac
lsof -ti:5001 | xargs kill -9
lsof -ti:5002 | xargs kill -9

# Windows
netstat -ano | findstr :5001
taskkill /PID <PID> /F
```

## 📚 Próximos Passos

- [ ] Implementar autenticação e autorização (JWT)
- [ ] Adicionar mensageria (RabbitMQ/Kafka) para comunicação assíncrona
- [ ] Implementar Circuit Breaker e Retry Policies
- [ ] Adicionar logs estruturados (Serilog)
- [ ] Implementar testes de integração
- [ ] Configurar CI/CD (GitHub Actions)
- [ ] Adicionar monitoramento (Prometheus/Grafana)
- [ ] Implementar API Gateway
- [ ] Adicionar cache distribuído (Redis)

## 👥 Autor

Seu Nome - [GitHub](https://github.com/seu-usuario)

## 📄 Licença

Este projeto está sob a licença MIT.