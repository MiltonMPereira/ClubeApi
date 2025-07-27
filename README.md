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
