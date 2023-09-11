using System.Runtime.Serialization;

namespace Popbill
{
    [DataContract]
    public class UseHistoryResult
    {
        [DataMember] long code;
        [DataMember] long total;
        [DataMember] long perPage;
        [DataMember] long pageNum;
        [DataMember] long pageCount;
    }
}