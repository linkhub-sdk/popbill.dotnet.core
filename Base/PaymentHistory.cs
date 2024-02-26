using System;
using System.Runtime.Serialization;

namespace Popbill
{
    [DataContract]
    public class PaymentHistory
    {
        [DataMember] public string productType;
        [DataMember] public string productName;
        [DataMember] public string settleType;
        [DataMember] public string settlerName;
        [DataMember] public string settlerEmail;
        [DataMember] public string settleCost;
        [DataMember] public string settlePoint;
        [DataMember] public int settleState;
        [DataMember] public string regDT;
        [DataMember] public string stateDT;
    }
}