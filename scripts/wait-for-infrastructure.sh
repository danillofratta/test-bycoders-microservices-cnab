#!/bin/bash

echo "🔍 Verificando status da infraestrutura..."

# Verificar se PostgreSQL está respondendo
echo "📊 Verificando PostgreSQL..."
until PGPASSWORD=root psql -h postgres -U admin -d cnab_consumer -c '\q'; do
  >&2 echo "PostgreSQL não está disponível - aguardando..."
  sleep 2
done
echo "✅ PostgreSQL está pronto!"

# Verificar se a tabela transactions existe
echo "📋 Verificando tabelas do banco..."
TABLE_EXISTS=$(PGPASSWORD=root psql -h postgres -U admin -d cnab_consumer -t -c "SELECT COUNT(*) FROM information_schema.tables WHERE table_name='transactions' AND table_schema='public';")
if [ "$TABLE_EXISTS" -gt 0 ]; then
    echo "✅ Tabela transactions existe!"
else
    echo "❌ Tabela transactions não encontrada!"
    exit 1
fi

# Verificar se RabbitMQ está respondendo  
echo "🐰 Verificando RabbitMQ..."
until curl -f http://rabbitmq:15672/api/whoami -u guest:guest > /dev/null 2>&1; do
  >&2 echo "RabbitMQ não está disponível - aguardando..."
  sleep 2
done
echo "✅ RabbitMQ está pronto!"

echo "🎉 Toda a infraestrutura está pronta!"