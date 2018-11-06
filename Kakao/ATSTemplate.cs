using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Popbill.Kakao
{
    [DataContract]
    public class ATSTemplate
    {
        [DataMember] public string templateCode;
        [DataMember] public string templateName;
        [DataMember] public string template;
        [DataMember] public string plusFriendID;
        [DataMember] public List<KakaoButton> btns;
    }
}