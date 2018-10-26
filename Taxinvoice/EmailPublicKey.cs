using System.Runtime.Serialization;

namespace Popbill.Taxinvoice
{
    [DataContract]
    public class EmailPublicKey
    {
        [DataMember] public string confirmNum;
        [DataMember] public string email;
    }
}