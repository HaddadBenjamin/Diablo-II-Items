﻿using System;
using System.Collections.Generic;
using DiabloII.Items.Api.Requests.Suggestions;
using DiabloII.Items.Api.Responses.Suggestions;

namespace DiabloII.Items.Api.Services.Suggestions
{
    public interface ISuggestionsService
    {
        SuggestionDto Create(CreateASuggestionDto createASugestion);

        SuggestionDto Vote(VoteToASuggestionDto voteToASuggestion);

        SuggestionDto Comment(CommentASuggestionDto commentASuggestion);

        IReadOnlyCollection<SuggestionDto> GetAll();

        Guid Delete(DeleteASuggestionDto deleteASuggestion);

        SuggestionDto DeleteAComment(DeleteASuggestionCommentDto deleteASuggestionComment);
    }
}