using System.Runtime.Serialization;

namespace Popbill.Statement
{
    [DataContract]
    public class StatementDetail
    {
        [DataMember] public int? serialNum;
        [DataMember] public string purchaseDT;
        [DataMember] public string itemName;
        [DataMember] public string spec;
        [DataMember] public string unit;
        [DataMember] public string qty;
        [DataMember] public string unitCost;
        [DataMember] public string supplyCost;
        [DataMember] public string tax;
        [DataMember] public string remark;
        [DataMember] public string spare1;
        [DataMember] public string spare2;
        [DataMember] public string spare3;
        [DataMember] public string spare4;
        [DataMember] public string spare5;
    }
}