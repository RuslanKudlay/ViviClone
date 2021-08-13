using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Models
{
    public enum StateSideSearchMenu
    {
        Undefined,
        WithoutParams,

        /* Brands, CV or both */
        OnlyBrands,
        OnlyParams,
        BrandsParams,

        /* Search */
        OnlySearch,
        SearchBrands,
        SearchParams,
        SearchBrandsParams,

        /* GOW */
        GOW,
        GowBrands,
        GowParams,
        GowBrandsParams,

        /* professional */
        BrandsProfessional
    }
}
