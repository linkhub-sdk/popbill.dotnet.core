using System.Runtime.Serialization;
namespace Popbill
{
    [DataContract]
    public class RefundForm
    {
        [DataMember(Name = "contactname")]
        public string contactName;
        [DataMember(Name = "tel")]
        public string tel;
        [DataMember(Name = "requestpoint")]
        public string requestPoint;
        [DataMember(Name = "accountbank")]
        public string accountBank;
        [DataMember(Name = "accountnum")]
        public string accountNum;
        [DataMember(Name = "accountname")]
        public string accountName;
        [DataMember(Name = "reason")]
        public string reason;
    }
}