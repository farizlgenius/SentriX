using Microsoft.EntityFrameworkCore;
using Setting.Infrastructure.Persistences.Entities;
using SharedKernel.Domain;

namespace Setting.Infrastructure.Persistences;

public class SettingDbContext(DbContextOptions<SettingDbContext> options) : DbContext(options)
{
      public const string Schema = "setting";
      public DbSet<CardFormat> CardFormats {get; set;} 

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
            base.OnModelCreating(modelBuilder);

            // ⭐ Module schema
            modelBuilder.HasDefaultSchema(Schema);

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                  if (typeof(BaseEntity).IsAssignableFrom(entity.ClrType))
                  {
                        modelBuilder.Entity(entity.ClrType)
                            .Property(nameof(BaseEntity.created_at))
                            .HasDefaultValueSql("NOW()");

                        modelBuilder.Entity(entity.ClrType)
                            .Property(nameof(BaseEntity.updated_at))
                            .HasDefaultValueSql("NOW()");
                  }
            }

            modelBuilder.Entity<CardFormat>()
            .HasData(
                  new CardFormat
                  {
                        id=1,
                        name="26-bit Wiegand",
                        component_id=0,
                        fac=-1,
                        offset=0,
                        function_id=1,
                        flag=0,
                        bits=26,
                        pe_ln=0,
                        pe_loc=-1,
                        po_ln=0,
                        po_loc=-1,
                        fc_ln=0,
                        fc_loc=-1,
                        ch_ln=26,
                        ch_loc=0,
                        ic_ln=0,
                        ic_loc=-1,
                        location_id = 0,
                        is_active=true,
                        is_default =true

                  },
                  new CardFormat
                  {
                        id=2,
                        name="32-bit Wiegand",
                        component_id=1,
                        fac=-1,
                        offset=0,
                        function_id=1,
                        flag=0,
                        bits=32,
                        pe_ln=0,
                        pe_loc=-1,
                        po_ln=0,
                        po_loc=-1,
                        fc_ln=0,
                        fc_loc=-1,
                        ch_ln=32,
                        ch_loc=0,
                        ic_ln=0,
                        ic_loc=-1,
                        location_id = 0,
                        is_active=true,
                         is_default =true

                  },
                  new CardFormat
                  {
                        id=3,
                        name="37-bit Wiegand",
                        component_id=2,
                        fac=-1,
                        offset=0,
                        function_id=1,
                        flag=0,
                        bits=37,
                        pe_ln=0,
                        pe_loc=-1,
                        po_ln=0,
                        po_loc=-1,
                        fc_ln=0,
                        fc_loc=-1,
                        ch_ln=37,
                        ch_loc=0,
                        ic_ln=0,
                        ic_loc=-1,
                        location_id = 0,
                        is_active=true,
                         is_default =true

                  }
            );
      }

}