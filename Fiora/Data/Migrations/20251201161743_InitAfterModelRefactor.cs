using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiora.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitAfterModelRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreAdmin = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CorreoAdmin = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PasswordAdmin = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Arreglo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemporadaArreglo = table.Column<int>(type: "int", nullable: false),
                    OcasionArreglo = table.Column<int>(type: "int", nullable: false),
                    NombreArreglo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TipoArreglo = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    PrecioArreglo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TiempoEstimadoHoras = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arreglo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCliente = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CorreoCliente = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PasswordCliente = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TelefonoCliente = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DireccionCliente = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FechaRegistroCliente = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inventario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoItem = table.Column<int>(type: "int", nullable: false),
                    NombreProducto = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reporte",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Periodo = table.Column<int>(type: "int", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalPedidos = table.Column<int>(type: "int", nullable: false),
                    TotalVentas = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TicketPromedio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PedidosCompletados = table.Column<int>(type: "int", nullable: false),
                    PedidosPendientes = table.Column<int>(type: "int", nullable: false),
                    PedidosCancelados = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reporte", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pedido",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OcasionPedido = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    NombreCliente = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    DireccionEnvio = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MensajePedido = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModoPago = table.Column<int>(type: "int", nullable: false),
                    MontoTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EstadoPedido = table.Column<int>(type: "int", nullable: false),
                    Servicio = table.Column<bool>(type: "bit", nullable: false),
                    FechaHoraEntrega = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DireccionEvento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TematicaEvento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ColoresEvento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdminId = table.Column<int>(type: "int", nullable: true),
                    ArregloId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedido", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pedido_Admin_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admin",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pedido_Arreglo_ArregloId",
                        column: x => x.ArregloId,
                        principalTable: "Arreglo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pedido_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArregloInventario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArregloId = table.Column<int>(type: "int", nullable: false),
                    InventarioId = table.Column<int>(type: "int", nullable: false),
                    CantidadNecesaria = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArregloInventario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArregloInventario_Arreglo_ArregloId",
                        column: x => x.ArregloId,
                        principalTable: "Arreglo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArregloInventario_Inventario_InventarioId",
                        column: x => x.InventarioId,
                        principalTable: "Inventario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PedidoInventario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PedidoId = table.Column<int>(type: "int", nullable: false),
                    InventarioId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoInventario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidoInventario_Inventario_InventarioId",
                        column: x => x.InventarioId,
                        principalTable: "Inventario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidoInventario_Pedido_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedido",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArregloInventario_ArregloId",
                table: "ArregloInventario",
                column: "ArregloId");

            migrationBuilder.CreateIndex(
                name: "IX_ArregloInventario_InventarioId",
                table: "ArregloInventario",
                column: "InventarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_AdminId",
                table: "Pedido",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_ArregloId",
                table: "Pedido",
                column: "ArregloId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_ClienteId",
                table: "Pedido",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoInventario_InventarioId",
                table: "PedidoInventario",
                column: "InventarioId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoInventario_PedidoId",
                table: "PedidoInventario",
                column: "PedidoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArregloInventario");

            migrationBuilder.DropTable(
                name: "PedidoInventario");

            migrationBuilder.DropTable(
                name: "Reporte");

            migrationBuilder.DropTable(
                name: "Inventario");

            migrationBuilder.DropTable(
                name: "Pedido");

            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "Arreglo");

            migrationBuilder.DropTable(
                name: "Cliente");
        }
    }
}
