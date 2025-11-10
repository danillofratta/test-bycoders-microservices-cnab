#!/bin/bash
set -e

echo "Inicializando banco de dados CNAB..."

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
    -- Criar sequence se não existir
    DO \$\$ BEGIN
        IF NOT EXISTS (SELECT 1 FROM pg_sequences WHERE schemaname = 'public' AND sequencename = 'transactions_id_seq') THEN
            CREATE SEQUENCE transactions_id_seq
                START WITH 1
                INCREMENT BY 1
                NO MINVALUE
                NO MAXVALUE
                CACHE 1;
        END IF;
    END \$\$;

    -- Criar tabela transactions se não existir
    CREATE TABLE IF NOT EXISTS public.transactions
    (
        id integer NOT NULL DEFAULT nextval('transactions_id_seq'::regclass),
        type integer NOT NULL,
        nature character varying(20) COLLATE pg_catalog."default" NOT NULL,
        value numeric(18,2) NOT NULL,
        signed_value numeric(18,2) NOT NULL,
        cpf character varying(11) COLLATE pg_catalog."default" NOT NULL,
        card character varying(12) COLLATE pg_catalog."default" NOT NULL,
        occurred_at timestamp with time zone NOT NULL,
        store_owner character varying COLLATE pg_catalog."default",
        store_name character varying COLLATE pg_catalog."default",
        CONSTRAINT transactions_pkey PRIMARY KEY (id),
        CONSTRAINT uq_transaction UNIQUE (type, value, cpf, card, occurred_at, store_name, store_owner)
    );

    -- Criar índice se não existir
    CREATE UNIQUE INDEX IF NOT EXISTS uq_transaction_idx
        ON public.transactions USING btree
        (type ASC NULLS LAST, value ASC NULLS LAST, cpf ASC NULLS LAST, 
         card ASC NULLS LAST, occurred_at ASC NULLS LAST, 
         lower(btrim(store_name::text)) ASC NULLS LAST, 
         lower(btrim(store_owner::text)) ASC NULLS LAST);

    -- Verificar se a tabela foi criada
    DO \$\$ 
    BEGIN
        IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'transactions' AND table_schema = 'public') THEN
            RAISE NOTICE 'Tabela transactions criada com sucesso!';
        ELSE
            RAISE EXCEPTION 'Falha ao criar tabela transactions';
        END IF;
    END \$\$;
EOSQL

echo "Banco de dados CNAB inicializado com sucesso!"