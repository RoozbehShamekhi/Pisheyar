﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Pisheyar.Application.Categories.Queries.GetAllCategories
{
    public class AllCategoriesVm
    {
        public string Message { get; set; }

        public bool Result { get; set; }

        public IEnumerable<AllCategoryDto> Categories { get; set; }
    }
}