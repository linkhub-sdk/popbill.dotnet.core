using System.Runtime.Serialization;

namespace Popbill
{
    [DataContract]
    public class Response
    {
        [DataMember] public long code;
        [DataMember] public string message;
    }
}