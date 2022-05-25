using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Popbill.Kakao
{
    [DataContract]
    public class KakaoReceiver
    {
        [DataMember] public string rcv;
        [DataMember] public string rcvnm;
        [DataMember] public string msg;
        [DataMember] public string altsjt;
        [DataMember] public string altmsg;
        [DataMember] public string interOPRefKey;
        [DataMember] public List<KakaoButton> btns;
    }
}