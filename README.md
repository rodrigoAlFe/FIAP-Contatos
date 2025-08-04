# FIAP-Contatos
Projeto Pós Tech Arquitetura de Sistemas .NET - FIAP

## Passo a Passo: Como Rodar o Projeto

### Pré-requisitos
1. Certifique-se de que tem o **Docker** e o **Docker Compose** instalados na sua máquina.
2. Verifique a arquitetura do seu sistema (AMD64 ou ARM64) para utilizar o perfil correto no `docker-compose`.

### Executando o Projeto com Docker Compose
O projeto utiliza **perfis do Docker Compose** para suportar diferentes arquiteturas (AMD64 e ARM64). Siga os passos abaixo para iniciar o ambiente:

#### Para Arquitetura AMD64:
1. Abra um terminal na pasta raiz do projeto, onde está localizado o arquivo `docker-compose.yml`.
2. Execute o seguinte comando:
  ```bash
   docker-compose --profile amd64 up -d
  ```
   Esse comando irá construir as imagens e iniciar os containers em background para a arquitetura AMD64.

#### Para Arquitetura ARM64:

1. Abra um terminal na pasta raiz do projeto.
2. Execute o seguinte comando:
  ```bash
   docker-compose --profile amd64 up -d
  ```
   Esse comando irá construir as imagens e iniciar os containers em background para a arquitetura ARM64.

### Acessando os Serviços pelo docker
Após iniciar os containers pelo docker, os serviços estarão disponíveis nas seguintes portas:

- Cadastro API: http://localhost:8082/swagger
- Persistência API: http://localhost:8080/swagger
- RabbitMq: http://localhost:15672/
- Prometheus: http://localhost:9090/
- Grafana: http://localhost:3000/

### Estrutura de Projetos na Solução
A solução está organizada em duas pastas principais: src e test.

1. Projetos Principais (src)
Os projetos localizados na pasta src formam o núcleo funcional da aplicação:

- persistencia-api: API responsável por gerenciar os dados de contatos, incluindo operações de CRUD e integração com o banco de dados.
- cadastro-api: API que consome a persistencia-api e publica mensagens no RabbitMQ para processamento assíncrono.

2. Projetos de Testes (test)
Os projetos na pasta test garantem a qualidade do código por meio de testes automatizados:

- Domain.Test: Testes unitários para validar as regras de negócio na camada de domínio.
- Infrastructure.Test: Testes de integração para validar repositórios e interações com o banco de dados.

### Estrutura de Pastas
A estrutura do projeto é organizada da seguinte forma:
```
FIAP-Contatos/
├── docker/
│   ├── [docker-compose.yml](http://_vscodecontentref_/1)
│   ├── amd64/
│   │   └── Dockerfile
│   └── arm64/
│       └── Dockerfile
├── src/
│   ├── cadastro-api/
│   └── persistencia-api/
├── test/
│   ├── Domain.Test/
│   └── Infrastructure.Test/
```