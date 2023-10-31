using System.Runtime.Serialization;
namespace Popbill
{
    [DataContract]
    public class RefundForm
    {
        [DataMember] public string contactName;
        [DataMember] public string tel;
        [DataMember] public string requestPoint;
        [DataMember] public string accountBank;
        [DataMember] public string accountNum;
        [DataMember] public string accountName;
        [DataMember] public string reason;
    }
}