using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Popbill
{
    [DataContract]
    public class RefundHistoryResult
    {
        long code;
        long total;
        long perPage;
        long pageNum;
        long pageCount;
        List<RefundHistory> list;
    }
}