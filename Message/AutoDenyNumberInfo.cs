using System.Runtime.Serialization;

namespace Popbill.Message
{
    [DataContract]
    public class AutoDenyNumberInfo
    {
        [DataMember] public string smsdenyNumber;
        [DataMember] public string regDT;
    }
}