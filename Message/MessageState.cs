using System.Runtime.Serialization;

namespace Popbill.Message
{
    [DataContract]
    public class MessageState
    {
        [DataMember] public string rNum;
        [DataMember] public string sn;
        [DataMember] public string stat;
        [DataMember] public string rlt;
        [DataMember] public string sDT;
        [DataMember] public string rDT;
        [DataMember] public string net;
        [DataMember] public string srt;
    }
}