﻿using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Popbill.Cashbill
{
    [DataContract]
    public class BulkCashbillResult
    {
        [DataMember]
        public long code;
        [DataMember]
        public string message;
        [DataMember]
        public string receiptID;
        [DataMember]
        public string receiptDT;
        [DataMember]
        public string submitID;
        [DataMember]
        public long submitCount;
        [DataMember]
        public long successCount;
        [DataMember]
        public long failCount;
        [DataMember]
        public long txState;
        [DataMember]
        public string txStartDT;
        [DataMember]
        public string txEndDT;
        [DataMember]
        public long txResultCode;
        [DataMember]
        public List<BulkCashbillIssueResult> issueResult;
    }
}