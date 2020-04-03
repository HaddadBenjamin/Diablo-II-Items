﻿using System;
using System.Linq;
using DiabloII.Items.Api.DbContext.Suggestions.Models;
using DiabloII.Items.Api.Requests.Suggestions;
using DiabloII.Items.Api.Responses.Suggestions;

namespace DiabloII.Items.Api.Mappers.Suggestions
{
    public static class SuggestionMapper
    {
        public static SuggestionDto ToSuggestionDto(Suggestion suggestion) => new SuggestionDto
        {
            Id = suggestion.Id,
            Ip = suggestion.Ip,
            Content = suggestion.Content,
            PositiveVoteCount = suggestion.Votes.Count(vote => vote.IsPositive),
            NegativeVoteCount = suggestion.Votes.Count(vote => !vote.IsPositive),
            Votes = suggestion.Votes.Select(ToSugugestionVoteDto).ToList(),
            Comments = suggestion.Comments.Select(ToSuggestionCommentDto).ToList()
        };

        private static SuggestionCommentDto ToSuggestionCommentDto(SuggestionComment comment)
            => new SuggestionCommentDto {Ip = comment.Ip, Comment = comment.Comment};

        private static SuggestionVoteDto ToSugugestionVoteDto(SuggestionVote vote)
            => new SuggestionVoteDto {Ip = vote.Ip, IsPositive = vote.IsPositive};

        public static Suggestion ToSuggestion(CreateASuggestionDto createASuggestionDto) => new Suggestion
        {
            Id = Guid.NewGuid(),
            Ip = createASuggestionDto.Ip,
            Content = createASuggestionDto.Content,
        };

        public static SuggestionVote ToSuggestionVote(VoteToASuggestionDto voteToASuggestionDto) => new SuggestionVote
        {
            Id = Guid.NewGuid(),
            IsPositive = voteToASuggestionDto.IsPositive,
            Ip = voteToASuggestionDto.Ip
        };

        public static SuggestionComment ToSuggestionComment(CommentASuggestionDto commentASuggestionDto) => new SuggestionComment
        {
            Id = Guid.NewGuid(),
            Comment = commentASuggestionDto.Comment,
            Ip = commentASuggestionDto.Ip
        };
    }
}