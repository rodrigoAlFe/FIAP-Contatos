using FIAP.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIAP.Infrastructure.EntitiesConfiguration;

public class ContactEntityConfig : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder
            .ToTable("Contatos")
            .HasKey(c => c.Id);
        
        builder
            .Property(c => c.Id)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasColumnName("Id")
            .HasColumnType("int");
            
        builder
            .Property(c => c.Name)
            .IsRequired()
            .HasColumnName("Nome")
            .HasColumnType("varchar(100)")
            .HasMaxLength(100);
        
        builder
            .Property(c => c.Email)
            .IsRequired()
            .HasColumnName("Email")
            .HasColumnType("varchar(255)")
            .HasMaxLength(255);
        
        builder
            .Property(c => c.Phone)
            .IsRequired()
            .HasColumnName("Telefone")
            .HasColumnType("varchar(20)")
            .HasMaxLength(20);
        
        builder
            .Property(c=> c.Ddd )
            .IsRequired()
            .HasColumnName("DDD")
            .HasColumnType("varchar(2)")
            .HasMaxLength(2);
        
        builder
            .Property(c => c.CreatedAt)
            .IsRequired()
            .HasColumnName("CriadoEm")
            .HasColumnType("datetime");
        
        builder
            .Property(c => c.UpdatedAt)
            .IsRequired()
            .HasColumnName("AtualizadoEm")
            .HasColumnType("datetime");
        
        builder
            .HasIndex(c => c.Email, "IX_Contatos_Email")
            .IsUnique();
    }
}