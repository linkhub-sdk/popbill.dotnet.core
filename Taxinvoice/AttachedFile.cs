using System.Runtime.Serialization;

namespace Popbill.Taxinvoice
{
    [DataContract]
    public class AttachedFile
    {
        [DataMember] public int serialNum;
        [DataMember] public string attachedFile;
        [DataMember] public string displayName;
        [DataMember] public string regDT;
    }
}