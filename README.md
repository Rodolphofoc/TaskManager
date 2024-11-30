# TaskManager

**TaskManager** é uma aplicação de gerenciamento de tarefas desenvolvida com **ASP.NET Core** e utilizando **SQL Server** como banco de dados. O projeto é containerizado com **Docker** para facilitar a execução e a configuração do ambiente.

## Requisitos

Antes de rodar o projeto, você precisará ter o **Docker** instalado em sua máquina. Se ainda não o fez, siga as instruções de instalação do Docker [aqui](https://docs.docker.com/get-docker/).

Além disso, é necessário ter o **.NET SDK** instalado localmente para a construção da aplicação, caso deseje executar sem o Docker.

## Funcionalidades

- **CRUD de Tarefas**: A API permite a criação, leitura, atualização e exclusão de tarefas.
- **Swagger UI**: Interface gráfica para testar os endpoints da API.
- **Migrações Automáticas**: O banco de dados SQL Server é configurado e migrado automaticamente.

## Estrutura do Projeto

O projeto está dividido nas seguintes camadas:

- **Application**: Contém a lógica de negócios da aplicação.
- **Domain**: Define as entidades e lógica de domínio.
- **Infrastructure**: Implementa a interação com o banco de dados e outros serviços.
- **CrossCutting**: Funções transversais, como logging e validações.
- **Apresentation**: Camada responsável pela API e pela interação com o cliente.
- **Tests**: Contém os testes da aplicação.

## Passos para rodar o projeto no Docker

### 1. Clonar o Repositório

Clone o repositório para sua máquina local:

```bash
git clone https://github.com/Rodolphofoc/TaskManager.git
cd TaskManager
