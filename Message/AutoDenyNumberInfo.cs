using System.Runtime.Serialization;

namespace Popbill.Message
{
    [DataContract]
    public class AutoDenyNumberInfo
    {
        [DataMember] string smsdenyNumber;
        [DataMember] string regDT;
    }
}