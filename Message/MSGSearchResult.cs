﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Popbill.Message
{
    [DataContract]
    public class MSGSearchResult
    {
        [DataMember] public int code;
        [DataMember] public int total;
        [DataMember] public int perPage;
        [DataMember] public int pageNum;
        [DataMember] public int pageCount;
        [DataMember] public string message;
        [DataMember] public List<MessageResult> list;
    }
}