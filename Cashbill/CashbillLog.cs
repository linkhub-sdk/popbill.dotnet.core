using System.Runtime.Serialization;

namespace Popbill.Cashbill
{
    [DataContract]
    public class CashbillLog
    {
        [DataMember] public int docLogType;
        [DataMember] public string log;
        [DataMember] public string procType;
        [DataMember] public string procMemo;
        [DataMember] public string regDT;
        [DataMember] public string ip;
    }
}