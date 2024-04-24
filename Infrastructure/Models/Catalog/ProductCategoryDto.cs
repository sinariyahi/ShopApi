using Infrastructure.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models.Catalog
{
    public class ProductCategoryDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public int? ParentId { get; set; }
        public List<IFormFile> File { get; set; }
        public List<IFormFile> MainFeatureFile { get; set; }

        public List<FileItemDto> FileAttachment { get; set; }
        public List<FileItemDto> MainFeatureFileAttachment { get; set; }
        public string EnName { get; set; }

        public string ParentName { get; set; }
        public int? SortOrder { get; set; }
        public string Remark { get; set; }
        public List<ProductCategoryFeatureDto> Features { get; set; } = new List<ProductCategoryFeatureDto>();
        public List<int> MainFeatures { get; set; } = new List<int>();
        public int? MainFeatureId { get; set; }

        public int FeatureCount { get; set; } = 0;
    }


    public class UserProductCategoryDto
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }

        public string CategoryName { get; set; }
        public int? SortOrder { get; set; }

        public string File { get; set; }
        public string EnName { get; set; }

        public string Remark { get; set; }


    //    public List<TreeDto> Children { get; set; }
    }






    public class ProductFeaturesDto
    {
        public int? Max { get; set; }
        public int? Min { get; set; }
        public string Option { get; set; }
        public ControlType ControlType { get; set; }
        public UnitType UnitType { get; set; }

    }

    public class FeaturesWithCategoryDto
    {
        public int CategoryId { get; set; }
        public List<FeaturesWithValueDto> List { get; set; }
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
    }

    public class FeaturesWithValueDto
    {
        public int FeatureId { get; set; }
        public string Value { get; set; }
        public string Title { get; set; }
        public int? From { get; set; }
        public int? To { get; set; }
        public ControlType ControlType { get; set; }

    }


    public class ProductListDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Remark { get; set; }
        public int? SortOrder { get; set; }
        public bool IsActive { get; set; }
        public string Code { get; set; }
        public int? ParentId { get; set; }
        public string ParentName { get; set; }
        public int FeatureCount { get; set; } = 0;
        public int Key { get; set; }
        public string Title { get; set; }
      //  public List<TreeDto> Children { get; set; } = new List<TreeDto>();
        public int Value { get; set; }

        //public List<ProductFeatureDto> Features { get; set; } = new List<ProductFeatureDto>();

    }



    public class SearchInputDto
    {
        public string Text { get; set; }
        public string Code { get; set; }
    }
    public class ProductCategoryFeatureDto
    {
        public int Id { get; set; }
        public int? SortOrder { get; set; }
        public string Title { get; set; }

    }

    public class ProductFeatureDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Remark { get; set; }
        public UnitType UnitType { get; set; }
        public ControlType ControlType { get; set; }
        public string UnitTypeTitle { get; set; }
        public string ControlTypeTitle { get; set; }
    }
}
