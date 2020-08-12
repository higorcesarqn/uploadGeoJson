CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

CREATE EXTENSION IF NOT EXISTS pgcrypto;
CREATE EXTENSION IF NOT EXISTS postgis;
CREATE EXTENSION IF NOT EXISTS tablefunc;

CREATE TABLE tb_geojson (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    data_inclusao timestamp without time zone NOT NULL DEFAULT (Now()),
    data_atualizacao timestamp without time zone NULL,
    file_name text NULL,
    size bigint NOT NULL,
    row bigserial NOT NULL,
    CONSTRAINT pk_tb_geojson PRIMARY KEY (id)
);

CREATE TABLE tb_empreendimento (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    data_inclusao timestamp without time zone NOT NULL DEFAULT (Now()),
    data_atualizacao timestamp without time zone NULL,
    geometry geometry NULL,
    empreedimento text NULL,
    lote text NULL,
    numero_cadastro text NULL,
    area text NULL,
    area_desapropriar text NULL,
    numero_processo text NULL,
    localizacao text NULL,
    id_geojson uuid NULL,
    row bigserial NOT NULL,
    CONSTRAINT pk_tb_empreendimento PRIMARY KEY (id),
    CONSTRAINT fk_tb_empreendimento_tb_geojson_id_geojson FOREIGN KEY (id_geojson) REFERENCES tb_geojson (id) ON DELETE CASCADE
);

CREATE UNIQUE INDEX ix_tb_empreendimento_empreedimento ON tb_empreendimento (empreedimento);

CREATE UNIQUE INDEX index_id ON tb_empreendimento (id);

CREATE INDEX ix_tb_empreendimento_id_geojson ON tb_empreendimento (id_geojson);

CREATE UNIQUE INDEX ix_tb_empreendimento_row ON tb_empreendimento (row);

CREATE UNIQUE INDEX ix_tb_geojson_row ON tb_geojson (row);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20200812183045_Init', '3.1.7');

