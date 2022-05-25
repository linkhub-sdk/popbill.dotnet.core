﻿using System.Runtime.Serialization;

namespace Popbill.Fax
{
    [DataContract]
    public class FaxReceiver
    {
        [DataMember(Name = "rcv")] public string receiveNum;
        [DataMember(Name = "rcvnm")] public string receiveName;
        [DataMember] public string interOPRefKey;
    }
}