# RabbitMQ Messaging Implementation

Este documento explica como o RabbitMQ foi integrado ao projeto FIAP-Contatos para implementar mensageria assíncrona.

## Arquitetura

O projeto agora inclui:

- **RabbitMQ Server**: Container Docker para o broker de mensagens
- **Message Publisher**: No `cadastro-api` para publicar mensagens
- **Message Consumer**: No `persistencia-api` para consumir mensagens

## Funcionamento

### Fluxo de Criação de Contatos (Assíncrono via RabbitMQ)

1. Cliente faz POST para `/api/contatos` no `cadastro-api`
2. `cadastro-api` publica mensagem `ContatoCreatedMessage` no RabbitMQ
3. `cadastro-api` retorna HTTP 202 (Accepted) imediatamente
4. `persistencia-api` consome a mensagem em background
5. `persistencia-api` persiste o contato no banco de dados

### Outras Operações (Síncronas via HTTP)

- GET, PUT, DELETE continuam usando comunicação HTTP direta entre as APIs

## Configuração

### Docker Compose

O RabbitMQ foi adicionado como um novo serviço:

```yaml
rabbitmq:
  image: rabbitmq:3-management
  container_name: fiap.rabbitmq
  ports:
    - "5672:5672"     # RabbitMQ
    - "15672:15672"   # Management UI
  environment:
    - RABBITMQ_DEFAULT_USER=admin
    - RABBITMQ_DEFAULT_PASS=admin123
```

### Configuração das APIs

Ambas as APIs têm configuração RabbitMQ em `appsettings.json`:

```json
{
  "RabbitMq": {
    "ConnectionString": "amqp://admin:admin123@fiap.rabbitmq:5672/",
    "ExchangeName": "contatos-exchange",
    "QueueName": "contatos-queue",
    "RoutingKey": "contato.created"
  }
}
```

## Como Testar

1. **Iniciar os serviços:**
   ```bash
   docker-compose up
   ```

2. **Acessar o RabbitMQ Management UI:**
   - URL: http://localhost:15672
   - Usuário: admin
   - Senha: admin123

3. **Criar um contato via API:**
   ```bash
   curl -X POST http://localhost:8082/api/contatos \
     -H "Content-Type: application/json" \
     -d '{
       "nome": "João Silva",
       "telefone": "99999-9999",
       "email": "joao@example.com",
       "ddd": 11
     }'
   ```

4. **Verificar no RabbitMQ Management UI:**
   - Vá para a aba "Queues"
   - Veja as mensagens sendo processadas na queue `contatos-queue`

5. **Verificar no banco de dados:**
   - O contato deve ser criado automaticamente pelo consumer

## Benefícios da Implementação

1. **Desacoplamento**: As APIs não dependem mais de comunicação síncrona
2. **Escalabilidade**: Múltiplos consumers podem processar mensagens
3. **Resiliência**: Mensagens são persistidas até serem processadas
4. **Performance**: O `cadastro-api` responde imediatamente sem esperar o banco

## Extensões Futuras

Esta implementação pode ser estendida para:

- Adicionar mais operações assíncronas (UPDATE, DELETE)
- Implementar eventos de domínio
- Adicionar dead letter queues para tratamento de erros
- Implementar padrões como Saga para transações distribuídas