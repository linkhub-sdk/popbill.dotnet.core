using System.Runtime.Serialization;

namespace Popbill
{
    [DataContract]
    public class RefundHistory
    {
        [DataMember] string reqDT;
        [DataMember] string requestPoint;
        [DataMember] string accountBank;
        [DataMember] string accountNum;
        [DataMember] string accountName;
        [DataMember] int state;
        [DataMember] string reason;
    }
}