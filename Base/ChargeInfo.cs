using System.Runtime.Serialization;

namespace Popbill
{
    [DataContract]
    public class ChargeInfo
    {
        [DataMember] public string unitCost;
        [DataMember] public string chargeMethod;
        [DataMember] public string rateSystem;
    }
}