using System.Runtime.Serialization;
using System.Collections.Generic;


namespace Popbill.HomeTax
{
    [DataContract]
    public class HTTaxinvoiceSearch
    {
        [DataMember] public int? code;
        [DataMember] public int? total;
        [DataMember] public int? perPage;
        [DataMember] public int? pageNum;
        [DataMember] public int? pageCount;
        [DataMember] public string message;
        [DataMember] public List<HTTaxinvoiceAbbr> list;
    }
}