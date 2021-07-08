namespace Pharm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Адрес",
                c => new
                    {
                        Кодадреса = c.Int(name: "Код адреса", nullable: false, identity: true),
                        Субъект = c.String(nullable: false, maxLength: 128),
                        Город = c.String(nullable: false, maxLength: 128),
                        Улица = c.String(nullable: false, maxLength: 128),
                        Дом = c.String(nullable: false, maxLength: 12),
                        Квартира = c.String(maxLength: 12),
                    })
                .PrimaryKey(t => t.Кодадреса);
            
            CreateTable(
                "dbo.Контактные данные",
                c => new
                    {
                        Кодконтактныхданных = c.Int(name: "Код контактных данных", nullable: false, identity: true),
                        Кодадреса = c.Int(name: "Код адреса"),
                        Номертелефона = c.String(name: "Номер телефона", nullable: false, maxLength: 20),
                        Сайт = c.String(maxLength: 64),
                        Электроннаяпочта = c.String(name: "Электронная почта", nullable: false, maxLength: 64),
                    })
                .PrimaryKey(t => t.Кодконтактныхданных)
                .ForeignKey("dbo.Адрес", t => t.Кодадреса)
                .Index(t => t.Кодадреса);
            
            CreateTable(
                "dbo.Изготовитель",
                c => new
                    {
                        Кодизготовителя = c.Int(name: "Код изготовителя", nullable: false, identity: true),
                        Кодконтактныхданных = c.Int(name: "Код контактных данных", nullable: false),
                        Название = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Кодизготовителя)
                .ForeignKey("dbo.Контактные данные", t => t.Кодконтактныхданных)
                .Index(t => t.Кодконтактныхданных);
            
            CreateTable(
                "dbo.Препарат",
                c => new
                    {
                        Кодпрепарата = c.Int(name: "Код препарата", nullable: false, identity: true),
                        Кодизготовителя = c.Int(name: "Код изготовителя", nullable: false),
                        Название = c.String(nullable: false, maxLength: 128),
                        Группа = c.String(nullable: false, maxLength: 128),
                        Закупочнаяцена = c.Decimal(name: "Закупочная цена ₽", nullable: false, storeType: "money"),
                        Конечнаяцена = c.Decimal(name: "Конечная цена ₽", nullable: false, storeType: "money"),
                    })
                .PrimaryKey(t => t.Кодпрепарата)
                .ForeignKey("dbo.Изготовитель", t => t.Кодизготовителя)
                .Index(t => t.Кодизготовителя);
            
            CreateTable(
                "dbo.Заказ_Препарат",
                c => new
                    {
                        Кодзаказа = c.Int(name: "Код заказа", nullable: false),
                        Кодпрепарата = c.Int(name: "Код препарата", nullable: false),
                        Количествопрепарата = c.Int(name: "Количество препарата", nullable: false),
                    })
                .PrimaryKey(t => new { t.Кодзаказа, t.Кодпрепарата })
                .ForeignKey("dbo.Заказ", t => t.Кодзаказа)
                .ForeignKey("dbo.Препарат", t => t.Кодпрепарата)
                .Index(t => t.Кодзаказа)
                .Index(t => t.Кодпрепарата);
            
            CreateTable(
                "dbo.Заказ",
                c => new
                    {
                        Кодзаказа = c.Int(name: "Код заказа", nullable: false, identity: true),
                        Кодсотрудника = c.Int(name: "Код сотрудника", nullable: false),
                        Кодсоциальнойкарты = c.String(name: "Код социальной карты", maxLength: 19),
                        Датаивремяисполнения = c.DateTime(name: "Дата и время исполнения", nullable: false),
                        Стоимость = c.Decimal(name: "Стоимость ₽", storeType: "money"),
                    })
                .PrimaryKey(t => t.Кодзаказа)
                .ForeignKey("dbo.Сотрудник", t => t.Кодсотрудника)
                .ForeignKey("dbo.Социальная карта", t => t.Кодсоциальнойкарты)
                .Index(t => t.Кодсотрудника)
                .Index(t => t.Кодсоциальнойкарты);
            
            CreateTable(
                "dbo.Сотрудник",
                c => new
                    {
                        Кодсотрудника = c.Int(name: "Код сотрудника", nullable: false, identity: true),
                        Коддолжности = c.Int(name: "Код должности", nullable: false),
                        Кодконтактныхданных = c.Int(name: "Код контактных данных", nullable: false),
                        Логин = c.String(nullable: false, maxLength: 128),
                        Кодличныхданных = c.Int(name: "Код личных данных", nullable: false),
                        Датаприёманаработу = c.DateTime(name: "Дата приёма на работу", nullable: false, storeType: "date"),
                    })
                .PrimaryKey(t => t.Кодсотрудника)
                .ForeignKey("dbo.Должность", t => t.Коддолжности)
                .ForeignKey("dbo.Личные данные", t => t.Кодличныхданных)
                .ForeignKey("dbo.Пользователи", t => t.Логин)
                .ForeignKey("dbo.Контактные данные", t => t.Кодконтактныхданных)
                .Index(t => t.Коддолжности)
                .Index(t => t.Кодконтактныхданных)
                .Index(t => t.Логин)
                .Index(t => t.Кодличныхданных);
            
            CreateTable(
                "dbo.Должность",
                c => new
                    {
                        Коддолжности = c.Int(name: "Код должности", nullable: false, identity: true),
                        Названиедолжности = c.String(name: "Название должности", nullable: false, maxLength: 128),
                        Оклад = c.Decimal(name: "Оклад ₽", nullable: false, storeType: "money"),
                    })
                .PrimaryKey(t => t.Коддолжности);
            
            CreateTable(
                "dbo.Личные данные",
                c => new
                    {
                        Кодличныхданных = c.Int(name: "Код личных данных", nullable: false, identity: true),
                        Фамилия = c.String(nullable: false, maxLength: 128),
                        Имя = c.String(nullable: false, maxLength: 128),
                        Отчество = c.String(nullable: false, maxLength: 128),
                        Серияпаспорта = c.Int(name: "Серия паспорта"),
                        Номерпаспорта = c.Int(name: "Номер паспорта"),
                        Датарождения = c.DateTime(name: "Дата рождения", nullable: false, storeType: "date"),
                        Образование = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Кодличныхданных);
            
            CreateTable(
                "dbo.Пользователи",
                c => new
                    {
                        Логин = c.String(nullable: false, maxLength: 128),
                        Пароль = c.String(nullable: false, maxLength: 128),
                        Роль = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Логин);
            
            CreateTable(
                "dbo.Социальная карта",
                c => new
                    {
                        Кодсоциальнойкарты = c.String(name: "Код социальной карты", nullable: false, maxLength: 19),
                        Фамилия = c.String(nullable: false, maxLength: 128),
                        Имя = c.String(nullable: false, maxLength: 128),
                        Отчество = c.String(nullable: false, maxLength: 128),
                        Срокдействия = c.DateTime(name: "Срок действия", nullable: false, storeType: "date"),
                        Скидка = c.Decimal(nullable: false, precision: 18, scale: 2, storeType: "numeric"),
                    })
                .PrimaryKey(t => t.Кодсоциальнойкарты);
            
            CreateTable(
                "dbo.Препарат_Поставка",
                c => new
                    {
                        Кодпрепарата = c.Int(name: "Код препарата", nullable: false),
                        Кодпоставки = c.Int(name: "Код поставки", nullable: false),
                        Количествопрепарата = c.Int(name: "Количество препарата", nullable: false),
                    })
                .PrimaryKey(t => new { t.Кодпрепарата, t.Кодпоставки })
                .ForeignKey("dbo.Поставка", t => t.Кодпоставки)
                .ForeignKey("dbo.Препарат", t => t.Кодпрепарата)
                .Index(t => t.Кодпрепарата)
                .Index(t => t.Кодпоставки);
            
            CreateTable(
                "dbo.Поставка",
                c => new
                    {
                        Кодпоставки = c.Int(name: "Код поставки", nullable: false, identity: true),
                        Кодпоставщика = c.Int(name: "Код поставщика", nullable: false),
                        Кодсклада = c.Int(name: "Код склада", nullable: false),
                        Стоимость = c.Decimal(name: "Стоимость ₽", nullable: false, storeType: "money"),
                    })
                .PrimaryKey(t => t.Кодпоставки)
                .ForeignKey("dbo.Поставщик", t => t.Кодпоставщика)
                .ForeignKey("dbo.Склад", t => t.Кодсклада)
                .Index(t => t.Кодпоставщика)
                .Index(t => t.Кодсклада);
            
            CreateTable(
                "dbo.Поставщик",
                c => new
                    {
                        Кодпоставщика = c.Int(name: "Код поставщика", nullable: false, identity: true),
                        Кодконтактныхданных = c.Int(name: "Код контактных данных", nullable: false),
                        Название = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Кодпоставщика)
                .ForeignKey("dbo.Контактные данные", t => t.Кодконтактныхданных)
                .Index(t => t.Кодконтактныхданных);
            
            CreateTable(
                "dbo.Склад",
                c => new
                    {
                        Кодсклада = c.Int(name: "Код склада", nullable: false, identity: true),
                        Площадьм2 = c.Int(name: "Площадь м2", nullable: false),
                    })
                .PrimaryKey(t => t.Кодсклада);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Сотрудник", "Код контактных данных", "dbo.Контактные данные");
            DropForeignKey("dbo.Поставщик", "Код контактных данных", "dbo.Контактные данные");
            DropForeignKey("dbo.Изготовитель", "Код контактных данных", "dbo.Контактные данные");
            DropForeignKey("dbo.Препарат", "Код изготовителя", "dbo.Изготовитель");
            DropForeignKey("dbo.Препарат_Поставка", "Код препарата", "dbo.Препарат");
            DropForeignKey("dbo.Поставка", "Код склада", "dbo.Склад");
            DropForeignKey("dbo.Препарат_Поставка", "Код поставки", "dbo.Поставка");
            DropForeignKey("dbo.Поставка", "Код поставщика", "dbo.Поставщик");
            DropForeignKey("dbo.Заказ_Препарат", "Код препарата", "dbo.Препарат");
            DropForeignKey("dbo.Заказ", "Код социальной карты", "dbo.Социальная карта");
            DropForeignKey("dbo.Сотрудник", "Логин", "dbo.Пользователи");
            DropForeignKey("dbo.Сотрудник", "Код личных данных", "dbo.Личные данные");
            DropForeignKey("dbo.Заказ", "Код сотрудника", "dbo.Сотрудник");
            DropForeignKey("dbo.Сотрудник", "Код должности", "dbo.Должность");
            DropForeignKey("dbo.Заказ_Препарат", "Код заказа", "dbo.Заказ");
            DropForeignKey("dbo.Контактные данные", "Код адреса", "dbo.Адрес");
            DropIndex("dbo.Поставщик", new[] { "Код контактных данных" });
            DropIndex("dbo.Поставка", new[] { "Код склада" });
            DropIndex("dbo.Поставка", new[] { "Код поставщика" });
            DropIndex("dbo.Препарат_Поставка", new[] { "Код поставки" });
            DropIndex("dbo.Препарат_Поставка", new[] { "Код препарата" });
            DropIndex("dbo.Сотрудник", new[] { "Код личных данных" });
            DropIndex("dbo.Сотрудник", new[] { "Логин" });
            DropIndex("dbo.Сотрудник", new[] { "Код контактных данных" });
            DropIndex("dbo.Сотрудник", new[] { "Код должности" });
            DropIndex("dbo.Заказ", new[] { "Код социальной карты" });
            DropIndex("dbo.Заказ", new[] { "Код сотрудника" });
            DropIndex("dbo.Заказ_Препарат", new[] { "Код препарата" });
            DropIndex("dbo.Заказ_Препарат", new[] { "Код заказа" });
            DropIndex("dbo.Препарат", new[] { "Код изготовителя" });
            DropIndex("dbo.Изготовитель", new[] { "Код контактных данных" });
            DropIndex("dbo.Контактные данные", new[] { "Код адреса" });
            DropTable("dbo.Склад");
            DropTable("dbo.Поставщик");
            DropTable("dbo.Поставка");
            DropTable("dbo.Препарат_Поставка");
            DropTable("dbo.Социальная карта");
            DropTable("dbo.Пользователи");
            DropTable("dbo.Личные данные");
            DropTable("dbo.Должность");
            DropTable("dbo.Сотрудник");
            DropTable("dbo.Заказ");
            DropTable("dbo.Заказ_Препарат");
            DropTable("dbo.Препарат");
            DropTable("dbo.Изготовитель");
            DropTable("dbo.Контактные данные");
            DropTable("dbo.Адрес");
        }
    }
}
