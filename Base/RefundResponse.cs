using System.Runtime.Serialization;

namespace Popbill
{
    [DataContract]
    public class RefundResponse
    {
        [DataMember] long code;
        [DataMember] string message;
        [DataMember] string refundCode;
    }
}