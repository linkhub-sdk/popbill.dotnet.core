using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Popbill
{
    [DataContract]
    public class PaymentHistoryResult
    {
        [DataMember] long code;
        [DataMember] long total;
        [DataMember] long perPage;
        [DataMember] long pageNum;
        [DataMember] long pageCount;
        [DataMember] List<PaymentHistory> list;
    }
}