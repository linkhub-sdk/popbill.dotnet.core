using System.Runtime.Serialization;

namespace Popbill
{
    [DataContract]
    public class PaymentHistory
    {
        [DataMember] string code;
        [DataMember] string message;
        [DataMember] string settleCode;
    }
}