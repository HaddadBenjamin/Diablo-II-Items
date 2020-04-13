﻿using System;
using System.Linq;
using System.Threading.Tasks;
using DiabloII.Application.Requests.Suggestions;
using DiabloII.Application.Tests.Startup;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace DiabloII.Application.Tests.Suggestions.Create
{
    [Binding]
    [Scope(Tag = "suggestion")]
    public class DeleteASuggestionCommentSteps
    {
        private readonly SuggestionApi _suggestionApi;

        public DeleteASuggestionCommentSteps(MyTestContext testContext)
        {
            _suggestionApi = testContext.Apis.Suggestions;
        }

        [When(@"I delete the suggestion comment ""(.*)"" from the suggestion ""(.*)""")]
        public async Task WhenIDeleteTheSuggestionCommentFromTheSuggestion(string suggestionCommentContent, string suggestionContent, Table table)
        {
            var suggestionDto = (await _suggestionApi.GetAll())
                .Single(suggestion => suggestion.Content == suggestionContent);
            var (suggestionId, suggestionCommentId) = (
                suggestionDto.Id,
                suggestionDto.Comments.Single(comment => comment.Comment == suggestionCommentContent).Id);

            var dto = table.CreateInstance<DeleteASuggestionCommentDto>();
            dto.Id = suggestionCommentId;
            dto.SuggestionId = suggestionId;

            await _suggestionApi.DeleteComment(dto);
        }
    }
}