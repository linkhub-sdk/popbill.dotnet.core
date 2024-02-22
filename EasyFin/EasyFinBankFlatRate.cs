using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;


namespace Popbill.EasyFin
{
    public class EasyFinBankFlatRate
    {
        [DataMember]
        public string referenceID;
        [DataMember]
        public string contractDT;
        [DataMember]
        public string useEndDate;
        [DataMember]
        public int? baseDate;
        [DataMember]
        public int? state;
        [DataMember]
        public bool? closeRequestYN;
        [DataMember]
        public bool? useRestrictYN;
        [DataMember]
        public bool? closeOnExpired;
        [DataMember]
        public bool? unPaidYN;
    }
}
