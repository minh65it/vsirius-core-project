using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EtsyIntegrationDataAPI.Migrations
{
    /// <inheritdoc />
    public partial class updateOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthenEtsy",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodeChallenge = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodeVerifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Scopes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccessToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldAccessToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldRefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthenEtsy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    receipt_id = table.Column<long>(type: "bigint", nullable: false),
                    receipt_type = table.Column<long>(type: "bigint", nullable: true),
                    seller_user_id = table.Column<long>(type: "bigint", nullable: true),
                    seller_email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    buyer_user_id = table.Column<long>(type: "bigint", nullable: true),
                    buyer_email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    first_line = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    second_line = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    state = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    zip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    formatted_address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    country_iso = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    payment_method = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    payment_email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    message_from_seller = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    message_from_buyer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    message_from_payment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_paid = table.Column<bool>(type: "bit", nullable: true),
                    is_shipped = table.Column<bool>(type: "bit", nullable: true),
                    create_timestamp = table.Column<long>(type: "bigint", nullable: true),
                    created_timestamp = table.Column<long>(type: "bigint", nullable: true),
                    update_timestamp = table.Column<long>(type: "bigint", nullable: true),
                    updated_timestamp = table.Column<long>(type: "bigint", nullable: true),
                    is_gift = table.Column<bool>(type: "bit", nullable: true),
                    gift_message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gift_sender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    grandtotal_amount = table.Column<long>(type: "bigint", nullable: true),
                    grandtotal_divisor = table.Column<long>(type: "bigint", nullable: true),
                    grandtotal_currency_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    subtotal_amount = table.Column<long>(type: "bigint", nullable: true),
                    subtotal_divisor = table.Column<long>(type: "bigint", nullable: true),
                    subtotal_currency_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    total_price_amount = table.Column<long>(type: "bigint", nullable: true),
                    total_price_divisor = table.Column<long>(type: "bigint", nullable: true),
                    total_price_currency_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    total_shipping_cost_amount = table.Column<long>(type: "bigint", nullable: true),
                    total_shipping_cost_divisor = table.Column<long>(type: "bigint", nullable: true),
                    total_shipping_cost_currency_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    total_tax_cost_amount = table.Column<long>(type: "bigint", nullable: true),
                    total_tax_cost_divisor = table.Column<long>(type: "bigint", nullable: true),
                    total_tax_cost_currency_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    total_vat_cost_amount = table.Column<long>(type: "bigint", nullable: true),
                    total_vat_cost_divisor = table.Column<long>(type: "bigint", nullable: true),
                    total_vat_cost_currency_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    discount_amt_amount = table.Column<long>(type: "bigint", nullable: true),
                    discount_amt_divisor = table.Column<long>(type: "bigint", nullable: true),
                    discount_amt_currency_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gift_wrap_price_amount = table.Column<long>(type: "bigint", nullable: true),
                    gift_wrap_price_divisor = table.Column<long>(type: "bigint", nullable: true),
                    gift_wrap_price_currency_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_submit = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ProductEtsy",
                columns: table => new
                {
                    item_sku = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    who_made = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    when_made = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    taxonomy_id = table.Column<long>(type: "bigint", nullable: false),
                    listing_id = table.Column<long>(type: "bigint", nullable: false),
                    shipping_profile_id = table.Column<long>(type: "bigint", nullable: true),
                    return_policy_id = table.Column<long>(type: "bigint", nullable: true),
                    shop_section_id = table.Column<long>(type: "bigint", nullable: true),
                    processing_min = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    processing_max = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    item_weight = table.Column<float>(type: "real", nullable: true),
                    item_length = table.Column<float>(type: "real", nullable: true),
                    item_width = table.Column<float>(type: "real", nullable: true),
                    item_height = table.Column<float>(type: "real", nullable: true),
                    item_weight_unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    item_dimensions_unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    is_personalizable = table.Column<bool>(type: "bit", nullable: true),
                    personalization_is_required = table.Column<bool>(type: "bit", nullable: true),
                    personalization_char_count_max = table.Column<int>(type: "int", nullable: true),
                    personalization_instructions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    production_partner_ids = table.Column<int>(type: "int", nullable: true),
                    is_supply = table.Column<bool>(type: "bit", nullable: false),
                    is_customizable = table.Column<bool>(type: "bit", nullable: false),
                    should_auto_renew = table.Column<bool>(type: "bit", nullable: false),
                    is_taxable = table.Column<bool>(type: "bit", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createdDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductEtsy", x => x.item_sku);
                });

            migrationBuilder.CreateTable(
                name: "PropertiesByTaxonomy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    taxonomy_id = table.Column<long>(type: "bigint", nullable: false),
                    display_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    property_id = table.Column<long>(type: "bigint", nullable: false),
                    value_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    scale_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertiesByTaxonomy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Taxonomy",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    level = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    parent_id = table.Column<int>(type: "int", nullable: true),
                    full_path_taxonomy_ids = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Taxonomyid = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taxonomy", x => x.id);
                    table.ForeignKey(
                        name: "FK_Taxonomy_Taxonomy_Taxonomyid",
                        column: x => x.Taxonomyid,
                        principalTable: "Taxonomy",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Refund",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    amount = table.Column<long>(type: "bigint", nullable: true),
                    amount_divisor = table.Column<long>(type: "bigint", nullable: true),
                    amount_currency_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_timestamp = table.Column<long>(type: "bigint", nullable: true),
                    reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    note_from_issuer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    receipt_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Refund", x => x.id);
                    table.ForeignKey(
                        name: "FK_Refund_Order_receipt_id",
                        column: x => x.receipt_id,
                        principalTable: "Order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shipment",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    receipt_shipping_id = table.Column<long>(type: "bigint", nullable: true),
                    shipment_notification_timestamp = table.Column<long>(type: "bigint", nullable: true),
                    carrier_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tracking_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    receipt_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipment", x => x.id);
                    table.ForeignKey(
                        name: "FK_Shipment_Order_receipt_id",
                        column: x => x.receipt_id,
                        principalTable: "Order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    transaction_id = table.Column<long>(type: "bigint", nullable: true),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    seller_user_id = table.Column<long>(type: "bigint", nullable: true),
                    buyer_user_id = table.Column<long>(type: "bigint", nullable: true),
                    create_timestamp = table.Column<long>(type: "bigint", nullable: true),
                    created_timestamp = table.Column<long>(type: "bigint", nullable: true),
                    paid_timestamp = table.Column<long>(type: "bigint", nullable: true),
                    shipped_timestamp = table.Column<long>(type: "bigint", nullable: true),
                    quantity = table.Column<long>(type: "bigint", nullable: true),
                    listing_image_id = table.Column<long>(type: "bigint", nullable: true),
                    receipt_id = table.Column<long>(type: "bigint", nullable: false),
                    is_digital = table.Column<bool>(type: "bit", nullable: true),
                    file_data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    listing_id = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    transaction_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    product_id = table.Column<long>(type: "bigint", nullable: true),
                    sku = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    price_amount = table.Column<long>(type: "bigint", nullable: true),
                    price_divisor = table.Column<long>(type: "bigint", nullable: true),
                    price_currency_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    shipping_cost_amount = table.Column<long>(type: "bigint", nullable: true),
                    shipping_cost_divisor = table.Column<long>(type: "bigint", nullable: true),
                    shipping_cost_currency_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    shipping_profile_id = table.Column<long>(type: "bigint", nullable: true),
                    min_processing_days = table.Column<long>(type: "bigint", nullable: true),
                    max_processing_days = table.Column<long>(type: "bigint", nullable: true),
                    shipping_method = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    shipping_upgrade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    expected_ship_date = table.Column<long>(type: "bigint", nullable: true),
                    buyer_coupon = table.Column<long>(type: "bigint", nullable: true),
                    shop_coupon = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.id);
                    table.ForeignKey(
                        name: "FK_Transaction_Order_receipt_id",
                        column: x => x.receipt_id,
                        principalTable: "Order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImageProduct",
                columns: table => new
                {
                    item_sku = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    listing_image_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    listing_id = table.Column<long>(type: "bigint", nullable: true),
                    rank = table.Column<int>(type: "int", nullable: false),
                    overwrite = table.Column<bool>(type: "bit", nullable: false),
                    is_watermarked = table.Column<bool>(type: "bit", nullable: false),
                    alt_text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageProduct", x => new { x.item_sku, x.listing_image_id });
                    table.ForeignKey(
                        name: "FK_ImageProduct_ProductEtsy_item_sku",
                        column: x => x.item_sku,
                        principalTable: "ProductEtsy",
                        principalColumn: "item_sku",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialProduct",
                columns: table => new
                {
                    item_sku = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    material = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialProduct", x => new { x.item_sku, x.material });
                    table.ForeignKey(
                        name: "FK_MaterialProduct_ProductEtsy_item_sku",
                        column: x => x.item_sku,
                        principalTable: "ProductEtsy",
                        principalColumn: "item_sku",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertiesProduct",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    item_sku = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    taxonomy_id = table.Column<long>(type: "bigint", nullable: false),
                    property_id = table.Column<long>(type: "bigint", nullable: false),
                    value_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    scale_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertiesProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertiesProduct_ProductEtsy_item_sku",
                        column: x => x.item_sku,
                        principalTable: "ProductEtsy",
                        principalColumn: "item_sku",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StyleProduct",
                columns: table => new
                {
                    item_sku = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    style = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StyleProduct", x => new { x.item_sku, x.style });
                    table.ForeignKey(
                        name: "FK_StyleProduct_ProductEtsy_item_sku",
                        column: x => x.item_sku,
                        principalTable: "ProductEtsy",
                        principalColumn: "item_sku",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TagProduct",
                columns: table => new
                {
                    item_sku = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    tag = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagProduct", x => new { x.item_sku, x.tag });
                    table.ForeignKey(
                        name: "FK_TagProduct_ProductEtsy_item_sku",
                        column: x => x.item_sku,
                        principalTable: "ProductEtsy",
                        principalColumn: "item_sku",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductDatum",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    property_id = table.Column<long>(type: "bigint", nullable: true),
                    property_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    scale_id = table.Column<long>(type: "bigint", nullable: true),
                    scale_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    value_ids = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    values = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    transaction_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDatum", x => x.id);
                    table.ForeignKey(
                        name: "FK_ProductDatum_Transaction_transaction_id",
                        column: x => x.transaction_id,
                        principalTable: "Transaction",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Variation",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    property_id = table.Column<long>(type: "bigint", nullable: true),
                    value_id = table.Column<long>(type: "bigint", nullable: true),
                    formatted_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    formatted_value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    transaction_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Variation", x => x.id);
                    table.ForeignKey(
                        name: "FK_Variation_Transaction_transaction_id",
                        column: x => x.transaction_id,
                        principalTable: "Transaction",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductDatum_transaction_id",
                table: "ProductDatum",
                column: "transaction_id");

            migrationBuilder.CreateIndex(
                name: "IX_PropertiesProduct_item_sku",
                table: "PropertiesProduct",
                column: "item_sku");

            migrationBuilder.CreateIndex(
                name: "IX_Refund_receipt_id",
                table: "Refund",
                column: "receipt_id");

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_receipt_id",
                table: "Shipment",
                column: "receipt_id");

            migrationBuilder.CreateIndex(
                name: "IX_Taxonomy_Taxonomyid",
                table: "Taxonomy",
                column: "Taxonomyid");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_receipt_id",
                table: "Transaction",
                column: "receipt_id");

            migrationBuilder.CreateIndex(
                name: "IX_Variation_transaction_id",
                table: "Variation",
                column: "transaction_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthenEtsy");

            migrationBuilder.DropTable(
                name: "ImageProduct");

            migrationBuilder.DropTable(
                name: "MaterialProduct");

            migrationBuilder.DropTable(
                name: "ProductDatum");

            migrationBuilder.DropTable(
                name: "PropertiesByTaxonomy");

            migrationBuilder.DropTable(
                name: "PropertiesProduct");

            migrationBuilder.DropTable(
                name: "Refund");

            migrationBuilder.DropTable(
                name: "Shipment");

            migrationBuilder.DropTable(
                name: "StyleProduct");

            migrationBuilder.DropTable(
                name: "TagProduct");

            migrationBuilder.DropTable(
                name: "Taxonomy");

            migrationBuilder.DropTable(
                name: "Variation");

            migrationBuilder.DropTable(
                name: "ProductEtsy");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "Order");
        }
    }
}
