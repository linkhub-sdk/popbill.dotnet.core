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
        [DataMember] public string spare6;
        [DataMember] public string spare7;
        [DataMember] public string spare8;
        [DataMember] public string spare9;
        [DataMember] public string spare10;
        [DataMember] public string spare11;
        [DataMember] public string spare12;
        [DataMember] public string spare13;
        [DataMember] public string spare14;
        [DataMember] public string spare15;
        [DataMember] public string spare16;
        [DataMember] public string spare17;
        [DataMember] public string spare18;
        [DataMember] public string spare19;
        [DataMember] public string spare20;
        [DataMember] public string unit;
    }
}