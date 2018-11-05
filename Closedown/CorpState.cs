using System;
using System.Runtime.Serialization;

namespace Popbill.Closedown
{
    [DataContract]
    public class CorpState
    {
        [DataMember] public string corpNum;
        [DataMember] public string type;
        [DataMember] public string typeDate;
        [DataMember] public string state;
        [DataMember] public string stateDate;
        [DataMember] public string checkDate;
    }
}