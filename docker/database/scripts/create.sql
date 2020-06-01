CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

CREATE EXTENSION IF NOT EXISTS pgcrypto;
CREATE EXTENSION IF NOT EXISTS postgis;
CREATE EXTENSION IF NOT EXISTS tablefunc;

CREATE TABLE tb_geo (
    id uuid NOT NULL DEFAULT (gen_random_uuid()),
    data_inclusao timestamp without time zone NOT NULL DEFAULT (Now()),
    data_atualizacao timestamp without time zone NULL,
    geometry geometry NULL,
    properties json NULL,
    row bigserial NOT NULL,
    CONSTRAINT pk_tb_geo PRIMARY KEY (id)
);

CREATE UNIQUE INDEX index_id ON tb_geo (id);
CREATE UNIQUE INDEX ix_tb_geo_row ON tb_geo (row);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20200601200435_DbInit', '3.1.4');
