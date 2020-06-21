using System;
using System.Collections.Generic;
using System.Text;

namespace PackingSlip.Domain.Models
{
    public class FreeProductHeaderModel
    {
        public List<FreeProductDetailModel> ParentProducts { get; set; }
        public List<FreeProductDetailModel> ChildProducts { get; set; }

        public FreeProductHeaderModel()
        {
            ParentProducts = new List<FreeProductDetailModel>();
            ChildProducts = new List<FreeProductDetailModel>();
        }
    }
}
