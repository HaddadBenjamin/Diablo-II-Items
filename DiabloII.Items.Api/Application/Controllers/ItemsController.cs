﻿using System.Collections.Generic;
using System.Linq;
using DiabloII.Items.Api.Application.Mappers.Items;
using DiabloII.Items.Api.Application.Requests.Items;
using DiabloII.Items.Api.Application.Responses.Items;
using DiabloII.Items.Api.Domain.Queries.Items;
using DiabloII.Items.Api.Infrastructure.Services.Items;
using Microsoft.AspNetCore.Mvc;

namespace DiabloII.Items.Api.Application.Controllers
{
    // Remember : dotnet run watch.
    [Route("api/v1/[controller]")]
    public class ItemsController : Controller
    {
        private readonly IItemsService _itemsService;

        public ItemsController(IItemsService itemsService) => _itemsService = itemsService;

        [Route("getalluniques")]
        [HttpGet]
        public IReadOnlyCollection<ItemDto> GetAllUniques() => _itemsService
            .GetAllUniques()
            .Select(ItemMapper.ToItemDto)
            .ToList();

        [Route("searchuniques")]
        [HttpGet]
        public IReadOnlyCollection<ItemDto> SearchUniques(SearchUniquesDto searchDto) => _itemsService
            .SearchUniques(new SearchUniquesQuery(searchDto))
            .Select(ItemMapper.ToItemDto)
            .ToList();
    }
}
