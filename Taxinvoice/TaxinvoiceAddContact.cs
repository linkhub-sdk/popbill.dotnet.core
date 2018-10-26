using System;
using System.Runtime.Serialization;

namespace Popbill.Taxinvoice
{
    [DataContract]
    public class TaxinvoiceAddContact
    {
        [DataMember] public int? serialNum;
        [DataMember] public string email;
        [DataMember] public string contactName;
    }
}