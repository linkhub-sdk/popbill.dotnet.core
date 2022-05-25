using System;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Popbill.Message
{
    public enum MessageType
    {
        SMS,
        LMS,
        XMS,
        MMS
    };

    public class MessageService : BaseService
    {
        public MessageService(string LinkID, string SecretKey) : base(LinkID, SecretKey)
        {
            this.AddScope("150");
            this.AddScope("151");
            this.AddScope("152");
        }

        #region 발신번호 API

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

        #endregion

        #region Send API

        //단문 단건 전송
        public string SendSMS(string CorpNum, string snd, string sndnm, string rcv, string rcvnm, string content,
            DateTime? sndDT = null, bool adsYN = false, string requestNum = null, string UserID = null)
        {
            List<Message> messages = new List<Message>();
            Message msg = new Message();
            msg.receiveNum = rcv;
            msg.receiveName = rcvnm;
            messages.Add(msg);

            return sendMessage(MessageType.SMS, CorpNum, snd, sndnm, null, content, messages, sndDT, adsYN, requestNum,
                UserID);
        }

        //단문 대량 전송
        public string SendSMS(string CorpNum, string snd, List<Message> messages,
            DateTime? sndDT = null, bool adsYN = false, string requestNum = null, string UserID = null)
        {
            return sendMessage(MessageType.SMS, CorpNum, snd, null, null, null, messages, sndDT, adsYN, requestNum,
                UserID);
        }

        //단문 동보 전송
        public string SendSMS(string CorpNum, string snd, string content, List<Message> messages,
            DateTime? sndDT = null, bool adsYN = false, string requestNum = null, string UserID = null)
        {
            return sendMessage(MessageType.SMS, CorpNum, snd, null, null, content, messages, sndDT, adsYN, requestNum,
                UserID);
        }

        //장문 단건 전송
        public string SendLMS(string CorpNum, string snd, string sndnm, string rcv, string rcvnm, string subject,
            string content, DateTime? sndDT = null, bool adsYN = false, string requestNum = null, string UserID = null)
        {
            List<Message> messages = new List<Message>();
            Message msg = new Message();
            msg.receiveNum = rcv;
            msg.receiveName = rcvnm;
            messages.Add(msg);

            return sendMessage(MessageType.LMS, CorpNum, snd, sndnm, subject, content, messages, sndDT, adsYN,
                requestNum, UserID);
        }

        //장문 대량 전송
        public string SendLMS(string CorpNum, string snd, List<Message> messages, DateTime? sndDT = null,
            bool adsYN = false, string requestNum = null, string UserID = null)
        {
            return sendMessage(MessageType.LMS, CorpNum, snd, null, null, null, messages, sndDT, adsYN,
                requestNum, UserID);
        }

        //장문 동보 전송
        public string SendLMS(string CorpNum, string snd, string subject, string content, List<Message> messages,
            DateTime? sndDT = null, bool adsYN = false, string requestNum = null, string UserID = null)
        {
            return sendMessage(MessageType.LMS, CorpNum, snd, null, subject, content, messages, sndDT, adsYN,
                requestNum, UserID);
        }

        //단문/장문 자동인식 단건 전송
        public string SendXMS(string CorpNum, string snd, string sndnm, string rcv, string rcvnm, string subject,
            string content, DateTime? sndDT = null, bool adsYN = false, string requestNum = null, string UserID = null)
        {
            List<Message> messages = new List<Message>();
            Message msg = new Message();
            msg.receiveNum = rcv;
            msg.receiveName = rcvnm;
            messages.Add(msg);

            return sendMessage(MessageType.XMS, CorpNum, snd, sndnm, subject, content, messages, sndDT, adsYN,
                requestNum, UserID);
        }

        //단문/장문 자동인식 대량 전송
        public string SendXMS(string CorpNum, string snd, List<Message> messages,
            DateTime? sndDT = null, bool adsYN = false, string requestNum = null, string UserID = null)
        {
            return sendMessage(MessageType.XMS, CorpNum, snd, null, null, null, messages, sndDT, adsYN,
                requestNum, UserID);
        }

        //단문/장문 자동인식 동보 전송
        public string SendXMS(string CorpNum, string snd, string subject, string content, List<Message> messages,
            DateTime? sndDT = null, bool adsYN = false, string requestNum = null, string UserID = null)
        {
            return sendMessage(MessageType.XMS, CorpNum, snd, null, subject, content, messages, sndDT, adsYN,
                requestNum, UserID);
        }

        //sendMessage (SMS / LMS / XMS)
        private string sendMessage(MessageType msgType, string CorpNum, string snd, string sndnm,
            string subject, string content, List<Message> messages, DateTime? sndDT, bool adsYN,
            string requestNum, string UserID)
        {
            if (messages == null || messages.Count == 0)
                throw new PopbillException(-99999999, "전송할 메시지가 입력되지 않았습니다.");

            sendRequest request = new sendRequest();

            request.snd = snd;
            request.sndnm = sndnm;
            request.subject = subject;
            request.content = content;
            request.msgs = messages;
            request.sndDT = sndDT == null ? null : sndDT.Value.ToString("yyyyMMddHHmmss");
            request.adsYN = adsYN;
            request.requestNum = requestNum;

            string PostData = toJsonString(request);

            ReceiptResponse response =
                httppost<ReceiptResponse>("/" + msgType.ToString(), CorpNum, PostData, null, null, UserID);

            return response.receiptNum;
        }

        //포토 단건 전송
        public string SendMMS(string CorpNum, string snd, string sndnm, string rcv, string rcvnm,
            string subject, string content, string mmsfilepath, DateTime? sndDT = null, bool adsYN = false,
            string requestNum = null, string UserID = null)
        {
            List<Message> messages = new List<Message>();
            Message msg = new Message();
            msg.receiveNum = rcv;
            msg.receiveName = rcvnm;
            messages.Add(msg);

            return SendMMS(CorpNum, snd, sndnm, subject, content, messages, mmsfilepath, sndDT, adsYN, requestNum,
                UserID);
        }

        //포토 대량 전송
        public string SendMMS(string CorpNum, string snd, List<Message> messages, string mmsfilepath,
            DateTime? sndDT = null, bool adsYN = false, string requestNum = null, string UserID = null)
        {
            return SendMMS(CorpNum, snd, null, null, null, messages, mmsfilepath, sndDT, adsYN, requestNum, UserID);
        }

        //포토 동보 전송
        public string SendMMS(string CorpNum, string snd, string subject, string content, List<Message> messages,
            string mmsfilepath, DateTime? sndDT = null, bool adsYN = false, string requestNum = null,
            string UserID = null)
        {
            return SendMMS(CorpNum, snd, null, subject, content, messages, mmsfilepath, sndDT, adsYN, requestNum,
                UserID);
        }

        //sendMessage (MMS)
        public string SendMMS(string CorpNum, string snd, string sndnm, string subject, string content,
            List<Message> messages, string mmsfilepath, DateTime? sndDT = null, bool adsYN = false,
            string requestNum = null, string UserID = null)
        {
            if (messages == null || messages.Count == 0)
                throw new PopbillException(-99999999, "전송할 메시지가 입력되지 않았습니다.");

            sendRequest request = new sendRequest();

            request.snd = snd;
            request.sndnm = sndnm;
            request.subject = subject;
            request.content = content;
            request.msgs = messages;
            request.sndDT = sndDT == null ? null : sndDT.Value.ToString("yyyyMMddHHmmss");
            request.adsYN = adsYN;
            request.requestNum = requestNum;

            string PostData = toJsonString(request);

            List<UploadFile> UploadFiles = new List<UploadFile>();

            UploadFile uf = new UploadFile();

            uf.FieldName = "file";
            uf.FileName = System.IO.Path.GetFileName(mmsfilepath);
            uf.FileData = new FileStream(mmsfilepath, FileMode.Open, FileAccess.Read);

            UploadFiles.Add(uf);

            ReceiptResponse response =
                httppostFile<ReceiptResponse>("/MMS", CorpNum, PostData, UploadFiles, null, UserID);

            return response.receiptNum;
        }

        //예약전송 취소
        public Response CancelReserve(string CorpNum, string receiptNum, string UserID = null)
        {
            if (string.IsNullOrEmpty(receiptNum))
                throw new PopbillException(-99999999, "접수번호가 입력되지 않았습니다.");

            return httpget<Response>("/Message/" + receiptNum + "/Cancel", CorpNum, UserID);
        }

        //예약전송 취소 - 요청번호 할당
        public Response CancelReserveRN(string CorpNum, string requestNum, string UserID = null)
        {
            if (string.IsNullOrEmpty(requestNum))
                throw new PopbillException(-99999999, "요청번호(requestNum)가 입력되지 않았습니다.");

            return httpget<Response>("/Message/Cancel/" + requestNum, CorpNum, UserID);
        }

        #endregion

        #region Info API

        //전송내역 확인
        public List<MessageResult> GetMessages(string CorpNum, string receiptNum, string UserID = null)
        {
            if (string.IsNullOrEmpty(receiptNum))
                throw new PopbillException(-99999999, "접수번호가 입력되지 않았습니다.");

            return httpget<List<MessageResult>>("/Message/" + receiptNum, CorpNum, UserID);
        }

        //전송내역 확인 - 요청번호 할당
        public List<MessageResult> GetMessagesRN(string CorpNum, string requestNum, string UserID = null)
        {
            if (string.IsNullOrEmpty(requestNum))
                throw new PopbillException(-99999999, "요청번호(requestNum)가 입력되지 않았습니다.");

            return httpget<List<MessageResult>>("/Message/Get/" + requestNum, CorpNum, UserID);
        }

        //전송내역 요약정보 확인
        public List<MessageState> GetStates(string CorpNum, List<string> ReciptNumList, string UserID = null)
        {
            if (ReciptNumList == null || ReciptNumList.Count == 0)
                throw new PopbillException(-99999999, "문자전송 접수번호가 입력되지 않았습니다.");

            string PostData = toJsonString(ReciptNumList);
            return httppost<List<MessageState>>("/Message/States", CorpNum, PostData, null, null, UserID);
        }

        //전송내역 목록 조회
        public MSGSearchResult Search(string CorpNum, string SDate, string EDate, string[] State = null,
            string[] Item = null, bool? ReserveYN = false, bool? SenderYN = false, int? Page = null,
            int? PerPage = null, string Order = null, string QString = null, string UserID = null)
        {
            if (string.IsNullOrEmpty(SDate)) throw new PopbillException(-99999999, "시작일자가 입력되지 않았습니다.");
            if (string.IsNullOrEmpty(EDate)) throw new PopbillException(-99999999, "종료일자가 입력되지 않았습니다.");

            string uri = "/Message/Search";
            uri += "?SDate=" + SDate;
            uri += "&EDate=" + EDate;
            if (State != null) uri += "&State=" + string.Join(",", State);
            if (Item != null) uri += "&Item=" + string.Join(",", Item);
            if (ReserveYN != null && (bool) ReserveYN) uri += "&ReserveYN=1";
            if (SenderYN != null && (bool) SenderYN) uri += "&SenderYN=1";
            if (Page != null) uri += "&Page=" + Page.ToString();
            if (PerPage != null) uri += "&PerPage=" + PerPage.ToString();
            if (Order != null) uri += "&Order=" + Order;
            if (QString != null) uri += "&QString=" + HttpUtility.UrlEncode(QString);

            return httpget<MSGSearchResult>(uri, CorpNum, UserID);
        }

        //문자 전송내역 팝업
        public string GetSentListURL(string CorpNum, string UserID)
        {
            URLResponse response = httpget<URLResponse>("/Message/?TG=BOX", CorpNum, UserID);

            return response.url;
        }

        //080 수신거부 목록 확인
        public List<AutoDeny> GetAutoDenyList(string CorpNum, string UserID = null)
        {
            return httpget<List<AutoDeny>>("/Message/Denied", CorpNum, UserID);
        }

        #endregion

        #region Point API

        //전송단가 확인
        public Single GetUnitCost(string CorpNum, MessageType msgType, string UserID = null)
        {
            UnitCostResponse response =
                httpget<UnitCostResponse>("/Message/UnitCost?Type=" + msgType.ToString(), CorpNum, UserID);

            return response.unitCost;
        }

        //과금정보 확인
        public ChargeInfo GetChargeInfo(string CorpNum, MessageType msgType, string UserID = null)
        {
            ChargeInfo response =
                httpget<ChargeInfo>("/Message/ChargeInfo?Type=" + msgType.ToString(), CorpNum, UserID);
            return response;
        }

        #endregion

        [DataContract]
        private class sendRequest
        {
            [DataMember] public string snd = null;
            [DataMember] public string sndnm = null;
            [DataMember] public string subject = null;
            [DataMember] public string content = null;
            [DataMember] public string sndDT = null;
            [DataMember] public string requestNum = null;
            [DataMember] public List<Message> msgs;
            [DataMember] public bool adsYN = false;
        }

        [DataContract]
        public class ReceiptResponse
        {
            [DataMember] public string receiptNum;
        }
    }
}