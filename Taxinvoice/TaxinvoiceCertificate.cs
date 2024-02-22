using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Popbill.Taxinvoice
{
    [DataContract]
    public class TaxinvoiceCertificate
    {
        [DataMember]
        public string regDT;
        [DataMember]
        public string expireDT;
        [DataMember]
        public string issuerDN;
        [DataMember]
        public string subjectDN;
        [DataMember]
        public string issuerName;
        [DataMember]
        public string oid;
        [DataMember]
        public string regContactName;
        [DataMember]
        public string regContactID;
    }
}