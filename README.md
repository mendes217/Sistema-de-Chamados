<div align ="center;">
    <img src="https://github.com/user-attachments/assets/9358e3a7-cd9d-4c96-bdf6-80bbe30bd593" style="width: 180px; height: 150px;" alt="Logo do sistema;align: center;">
    <p >
    Sistema de Chamados e Respostas, Integrado com Inteligência Artificial (IA).
    </p>
</div>

### 📕 Sobre
    
**TecPorte** Sistema de Chamados desenvolvido para Workshop
 da universidade Unip-Tatuape.

### 🎯 Objetivo
A **Tec Porte** é uma empresa voltada para o desenvolvimento de soluções tecnológicas 
aplicadas a institutos acadêmicos. Seu objetivo é promover uma melhoria significativa na 
comunicação entre a instituição, seus estudantes, professores e colaboradores, tornando 
as interações mais eficientes, acessíveis e seguras.
além disso esse  projeto foi desenvolvido com o objetivo de:
- Consolidar conhecimentos em desenvolvimento backend
- Aplicar conceitos de API REST
- Trabalhar regras de negócio reais
- Demonstrar habilidades práticas para vagas de estágio ou desenvolvedor júnior 
### 🛠️ Tecnologias Utilizadas

- C# (.NET 8)
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT (JSON Web Token) para autenticação
- Swagger (OpenAPI) para documentação da API
### 🚀 Funcionalidades

- Cadastro e autenticação de usuários
- Controle de permissões (Usuário, Técnico e Administrador)
- Abertura de chamados de suporte
- Alteração de status dos chamados (Aberto, Em andamento, Resolvido)
- Comentários e histórico de interações
- Filtros por status, prioridade e data
- Registro de data de criação e atualização dos chamados

---

### 🗂️ Modelagem de Dados

Principais entidades do sistema:
- Usuário
- Perfil
- Chamado
- Comentário

O relacionamento entre as entidades foi pensado para refletir cenários reais de um sistema de suporte técnico.

---

### 🔐 Autenticação e Segurança

A autenticação da API é realizada utilizando JWT (JSON Web Token).

Após o login, o token deve ser enviado em todas as requisições protegidas no header:

As permissões de acesso variam conforme o perfil do usuário.

---

## ▶️ Como Executar o Projeto

### Pré-requisitos
- .NET 8 SDK
- SQL Server
- Visual Studio ou Visual Studio Code

### Passos para execução

```bash
git clone https://github.com/mendes217/Sistema-de-Chamados.git
cd Sistema-Chamados-api
dotnet restore
dotnet run





