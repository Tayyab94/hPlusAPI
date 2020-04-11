using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiPractice.Classes
{
    public class ProductQueryParameters :QueryParameters
    {
        public string Sku { get; set; }


        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public string Name { get; set; }

        public string SortBy { get; set; } = "Id";

        private string _sortOrder;

        public string SortOrder
        {
            get { return _sortOrder; }
            
            set
            { 
                    if(value=="asc" || value=="dssc")
                    {
                        _sortOrder = value;
                    
                    }

            }
        }

    }
}
