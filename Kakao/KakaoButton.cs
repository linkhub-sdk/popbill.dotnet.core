﻿using System.Runtime.Serialization;

namespace Popbill.Kakao
{
    [DataContract]
    public class KakaoButton
    {
        [DataMember] public string n;
        [DataMember] public string t;
        [DataMember] public string u1;
        [DataMember] public string u2;
        [DataMember] public string tg;
    }
}