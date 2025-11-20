# 🚀 Nova Career API — Advanced Business Development  
API para gerenciamento de carreiras, trilhas de aprendizado, usuários e recomendações personalizadas.

---

## 📘 Sobre o Projeto

A **Nova Career API** é uma API RESTful construída com **.NET 8**, seguindo boas práticas de arquitetura em camadas, separação de responsabilidades e documentação interativa via Swagger.  

Este projeto faz parte da disciplina **Advanced Business Development with .NET** e representa a entrega final da Global Solution.

---

## 🏛️ Arquitetura e Design da Solução

A solução segue uma estrutura baseada em **Clean Architecture simplificada**, dividida em quatro camadas principais:

### 📂 Estrutura da Solução (NovaAPI.sln)

| Projeto         | Camada        | Responsabilidade |
|-----------------|----------------|------------------|
| **NovaUI**      | Presentation   | Ponto de entrada da aplicação (Web API). Contém Controllers, Program.cs, filtros e documentação. |
| **NovaBusiness**| Application    | Lógica de negócios, serviços e validações. Ponte entre UI e infraestrutura. |
| **NovaModel**   | Domain         | Entidades, DTOs, enums e interfaces. Núcleo do domínio. |
| **NovaData**    | Infrastructure | Acesso a dados (EF Core), repositórios e migrations. |

---

## 🔧 Decisões Técnicas

- **.NET 8 Web API**  
- **Entity Framework Core** como ORM  
- **Banco Oracle** (compatível FIAP)  
- **Swagger / OpenAPI** para documentação  
- **Repository Pattern** para desacoplamento  
- **DTOs** para transporte seguro de dados  
- **Tratamento de erros com ProblemDetails (RFC 7807)**  

---
# ATENÇÃO!!
## ⚙️ Configuração do Ambiente

Antes de rodar o projeto, você PRECISA configurar sua conexão com o banco:

### 📄 `appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "User Id=SEU_ID;Password=SEU_PASSWORD;Data Source=oracle.fiap.com.br:1521/ORCL;"
  }
}
```

> 🔐 **Atenção:** Se não, o projeto não funcionará.

---

## 🚀 Como Rodar o Projeto

### ✔️ Pré-requisitos
- .NET 8 SDK  
- Acesso ao Oracle FIAP  

### 1️⃣ Clonar e restaurar dependências

```bash
git clone <url-do-seu-repositorio>
cd NovaAPI
dotnet restore
```

### 2️⃣ Aplicar migrations

```bash
dotnet ef database update --project NovaData --startup-project NovaUI
```

### 3️⃣ Executar a API

```bash
dotnet run --project NovaUI
```

A API estará disponível em:

👉 **https://localhost:7283/swagger**

---

## 📖 Documentação — Endpoints da API

<img width="1637" height="864" alt="image" src="https://github.com/user-attachments/assets/8df7ca93-d31d-40b0-ae35-b46dd3259ab6" />

<img width="1616" height="744" alt="image" src="https://github.com/user-attachments/assets/8e546ed4-a153-4f86-a614-2ae88d5eacee" />

<img width="1630" height="724" alt="image" src="https://github.com/user-attachments/assets/546aec3f-c331-4b0c-a0f8-61da85cf247d" />

A documentação completa estará no Swagger:

👉 **https://localhost:7283/swagger**

### 🔹 Principais Recursos

---

### 👤 Usuários (`/api/Usuarios`)

- `GET /api/Usuarios`  
- `GET /api/Usuarios/{id}`  
- `GET /api/Usuarios/search`  
- `POST /api/Usuarios`  
- `PUT /api/Usuarios/{id}`  
- `DELETE /api/Usuarios/{id}`  

---

### 🧩 Skills (`/api/Skills`)

- `GET /api/Skills`  
- `GET /api/Skills/usuario/{usuarioId}`  
- `GET /api/Skills/{id}`  
- `POST /api/Skills/usuario/{usuarioId}`  
- `PUT /api/Skills/{id}`  
- `DELETE /api/Skills/{id}`  

---

### 🛣️ Trilhas (`/api/Trilhas`)

- `GET /api/Trilhas`  
- `GET /api/Trilhas/{id}`  
- `POST /api/Trilhas`  
- `PUT /api/Trilhas/{id}`  
- `DELETE /api/Trilhas/{id}`  

---

### 🎯 Recomendações (`/api/Recomendacoes`)

- `GET /api/Recomendacoes/usuario/{usuarioId}`  

---

## 🧠 Regras de Negócio

### 👤 Usuário

- E-mails são únicos no sistema.  
- Usuários **não podem ser excluídos** se possuírem trilhas ativas vinculadas.  

### 🧩 Recomendação

- As recomendações são geradas com base no **gap de skills** entre o usuário e a trilha desejada.

---

## ⚠️ Tratamento de Erros

- `400 Bad Request` → erros de validação  
- `404 Not Found` → recurso não encontrado  
- `ProblemDetails` → respostas padronizadas conforme RFC 7807  

---

## 🧪 Exemplos de Uso (cURL)

### Criar usuário

```bash
curl -X POST "https://localhost:7283/api/Usuarios" \
-H "Content-Type: application/json" \
-d '{
  "nome": "João Silva",
  "email": "joao@email.com",
  "nivel": "Junior"
}'
```

### Obter recomendações de um usuário

```bash
curl -X GET "https://localhost:7283/api/Recomendacoes/usuario/1"
```

---

## 👨‍💻 Desenvolvido por  
**JoaoGFG**
