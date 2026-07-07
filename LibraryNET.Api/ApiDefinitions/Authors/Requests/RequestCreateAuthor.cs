using FluentValidation;
using LibraryNET.Data.Entities;

namespace LibraryNET.Api.ApiDefinitions.Authors.Requests;

public sealed class RequestCreateAuthor
{
    public string? Name { get; init; }
    
    internal class Validator : AbstractValidator<RequestCreateAuthor>
    {
        public Validator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(BookAuthor.Constraints.NameMaxLength);
            
        }
    }
}