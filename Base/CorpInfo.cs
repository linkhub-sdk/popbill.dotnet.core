using System;
using System.Runtime.Serialization;

namespace Popbill
{
    [DataContract]
    public class CorpInfo
    {
        [DataMember] public string ceoname;
        [DataMember] public string corpName;
        [DataMember] public string addr;
        [DataMember] public string bizType;
        [DataMember] public string bizClass;
    }
}