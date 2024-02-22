using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Popbill.Taxinvoice
{
    [DataContract]
    public class Taxinvoice

    {
        [DataMember] public string ntsconfirmNum;
        [DataMember] public string issueType;
        [DataMember] public string taxType;
        [DataMember] public string issueTiming;
        [DataMember] public string chargeDirection;
        [DataMember] public string serialNum;
        [DataMember] public int? kwon;
        [DataMember] public int? ho;
        [DataMember] public string writeDate;
        [DataMember] public string purposeType;
        [DataMember] public string supplyCostTotal;
        [DataMember] public string taxTotal;
        [DataMember] public string totalAmount;
        [DataMember] public string cash;
        [DataMember] public string chkBill;
        [DataMember] public string credit;
        [DataMember] public string note;
        [DataMember] public string remark1;
        [DataMember] public string remark2;
        [DataMember] public string remark3;
        [DataMember] public List<TaxinvoiceDetail> detailList;
        
        [DataMember] public string invoicerMgtKey;
        [DataMember] public string invoicerCorpNum;
        [DataMember] public string invoicerTaxRegID;
        [DataMember] public string invoicerCorpName;
        [DataMember] public string invoicerCEOName;
        [DataMember] public string invoicerAddr;
        [DataMember] public string invoicerBizClass;
        [DataMember] public string invoicerBizType;
        [DataMember] public string invoicerContactName;
        [DataMember] public string invoicerDeptName;
        [DataMember] public string invoicerTEL;
        [DataMember] public string invoicerHP;
        [DataMember] public string invoicerEmail;
        [DataMember] public bool? invoicerSMSSendYN;
        
        [DataMember] public string invoiceeMgtKey;
        [DataMember] public string invoiceeType;
        [DataMember] public string invoiceeCorpNum;
        [DataMember] public string invoiceeTaxRegID;
        [DataMember] public string invoiceeCorpName;
        [DataMember] public string invoiceeCEOName;
        [DataMember] public string invoiceeAddr;
        [DataMember] public string invoiceeBizType;
        [DataMember] public string invoiceeBizClass;
        [DataMember] public int? closeDownState;
        [DataMember] public string closeDownStateDate;
        [DataMember] public string invoiceeContactName1;
        [DataMember] public string invoiceeDeptName1;
        [DataMember] public string invoiceeTEL1;
        [DataMember] public string invoiceeHP1;
        [DataMember] public string invoiceeEmail1;
        [DataMember] public string invoiceeContactName2;
        [DataMember] public string invoiceeDeptName2;
        [DataMember] public string invoiceeTEL2;
        [DataMember] public string invoiceeHP2;
        [DataMember] public string invoiceeEmail2;
        [DataMember] public bool? invoiceeSMSSendYN;
        
        [DataMember] public string trusteeMgtKey;
        [DataMember] public string trusteeCorpNum;
        [DataMember] public string trusteeTaxRegID;
        [DataMember] public string trusteeCorpName;
        [DataMember] public string trusteeCEOName;
        [DataMember] public string trusteeAddr;
        [DataMember] public string trusteeBizType;
        [DataMember] public string trusteeBizClass;
        [DataMember] public string trusteeContactName;
        [DataMember] public string trusteeDeptName;
        [DataMember] public string trusteeTEL;
        [DataMember] public string trusteeHP;
        [DataMember] public string trusteeEmail;
        [DataMember] public bool? trusteeSMSSendYN;

        [DataMember] public int? modifyCode;
        [DataMember] public string orgNTSConfirmNum;
        [DataMember] public string originalTaxinvoiceKey;
        [DataMember] public List<TaxinvoiceAddContact> addContactList;

        [DataMember] public bool? businessLicenseYN;
        [DataMember] public bool? bankBookYN;

        [DataMember] public bool? writeSpecification;
        [DataMember] public bool? forceIssue;
        [DataMember] public string dealInvoiceMgtKey;
        [DataMember] public string memo;
        [DataMember] public string emailSubject;

        [DataMember] public bool? faxsendYN;
        [DataMember] public string faxreceiveNum;
      }
}