using FluentValidation;
using LibraryNET.Data.Entities;

namespace LibraryNET.Api.ApiDefinitions.Users.Requests;

public sealed class RequestCreateUser
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? EmailAddress { get; set; }
    
    internal class Validator : AbstractValidator<RequestCreateUser>
    {
        public Validator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MaximumLength(UserDetails.Constraints.NameMaxLength);
            
            RuleFor(x => x.Address)
                .NotNull()
                .NotEmpty()
                .MaximumLength(UserDetails.Constraints.AddressMaxLength);
            
            RuleFor(x => x.PhoneNumber)
                .MaximumLength(UserDetails.Constraints.PhoneNumberMaxLength);
            
            RuleFor(x => x.EmailAddress)
                .NotNull()
                .NotEmpty()
                .MaximumLength(UserDetails.Constraints.EmailAddressMaxLength);
        }
    }
}