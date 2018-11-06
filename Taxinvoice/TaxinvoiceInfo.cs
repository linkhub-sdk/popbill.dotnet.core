using System.Runtime.Serialization;

namespace Popbill.Taxinvoice
{
    [DataContract]
    public class TaxinvoiceInfo
    {
        [DataMember] public string itemKey;
        [DataMember] public string taxType;
        [DataMember] public string writeDate;
        [DataMember] public string regDT;
        [DataMember] public string issueType;
        [DataMember] public string supplyCostTotal;
        [DataMember] public string taxTotal;
        [DataMember] public string purposeType;
        [DataMember] public string issueDT;
        [DataMember] public bool? lateIssueYN;
        [DataMember] public string preIssueDT;
        [DataMember] public bool? openYN;
        [DataMember] public string openDT;
        [DataMember] public string stateMemo;
        [DataMember] public int stateCode;
        [DataMember] public string stateDT;
        [DataMember] public string ntsconfirmNum;
        [DataMember] public string ntsresult;
        [DataMember] public string ntssendDT;
        [DataMember] public string ntsresultDT;
        [DataMember] public string ntssendErrCode;
        [DataMember] public int? modifyCode;
        [DataMember] public bool? interOPYN;
        [DataMember] public string invoicerCorpName;
        [DataMember] public string invoicerCorpNum;
        [DataMember] public string invoicerMgtKey;
        [DataMember] public bool? invoicerPrintYN;
        [DataMember] public string invoiceeCorpName;
        [DataMember] public string invoiceeCorpNum;
        [DataMember] public string invoiceeMgtKey;
        [DataMember] public bool? invoiceePrintYN;
        [DataMember] public int? closeDownState;
        [DataMember] public string closeDownStateDate;
        [DataMember] public string trusteeCorpName;
        [DataMember] public string trusteeCorpNum;
        [DataMember] public string trusteeMgtKey;
        [DataMember] public bool? trusteePrintYN;
    }
}