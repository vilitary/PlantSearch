using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PlantSearch.Models;

public partial class Tree3Context : DbContext
{
    public Tree3Context()
    {
    }

    public Tree3Context(DbContextOptions<Tree3Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<CultivationMethod> CultivationMethods { get; set; }

    public virtual DbSet<DiseaseType> DiseaseTypes { get; set; }

    public virtual DbSet<Diseasetreatment> Diseasetreatments { get; set; }

    public virtual DbSet<FertilizerType> FertilizerTypes { get; set; }

    public virtual DbSet<Plant> Plants { get; set; }

    public virtual DbSet<PlantType> PlantTypes { get; set; }

    public virtual DbSet<Plantimage> Plantimages { get; set; }

    public virtual DbSet<Season> Seasons { get; set; }

    public virtual DbSet<SoilType> SoilTypes { get; set; }

    public virtual DbSet<SuitableRegion> SuitableRegions { get; set; }

    public DbSet<PlantSoil> PlantSoils { get; set; }
    public DbSet<PlantSeason> PlantSeasons { get; set; }
    public DbSet<PlantFertilizer> PlantFertilizers { get; set; }
    public DbSet<PlantDisease> PlantDiseases { get; set; }
    public DbSet<PlantRegion> PlantRegions { get; set; }
    public DbSet<PlantMethod> PlantMethods { get; set; }
    public DbSet<PlantPlanttype> PlantPlanttypes { get; set; }

    public DbSet<Comment> Comments { get; set; }
    public DbSet<Rating> Ratings { get; set; }


    //     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //         => optionsBuilder.UseNpgsql("Host=localhost;Database=Tree3;Username=postgres;Password=1234");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("account_pkey");

            entity.ToTable("account");

            entity.HasIndex(e => e.Username, "account_username_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasDefaultValueSql("'user'::character varying")
                .HasColumnName("role");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });
        modelBuilder.Entity<Plant>()
    .HasMany(p => p.Types)
    .WithMany(t => t.Plants)
    .UsingEntity(j => j.ToTable("PlantPlanttype"));

        modelBuilder.Entity<CultivationMethod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cultivation_method_pkey");

            entity.ToTable("cultivation_method");

