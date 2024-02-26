using System.Runtime.Serialization;

namespace Popbill
{
    [DataContract]
    public class PaymentResponse
    {
        [DataMember] public string code;
        [DataMember] public string message;
        [DataMember] public string settleCode;
    }
}