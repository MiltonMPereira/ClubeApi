\# ClubeApi

API em .NET Core para controle de acesso às áreas de um clube, utilizando Entity Framework (code-first). Proposta de solução para o desafio proposto abaixo.


\## 📝 Desafio



\### 📌 Objetivo



Avaliar experiência prática com C#, .NET Core, Entity Framework, organização de código e boas práticas.



\### 🧪 Contexto



Criar um sistema de controle de acesso a um clube. Sócios podem acessar áreas como piscina, quadras e academia. Cada sócio possui um plano que define quais áreas ele pode acessar. Cada tentativa de acesso deve ser registrada, informando se foi bem-sucedida ou não.



\### 🧩 Tarefas



 Modelar entidades principais:

&nbsp; - Sócio

&nbsp; - Plano de acesso

&nbsp; - Área do clube

&nbsp; - Tentativa de acesso


 Desenvolver API REST funcional:

&nbsp; - Cadastro de sócios, planos e tentativas de acesso


 Regras obrigatórias:

&nbsp; - Um sócio só pode acessar áreas permitidas em seu plano.

&nbsp; - Cada tentativa de acesso deve registrar: sócio, área, data/hora e resultado (autorizado ou negado).

 Requisitos técnicos:

&nbsp; - Lógica de negócio separada da camada de API.

&nbsp; - Entity Framework code-first (SQLite em memória ou SQL Server local).

&nbsp; - Ao menos um teste automatizado validando a regra de acesso.

 
---
 
\## ⚙️ Tecnologias



\- .NET Core

\- Entity Framework Core

\- SQLite (In-Memory para testes)

\- xUnit (testes automatizados)

\- Swagger/OpenAPI (para documentação da API)



---

## 🏛 Padrões de Design e Arquitetura Implementados

O projeto foi desenvolvido adotando os seguintes padrões de design e princípios de arquitetura de software:

### Repository Pattern
- Implementado para abstrair a camada de acesso a dados
- Isola a lógica de acesso a dados do resto da aplicação
- Facilita a substituição do mecanismo de persistência
  
### Unit of Work
- Padrão implementado para gerenciar transações e o contexto do EF Core
- Garante que todas as operações em múltiplos repositórios sejam atomicas
  
### Domain-Driven Design (DDD)
- Separação clara entre:
    Camada de Domínio: Contém as entidades, value objects e regras de negócio

    Camada de Aplicação: Orquestra os casos de uso

    Camada de Infraestrutura: Implementação concreta de repositórios e acesso a dados

- Entidades ricas com comportamento (não apenas propriedades)
- Agregações claramente definidas (ex: Socio como raiz de agregação)
  
### Inversão de Dependência (DIP)
- Implementado através de interfaces para todos os serviços e repositórios
- Injeção de dependência via construtor em todas as classes
- Configuração no container DI no Program.cs:
  
### Clean Architecture
- Organização do projeto seguindo os princípios de Clean Architecture:
  
    Core (Domain): Entidades, interfaces, regras de negócio
  
    Application: Casos de uso, serviços, DTOs
  
    Infrastructure: Implementações concretas (EF Core, repositórios)
  
    Presentation: Controllers, API endpoints

### CQRS (Simplificado)
- Separação entre operações de:

     Consulta: Operações de leitura (ex: obter histórico de acessos)
  
     Comando: Operações de escrita (ex: registrar tentativa de acesso)
- Implementado através de serviços distintos para operações complexas

### SOLID (Simplificado)
- Single Responsibility: Cada classe tem uma única responsabilidade
- Open/Closed: Aberto para extensão, fechado para modificação
- Liskov Substitution: Interfaces podem ser substituídas por implementações
- Interface Segregation: Múltiplas interfaces específicas
- Dependency Inversion: Depender de abstrações, não de implementações

  
---

\## Como executar



### 1. Clone o repositório

```bash
git clone https://github.com/MiltonMPereira/ClubeApi.git
```

### 2. Restaure os pacotes e rode a API
```bash
dotnet restore
dotnet run
```
A API estará disponível em: 
```bash
http://localhost:5274
```
Para executar os testes use o comando
 ```bash
dotnet test
```

Os testes validam principalmente os CRUD's e a regra de acesso, garantindo que sócios acessem apenas áreas permitidas.


## Passo a passo para testar 


1\. Cadastrar áreas 

Endpoint: POST /Area
 

Exemplo:

```bash

{
  "nome": "Piscina Olímpica"
}
```


2\. Cadastrar planos

 
Endpoint: POST /Plano
 
Exemplo:

```bash
{
  "nome": "Plano Premium",
  "areasPermitidasIds": [1] // IDs das áreas criadas anteriormente
}
```


3\. Cadastrar sócio
 
Endpoint: POST /Socio
 
Exemplo:
```bash

{
  "nome": "João Silva",
  "planoId": 1
}
```
 
4\. Registrar tentativa de acesso
 
Endpoint: POST /Acesso/acessar
 
Exemplo: 
```bash
{
  "socioId": 1,
  "areaClubeId": 1
}
```
O retorno indicará se a tentativa foi Autorizada ou Negada.

Exemplos cURL

Cadastrar área: 

```bash curl -X POST "http://localhost:5274/Area" -H "Content-Type: application/json" -d "{\\"nome\\":\\"Piscina Olímpica\\"}" ``` 

Cadastrar plano: 

```bash curl -X POST "http://localhost:5274/Plano" -H "Content-Type: application/json" -d "{\\"nome\\":\\"Plano Premium\\",\\"areasPermitidasIds\\":\[1]}" ``` 

Cadastrar sócio: 

```bash curl -X POST "http://localhost:5274/Socio" -H "Content-Type: application/json" -d "{\\"nome\\":\\"João Silva\\",\\"planoId\\":1}" ``` 

Registrar acesso: 

```bash curl -X POST "http://localhost:5274/Acesso" -H "Content-Type: application/json" -d "{\\"socioId\\":1,\\"areaId\\":1}" ``` 


💡 Decisões de implementação

* Entity Framework Core Code-First: facilita evolução do banco de dados.
* SQLite In-Memory nos testes: elimina dependências externas, agiliza execução em CI/CD.
* Separação por camadas: Controllers expõem endpoints, Services contêm regras de negócio.
* xUnit: leve, rápido, padrão do ecossistema .NET.

✨ Sugestões de melhorias futuras

* Implementar autenticação e autorização (JWT)
* Aumentar cobertura de testes
* Implementar paginação nas listagens
