using System.Runtime.Serialization;

namespace Popbill.Kakao
{
    [DataContract]
    public class PlusFriend
    {
        [DataMember] public string plusFriendID;
        [DataMember] public string plusFriendName;
        [DataMember] public string regDT;
    }
}