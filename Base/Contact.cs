using System.Runtime.Serialization;

namespace Popbill
{
    [DataContract]
    public class Contact
    {
        [DataMember] public string id;
        [DataMember] public string pwd;
        [DataMember] public string Password;
        [DataMember] public string personName;
        [DataMember] public string tel;
        [DataMember] public string hp;
        [DataMember] public string fax;
        [DataMember] public string email;
        [DataMember] public int searchRole;
        [DataMember] public bool searchAllAllowYN;
        [DataMember] public bool mgrYN;
        [DataMember] public string regDT;
        [DataMember] public int state;
    }
}