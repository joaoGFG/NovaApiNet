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
                entity.ToTable("NET_USUARIOS"); // ← PREFIXO ADICIONADO
                entity.HasKey(u => u.Id);
                
                entity.Property(u => u.Id)
                    .HasColumnName("ID");

                entity.Property(u => u.Nome)
                    .HasColumnName("NOME")
                    .IsRequired()
                    .HasMaxLength(120);

                entity.Property(u => u.Email)
                    .HasColumnName("EMAIL")
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasIndex(u => u.Email)
                    .IsUnique();

                entity.Property(u => u.AreaInteresse)
                    .HasColumnName("AREA_INTERESSE")
                    .HasMaxLength(80);

                entity.Property(u => u.ObjetivoProfissional)
                    .HasColumnName("OBJETIVO_PROFISSIONAL")
                    .HasMaxLength(200);

                entity.Property(u => u.CriadoEm)
                    .HasColumnName("CRIADO_EM")
                    .HasDefaultValueSql("SYSDATE");

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
                entity.ToTable("NET_SKILLS"); // ← PREFIXO ADICIONADO
                entity.HasKey(s => s.Id);

                entity.Property(s => s.Id)
                    .HasColumnName("ID");

                entity.Property(s => s.UsuarioId)
                    .HasColumnName("USUARIO_ID");

                entity.Property(s => s.Nome)
                    .HasColumnName("NOME")
                    .IsRequired()
                    .HasMaxLength(80);

                entity.Property(s => s.Nivel)
                    .HasColumnName("NIVEL")
                    .IsRequired();

                entity.HasIndex(s => new { s.UsuarioId, s.Nome })
                    .IsUnique();
            });

            // Configuração de Recomendação
            modelBuilder.Entity<RecomendacaoModel>(entity =>
            {
                entity.ToTable("NET_RECOMENDACOES"); // ← PREFIXO ADICIONADO
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Id)
                    .HasColumnName("ID");

                entity.Property(r => r.UsuarioId)
                    .HasColumnName("USUARIO_ID");

                entity.Property(r => r.Titulo)
                    .HasColumnName("TITULO")
                    .IsRequired()
                    .HasMaxLength(120);

                entity.Property(r => r.Descricao)
                    .HasColumnName("DESCRICAO")
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(r => r.CriadaEm)
                    .HasColumnName("CRIADA_EM")
                    .HasDefaultValueSql("SYSDATE");
            });

            // Configuração de Trilha
            modelBuilder.Entity<TrilhaModel>(entity =>
            {
                entity.ToTable("NET_TRILHAS"); // ← PREFIXO ADICIONADO
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Id)
                    .HasColumnName("ID");

                entity.Property(t => t.AreaInteresse)
                    .HasColumnName("AREA_INTERESSE")
                    .IsRequired()
                    .HasMaxLength(80);

                entity.Property(t => t.SkillRelacionada)
                    .HasColumnName("SKILL_RELACIONADA")
                    .IsRequired()
                    .HasMaxLength(80);

                entity.Property(t => t.NivelMinimo)
                    .HasColumnName("NIVEL_MINIMO")
                    .IsRequired();

                entity.Property(t => t.TituloRecomendacao)
                    .HasColumnName("TITULO_RECOMENDACAO")
                    .IsRequired()
                    .HasMaxLength(120);

                entity.Property(t => t.DescricaoRecomendacao)
                    .HasColumnName("DESCRICAO_RECOMENDACAO")
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasIndex(t => new { t.AreaInteresse, t.SkillRelacionada, t.NivelMinimo });
            });
        }
    }
}
