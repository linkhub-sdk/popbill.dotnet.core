using System.Runtime.Serialization;

namespace Popbill
{
    [DataContract]
    public class RefundResponse
    {
        [DataMember] public long code;
        [DataMember] public string message;
        [DataMember] public string refundCode;
    }
}