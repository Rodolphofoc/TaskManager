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

1. Clone o repositório para sua máquina local:

    ```bash
    git clone https://github.com/Rodolphofoc/TaskManager.git
    cd TaskManager
    ```

2. Certifique-se de que o **Docker** e **Docker Compose** estão instalados.

3. Rode o comando abaixo para iniciar os containers com o Docker Compose:

    ```bash
    docker-compose up --build
    ```

    Esse comando irá construir a imagem e rodar os containers necessários: o **SQL Server** e a **API do TaskManager**.

4. Aguarde o Docker terminar de construir e iniciar os containers.

## Acessando a aplicação

- Após a execução do comando `docker-compose up --build`, a API estará disponível na URL `http://localhost:80/`.
- O **Swagger UI** poderá ser acessado na URL `http://localhost:80/swagger`.

## Observações

- O banco de dados **SQL Server** será automaticamente configurado e a migração será aplicada ao rodar o projeto.
- O Docker Compose define as variáveis de ambiente necessárias para a configuração da conexão com o banco de dados.
- O projeto é configurado para rodar em um ambiente de **Desenvolvimento** por padrão.

## Comandos úteis

- Para parar e remover os containers, utilize:

    ```bash
    docker-compose down
    ```

- Para visualizar os logs de um container em execução:

    ```bash
    docker logs <nome_do_container>
    ```

- Para construir as imagens novamente e reiniciar os containers:

    ```bash
    docker-compose up --build
    ```

## Segunda etapa, necessidade de planejamento com PO

É necessário identificar na regra de negócio quem pode deletar uma tarefa?
Quais as formas de autenticação? JWT? OAuth?
Qualquer usuário pode deletar um projeto?
Há limites para o número de projetos existentes?
Há limites para o número de tarefas em andamento para um usuário?

## Terceira etapa, possiveis melhorias no projeto

O projeto pode ser ajustado para um CQRS para melhor desempenho.
Inclusão de Logs 
Observability em alguma cloud ajudaria em caso de erro e desempenho



