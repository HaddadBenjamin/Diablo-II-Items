﻿using System.Text.RegularExpressions;
using DiabloII.Items.Api.Exceptions;
using FluentValidation;

namespace DiabloII.Items.Api.Validators
{
    public static class CommonSuggestionValidatorExtensions
    {
        private static readonly string IpV4Regex = @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b";
      
        public static IRuleBuilder<T, string> ShouldNotBeNullOrEmpty<T>(this IRuleBuilder<T, string> ruleBuilder, string fieldName) => ruleBuilder
            .NotNull()
            .NotEmpty()
            .OnFailure(context => throw new BadRequestException($"{fieldName} should not be null or empty"));

        public static IRuleBuilder<T, string> ShouldBeAnIpV4<T>(this IRuleBuilder<T, string> ruleBuilder, string fieldName = "Ip") => ruleBuilder
            .Must(ip => Regex.Match(ip, IpV4Regex).Success)
            .OnFailure(context => throw new BadRequestException($"{fieldName} should be an IPV4"));

        public static IRuleBuilder<T, string> ShouldBeAValidIp<T>(this IRuleBuilder<T, string> ruleBuilder, string fieldName = "Ip") => ruleBuilder
            .ShouldNotBeNullOrEmpty(fieldName)
            .ShouldBeAnIpV4();
    }
}