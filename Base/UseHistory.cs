using System.Runtime.Serialization;

namespace Popbill
{
    [DataContract]
    public class UseHistory
    {
        [DataMember] public int itemCode;
        [DataMember] public int txType;
        [DataMember] public string txPoint;
        [DataMember] public string balance;
        [DataMember] public string txDT;
        [DataMember] public string userID;
        [DataMember] public string userName;
    }
}
