using System.Runtime.Serialization;

namespace Popbill.EasyFin
{
    [DataContract]
    public class UpdateEasyFinBankAccountForm
    {
        [DataMember]        public string AccountPWD;
        [DataMember]        public string AccountName;
        [DataMember]        public string BankID;
        [DataMember]        public string FastID;
        [DataMember]        public string FastPWD;
        [DataMember]        public string Memo;
    }
}