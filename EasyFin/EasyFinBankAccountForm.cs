using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Popbill.EasyFin
{
    [DataContract]
    public class EasyFinBankAccountForm
    {
        [DataMember]        public string BankCode;

        [DataMember]        public string AccountNumber;

        [DataMember]        public string AccountPWD;

        [DataMember]        public string AccountType;

        [DataMember]        public string IdentityNumber;

        [DataMember]        public string AccountName;

        [DataMember]        public string BankID;

        [DataMember]        public string FastID;

        [DataMember]        public string FastPWD;

        [DataMember]        public string Memo;

        [DataMember]        public string UsePeriod;
    }


}