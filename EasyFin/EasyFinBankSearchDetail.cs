using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace Popbill.EasyFin
{
    [DataContract]
    public class EasyFinBankSearchDetail
    {
        [DataMember]
        public string tid;

        [DataMember]
        public string trdate;

        [DataMember]
        public long? trserial;

        [DataMember]
        public string trdt;

        [DataMember]
        public string accIn;

        [DataMember]
        public string accOut;

        [DataMember]
        public string balance;

        [DataMember]
        public string remark1;

        [DataMember]
        public string remark2;

        [DataMember]
        public string remark3;

        [DataMember]
        public string remark4;

        [DataMember]
        public string regDT;

        [DataMember]
        public string memo;

    }
}
