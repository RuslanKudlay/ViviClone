using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Models.ViewsModel
{
    public class AdditionalFilters
    {
        public GroupSortBy[] SortBy { get; set; } =
        {
            new GroupSortBy
            {
                NameGroup = "Цене",
                SortBy = new List<SortBy>
                {
                    new SortBy
                    {
                        Name = "От дешёвых к дорогим",
                        Value = "cheap",
                        IsSelected = false
                    },
                    new SortBy
                    {
                        Name = "От дорогим к дешёвым",
                        Value = "expensive",
                        IsSelected = false
                    }
                }
            },
            new GroupSortBy
            {
                NameGroup = "Популярности",
                SortBy = new List<SortBy>
                {
                    new SortBy
                    {
                        Name = "По популярности",
                        Value = "popular",
                        IsSelected = false
                    },
                    new SortBy
                    {
                        Name = "По рейтингу",
                        Value = "rate",
                        IsSelected = false
                    }
                }
            },
            new GroupSortBy
            {
                NameGroup = "Названию товара",
                SortBy = new List<SortBy>
                {
                    new SortBy
                    {
                        Name = "&darr; От А до Я",
                        Value = "nameDesc",
                        IsSelected = false
                    },
                    new SortBy
                    {
                        Name = "&uarr; От Я до А",
                        Value = "nameAsc",
                        IsSelected = false
                    }
                }
            }
        };

        public TakeItems[] TakeItems { get; set; } =
        {
            new TakeItems
            {
                Value = 50,
                IsSelected = true
            },
            new TakeItems
            {
                Value = 75,
                IsSelected = false
            },
            new TakeItems
            {
                Value = 100,
                IsSelected = false
            },
            new TakeItems
            {
                Value = 200,
                IsSelected = false
            }
        };
    }

    public class GroupSortBy
    {
        public string NameGroup { get; set; }
        public List<SortBy> SortBy { get; set; }
    }

    public class SortBy
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public bool IsSelected { get; set; }
    }

    public class TakeItems
    {
        public int Value { get; set; }
        public bool IsSelected { get; set; }
    }
}
