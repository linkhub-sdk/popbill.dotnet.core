using System.Runtime.Serialization;

namespace Popbill
{
    [DataContract]
    public class RefundHistory
    {
        [DataMember] public string reqDT;
        [DataMember] public string requestPoint;
        [DataMember] public string accountBank;
        [DataMember] public string accountNum;
        [DataMember] public string accountName;
        [DataMember] public int state;
        [DataMember] public string reason;
    }
}