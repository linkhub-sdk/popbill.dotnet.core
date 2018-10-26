using System;
using System.Runtime.Serialization;

namespace Popbill
{
    [DataContract]
    public class EmailConfig
    {
        [DataMember] public string emailType;
        [DataMember] public bool? sendYN;
    }
}