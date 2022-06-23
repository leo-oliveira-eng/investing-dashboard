using DataProvider.Domain.Users.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Valuables.Utils;

namespace DataProvider.Persistence.SQL.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User));

            builder.HasKey(user => user.Id);

            builder.Ignore(user => user.DomainEvents);

            builder.OwnsOne(user => user.Email, email =>
            {
                email.Property(email => email.Address).HasColumnName(nameof(Email));
            });

            builder.OwnsOne(user => user.CPF, cpf =>
            {
                cpf.Property(cpf => cpf.Text).HasColumnName(nameof(CPF));
            });
        }
    }
}
