using System.Runtime.Serialization;

namespace Popbill.Statement
{
    [DataContract]
    public class StatementLog
    {
        [DataMember] public int docLogType;
        [DataMember] public string log;
        [DataMember] public string procType;
        [DataMember] public string procName;
        [DataMember] public string procCorpName;
        [DataMember] public string procContactName;
        [DataMember] public string procMemo;
        [DataMember] public string regDT;
        [DataMember] public string ip;
    }
}