using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Popbill.Kakao
{
    public enum KakaoType
    {
        ATS,
        FTS,
        FMS
    };

    public class KakaoService : BaseService
    {
        public KakaoService(string LinkID, string SecretKey) : base(LinkID, SecretKey)
        {
            this.AddScope("153");
            this.AddScope("154");
            this.AddScope("155");
        }

        #region Manage API

        //플러스친구 계정관리 팝업 URL
        public string GetPlusFriendMgtURL(string CorpNum, string UserID)
        {
            URLResponse response = httpget<URLResponse>("/KakaoTalk/?TG=SENDER", CorpNum, UserID);

            return response.url;
        }

        //플러스친구 목록 확인
        public List<PlusFriend> ListPlusFriendID(string CorpNum, string UserID = null)
        {
            return httpget<List<PlusFriend>>("/KakaoTalk/ListPlusFriendID", CorpNum, UserID);
        }

        //발신번호 관리 팝업 URL
        public string GetSenderNumberMgtURL(string CorpNum, string UserID)
        {
            URLResponse response = httpget<URLResponse>("/Message/?TG=SENDER", CorpNum, UserID);

            return response.url;
        }

        //발신번호 목록 확인
        public List<SenderNumber> GetSenderNumberList(string CorpNum, string UserID = null)
        {
            return httpget<List<SenderNumber>>("/Message/SenderNumber", CorpNum, UserID);
        }

        //알림톡 템플릿관리 팝업 URL
        public string GetATSTemplateMgtURL(string CorpNum, string UserID)
        {
            URLResponse response = httpget<URLResponse>("/KakaoTalk/?TG=SENDER", CorpNum, UserID);

            return response.url;
        }

        //알림톡 템플릿 목록 확인
        public List<ATSTemplate> ListATSTemplate(string CorpNum, string UserID = null)
        {
            return httpget<List<ATSTemplate>>("/KakaoTalk/ListATSTemplate", CorpNum, UserID);
        }

        #endregion

        #region 알림톡

        //알림톡 단건전송
        public string SendATS(string CorpNum, string templateCode, string snd, string receiveNum, string receiveName,
            string msg, string altmsg, string altSendType, DateTime? sndDT, string requestNum = null,
            string UserID = null)
        {
            if (string.IsNullOrEmpty(receiveNum)) throw new PopbillException(-99999999, "수신번호가 입력되지 않았습니다.");

            List<KakaoReceiver> receivers = new List<KakaoReceiver>();

            KakaoReceiver receiver = new KakaoReceiver();
            receiver.rcv = receiveNum;
            receiver.rcvnm = receiveName;
            receiver.msg = msg;
            receiver.altmsg = altmsg;

            receivers.Add(receiver);

            return SendATS(CorpNum, templateCode, snd, msg, altmsg, receivers, altSendType, sndDT, requestNum, UserID);
        }

        //알림톡 대량전송
        public string SendATS(string CorpNum, string templateCode, string snd, List<KakaoReceiver> receivers,
            string altSendType, DateTime? sndDT, string requestNum = null, string UserID = null)
        {
            return SendATS(CorpNum, templateCode, snd, null, null, receivers, altSendType, sndDT, requestNum, UserID);
        }

        //알림톡 동보전송
        public string SendATS(string CorpNum, string templateCode, string snd, string content, string altContent,
            List<KakaoReceiver> receivers, string altSendType, DateTime? sndDT, string requestNum = null,
            string UserID = null)
        {
            if (string.IsNullOrEmpty(templateCode)) throw new PopbillException(-99999999, "알림톡 템플릿 코드가 입력되지 않았습니다.");
            if (string.IsNullOrEmpty(snd)) throw new PopbillException(-99999999, "발신번호가 입력되지 않았습니다.");

            ATSSendRequest request = new ATSSendRequest();

            request.templateCode = templateCode;
            request.snd = snd;
            request.content = content;
            request.altContent = altContent;
            request.altSendType = altSendType;
            request.sndDT = sndDT == null ? null : sndDT.Value.ToString("yyyyMMddHHmmss");
            request.msgs = receivers;
            request.requestNum = requestNum;

            string PostData = toJsonString(request);
            ReceiptResponse response = httppost<ReceiptResponse>("/ATS", CorpNum, PostData, null, null, UserID);

            return response.receiptNum;
        }

        #endregion

        #region 친구톡 텍스트

        //친구톡 텍스트 단건전송
        public string SendFTS(string CorpNum, string plusFriendID, string snd, string receiverNum, string receiverName,
            string content, string altContent, List<KakaoButton> buttons, string altSendType, bool adsYN,
            DateTime? sndDT, string requestNum = null, string UserID = null)
        {
            List<KakaoReceiver> receivers = new List<KakaoReceiver>();

            KakaoReceiver receiver = new KakaoReceiver();
            receiver.rcv = receiverNum;
            receiver.rcvnm = receiverName;
            receiver.msg = content;
            receiver.altmsg = altContent;

            receivers.Add(receiver);

            return SendFTS(CorpNum, plusFriendID, snd, content, altContent, receivers, buttons, altSendType, adsYN,
                sndDT, requestNum, UserID);
        }

        //친구톡 텍스트 대량전송
        public string SendFTS(string CorpNum, string plusFriendID, string snd, List<KakaoReceiver> receivers,
            List<KakaoButton> buttons, string altSendType, bool adsYN, DateTime? sndDT, string requestNum = null,
            string UserID = null)
        {
            return SendFTS(CorpNum, plusFriendID, snd, null, null, receivers, buttons, altSendType, adsYN,
                sndDT, requestNum, UserID);
        }

        //친구톡 텍스트 동보전송
        public string SendFTS(string CorpNum, string plusFriendID, string snd, string content, string altContent,
            List<KakaoReceiver> receivers, List<KakaoButton> buttons, string altSendType, bool adsYN, DateTime? sndDT,
            string requestNum = null, string UserID = null)
        {
            if (string.IsNullOrEmpty(plusFriendID)) throw new PopbillException(-99999999, "플러스친구 아이디가 입력되지 않았습니다.");
            if (string.IsNullOrEmpty(snd)) throw new PopbillException(-99999999, "발신번호가 입력되지 않았습니다.");

            FTSSendRequest request = new FTSSendRequest();

            request.plusFriendID = plusFriendID;
            request.snd = snd;
            request.content = content;
            request.altContent = altContent;
            request.altSendType = altSendType;
            request.adsYN = adsYN;
            request.sndDT = sndDT == null ? null : sndDT.Value.ToString("yyyyMMddHHmmss");
            request.msgs = receivers;
            request.btns = buttons;
            request.requestNum = requestNum;

            string PostDate = toJsonString(request);

            ReceiptResponse response = httppost<ReceiptResponse>("/FTS", CorpNum, PostDate, null, null, UserID);

            return response.receiptNum;
        }

        #endregion

        #region 친구톡 이미지

        //친구톡 이미지 단건전송
        public string SendFMS(string CorpNum, string plusFriendID, string snd, string receiverNum, string receiverName,
            string content, string altContent, List<KakaoButton> buttons, string altSendType, bool adsYN,
            DateTime? sndDT, string imageURL, string fmsfilepath, string requestNum = null, string UserID = null)
        {
            List<KakaoReceiver> receivers = new List<KakaoReceiver>();

            KakaoReceiver receiver = new KakaoReceiver();
            receiver.rcv = receiverNum;
            receiver.rcvnm = receiverName;
            receiver.msg = content;
            receiver.altmsg = altContent;

            receivers.Add(receiver);

            return SendFMS(CorpNum, plusFriendID, snd, content, altContent, receivers, buttons, altSendType, adsYN,
                sndDT, imageURL, fmsfilepath, requestNum, UserID);
        }

        //친구톡 이미지 대량전송
        public string SendFMS(string CorpNum, string plusFriendID, string snd, List<KakaoReceiver> receivers,
            List<KakaoButton> buttons, string altSendType, bool adsYN, DateTime? sndDT, string imageURL,
            string fmsfilepath, string requestNum = null, string UserID = null)
        {
            return SendFMS(CorpNum, plusFriendID, snd, null, null, receivers, buttons, altSendType, adsYN,
                sndDT, imageURL, fmsfilepath, requestNum, UserID);
        }

        //친구톡 이미지 동보전송
        public string SendFMS(string CorpNum, string plusFriendID, string snd, string content, string altContent,
            List<KakaoReceiver> receivers, List<KakaoButton> buttons, string altSendType, bool adsYN, DateTime? sndDT,
            string imageURL, string fmsfilepath, string requestNum = null, string UserID = null)
        {
            if (string.IsNullOrEmpty(value: plusFriendID))
                throw new PopbillException(code: -99999999, Message: "플러스친구 아이디가 입력되지 않았습니다.");
            if (string.IsNullOrEmpty(value: snd))
                throw new PopbillException(code: -99999999, Message: "발신번호가 입력되지 않았습니다.");

            FTSSendRequest request = new FTSSendRequest();

            request.plusFriendID = plusFriendID;
            request.snd = snd;
            request.content = content;
            request.altContent = altContent;
            request.altSendType = altSendType;
            request.sndDT = sndDT == null ? null : sndDT.Value.ToString(format: "yyyyMMddHHmmss");
            request.adsYN = adsYN;
            request.imageURL = imageURL;
            request.msgs = receivers;
            request.btns = buttons;
            request.requestNum = requestNum;

            string PostDate = toJsonString(request);

            List<UploadFile> UploadFiles = new List<UploadFile>();

            try
            {
                UploadFile uf = new UploadFile();

                uf.FieldName = "file";
                uf.FileName = System.IO.Path.GetFileName(fmsfilepath);
                uf.FileData = new FileStream(fmsfilepath, FileMode.Open, FileAccess.Read);

                UploadFiles.Add(item: uf);
            }
            catch (Exception fe)
            {
                throw new PopbillException(code: -99999999, Message: fe.Message);
            }


            ReceiptResponse response =
                httppostFile<ReceiptResponse>("/FMS", CorpNum, PostDate, UploadFiles, null, UserID);

            return response.receiptNum;
        }

        #endregion

        //예약전송 취소
        public Response CancelReserve(string CorpNum, string receiptNum, string UserID = null)
        {
            if (string.IsNullOrEmpty(receiptNum)) throw new PopbillException(-99999999, "접수번호가 입력되지 않았습니다.");

            return httpget<Response>("/KakaoTalk/" + receiptNum + "/Cancel", CorpNum, UserID);
        }

        //예약전송 취소 - 요청번호 할당
        public Response CancelReserveRN(string CorpNum, string requestNum, string UserID = null)
        {
            if (string.IsNullOrEmpty(requestNum))
                throw new PopbillException(-99999999, "요청번호(requestNum)가 입력되지 않았습니다.");

            return httpget<Response>("/KakaoTalk//Cancel/" + requestNum, CorpNum, UserID);
        }

        #region Info API

        //알림톡/친구톡 전송내역 확인
        public KakaoSentResult GetMessages(string CorpNum, string receiptNum, string UserID = null)
        {
            if (string.IsNullOrEmpty(receiptNum)) throw new PopbillException(-99999999, "접수번호가 입력되지 않았습니다.");

            return httpget<KakaoSentResult>("/KakaoTalk/" + receiptNum, CorpNum, UserID);
        }

        //알림톡/친구톡 전송내력 확인 - 요청번호 할당
        public KakaoSentResult GetMessagesRN(string CorpNum, string requestNum, string UserID = null)
        {
            if (string.IsNullOrEmpty(requestNum))
                throw new PopbillException(-99999999, "요청번호(requestNum)가 입력되지 않았습니다.");

            return httpget<KakaoSentResult>("/KakaoTalk/Get/" + requestNum, CorpNum, UserID);
        }

        //전송내역 목록 조회
        public KakaoSearchResult Search(string CorpNum, string SDate, string EDate, string[] State = null,
            string[] Item = null, string ReserveYN = null, bool? SenderYN = false, int? Page = null,
            int? PerPage = null, string Order = null, string Qstring = null, string UserID = null)
        {
            if (string.IsNullOrEmpty(SDate)) throw new PopbillException(-99999999, "시작일자가 입력되지 않았습니다.");
            if (string.IsNullOrEmpty(EDate)) throw new PopbillException(-99999999, "종료일자가 입력되지 않았습니다.");

            string uri = "/KakaoTalk/Search";
            uri += "?SDate=" + SDate;
            uri += "&EDate=" + EDate;
            if (State != null) uri += "&State=" + string.Join(",", State);
            if (Item != null) uri += "&Item=" + string.Join(",", Item);
            if (ReserveYN != null) uri += "&ReserveYN=" + ReserveYN;
            if (SenderYN != null && (bool) SenderYN) uri += "&SenderYN=1";
            if (Page != null) uri += "&Page=" + Page.ToString();
            if (PerPage != null) uri += "&PerPage=" + PerPage.ToString();
            if (Order != null) uri += "&Order=" + Order;
            if (Qstring != null) uri += "&Qstring=" + Qstring;

            return httpget<KakaoSearchResult>(uri, CorpNum, UserID);
        }

        //카카오톡 전송내역 팝업
        public string GetSentListURL(string CorpNum, string UserID)
        {
            URLResponse response = httpget<URLResponse>("/KakaoTalk/?TG=BOX", CorpNum, UserID);

            return response.url;
        }

        #endregion

        #region Point API

        //전송단가 확인
        public Single GetUnitCost(string CorpNum, KakaoType msgType, string UserID = null)
        {
            UnitCostResponse response =
                httpget<UnitCostResponse>("/KakaoTalk/UnitCost?Type=" + msgType.ToString(), CorpNum, UserID);

            return response.unitCost;
        }

        //과금정보 확인
        public ChargeInfo GetChargeInfo(string CorpNum, KakaoType msgType, string UserID = null)
        {
            ChargeInfo response =
                httpget<ChargeInfo>("/KakaoTalk/ChargeInfo?Type=" + msgType.ToString(), CorpNum, UserID);
            return response;
        }

        #endregion


        [DataContract]
        private class FTSSendRequest
        {
            [DataMember] public string plusFriendID = null;
            [DataMember] public string snd = null;
            [DataMember] public string content = null;
            [DataMember] public string altContent = null;
            [DataMember] public string altSendType = null;
            [DataMember] public string sndDT = null;
            [DataMember] public bool? adsYN = false;
            [DataMember] public string imageURL = null;
            [DataMember] public string requestNum = null;
            [DataMember] public List<KakaoButton> btns = null;
            [DataMember] public List<KakaoReceiver> msgs = null;
        }


        [DataContract]
        private class ATSSendRequest
        {
            [DataMember] public string templateCode = null;
            [DataMember] public string snd = null;
            [DataMember] public string content = null;
            [DataMember] public string altContent = null;
            [DataMember] public string altSendType = null;
            [DataMember] public string sndDT = null;
            [DataMember] public string requestNum = null;
            [DataMember] public List<KakaoReceiver> msgs;
        }

        [DataContract]
        public class ReceiptResponse
        {
            [DataMember] public string receiptNum = null;
        }
    }
}