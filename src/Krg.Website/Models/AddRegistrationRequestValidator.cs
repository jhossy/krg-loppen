﻿using FluentValidation;
using Krg.Domain.Models;

namespace Krg.Website.Models
{
    public class AddRegistrationRequestValidator : AbstractValidator<AddRegistrationRequest>
    {
        public AddRegistrationRequestValidator()
        {
            RuleFor(request => request.EventDate).NotEmpty();
            RuleFor(request => request.Name).MinimumLength(1);
            RuleFor(request => request.Email).EmailAddress();
            RuleFor(request => request.PhoneNo).MinimumLength(8);
            RuleFor(request => request.Department).NotEmpty();
        }
    }
}
