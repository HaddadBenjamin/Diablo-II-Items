﻿using DiabloII.Application.Responses.Suggestions;
using Halcyon.HAL;
using Microsoft.AspNetCore.Mvc;

namespace DiabloII.Application.Services.Hals.Suggestions
{
    public interface ISuggestionHalService
    {
        HALResponse AddLinks(SuggestionDto dto, ControllerBase controller);
    }
}