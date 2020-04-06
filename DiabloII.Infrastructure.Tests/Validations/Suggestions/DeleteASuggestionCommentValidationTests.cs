using System;
using System.Collections.Generic;
using System.Linq;
using DiabloII.Domain.Commands.Suggestions;
using DiabloII.Domain.Exceptions;
using DiabloII.Domain.Models.Suggestions;
using DiabloII.Domain.Repositories;
using DiabloII.Domain.Validations.Suggestions.DeleteAComment;
using DiabloII.Infrastructure.DbContext;
using DiabloII.Infrastructure.Repositories;
using DiabloII.Infrastructure.Tests.Helpers;
using NUnit.Framework;
using Shouldly;

namespace DiabloII.Infrastructure.Tests.Validations.Suggestions
{
    [TestFixture]
    public class DeleteASuggestionCommentValidationTests
    {
        private ApplicationDbContext _dbContext;
        private DeleteASuggestionCommentValidator _validator;
        private DeleteASuggestionCommentValidationContext _validationContext;
        private ISuggestionRepository _repository;

        [SetUp]
        public void Setup()
        {
            var validCommand = new DeleteASuggestionCommentCommand
            {
                Id = Guid.NewGuid(),
                SuggestionId = Guid.NewGuid(),
                UserId = "any value"
            };

            _dbContext = DatabaseHelpers.CreateMyTestDbContext();
            _repository = new SuggestionRepository(_dbContext);

            _validator = new DeleteASuggestionCommentValidator();
            _validationContext = new DeleteASuggestionCommentValidationContext(validCommand, _repository);
        }

        [Test]
        public void WhenUserIdIsNull_ShouldThrowABadRequestException()
        {
            _validationContext.Command.UserId = null;

            Should.Throw<BadRequestException>(() => _validator.Validate(_validationContext));
        }

        [Test]
        public void WhenUserIdIsEmpty_ShouldThrowABadRequestException()
        {
            _validationContext.Command.UserId = string.Empty;

            Should.Throw<BadRequestException>(() => _validator.Validate(_validationContext));
        }

        [Test]
        public void WhenSuggestionDoesNotExists_ShouldThrowANotFoundException() =>
            Should.Throw<NotFoundException>(() => _validator.Validate(_validationContext));

        [Test]
        public void WhenSuggestionCommentDoesNotExists_ShouldThrowANotFoundException()
        {
            AddTheValidSuggestion();

            var suggestion = _dbContext.Suggestions.First();
            var comment = suggestion.Comments.First();

            _repository.RemoveUserComment(suggestion.Id, comment.Id, comment.CreatedBy);
            _dbContext.SaveChanges();

            Should.Throw<NotFoundException>(() => _validator.Validate(_validationContext));
        }

        [Test]
        public void WhenUserIsNotOwnerOfTheSuggestionComment_ShouldThrowAUnauthorizedException()
        {
            AddTheValidSuggestion();

            var suggestion = _dbContext.Suggestions.First();
            var comment = suggestion.Comments.First();
            var newComment = new SuggestionComment
            {
                Id = comment.Id,
                CreatedBy = "other user id"
            };

            _repository.RemoveUserComment(suggestion.Id, comment.Id, comment.CreatedBy);
            _repository.AddComment(suggestion.Id, newComment);
            _dbContext.SaveChanges();

            Should.Throw<UnauthorizedException>(() => _validator.Validate(_validationContext));
        }

        [Test]
        public void WhenCommandIsValid_ShouldSuccess()
        {
            AddTheValidSuggestion();

            Should.NotThrow(() => _validator.Validate(_validationContext));
        }

        private void AddTheValidSuggestion()
        {
            var suggestion = new Suggestion
            {
                Id = _validationContext.Command.SuggestionId,
                CreatedBy = _validationContext.Command.UserId,
                Comments = new List<SuggestionComment>
                {
                    new SuggestionComment
                    {
                        Id = _validationContext.Command.Id,
                        CreatedBy = _validationContext.Command.UserId
                    }
                }
            };

            _dbContext.Suggestions.Add(suggestion);
            _dbContext.SaveChanges();
        }
    }
}