using BIP.DataAccess.Dtos.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIP.Core.Features.ApplicationUser.Commands.Validations
{
    public class RegisterRequestdtoValidator : AbstractValidator<RegisterRequestdto>
    {
        public RegisterRequestdtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty()
                .WithMessage("Password must be at least 8 digits and should contain Lowercase, NonAlphanummeric and Uppercase");

            RuleFor(x => x.UserName)
           .NotEmpty()
           .Matches("^[1-4][0-9]{9}$")
           .WithMessage("UserName must be a 10-digit identity number starting with 1, 2, 3, or 4.");

        }
    }
}