            entity.HasIndex(e => e.MethodName, "cultivation_method_method_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.MethodName)
                .HasMaxLength(100)
                .HasColumnName("method_name");
            entity.Property(e => e.Notes).HasColumnName("notes");
        });

        modelBuilder.Entity<DiseaseType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("disease_type_pkey");

            entity.ToTable("disease_type");

            entity.HasIndex(e => e.DiseaseName, "disease_type_disease_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Causes).HasColumnName("causes");
            entity.Property(e => e.DiseaseName)
                .HasMaxLength(100)
                .HasColumnName("disease_name");
            entity.Property(e => e.Symptoms).HasColumnName("symptoms");
        });

        modelBuilder.Entity<Diseasetreatment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("diseasetreatment_pkey");

            entity.ToTable("diseasetreatment");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DiseaseId).HasColumnName("disease_id");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.TreatmentMethod).HasColumnName("treatment_method");

            entity.HasOne(d => d.Disease).WithMany(p => p.Diseasetreatments)
                .HasForeignKey(d => d.DiseaseId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("diseasetreatment_disease_id_fkey");
        });

        modelBuilder.Entity<FertilizerType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("fertilizer_type_pkey");

            entity.ToTable("fertilizer_type");

            entity.HasIndex(e => e.TypeName, "fertilizer_type_type_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.TypeName)
                .HasMaxLength(100)
                .HasColumnName("type_name");
        });

        modelBuilder.Entity<Plant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("plant_pkey");

            entity.ToTable("plant");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.GrowthDuration).HasColumnName("growth_duration");
            entity.Property(e => e.Origin)
                .HasMaxLength(100)
                .HasColumnName("origin");
            entity.Property(e => e.PlantName)
                .HasMaxLength(100)
                .HasColumnName("plant_name");
            entity.Property(e => e.ScienceName)
                .HasMaxLength(100)
                .HasColumnName("science_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasMany(d => d.Diseases).WithMany(p => p.Plants)
             .UsingEntity<PlantDisease>(
                 r => r.HasOne(pd => pd.DiseaseType)
                       .WithMany()
                       .HasForeignKey(pd => pd.DiseaseId)
                       .HasConstraintName("plant_disease_disease_id_fkey"),
                 l => l.HasOne(pd => pd.Plant)
                       .WithMany()
                       .HasForeignKey(pd => pd.PlantId)
                       .HasConstraintName("plant_disease_plant_id_fkey"),
                 j =>
                 {
                     j.HasKey(pd => new { pd.PlantId, pd.DiseaseId }).HasName("plant_disease_pkey");
                     j.ToTable("plant_disease");
                     j.Property(pd => pd.PlantId).HasColumnName("plant_id");
                     j.Property(pd => pd.DiseaseId).HasColumnName("disease_id");
                 });

            entity.HasMany(d => d.Fertilizers).WithMany(p => p.Plants)
                .UsingEntity<PlantFertilizer>(
                    r => r.HasOne(pf => pf.FertilizerType)
                          .WithMany()
                          .HasForeignKey(pf => pf.FertilizerId)
                          .HasConstraintName("plant_fertilizer_fertilizer_id_fkey"),
                    l => l.HasOne(pf => pf.Plant)
                          .WithMany()
                          .HasForeignKey(pf => pf.PlantId)
                          .HasConstraintName("plant_fertilizer_plant_id_fkey"),
                    j =>
                    {
                        j.HasKey(pf => new { pf.PlantId, pf.FertilizerId }).HasName("plant_fertilizer_pkey");
                        j.ToTable("plant_fertilizer");
                        j.Property(pf => pf.PlantId).HasColumnName("plant_id");
                        j.Property(pf => pf.FertilizerId).HasColumnName("fertilizer_id");
                    });

            entity.HasMany(d => d.Methods).WithMany(p => p.Plants)
                .UsingEntity<PlantMethod>(
                    r => r.HasOne(pm => pm.CultivationMethod)
                          .WithMany()
                          .HasForeignKey(pm => pm.MethodId)
                          .HasConstraintName("plant_method_method_id_fkey"),
                    l => l.HasOne(pm => pm.Plant)
                          .WithMany()
                          .HasForeignKey(pm => pm.PlantId)
                          .HasConstraintName("plant_method_plant_id_fkey"),
                    j =>
                    {
                        j.HasKey(pm => new { pm.PlantId, pm.MethodId }).HasName("plant_method_pkey");
                        j.ToTable("plant_method");
                        j.Property(pm => pm.PlantId).HasColumnName("plant_id");
                        j.Property(pm => pm.MethodId).HasColumnName("method_id");
                    });

            entity.HasMany(d => d.Regions).WithMany(p => p.Plants)
                .UsingEntity<PlantRegion>(
                    r => r.HasOne(pr => pr.SuitableRegion)
                          .WithMany()
                          .HasForeignKey(pr => pr.RegionId)
                          .HasConstraintName("plant_region_region_id_fkey"),
                    l => l.HasOne(pr => pr.Plant)
                          .WithMany()
                          .HasForeignKey(pr => pr.PlantId)
                          .HasConstraintName("plant_region_plant_id_fkey"),
                    j =>
                    {
                        j.HasKey(pr => new { pr.PlantId, pr.RegionId }).HasName("plant_region_pkey");
                        j.ToTable("plant_region");
                        j.Property(pr => pr.PlantId).HasColumnName("plant_id");
                        j.Property(pr => pr.RegionId).HasColumnName("region_id");
                    });

            entity.HasMany(d => d.Seasons).WithMany(p => p.Plants)
                .UsingEntity<PlantSeason>(
                    r => r.HasOne(ps => ps.Season)
                          .WithMany()
                          .HasForeignKey(ps => ps.SeasonId)
                          .HasConstraintName("plant_season_season_id_fkey"),
                    l => l.HasOne(ps => ps.Plant)
                          .WithMany()
                          .HasForeignKey(ps => ps.PlantId)
                          .HasConstraintName("plant_season_plant_id_fkey"),
                    j =>
                    {
                        j.HasKey(ps => new { ps.PlantId, ps.SeasonId }).HasName("plant_season_pkey");
                        j.ToTable("plant_season");
                        j.Property(ps => ps.PlantId).HasColumnName("plant_id");
                        j.Property(ps => ps.SeasonId).HasColumnName("season_id");
                    });

            entity.HasMany(d => d.Soils).WithMany(p => p.Plants)
     .UsingEntity<PlantSoil>(
         j => j.HasOne(ps => ps.SoilType)
               .WithMany()
               .HasForeignKey(ps => ps.SoilId)
               .HasConstraintName("plant_soil_soil_id_fkey"),
         j => j.HasOne(ps => ps.Plant)
               .WithMany()
               .HasForeignKey(ps => ps.PlantId)
               .HasConstraintName("plant_soil_plant_id_fkey"),
         j =>
         {
             j.HasKey(ps => new { ps.PlantId, ps.SoilId }).HasName("plant_soil_pkey");
             j.ToTable("plant_soil");
             j.Property(ps => ps.PlantId).HasColumnName("plant_id");
             j.Property(ps => ps.SoilId).HasColumnName("soil_id");
         });

            entity.HasMany(d => d.Types).WithMany(p => p.Plants)
       .UsingEntity<PlantPlanttype>(
           r => r.HasOne(pt => pt.PlantType)
                 .WithMany()
                 .HasForeignKey(pt => pt.TypeId)
                 .HasConstraintName("plant_planttype_type_id_fkey"),
           l => l.HasOne(pt => pt.Plant)
                 .WithMany()
                 .HasForeignKey(pt => pt.PlantId)
                 .HasConstraintName("plant_planttype_plant_id_fkey"),
           j =>
           {
               j.HasKey(pt => new { pt.PlantId, pt.TypeId }).HasName("plant_planttype_pkey");
               j.ToTable("plant_planttype");
               j.Property(pt => pt.PlantId).HasColumnName("plant_id");
               j.Property(pt => pt.TypeId).HasColumnName("type_id");
           });
        });

        modelBuilder.Entity<PlantType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("plant_type_pkey");

            entity.ToTable("plant_type");

            entity.HasIndex(e => e.TypeName, "plant_type_type_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TypeName)
                .HasMaxLength(100)
                .HasColumnName("type_name");
        });

        modelBuilder.Entity<Plantimage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("plantimages_pkey");

            entity.ToTable("plantimages");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ImageUrl).HasColumnName("image_url");
            entity.Property(e => e.PlantId).HasColumnName("plant_id");

            entity.HasOne(d => d.Plant).WithMany(p => p.Plantimages)
                .HasForeignKey(d => d.PlantId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("plantimages_plant_id_fkey");
        });

        modelBuilder.Entity<Season>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("season_pkey");

            entity.ToTable("season");

            entity.HasIndex(e => e.SeasonName, "season_season_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EndMonth).HasColumnName("end_month");
            entity.Property(e => e.SeasonName)
                .HasMaxLength(50)
                .HasColumnName("season_name");
            entity.Property(e => e.StartMonth).HasColumnName("start_month");
        });

        modelBuilder.Entity<SoilType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("soil_type_pkey");

            entity.ToTable("soil_type");

            entity.HasIndex(e => e.SoilName, "soil_type_soil_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.SoilName)
                .HasMaxLength(100)
                .HasColumnName("soil_name");
        });

        modelBuilder.Entity<SuitableRegion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("suitable_region_pkey");

            entity.ToTable("suitable_region");

            entity.HasIndex(e => e.RegionName, "suitable_region_region_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClimateType)
                .HasMaxLength(100)
                .HasColumnName("climate_type");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.RegionName)
                .HasMaxLength(100)
                .HasColumnName("region_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
