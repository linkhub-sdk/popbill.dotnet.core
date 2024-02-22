﻿using System;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Popbill.EasyFin
{
    [DataContract]
    public class EasyFinBankSearchResult
    {
        [DataMember]
        public int? code;

        [DataMember]
        public int? total;

        [DataMember]
        public int? perPage;

        [DataMember]
        public int? pageNum;

        [DataMember]
        public int? pageCount;

        [DataMember]
        public string message;

        [DataMember]
        public string lastScrapDT;

        [DataMember]
        public string balance;

        [DataMember]
        public List<EasyFinBankSearchDetail> list;
    }
}
