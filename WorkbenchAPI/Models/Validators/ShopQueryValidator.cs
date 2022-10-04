using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkbenchAPI.Entities;

namespace WorkbenchAPI.Models.Validators
{
    public class ShopQueryValidator :AbstractValidator<ShopQuery>
    {
        private int[] allowedPageSizes = new[] { 5, 10, 15 };
        private  string[] allowedSortByColumnNames = { nameof(Shop.Name), nameof(Shop.Description), nameof(Shop.Category) };
        public ShopQueryValidator()
        {
            RuleFor(r=>r.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(r => r.PageSize).Custom((value, context) =>
            {
                if (!allowedPageSizes.Contains(value))
                {
                    context.AddFailure("PageSize", $"PageSize must in [{string.Join(",", allowedPageSizes)}]");
                }
            });
            RuleFor(r=>r.SortBy).Must(value => string.IsNullOrEmpty(value) || allowedSortByColumnNames.Contains(value))
                .WithMessage($"Sort by is optional, or must be in [{string.Join(",",allowedSortByColumnNames)}]");
        }
    }
}
