using System.Runtime.Serialization;

namespace Popbill.AccountCheck
{
    [DataContract]
    public class DepositorCheckInfo
    {
        [DataMember]
        public string bankCode;
        [DataMember]
        public string accountNumber;
        [DataMember]
        public string accountName;
        [DataMember]
        public string checkDate;
        [DataMember]
        public string result;
        [DataMember]
        public string resultMessage;
        [DataMember]
        public string identityNumType;
        [DataMember]
        public string identityNum;
    }
}
