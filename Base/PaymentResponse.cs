using System.Runtime.Serialization;

namespace Popbill
{
    [DataContract]
    public class PaymentResponse
    {
        [DataMember] public long code;
        [DataMember] public string message;
        [DataMember] public string settleCode;
    }
}