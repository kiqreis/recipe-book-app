# 📚 Recipe Book App

Sistema moderno de gerenciamento de receitas culinárias, desenvolvido com .NET 8 e utilizando arquitetura limpa voltada para escalabilidade, testabilidade e boas práticas de desenvolvimento. A aplicação permite o cadastro de receitas, planejamento de refeições, autenticação via Google e muito mais.

---

## 🚀 Funcionalidades

- ✅ Cadastro, edição e exclusão de receitas
- ✅ Upload e validação de arquivos de imagem (Blob Storage + FileTypeChecker)
- ✅ Autenticação via Google (OAuth 2.0)
- ✅ Integração com Service Bus
- ✅ Planejamento de refeições
- ✅ Geração de dados fictícios com Bogus
- ✅ API documentada com Swagger (Swashbuckle)
- ✅ Suporte a internacionalização e mensagens amigáveis

---

## 🧱 Tecnologias e Bibliotecas

| Categoria | Tecnologias |
|----------|-------------|
| **Back-end** | .NET 8, ASP.NET Core |
| **Validação** | FluentValidation |
| **ORM/DAO** | EF Core, Dapper |
| **Testes** | FluentAssertions, xUnit, Bogus |
| **Autenticação** | OAuth 2.0, Google Identity, IdentityModel |
| **DevOps** | Docker, Dockerfile, Docker Compose |
| **Armazenamento** | Azure Blob Storage |
| **Mensageria** | Azure Service Bus |
| **Documentação** | Swashbuckle (Swagger/OpenAPI) |
| **Mapeamento** | AutoMapper |
| **Identificadores** | Sqids |
| **Outros** | FileTypeChecker, Design Pattern Driven Architecture |

---

## 📁 Estrutura do Projeto

```
src/
├── MyRecipeBook.Api              # API RESTful com autenticação e middlewares
├── MyRecipeBook.Application     # Casos de uso e validações com FluentValidation
├── MyRecipeBook.Domain          # Entidades e interfaces
├── MyRecipeBook.Infrastructure  # Dapper, EF Core, ServiceBus, Blob Storage
├── MyRecipeBook.Communication   # Contratos de requisição/resposta
├── MyRecipeBook.Exceptions      # Tratamento centralizado de exceções
test/
├── MyRecipeBook.UnitTests       # Testes com FluentAssertions e Bogus
```

---

## 🐳 Como Executar com Docker

```bash
# Clone o repositório
git clone https://github.com/kiqreis/recipe-book-app.git
cd recipe-book-app

# Build e run
docker-compose up --build
```

A aplicação estará disponível em: [http://localhost:8080/swagger](http://localhost:8080/swagger)

---

## 📦 Pré-requisitos

- .NET 8 SDK
- Docker e Docker Compose
- SQL Server (Docker já configura a instância local)

---

## 🧪 Testes

Execute os testes com:

```bash
dotnet test
```

---

## 📜 Licença

Distribuído sob a licença MIT. Veja `LICENSE` para mais informações.
