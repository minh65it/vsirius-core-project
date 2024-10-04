using FluentValidation;
namespace AmazonIntegrationDataApi.Dtos
{
    public class AmazonJewelryDataFeedValidator : AbstractValidator<AmazonJewelryDataFeedItem_Dto>
    {
        public AmazonJewelryDataFeedValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.brand_name).NotEmpty().Length(1, 50).WithMessage("An alphanumeric string; 1 character minimum in length and 50 characters maximum in length.");
            RuleFor(x => x.item_name).NotEmpty().MaximumLength(250).WithMessage("Max. 250 characters");
            RuleFor(x => x.manufacturer).NotEmpty().Length(1, 50).WithMessage("An alphanumeric string; 1 character minimum in length and 50 characters maximum in length.");

        }
    }
    
}
