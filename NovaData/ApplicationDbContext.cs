using Microsoft.EntityFrameworkCore;
using NovaModel;

namespace NovaData
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }

        public DbSet<UsuarioModel> Usuarios { get; set; }
        public DbSet<SkillModel> Skills { get; set; }
        public DbSet<RecomendacaoModel> Recomendacoes { get; set; }
        public DbSet<TrilhaModel> Trilhas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração de Usuario
            modelBuilder.Entity<UsuarioModel>(entity =>
            {
                entity.ToTable("Usuarios");
                entity.HasKey(u => u.Id);
                
                entity.Property(u => u.Nome)
                    .IsRequired()
                    .HasMaxLength(120);

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasIndex(u => u.Email)
                    .IsUnique();

                entity.Property(u => u.AreaInteresse)
                    .HasMaxLength(80);

                entity.Property(u => u.ObjetivoProfissional)
                    .HasMaxLength(200);

                entity.Property(u => u.CriadoEm)
                    .HasDefaultValueSql("GETDATE()");

                // Relacionamento com Skills
                entity.HasMany(u => u.Skills)
                    .WithOne(s => s.Usuario)
                    .HasForeignKey(s => s.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relacionamento com Recomendações
                entity.HasMany(u => u.Recomendacoes)
                    .WithOne(r => r.Usuario)
                    .HasForeignKey(r => r.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuração de Skill
            modelBuilder.Entity<SkillModel>(entity =>
            {
                entity.ToTable("Skills");
                entity.HasKey(s => s.Id);

                entity.Property(s => s.Nome)
                    .IsRequired()
                    .HasMaxLength(80);

                entity.Property(s => s.Nivel)
                    .IsRequired();

                entity.HasIndex(s => new { s.UsuarioId, s.Nome })
                    .IsUnique();
            });

            // Configuração de Recomendação
            modelBuilder.Entity<RecomendacaoModel>(entity =>
            {
                entity.ToTable("Recomendacoes");
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Titulo)
                    .IsRequired()
                    .HasMaxLength(120);

                entity.Property(r => r.Descricao)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(r => r.CriadaEm)
                    .HasDefaultValueSql("GETDATE()");
            });

            // Configuração de Trilha
            modelBuilder.Entity<TrilhaModel>(entity =>
            {
                entity.ToTable("Trilhas");
                entity.HasKey(t => t.Id);

                entity.Property(t => t.AreaInteresse)
                    .IsRequired()
                    .HasMaxLength(80);

                entity.Property(t => t.SkillRelacionada)
                    .IsRequired()
                    .HasMaxLength(80);

                entity.Property(t => t.NivelMinimo)
                    .IsRequired();

                entity.Property(t => t.TituloRecomendacao)
                    .IsRequired()
                    .HasMaxLength(120);

                entity.Property(t => t.DescricaoRecomendacao)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasIndex(t => new { t.AreaInteresse, t.SkillRelacionada, t.NivelMinimo });
            });
        }
    }
}
