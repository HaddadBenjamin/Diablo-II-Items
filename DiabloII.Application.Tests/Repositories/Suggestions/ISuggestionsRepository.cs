﻿using System;
using System.Threading.Tasks;
using DiabloII.Application.Responses.Read.Domains.Suggestions;

namespace DiabloII.Application.Tests.Repositories.Suggestions
{
    public interface ISuggestionsRepository
    {
        Task<Guid> GetSuggestionId(string suggestionContent);

        Task<SuggestionDto> GetSuggestion(string suggestionContent);

        Guid GetSuggestionCommentId(SuggestionDto suggestionDto, string suggestionCommentContent);
    }
}