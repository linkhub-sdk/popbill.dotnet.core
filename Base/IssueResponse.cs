using System.Runtime.Serialization;

namespace Popbill
{
    [DataContract]
    public class IssueResponse
    {
        [DataMember] public long code;
        [DataMember] public string message;
        [DataMember] public string ntsConfirmNum;
    }
}