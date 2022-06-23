using BaseEntity.Domain.Entities;
using Valuables.Utils;

namespace DataProvider.Domain.Users.Models
{
    public class User : Entity
    {
        #region Properties

        public string Name { get; set; } = null!;

        public Email Email { get; set; } = null!;

        public CPF CPF { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        #endregion

        #region Constructors

        [Obsolete(ConstructorObsoleteMessage, true)]
        User() { }

        public User(string name, Email email, CPF cpf, string phoneNumber)
            : base(Guid.NewGuid())
        {
            Name = name;
            Email = email;
            CPF = cpf;
            PhoneNumber = phoneNumber;
        }

        #endregion
    }
}
