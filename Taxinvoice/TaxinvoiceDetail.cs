using System;
using System.Runtime.Serialization;

namespace Popbill.Taxinvoice
{
    [DataContract]
    public class TaxinvoiceDetail
    {
        [DataMember] public int? serialNum;
        [DataMember] public string purchaseDT;
        [DataMember] public string itemName;
        [DataMember] public string spec;
        [DataMember] public string qty;
        [DataMember] public string unitCost;
        [DataMember] public string supplyCost;
        [DataMember] public string tax;
        [DataMember] public string remark;
    }
}