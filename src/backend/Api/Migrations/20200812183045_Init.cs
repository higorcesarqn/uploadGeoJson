﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Api.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:pgcrypto", ",,")
                .Annotation("Npgsql:PostgresExtension:postgis", ",,")
                .Annotation("Npgsql:PostgresExtension:tablefunc", ",,");

            migrationBuilder.CreateTable(
                name: "tb_geojson",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false, defaultValueSql: "gen_random_uuid()"),
                    data_inclusao = table.Column<DateTime>(nullable: false, defaultValueSql: "Now()"),
                    data_atualizacao = table.Column<DateTime>(nullable: true),
                    file_name = table.Column<string>(nullable: true),
                    size = table.Column<long>(nullable: false),
                    row = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tb_geojson", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tb_empreendimento",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false, defaultValueSql: "gen_random_uuid()"),
                    data_inclusao = table.Column<DateTime>(nullable: false, defaultValueSql: "Now()"),
                    data_atualizacao = table.Column<DateTime>(nullable: true),
                    geometry = table.Column<Geometry>(nullable: true),
                    empreedimento = table.Column<string>(nullable: true),
                    lote = table.Column<string>(nullable: true),
                    numero_cadastro = table.Column<string>(nullable: true),
                    area = table.Column<string>(nullable: true),
                    area_desapropriar = table.Column<string>(nullable: true),
                    numero_processo = table.Column<string>(nullable: true),
                    localizacao = table.Column<string>(nullable: true),
                    id_geojson = table.Column<Guid>(nullable: true),
                    row = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tb_empreendimento", x => x.id);
                    table.ForeignKey(
                        name: "fk_tb_empreendimento_tb_geojson_id_geojson",
                        column: x => x.id_geojson,
                        principalTable: "tb_geojson",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_tb_empreendimento_empreedimento",
                table: "tb_empreendimento",
                column: "empreedimento",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "index_id",
                table: "tb_empreendimento",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_tb_empreendimento_id_geojson",
                table: "tb_empreendimento",
                column: "id_geojson");

            migrationBuilder.CreateIndex(
                name: "ix_tb_empreendimento_row",
                table: "tb_empreendimento",
                column: "row",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_tb_geojson_row",
                table: "tb_geojson",
                column: "row",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_empreendimento");

            migrationBuilder.DropTable(
                name: "tb_geojson");
        }
    }
}