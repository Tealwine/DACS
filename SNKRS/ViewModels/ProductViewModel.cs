using SNKRS.Models;
using System.Collections.Generic;

namespace SNKRS.ViewModels
{
    public class ProductViewModel
    {
        public Portfolio Product { get; set; }
        public IEnumerable<Portfolio> RelatedProducts { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
    }
}