using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace C3P1.Net.Shared.Data.Apps.BankBook.Dto
{
    public class SubCategoryWithCategoryInfoDto : SubCategory
    {
        public string? CategoryName { get; set; } = null;
        public string? CategoryCode { get; set; } = null;
    }
}
