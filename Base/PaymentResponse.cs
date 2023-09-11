using System.Runtime.Serialization;

namespace Popbill
{
    [DataContract]
    public class PaymentResponse
    {
        [DataMember] string code;
        [DataMember] string message;
        [DataMember] string settleCode;
    }
}