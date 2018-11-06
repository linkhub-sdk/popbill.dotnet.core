using System.Runtime.Serialization;

namespace Popbill.Kakao
{
    [DataContract]
    public class SenderNumber
    {
        [DataMember] public string number;
        [DataMember] public bool? representYN;
        [DataMember] public int? state;
    }
}