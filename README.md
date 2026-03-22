
🛒 Catálogo de Produtos - Desafio Técnico Agilean

  Este projeto é uma aplicação Full Stack de Catálogo de Produtos desenvolvida como parte
  do processo seletivo da Agilean. A solução contempla uma API RESTful robusta.

  🚀 Tecnologias Utilizadas

  Backend:
   * .NET 8 (C#)
   * Entity Framework Core (Code First) com SQLite.
   * AutoMapper: Mapeamento automático entre Entidades e DTOs para evitar exposição do
     domínio.
   * FluentValidation: Validação de dados desacoplada e automática.
   * xUnit + Moq + FluentAssertions: Testes Unitários cobrindo serviços e validadores.
   * Swagger/OpenAPI: Documentação interativa refinada com comentários XML.

  📋 Funcionalidades Implementadas

   * [x] CRUD Completo: Gestão total de produtos com persistência em SQLite.
   * [x] Soft Delete (Remoção Lógica): Produtos marcados como inativos em vez de excluídos
     permanentemente.
   * [x] Global Exception Handling: Middleware centralizado para tratamento de erros e
     respostas JSON padronizadas.
   * [x] Filtros e Buscas: Filtragem dinâmica por nome e categoria diretamente no banco de
     dados.
   * [x] Dashboard Analítico: Visão geral com métricas de negócio e alertas de estoque
     baixo.
   * [x] Diferencial - Paginação no Servidor: Implementação de Skip e Take com metadados
     enviados via Header HTTP (X-Pagination).
 

  ⚙️ Instruções de Instalação e Execução

  Rodando o Backend (API)
   1. Navegue até a pasta: cd Catalogo_loja
   2. Restaure e execute: dotnet run
      - API: https://localhost:7038
      - Swagger: https://localhost:7038/swagger

  Rodando os Testes
   1. Navegue até a pasta de testes: cd Catalogo_loja.Tests
   2. Execute: dotnet test


  🧠 Decisões Técnicas e Arquitetura

  1. Separação de Responsabilidades (SOLID)
  O projeto foi estruturado seguindo o princípio da responsabilidade única. Utilizamos a
  camada de Repository para isolar o acesso ao banco de dados e a camada de Service para
  orquestrar a lógica de negócio e mapeamentos, garantindo um código testável e de fácil
  manutenção.

  2. Inversão de Dependência
  Todas as dependências (Repositories e Services) são resolvidas via Injeção de Dependência
  nativa do .NET, utilizando interfaces para permitir o desacoplamento e a criação de mocks
  durante os testes unitários.

  3. Comunicação Frontend-Backend
  Para a paginação, optamos por não alterar o corpo do JSON retornado (mantendo a
  simplicidade para o front), mas sim utilizar o cabeçalho HTTP X-Pagination. O backend foi
  configurado via CORS para expor especificamente este header para o cliente React.

  4. Segurança e Resiliência
  A aplicação conta com um middleware de exceção que intercepta falhas e retorna mensagens
  amigáveis, além de utilizar o FluentValidation para garantir que apenas dados válidos
  cheguem à camada de persistência.


 🧠 Sugestões de melhoria ou oque eu implementaria se tivesse mais tempo 

  1- dockerização da aplicação
  2- Logging

           
