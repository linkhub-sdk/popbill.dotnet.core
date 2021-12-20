using System.Runtime.Serialization;

namespace Popbill.Fax
{
    [DataContract]
    public class FaxResult
    {
        [DataMember] public int? state;
        [DataMember] public int? result;
        [DataMember] public string sendNum;
        [DataMember] public string senderName;
        [DataMember] public string receiveNum;
        [DataMember] public string receiveName;
        [DataMember] public string receiveNumType;
        [DataMember] public string title;
        [DataMember] public int? sendPageCnt;
        [DataMember] public int? successPageCnt;
        [DataMember] public int? failPageCnt;
        [DataMember] public int? refundPageCnt;
        [DataMember] public int? cancelPageCnt;
        [DataMember] public string reserveDT;
        [DataMember] public string receiptDT;
        [DataMember] public string sendDT;
        [DataMember] public string resultDT;
        [DataMember] public string[] fileNames;
        [DataMember] public string receiptNum;
        [DataMember] public string requestNum;
        [DataMember] public string chargePageCnt;
        [DataMember] public string tiffFileSize;
    }
}