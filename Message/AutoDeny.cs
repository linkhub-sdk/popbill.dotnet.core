using System.Runtime.Serialization;

namespace Popbill.Message
{
    [DataContract]
    public class AutoDeny
    {
        [DataMember] public string number;
        [DataMember] public string regDT;
    }
}