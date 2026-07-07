using FluentValidation;

namespace LibraryNET.Api.ApiDefinitions.BookDetails.Requests;

internal sealed class RequestUpdateBookDetails
{
    public long? AuthorId { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    
    internal class Validator : AbstractValidator<RequestUpdateBookDetails>
    {
        public Validator()
        {
            RuleFor(x => x.AuthorId)
                .GreaterThan(0);
            
            RuleFor(x => x.Name)
                .MaximumLength(Data.Entities.BookDetails.Constraints.NameMaxLength);
            
            RuleFor(x => x.Description)
                .MaximumLength(Data.Entities.BookDetails.Constraints.DescriptionMaxLength);
        }
    }
}