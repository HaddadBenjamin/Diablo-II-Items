﻿using DiabloII.Items.Api.Domain.Commands.Suggestions;
using DiabloII.Items.Api.Infrastructure.Repositories.Suggestions;

namespace DiabloII.Items.Api.Application.Validations.Suggestions.Create
{
    public class CreateASuggestionValidationContext
    {
        public CreateASuggestionCommand Command { get; set; }
      
        public SuggestionDbContextValidationContext DbContextValidationContext { get; }

        public CreateASuggestionValidationContext(CreateASuggestionCommand command, ISuggestionRepository repository)
        {
            Command = command;
            DbContextValidationContext = new SuggestionDbContextValidationContext(repository)
            {
                Content = Command.Content
            };
        }
    }
}