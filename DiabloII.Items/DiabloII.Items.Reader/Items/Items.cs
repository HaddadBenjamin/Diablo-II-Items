﻿using System.Collections.Generic;

namespace DiabloII.Items.Reader
{
    public class Item
    {
        public string Name { get; set; }
        public string Quality { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public int LevelRequired { get; set; }
        public int Level { get; set; }
        public IEnumerable<ItemProperty> Properties { get; set; }
    }
}