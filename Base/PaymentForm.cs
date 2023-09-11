using System.Runtime.Serialization;

namespace Popbill
{
    [DataContract]
    public class PaymentForm
    {
        [DataMember] string settlerName;

        [DataMember] string settlerEmail;

        [DataMember] string notifyHP;

        [DataMember] string paymentName;

        [DataMember] string settleCost;
    }
}