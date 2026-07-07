using FluentValidation;

namespace LibraryNET.Api.ApiDefinitions.BookDetails.Requests;

internal sealed class RequestCreateBookDetails
{
    public long AuthorId { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }

    internal class Validator : AbstractValidator<RequestCreateBookDetails>
    {
        public Validator()
        {
            RuleFor(x => x.AuthorId)
                .GreaterThan(0);
            
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(Data.Entities.BookDetails.Constraints.NameMaxLength);
            
            RuleFor(x => x.Description)
                .NotNull()
                .NotEmpty()
                .MaximumLength(Data.Entities.BookDetails.Constraints.DescriptionMaxLength);
        }
    }
}