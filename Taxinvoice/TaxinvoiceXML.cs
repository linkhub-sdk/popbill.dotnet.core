using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Popbill.Taxinvoice
{
    [DataContract]
    public class TaxinvoiceXML
    {
        [DataMember]
        public int code;
        [DataMember]
        public String message;
        [DataMember]
        public String retObject;
    }
}