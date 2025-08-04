# FIAP-Contatos
Projeto Pós Tech Arquitetura de Sistemas .NET - FIAP

## Passo a Passo: Como Rodar o Projeto

### Executando o Projeto com Kubernetes
#### Pré-requisitos
1. Certifique-se de que o **Minikube** e o **Docker Desktop** estão instalados e configurados na sua máquina.
2. Verifique a arquitetura do seu sistema (AMD64 ou ARM64) para aplicar os deployments corretos.
#### Configurando o Minikube
1. **Inicie o Minikube** com o driver Docker:
   ```bash
   minikube start --driver=docker
   ```
2. Configurando o docker local para usar o Minikube
  ```bash
    eval $(minikube docker-env)
  ```
3. Verifique se o cluster está ativo:
  ```bash
    kubectl cluster-info
  ```
#### Aplicando a Estrutura do Kubernetes

1. Crie o namespace **fiap-app** para organizar os recursos:
  ```bash
    kubectl apply -f k8s/namespace.yaml
  ```

2. Aplique o arquivo de configuração para variáveis de ambiente:
  ```bash
    kubectl apply -f k8s/configmap.yaml
    kubectl apply -f k8s/configmap-prometheus.yaml
  ```
3. Persistent Volume Claims (PVCs)
Crie os volumes persistentes para os serviços:
  ```bash
    kubectl apply -f k8s/pvc-sqlserver.yaml
    kubectl apply -f k8s/pvc-rabbitmq.yaml
    kubectl apply -f k8s/pvc-prometheus.yaml
    kubectl apply -f k8s/pvc-grafana.yaml
    kubectl apply -f k8s/pvc-persistencia.yaml
    kubectl apply -f k8s/pvc-cadastro.yaml
  ```
4. Deployments e Services
Aplique os deployments e serviços para os componentes compartilhados
  ```bash
    kubectl apply -f k8s/deployment-sqlserver.yaml
    kubectl apply -f k8s/service-sqlserver.yaml

    kubectl apply -f k8s/deployment-rabbitmq.yaml
    kubectl apply -f k8s/service-rabbitmq.yaml

    kubectl apply -f k8s/deployment-prometheus.yaml
    kubectl apply -f k8s/service-prometheus.yaml

    kubectl apply -f k8s/deployment-grafana.yaml
    kubectl apply -f k8s/service-grafana.yaml
  ```
#### Deployments Especificos por Arquitetura
Aplique o deployments e serviços para **persistencia-api** e **cadastro-api**:
##### Para Arquitetura AMD64
  ```bash
      kubectl apply -f k8s/deployment-persistencia-amd64.yaml
      kubectl apply -f k8s/service-persistencia.yaml

      kubectl apply -f k8s/deployment-cadastro-amd64.yaml
      kubectl apply -f k8s/service-cadastro.yaml
  ```
##### Para Arquitetura ARM64
  ```bash
      kubectl apply -f k8s/deployment-persistencia-arm64.yaml
      kubectl apply -f k8s/service-persistencia.yaml

      kubectl apply -f k8s/deployment-cadastro-arm64.yaml
      kubectl apply -f k8s/service-cadastro.yaml
  ```
#### Configurando o Ingress
1. Certifique-se que o **ingress Controller** está habilitado no Minikube:
  ```bash
    minikube addons enable ingress
  ```

2. Aplique o arquivo de configuração do ingress:
  ```bash
    kubectl apply -f k8s/ingress-fiap-app.yaml
  ```
3. Conecte ao LoadBalancer services
  ```bash
    minikube tunnel
  ```
  Note que o terminal ficará bloqueado pela execução do comando, caso feche o terminal o pare o comando, o tunnel será fechado.

4. Atualize o arquivo **/etc/hosts**:
  ```bash
    127.0.0.1 cadastro-api.local grafana.local persistencia-api.local prometheus.local rabbitmq.local
  ```

#### Acessando os Serviços:
Após configurar o Ingress, os serviços estarão disponíveis nos seguintes domínios:

  - Cadastro API: http://cadastro-api.local
  - Persistência API: http://persistencia-api.local
  - RabbitMQ: http://rabbitmq.local
  - Prometheus: http://prometheus.local
  - Grafana: http://grafana.local

### Executando o Projeto com Docker Compose

#### Pré-requisitos
1. Certifique-se de que tem o **Docker** e o **Docker Compose** instalados na sua máquina.
2. Verifique a arquitetura do seu sistema (AMD64 ou ARM64) para utilizar o perfil correto no `docker-compose`.
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

#### Acessando os Serviços pelo docker
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
├── k8s/
├── docker/
│   ├── docker-compose.yml
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