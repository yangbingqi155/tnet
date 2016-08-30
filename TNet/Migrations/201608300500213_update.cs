namespace TNet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Business",
                c => new
                    {
                        idbuss = c.Int(nullable: false),
                        buss = c.String(nullable: false, maxLength: 60, unicode: false),
                        addr = c.String(maxLength: 100, unicode: false),
                        sellpt = c.String(maxLength: 60, unicode: false),
                        imgs = c.String(maxLength: 255, unicode: false),
                        cretime = c.DateTime(),
                        busstime = c.DateTime(),
                        blevel = c.Int(),
                        notes = c.String(maxLength: 50, unicode: false),
                        inuse = c.Boolean(),
                    })
                .PrimaryKey(t => t.idbuss);
            
            CreateTable(
                "dbo.Discount",
                c => new
                    {
                        idmerc = c.Int(nullable: false),
                        idspec = c.Int(nullable: false),
                        iddisc = c.Int(nullable: false),
                        month = c.Int(),
                        discount = c.Double(),
                        notes = c.String(maxLength: 50, unicode: false),
                        inuse = c.Boolean(),
                    })
                .PrimaryKey(t => new { t.idmerc, t.idspec, t.iddisc });
            
            CreateTable(
                "dbo.Merc",
                c => new
                    {
                        idmerc = c.Int(nullable: false),
                        idtype = c.Int(),
                        merc = c.String(nullable: false, maxLength: 60, unicode: false),
                        sellpt = c.String(maxLength: 80, unicode: false),
                        baseprice = c.Double(),
                        sellcount = c.Int(),
                        stime = c.DateTime(),
                        cretime = c.DateTime(),
                        entime = c.DateTime(),
                        netype = c.Int(),
                        imgs = c.String(maxLength: 255, unicode: false),
                        descs = c.String(maxLength: 255, unicode: false),
                        notes = c.String(maxLength: 50, unicode: false),
                        inuse = c.Boolean(),
                    })
                .PrimaryKey(t => t.idmerc);
            
            CreateTable(
                "dbo.MercType",
                c => new
                    {
                        idtype = c.Int(nullable: false),
                        name = c.String(nullable: false, maxLength: 60, unicode: false),
                        notes = c.String(maxLength: 50, unicode: false),
                        inuse = c.Boolean(),
                    })
                .PrimaryKey(t => t.idtype);
            
            CreateTable(
                "dbo.MyAddr",
                c => new
                    {
                        idaddr = c.Int(nullable: false),
                        iuser = c.Int(nullable: false),
                        province = c.String(maxLength: 10, unicode: false),
                        city = c.String(maxLength: 10, unicode: false),
                        street = c.String(maxLength: 120, unicode: false),
                        tag = c.String(maxLength: 10, unicode: false),
                        isdv = c.Boolean(),
                        notes = c.String(maxLength: 50, unicode: false),
                        inuse = c.Boolean(),
                    })
                .PrimaryKey(t => new { t.idaddr, t.iuser });
            
            CreateTable(
                "dbo.MyOrder",
                c => new
                    {
                        orderno = c.Int(nullable: false),
                        iduser = c.Int(nullable: false),
                        idpro = c.Int(nullable: false),
                        pro = c.String(maxLength: 60, unicode: false),
                        price = c.Double(),
                        addr = c.String(maxLength: 100, unicode: false),
                        cretime = c.DateTime(),
                        stime = c.DateTime(),
                        entime = c.DateTime(),
                        otype = c.Int(),
                        status = c.Int(),
                        notes = c.String(maxLength: 50, unicode: false),
                        inuse = c.Int(),
                    })
                .PrimaryKey(t => new { t.orderno, t.iduser, t.idpro });
            
            CreateTable(
                "dbo.Spec",
                c => new
                    {
                        idmerc = c.Int(nullable: false),
                        idspec = c.Int(nullable: false),
                        spec = c.String(maxLength: 60, unicode: false),
                        price = c.Double(),
                        sellcount = c.Int(),
                        unit = c.Int(),
                        up = c.Int(),
                        down = c.Int(),
                        attmonth = c.Int(),
                        stuprice = c.Double(),
                        moveprice = c.Double(),
                        usertype = c.String(maxLength: 50, unicode: false),
                        notes = c.String(maxLength: 100, unicode: false),
                        inuse = c.Boolean(),
                    })
                .PrimaryKey(t => new { t.idmerc, t.idspec });
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        idweixin = c.String(nullable: false, maxLength: 50, unicode: false),
                        iduser = c.Int(nullable: false),
                        name = c.String(maxLength: 50, unicode: false),
                        isoper = c.Boolean(),
                        phone = c.String(maxLength: 15, unicode: false),
                        comp = c.String(maxLength: 60, unicode: false),
                        sex = c.Int(),
                        avatar = c.String(maxLength: 50, unicode: false),
                        notes = c.String(maxLength: 50, unicode: false),
                        inuse = c.Boolean(),
                    })
                .PrimaryKey(t => new { t.idweixin, t.iduser });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.User");
            DropTable("dbo.Spec");
            DropTable("dbo.MyOrder");
            DropTable("dbo.MyAddr");
            DropTable("dbo.MercType");
            DropTable("dbo.Merc");
            DropTable("dbo.Discount");
            DropTable("dbo.Business");
        }
    }
}
