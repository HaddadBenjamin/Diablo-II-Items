﻿using DiabloII.Items.Api.Domain.Commands.Suggestions;
using DiabloII.Items.Api.Infrastructure.Repositories.Suggestions;

namespace DiabloII.Items.Api.Application.Validations.Suggestions.Comment
{
    public class CommentASuggestionValidationContext
    {
        public CommentASuggestionCommand Command { get; set; }

        public SuggestionDbContextValidationContext DbContextValidationContext { get; }

        public CommentASuggestionValidationContext(CommentASuggestionCommand command, ISuggestionRepository repository)
        {
            Command = command;
            DbContextValidationContext = new SuggestionDbContextValidationContext(repository, Command.SuggestionId);
        }
    }
}