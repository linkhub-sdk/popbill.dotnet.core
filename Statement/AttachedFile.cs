using System.Runtime.Serialization;

namespace Popbill.Statement
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