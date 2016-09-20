namespace TNet.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TN : DbContext
    {
        public TN()
            : base("name=TN")
        {
        }

        public virtual DbSet<Business> Businesses { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Discount> Discounts { get; set; }
        public virtual DbSet<ManageUser> ManageUsers { get; set; }
        public virtual DbSet<MenuItem> MenuItems { get; set; }
        public virtual DbSet<Merc> Mercs { get; set; }
        public virtual DbSet<MercImage> MercImages { get; set; }
        public virtual DbSet<MercType> MercTypes { get; set; }
        public virtual DbSet<MyAddr> MyAddrs { get; set; }
        public virtual DbSet<MyOrder> MyOrders { get; set; }
        public virtual DbSet<MyOrderPress> MyOrderPresses { get; set; }
        public virtual DbSet<Setup> Setups { get; set; }
        public virtual DbSet<SetupAddr> SetupAddrs { get; set; }
        public virtual DbSet<Spec> Specs { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Business>()
                .Property(e => e.buss)
                .IsUnicode(false);

            modelBuilder.Entity<Business>()
                .Property(e => e.contact)
                .IsUnicode(false);

            modelBuilder.Entity<Business>()
                .Property(e => e.phone)
                .IsUnicode(false);

            modelBuilder.Entity<Business>()
                .Property(e => e.city)
                .IsUnicode(false);

            modelBuilder.Entity<Business>()
                .Property(e => e.addr)
                .IsUnicode(false);

            modelBuilder.Entity<Business>()
                .Property(e => e.sellpt)
                .IsUnicode(false);

            modelBuilder.Entity<Business>()
                .Property(e => e.imgs)
                .IsUnicode(false);

            modelBuilder.Entity<Business>()
                .Property(e => e.runtime)
                .IsUnicode(false);

            modelBuilder.Entity<Business>()
                .Property(e => e.notes)
                .IsUnicode(false);

            modelBuilder.Entity<City>()
                .Property(e => e.idcity)
                .IsUnicode(false);

            modelBuilder.Entity<City>()
                .Property(e => e.city1)
                .IsUnicode(false);

            modelBuilder.Entity<City>()
                .Property(e => e.notes)
                .IsUnicode(false);

            modelBuilder.Entity<Discount>()
                .Property(e => e.notes)
                .IsUnicode(false);

            modelBuilder.Entity<ManageUser>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<ManageUser>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<ManageUser>()
                .Property(e => e.MD5Salt)
                .IsUnicode(false);

            modelBuilder.Entity<MenuItem>()
                .Property(e => e.ItemName)
                .IsUnicode(false);

            modelBuilder.Entity<MenuItem>()
                .Property(e => e.Link)
                .IsUnicode(false);

            modelBuilder.Entity<Merc>()
                .Property(e => e.merc1)
                .IsUnicode(false);

            modelBuilder.Entity<Merc>()
                .Property(e => e.sellpt)
                .IsUnicode(false);

            modelBuilder.Entity<Merc>()
                .Property(e => e.imgs)
                .IsUnicode(false);

            modelBuilder.Entity<Merc>()
                .Property(e => e.descs)
                .IsUnicode(false);

            modelBuilder.Entity<Merc>()
                .Property(e => e.notes)
                .IsUnicode(false);

            modelBuilder.Entity<Merc>()
                .Property(e => e.idbuss)
                .IsUnicode(false);

            modelBuilder.Entity<MercImage>()
                .Property(e => e.Path)
                .IsUnicode(false);

            modelBuilder.Entity<MercType>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<MercType>()
                .Property(e => e.notes)
                .IsUnicode(false);

            modelBuilder.Entity<MyAddr>()
                .Property(e => e.contact)
                .IsUnicode(false);

            modelBuilder.Entity<MyAddr>()
                .Property(e => e.phone)
                .IsUnicode(false);

            modelBuilder.Entity<MyAddr>()
                .Property(e => e.province)
                .IsUnicode(false);

            modelBuilder.Entity<MyAddr>()
                .Property(e => e.city)
                .IsUnicode(false);

            modelBuilder.Entity<MyAddr>()
                .Property(e => e.district)
                .IsUnicode(false);

            modelBuilder.Entity<MyAddr>()
                .Property(e => e.street)
                .IsUnicode(false);

            modelBuilder.Entity<MyAddr>()
                .Property(e => e.tag)
                .IsUnicode(false);

            modelBuilder.Entity<MyAddr>()
                .Property(e => e.notes)
                .IsUnicode(false);

            modelBuilder.Entity<MyOrder>()
                .Property(e => e.merc)
                .IsUnicode(false);

            modelBuilder.Entity<MyOrder>()
                .Property(e => e.spec)
                .IsUnicode(false);

            modelBuilder.Entity<MyOrder>()
                .Property(e => e.contact)
                .IsUnicode(false);

            modelBuilder.Entity<MyOrder>()
                .Property(e => e.addr)
                .IsUnicode(false);

            modelBuilder.Entity<MyOrder>()
                .Property(e => e.phone)
                .IsUnicode(false);

            modelBuilder.Entity<MyOrder>()
                .Property(e => e.notes)
                .IsUnicode(false);

            modelBuilder.Entity<MyOrderPress>()
                .Property(e => e.idpress)
                .IsUnicode(false);

            modelBuilder.Entity<MyOrderPress>()
                .Property(e => e.orderno)
                .IsUnicode(false);

            modelBuilder.Entity<MyOrderPress>()
                .Property(e => e.statust)
                .IsUnicode(false);

            modelBuilder.Entity<MyOrderPress>()
                .Property(e => e.oper)
                .IsUnicode(false);

            modelBuilder.Entity<Setup>()
                .Property(e => e.idsetup)
                .IsUnicode(false);

            modelBuilder.Entity<Setup>()
                .Property(e => e.idtype)
                .IsUnicode(false);

            modelBuilder.Entity<Setup>()
                .Property(e => e.setuptype)
                .IsUnicode(false);

            modelBuilder.Entity<Setup>()
                .Property(e => e.notes)
                .IsFixedLength();

            modelBuilder.Entity<SetupAddr>()
                .Property(e => e.idaddr)
                .IsUnicode(false);

            modelBuilder.Entity<SetupAddr>()
                .Property(e => e.idtype)
                .IsUnicode(false);

            modelBuilder.Entity<SetupAddr>()
                .Property(e => e.idsetup)
                .IsUnicode(false);

            modelBuilder.Entity<SetupAddr>()
                .Property(e => e.phone)
                .IsUnicode(false);

            modelBuilder.Entity<SetupAddr>()
                .Property(e => e.service)
                .IsUnicode(false);

            modelBuilder.Entity<SetupAddr>()
                .Property(e => e.notes)
                .IsFixedLength();

            modelBuilder.Entity<SetupAddr>()
                .Property(e => e.acceptime)
                .IsUnicode(false);

            modelBuilder.Entity<SetupAddr>()
                .Property(e => e.setuptime)
                .IsUnicode(false);

            modelBuilder.Entity<Spec>()
                .Property(e => e.spec1)
                .IsUnicode(false);

            modelBuilder.Entity<Spec>()
                .Property(e => e.usertype)
                .IsUnicode(false);

            modelBuilder.Entity<Spec>()
                .Property(e => e.notes)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.idweixin)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.phone)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.comp)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.avatar)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.notes)
                .IsUnicode(false);
        }
    }
}
