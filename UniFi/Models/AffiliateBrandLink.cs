using System.ComponentModel.DataAnnotations;

namespace UniFi.Models
{
    public class AffiliateBrandLink
    {
        public int Id { get; set; }
        public string BrandPage { get; set; }
        public int ProductId { get; set; }
        public string Visitor { get; set; }
        public string AffiliateCode { get; set; }
        public string AffiliateUser { get; set; }
        public string Source { get; set; }
        public DateTime InsertDate { get; set; }

    }
}
