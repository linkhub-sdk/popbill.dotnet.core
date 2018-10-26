using System.Runtime.Serialization;

namespace Popbill
{
    [DataContract]
    public class JoinForm
    {
        [DataMember] public string ID;
        [DataMember] public string PWD;
        [DataMember] public string LinkID;
        [DataMember] public string CorpNum;
        [DataMember] public string CEOName;
        [DataMember] public string CorpName;
        [DataMember] public string Addr;
        [DataMember] public string BizType;
        [DataMember] public string BizClass;
        [DataMember] public string ContactName;
        [DataMember] public string ContactEmail;
        [DataMember] public string ContactTEL;
        [DataMember] public string ContactHP;
        [DataMember] public string ContactFAX;
    }
}