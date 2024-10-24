using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VsiriusDataModel.Migrations
{
    /// <inheritdoc />
    public partial class initdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfigJson",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigJson", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DrivenHtmlPages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsShow = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrivenHtmlPages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Favourites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favourites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotificationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isView = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationMains",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotificationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsShow = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationMains", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isAll = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatus",
                columns: table => new
                {
                    IdGuild = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatus", x => x.IdGuild);
                });

            migrationBuilder.CreateTable(
                name: "PageView",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    PageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PageParam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PageTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastView = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstView = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateViewCount = table.Column<int>(type: "int", nullable: false),
                    TotalViews = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageView", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaimsEmployee",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaimsEmployee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolesEmployee",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolesEmployee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SecretIdentityUserInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CookiesInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Header = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Connection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateRequest = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecretIdentityUserInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "State",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_State", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRolesEmployee",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRolesEmployee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Address1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VsiriusCart",
                columns: table => new
                {
                    CartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VsiriusCart", x => x.CartId);
                });

            migrationBuilder.CreateTable(
                name: "VsiriusFilter",
                columns: table => new
                {
                    FilterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LabelName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VsiriusFilter", x => x.FilterId);
                });

            migrationBuilder.CreateTable(
                name: "VsiriusHomeProduct",
                columns: table => new
                {
                    item_sku = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsPopular = table.Column<bool>(type: "bit", nullable: true),
                    IsLatest = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VsiriusHomeProduct", x => x.item_sku);
                });

            migrationBuilder.CreateTable(
                name: "VsiriusOrderDetails",
                columns: table => new
                {
                    OrderDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuantityItem = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VsiriusOrderDetails", x => x.OrderDetailId);
                });

            migrationBuilder.CreateTable(
                name: "VsiriusOrders",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<bool>(type: "bit", nullable: true),
                    Address1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Shipping = table.Column<bool>(type: "bit", nullable: true),
                    PaymentMethod = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: true),
                    CreateOrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalQuantityItem = table.Column<int>(type: "int", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    isRead = table.Column<bool>(type: "bit", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VsiriusOrders", x => x.OrderId);
                });

            migrationBuilder.CreateTable(
                name: "VsiriusProduct",
                columns: table => new
                {
                    item_sku = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    feed_product_type = table.Column<string>(type: "nvarchar(900)", maxLength: 900, nullable: true),
                    brand_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    external_product_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    external_product_id_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_name = table.Column<string>(type: "nvarchar(900)", maxLength: 900, nullable: true),
                    item_length_numeric = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_length_numeric_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    main_image_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    target_gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    quantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    standard_price = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    item_type = table.Column<string>(type: "nvarchar(900)", maxLength: 900, nullable: true),
                    metal_type = table.Column<string>(type: "nvarchar(900)", maxLength: 900, nullable: true),
                    material_type1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    material_type2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    material_type3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    material_type4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    material_type5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    department_name1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    department_name2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    department_name3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    department_name4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    department_name5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gem_type1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gem_type2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gem_type3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    color_map = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    size_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    age_range_description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    part_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    watch_movement_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    target_audience_keywords1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    target_audience_keywords2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    target_audience_keywords3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    target_audience_keywords4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    target_audience_keywords5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    display_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    product_description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    model_name = table.Column<string>(type: "nvarchar(900)", maxLength: 900, nullable: true),
                    bullet_point1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bullet_point2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bullet_point3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bullet_point4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bullet_point5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bullet_point6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bullet_point7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bullet_point8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bullet_point9 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bullet_point10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_type_name = table.Column<string>(type: "nvarchar(900)", maxLength: 900, nullable: true),
                    number_of_boxes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_length_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_height = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_length = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_width = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_width_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_height_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cpsia_cautionary_statement1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cpsia_cautionary_statement2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cpsia_cautionary_statement3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cpsia_cautionary_statement4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    country_of_origin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    warranty_description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    condition_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    number_of_items = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_expiration_dated_product = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    setting_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    color_name = table.Column<string>(type: "nvarchar(900)", maxLength: 900, nullable: true),
                    ring_size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    depth_front_to_back = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    depth_front_to_back_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    depth_width_side_to_side = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    depth_width_side_to_side_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    depth_height_floor_to_top = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    depth_height_floor_to_top_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    package_height = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    package_width = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    package_length = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    package_length_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    package_weight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    package_weight_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    package_height_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    package_width_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    supplier_declared_dg_hz_regulation1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    supplier_declared_dg_hz_regulation2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    supplier_declared_dg_hz_regulation3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    supplier_declared_dg_hz_regulation4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    supplier_declared_dg_hz_regulation5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    supplier_declared_material_regulation1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    supplier_declared_material_regulation2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    supplier_declared_material_regulation3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lifecycle_supply_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    map_price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    style_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    unit_count = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    unit_count_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    closure_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    care_instructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fabric_type1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fabric_type2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fabric_type3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fabric_type4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fabric_type5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fabric_type6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fabric_type7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fabric_type8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fabric_type9 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fabric_type10 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    import_designation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    generic_keywords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    outer_material_type1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    outer_material_type2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    outer_material_type3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    outer_material_type4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    outer_material_type5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    material_composition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    size_map = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    other_image_url1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    other_image_url2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    other_image_url3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    other_image_url4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    other_image_url5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    other_image_url6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    other_image_url7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    other_image_url8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    swatch_image_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pt8_image_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pt7_image_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pt5_image_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pt4_image_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pt3_image_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pt2_image_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pt6_image_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pt1_image_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    relationship_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    variation_theme = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    package_level = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    parent_sku = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    package_contains_quantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    package_contains_identifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    parent_child = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    update_delete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    certificate_number1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    certificate_number2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    certificate_number3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    certificate_number4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    certificate_number5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    certificate_number6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    certificate_number7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    certificate_number8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    certificate_number9 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    grade_rating = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    certificate_type1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    certificate_type2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    certificate_type3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    certificate_type4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    certificate_type5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    certificate_type6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    certificate_type7 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    certificate_type8 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    certificate_type9 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    model_year = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    inner_material_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gtin_exemption_reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    production_method = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_color1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_color2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_color3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    metals_metal_weight_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stones_color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    occasion_type1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    occasion_type2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    occasion_type3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    occasion_type4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    occasion_type5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    clasp_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stones_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stones_creation_method = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    athlete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    team_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_shape1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_shape2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_shape3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_clarity1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_clarity2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_clarity3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    collection_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    metals_metal_weight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    metals_metal_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stones_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stones_clarity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stones_number_of_stones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stones_treatment_method = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fur_description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    metals_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    initial_character = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    metals_metal_stamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stones_shape = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    chain_length_unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stones_cut = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_cut1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_cut2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_cut3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    league_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    thesaurus_subject_keywords1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    thesaurus_subject_keywords2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    thesaurus_subject_keywords3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    thesaurus_subject_keywords4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    thesaurus_subject_keywords5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    chain_length_derived = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    chain_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_booking_date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    drop_length_unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    back_finding = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    drop_length = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_autographed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sport_type1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sport_type2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sport_type3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    seasons1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    seasons2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    seasons3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    seasons4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lifestyle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fit_type1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fit_type2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fit_type3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fit_type4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fit_type5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    weave_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    theme = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    special_size_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    thesaurus_attribute_keywords1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    thesaurus_attribute_keywords2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    thesaurus_attribute_keywords3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    thesaurus_attribute_keywords4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    thesaurus_attribute_keywords5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    catalog_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    specific_uses_keywords1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    specific_uses_keywords2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    specific_uses_keywords3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    specific_uses_keywords4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    specific_uses_keywords5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    band_material_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    case_thickness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    band_color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    case_material_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    water_resistance_depth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    water_resistance_depth_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    platinum_keywords1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    platinum_keywords2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    platinum_keywords3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    platinum_keywords4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    platinum_keywords5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    special_features1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    special_features2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    special_features3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    special_features4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    special_features5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    band_length = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    operating_system = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    case_thickness_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    supported_application = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    target_audience_base1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    target_audience_base2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    target_audience_base3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    target_audience_base4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    target_audience_base5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    band_length_unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    water_resistance_level = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    directions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    paint_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    number_of_pieces = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_assembly_required = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    included_components1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    included_components2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    included_components3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    included_components4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    included_components5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_resizable = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_creation_method1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_creation_method2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_creation_method3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_treatment_method1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_treatment_method2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_treatment_method3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gauge_derived = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lining_description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    number_of_compartments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    number_of_wheels = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pattern_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    wheel_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    band_thickness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    band_thickness_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    horsepower = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    horsepower_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    power_source_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    voltage_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    controller_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    maximum_operating_pressure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    maximum_operating_pressure_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    handle_location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    light_source_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    brightness_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    speed_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_torque = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_torque_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    insulation_resistance_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    mounting_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    number_of_teeth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    oem_equivalent_part_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    recommended_uses_for_product = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    shank_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    number_of_drawers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    belt_style = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    embellishment_feature1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    embellishment_feature2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    embellishment_feature3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    embellishment_feature4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    embellishment_feature5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    style_keywords1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    style_keywords2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    style_keywords3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    compatible_devices = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    front_style = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    country_as_labeled = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    neck_style = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pattern_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pocket_description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    shaft_style_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pearl_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    legal_compliance_certification_certifying_authority_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pearl_uniformity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pearl_shape = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pearl_surface_blemishes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pearl_lustre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    legal_compliance_certification_geographic_jurisdiction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    flash_point_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pearl_stringing_method = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pearl_minimum_color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    inscription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    number_of_pearls = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    number_of_stones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    duration_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    subject_character = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    duration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sub_brand_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sleeve_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fabric_wash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    designer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    band_size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dial_color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bezel_material_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    bezel_function = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dial_window_material_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    calendar_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    resale_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    authenticated_by = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    finish_type1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    finish_type2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fc_shelf_life_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ring_sizing_lower_range = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ring_sizing_upper_range = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    golf_club_flex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    connectivity_protocol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_pitch_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    energy_guide_cost_disclosure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_oem_authorized = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    energy_guide_annual_operating_cost = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    belt_width_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    belt_width_string = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    opacity_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    map_policy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    belt_width_derived = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    index_suppressed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    leg_style = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    opacity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    strap_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    underwire_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_display_weight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    total_diamond_weight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_display_diameter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_weight1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_weight2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_weight3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_weight_unit_of_measure1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_weight_unit_of_measure2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_weight_unit_of_measure3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    total_gem_weight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    size_per_pearl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_display_diameter_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_display_length = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_display_length_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    total_gem_weight_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_display_weight_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    total_diamond_weight_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    band_width = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    band_width_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    case_diameter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    case_diameter_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_shape = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    website_shipping_weight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    website_shipping_weight_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    maximum_size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    maximum_size_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    line_size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    line_size_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    paper_size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    paper_size_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_display_height = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_display_width = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_dimensions_unit_of_measure1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_dimensions_unit_of_measure2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_dimensions_unit_of_measure3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_display_height_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_display_width_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    total_metal_weight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    total_metal_weight_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_width_unit_of_measure1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_width_unit_of_measure2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_width_unit_of_measure3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_width1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_width2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_width3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    measurement_system = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_diameter_derived = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_diameter_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    maximum_weight_recommendation_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    maximum_weight_recommendation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_length1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_length2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_length3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_length_unit_of_measure1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_length_unit_of_measure2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_length_unit_of_measure3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_height1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_height2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    stone_height3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_thickness_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_thickness_derived = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    band_size_num = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    band_size_num_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    leg_diameter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    leg_diameter_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    neck_size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    neck_size_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    shoe_width = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    waist_size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    waist_size_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    shoe_width_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fulfillment_center_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    regulation_type1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    regulation_type2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    regulation_type3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    regulation_type4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    regulation_type5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    regulatory_compliance_certification_value1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    regulatory_compliance_certification_value2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    regulatory_compliance_certification_value3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    regulatory_compliance_certification_value4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    regulatory_compliance_certification_value5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    batteries_required = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    are_batteries_included = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    battery_cell_composition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    battery_type1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    battery_type2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    battery_type3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    number_of_batteries1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    number_of_batteries2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    number_of_batteries3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    battery_weight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    battery_weight_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    number_of_lithium_metal_cells = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    number_of_lithium_ion_cells = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lithium_battery_packaging = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lithium_battery_energy_content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lithium_battery_energy_content_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lithium_battery_weight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lithium_battery_weight_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    hazmat_united_nations_regulatory_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    safety_data_sheet_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_weight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_weight_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_volume = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_volume_unit_of_measure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    warranty_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    legal_compliance_certification_expiration_date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    legal_compliance_certification_regulatory_organization_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    legal_compliance_certification_status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    flash_point = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    legal_compliance_certification_date_of_issue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    legal_compliance_certification_metadata = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    legal_compliance_certification_value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    metal_stamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ghs_classification_class1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ghs_classification_class2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ghs_classification_class3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    california_proposition_65_compliance_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    california_proposition_65_chemical_names1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    california_proposition_65_chemical_names2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    california_proposition_65_chemical_names3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    california_proposition_65_chemical_names4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    california_proposition_65_chemical_names5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pesticide_marking_type1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pesticide_marking_type2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pesticide_marking_type3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pesticide_marking_registration_status1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pesticide_marking_registration_status2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pesticide_marking_registration_status3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pesticide_marking_certification_number1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pesticide_marking_certification_number2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pesticide_marking_certification_number3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fcc_radio_frequency_emission_compliance_registration_status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fcc_radio_frequency_emission_compliance_point_of_contact_email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fcc_radio_frequency_emission_compliance_point_of_contact_phone_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fcc_radio_frequency_emission_compliance_point_of_contact_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fcc_radio_frequency_emission_compliance_identification_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fcc_radio_frequency_emission_compliance_point_of_contact_address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cpsia_cautionary_description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    specific_uses_for_product1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    specific_uses_for_product2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    merchant_release_date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    list_price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    restock_date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fulfillment_latency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    offering_can_be_giftwrapped = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    item_package_quantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    offering_end_date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    max_order_quantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    merchant_shipping_group_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    offering_start_date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    product_tax_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    offering_can_be_gift_messaged = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    condition_note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sale_price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sale_from_date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sale_end_date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    product_site_launch_date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    max_aggregate_ship_quantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_discontinued_by_manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    delivery_schedule_group_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    product_expiration_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fc_shelf_life = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    business_price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    quantity_price_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    quantity_lower_bound1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    quantity_price1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    quantity_lower_bound2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    quantity_price2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    quantity_lower_bound3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    quantity_price3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    quantity_lower_bound4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    quantity_price4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    quantity_lower_bound5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    quantity_price5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    progressive_discount_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    progressive_discount_lower_bound1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    progressive_discount_value1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    progressive_discount_lower_bound2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    progressive_discount_value2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    progressive_discount_lower_bound3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    progressive_discount_value3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    national_stock_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    unspsc_code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pricing_action = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    image_standard_1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    image_standard_2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    image_standard_3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    image_standard_4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    image_standard_5 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VsiriusProduct", x => x.item_sku);
                });

            migrationBuilder.CreateTable(
                name: "VsiriusValidProduct",
                columns: table => new
                {
                    FilterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FieldName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LabelName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VsiriusValidProduct", x => x.FilterId);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VsiriusCartDetails",
                columns: table => new
                {
                    CartDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PriceSale = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InfoBrowser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VsiriusCartDetails", x => x.CartDetailId);
                    table.ForeignKey(
                        name: "FK_VsiriusCartDetails_VsiriusCart_CartId",
                        column: x => x.CartId,
                        principalTable: "VsiriusCart",
                        principalColumn: "CartId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_VsiriusCartDetails_CartId",
                table: "VsiriusCartDetails",
                column: "CartId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfigJson");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "DrivenHtmlPages");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Favourites");

            migrationBuilder.DropTable(
                name: "NotificationDetails");

            migrationBuilder.DropTable(
                name: "NotificationMains");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "OrderStatus");

            migrationBuilder.DropTable(
                name: "PageView");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "RoleClaimsEmployee");

            migrationBuilder.DropTable(
                name: "RolesEmployee");

            migrationBuilder.DropTable(
                name: "SecretIdentityUserInfo");

            migrationBuilder.DropTable(
                name: "State");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserRolesEmployee");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "VsiriusCartDetails");

            migrationBuilder.DropTable(
                name: "VsiriusFilter");

            migrationBuilder.DropTable(
                name: "VsiriusHomeProduct");

            migrationBuilder.DropTable(
                name: "VsiriusOrderDetails");

            migrationBuilder.DropTable(
                name: "VsiriusOrders");

            migrationBuilder.DropTable(
                name: "VsiriusProduct");

            migrationBuilder.DropTable(
                name: "VsiriusValidProduct");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "VsiriusCart");
        }
    }
}
