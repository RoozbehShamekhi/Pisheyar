﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Pisheyar.Application.Categories.Queries.GetCategoryByGuid
{
    public class CategoryVm
    {
        public string Message { get; set; }

        public bool Result { get; set; }

        public CategoryDto Category { get; set; }
    }
}