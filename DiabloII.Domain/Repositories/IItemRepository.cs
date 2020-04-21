﻿using System.Collections.Generic;
using DiabloII.Domain.Models.Items;
using DiabloII.Domain.Queries.Items;
using DiabloII.Domain.Repositories.Bases;

namespace DiabloII.Domain.Repositories
{
    public interface IItemRepository :
        IGetAllRepository<Item>,
        ISearchRepository<SearchUniquesQuery, Item>
    {
        #region Write
        void ResetTheItems(IList<Item> items, IList<ItemProperty> itemProperties);
        #endregion
    }
}