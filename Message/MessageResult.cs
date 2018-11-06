using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Popbill.Message
{
    [DataContract]
    public class MessageResult
    {
        [DataMember] public string subject;
        [DataMember] public string content;
        [DataMember] public string sendNum;
        [DataMember] public string senderName;
        [DataMember] public string receiveNum;
        [DataMember] public string receiveName;
        [DataMember] public string receiptDT;
        [DataMember] public string sendDT;
        [DataMember] public string resultDT;
        [DataMember] public string reserveDT;
        [DataMember] public int? state;
        [DataMember] public int? result;
        [DataMember] public string type;
        [DataMember] public string tranNet;
        [DataMember] public string receiptNum;
        [DataMember] public string requestNum;
    }
}