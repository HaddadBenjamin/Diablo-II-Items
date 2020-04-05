﻿using FluentValidation;

namespace DiabloII.Items.Api.Application.Validations.Suggestions.Vote
{
    public class VoteToASuggestionValidator : AbstractValidator<VoteToASuggestionValidationContext>
    {
        public VoteToASuggestionValidator()
        {
            RuleFor(context => context.Command.Ip).ShouldBeAValidIp();
            RuleFor(context => context.DbContextValidationContext).SuggestionShouldExists();
        }
    }
}