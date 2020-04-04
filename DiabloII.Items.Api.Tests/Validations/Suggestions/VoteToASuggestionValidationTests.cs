using System;
using DiabloII.Items.Api.Application.Requests.Suggestions;
using DiabloII.Items.Api.Application.Validations.Suggestions.Vote;
using DiabloII.Items.Api.Domain.Exceptions;
using DiabloII.Items.Api.Domain.Models.Suggestions;
using DiabloII.Items.Api.Infrastructure.DbContext;
using DiabloII.Items.Api.Infrastructure.Helpers;
using DiabloII.Items.Api.Infrastructure.Repositories.Suggestions;
using NUnit.Framework;
using Shouldly;

namespace DiabloII.Items.Api.Tests.Validations.Suggestions
{
    [TestFixture]
    public class VoteToASuggestionValidationTests
    {
        private ApplicationDbContext _dbContext;
        private VoteToASuggestionValidator _validator;
        private VoteToASuggestionValidationContext _validationContext;
        private ISuggestionRepository _repository;

        [SetUp]
        public void Setup()
        {
            var validDto = new VoteToASuggestionDto
            {
                Ip = "213.91.163.4",
                IsPositive = true,
                SuggestionId = Guid.NewGuid()
            };

            _dbContext = DatabaseHelpers.CreateMyTestDbContext();
            _repository = new SuggestionRepository(_dbContext);
          
            _validator = new VoteToASuggestionValidator();
            _validationContext = new VoteToASuggestionValidationContext(validDto, _repository);
        }

        [Test]
        public void WhenIpIsNull_ShouldThrowABadRequestException()
        {
            _validationContext.Dto.Ip = null;

            Should.Throw<BadRequestException>(() => _validator.Validate(_validationContext));
        }

        [Test]
        public void WhenIpIsEmpty_ShouldThrowABadRequestException()
        {
            _validationContext.Dto.Ip = string.Empty;

            Should.Throw<BadRequestException>(() => _validator.Validate(_validationContext));
        }

        [Test]
        public void WhenIpIsNotAnIpV4_ShouldThrowABadRequestException()
        {
            _validationContext.Dto.Ip = "213.91.163.4444";

            Should.Throw<BadRequestException>(() => _validator.Validate(_validationContext));
        }

        [Test]
        public void WhenSuggestionDoesNotExists_ShouldThrowANotFoundException() => 
            Should.Throw<NotFoundException>(() => _validator.Validate(_validationContext));

        [Test]
        public void WhenDtoIsValid_ShouldSuccess()
        {
            var suggestion = new Suggestion
            {
                Id = _validationContext.Dto.SuggestionId
            };

            _dbContext.Suggestions.Add(suggestion);
            _dbContext.SaveChanges();

            Should.NotThrow(() => _validator.Validate(_validationContext));
        }
    }
}