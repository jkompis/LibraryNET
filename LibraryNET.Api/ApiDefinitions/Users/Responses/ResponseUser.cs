using System;
using System.Linq.Expressions;
using LibraryNET.Data.Entities;

namespace LibraryNET.Api.ApiDefinitions.Users.Responses;

public sealed class ResponseUser
{
    public long Id { get; set; }
    public DateTime RegisteredAt { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public string? PhoneNumber { get; set; }
    public required string EmailAddress { get; set; }

    public static Expression<Func<User, ResponseUser>> Map()
    {
        return u => new ResponseUser
        {
            Id = u.Id,
            RegisteredAt = u.RegisteredAt,
            Name = u.UserDetails!.Name,
            Address = u.UserDetails!.Address,
            PhoneNumber = u.UserDetails!.PhoneNumber,
            EmailAddress = u.UserDetails!.EmailAddress
        };
    }
}