using System.Runtime.Serialization;

namespace Popbill
{
    [DataContract]
    public class PaymentForm
    {
        [DataMember] public string settlerName;

        [DataMember] public string settlerEmail;

        [DataMember] public string notifyHP;

        [DataMember] public string paymentName;

        [DataMember] public string settleCost;
    }
}