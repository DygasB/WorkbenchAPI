using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkbenchAPI.Entities;

namespace WorkbenchAPI.Models.Validators
{
    public class RegiresterClientDtoValidator : AbstractValidator<RegisterClientDto>
    {
        
        public RegiresterClientDtoValidator(WorkbenchDbContext workbenchDbContext)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(x => x.Password)
                .MinimumLength(6);
            RuleFor(x => x.ConfirmPassword).Equal(e => e.Password);

            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    var emailInUse = workbenchDbContext.Clients.Any(c => c.Email == value);
                    if (emailInUse)
                    {
                        context.AddFailure("Email", "That Email has taken");
                    }

                });
        }
    }
}
