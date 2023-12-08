using AadhaarSplitterAPI.DbSets;
using FluentValidation;
namespace AadhaarSplitterAPI.DbSetsValidator
{
    public class AadharValidator : AbstractValidator<Aadhar>
    {
        public AadharValidator()
        {
            RuleFor(aadhar => aadhar.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(aadhar => aadhar.Description).NotEmpty().WithMessage("Description is required.");
        }
    }
}
