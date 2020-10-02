using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NSE.Clientes.API.Models;
using NSE.Core.DomainObjects;

namespace NSE.Clientes.API.Data.Mappings
{
    public class ClienteMapping : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Nome)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.OwnsOne(x => x.Cpf, b =>
            {
                b.Property(x => x.Numero)
                    .IsRequired()
                    .HasMaxLength(CPF.QtdMaximaDeCaracteres)
                    .HasColumnName("Cpf")
                    .HasColumnType($"varchar({CPF.QtdMaximaDeCaracteres})");
            });

            builder.OwnsOne(x => x.Email, b =>
            {
                b.Property(x  => x.Endereco)
                    .IsRequired()
                    .HasColumnName("Email")
                    .HasColumnType($"varchar({Email.EnderecoMaxLenght})");
            });
            
            // 1 : 1 => Aluno : Endereco
            builder.HasOne(c => c.Endereco)
                .WithOne(c => c.Cliente);

            builder.ToTable("Clientes");

        }
    }
}