using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Popbill.Statement
{
    [DataContract]
    public class StatementInfo
    {
        [DataMember] public int? itemCode;
        [DataMember] public string itemKey;
        [DataMember] public string invoiceNum;
        [DataMember] public string mgtKey;
        [DataMember] public string taxType;
        [DataMember] public string writeDate;
        [DataMember] public string regDT;
        [DataMember] public string senderCorpName;
        [DataMember] public string senderCorpNum;
        [DataMember] public bool? senderPrintYN;
        [DataMember] public string receiverCorpName;
        [DataMember] public string receiverCorpNum;
        [DataMember] public bool? receiverPrintYN;
        [DataMember] public string supplyCostTotal;
        [DataMember] public string taxTotal;
        [DataMember] public string purposeType;
        [DataMember] public string issueDT;
        [DataMember] public int? stateCode;
        [DataMember] public string stateDT;
        [DataMember] public string stateMemo;
        [DataMember] public bool? openYN;
        [DataMember] public string openDT;
    }
}