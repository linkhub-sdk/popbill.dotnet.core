using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Popbill.Statement
{
    [DataContract]
    public class Statement
    {
        [DataMember] public int? itemCode;
        [DataMember] public string mgtKey;
        [DataMember] public string invoiceNum;
        [DataMember] public string formCode;
        [DataMember] public string writeDate;
        [DataMember] public string taxType;
        [DataMember] public string purposeType;
        [DataMember] public string serialNum;
        [DataMember] public string taxTotal;
        [DataMember] public string supplyCostTotal;
        [DataMember] public string totalAmount;
        [DataMember] public string remark1;
        [DataMember] public string remark2;
        [DataMember] public string remark3;
        [DataMember] public string senderCorpNum;
        [DataMember] public string senderTaxRegID;
        [DataMember] public string senderCorpName;
        [DataMember] public string senderCEOName;
        [DataMember] public string senderAddr;
        [DataMember] public string senderBizType;
        [DataMember] public string senderBizClass;
        [DataMember] public string senderContactName;
        [DataMember] public string senderDeptName;
        [DataMember] public string senderTEL;
        [DataMember] public string senderHP;
        [DataMember] public string senderEmail;
        [DataMember] public string senderFAX;
        [DataMember] public string receiverCorpNum;
        [DataMember] public string receiverTaxRegID;
        [DataMember] public string receiverCorpName;
        [DataMember] public string receiverCEOName;
        [DataMember] public string receiverAddr;
        [DataMember] public string receiverBizType;
        [DataMember] public string receiverBizClass;
        [DataMember] public string receiverContactName;
        [DataMember] public string receiverDeptName;
        [DataMember] public string receiverTEL;
        [DataMember] public string receiverHP;
        [DataMember] public string receiverEmail;
        [DataMember] public string receiverFAX;
        [DataMember] public List<StatementDetail> detailList;
        [DataMember] public propertyBag propertyBag;
        [DataMember] public bool? businessLicenseYN;
        [DataMember] public bool? bankBookYN;
        [DataMember] public bool? smssendYN;
        [DataMember] public bool? faxsendYN;
        [DataMember] public bool? autoacceptYN;
        [DataMember] public string memo;
        [DataMember] public string sendNum;
        [DataMember] public string receiveNum;

    }
}