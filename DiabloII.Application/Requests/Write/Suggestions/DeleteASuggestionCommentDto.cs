﻿using System;

namespace DiabloII.Application.Requests.Write.Suggestions
{
    public class DeleteASuggestionCommentDto
    {
        public Guid Id { get; set; }

        public Guid SuggestionId { get; set; }

        public string UserId { get; set; }
    }
}