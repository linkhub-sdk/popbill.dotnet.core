using System.Runtime.Serialization;
namespace Popbill
{
    [DataContract]
    public class RefundForm
    {
        [DataMember] string contactName;
        [DataMember] string tel;
        [DataMember] string requestPoint;
        [DataMember] string accountBank;
        [DataMember] string accountNum;
        [DataMember] string accountName;
        [DataMember] string reason;
    }
}