using System;
using System.Runtime.Serialization;

namespace Popbill.Taxinvoice
{
    [DataContract]
    public class TaxinvoiceLog
    {
        [DataMember] public int docLogType;
        [DataMember] public string log;
        [DataMember] public string procType;
        [DataMember] public string procCorpName;
        [DataMember] public string procContactName;
        [DataMember] public string procMemo;
        [DataMember] public string regDT;
        [DataMember] public string ip;
    }
}