using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        //발신번호 관리 팝업, 문자저송 내역 팝업 URL
        public string GetURL(string CorpNum, string TOGO, string UserID = null)
        {
            URLResponse response = httpget<URLResponse>("/Message/?TG=" + TOGO, CorpNum, UserID);

            return response.url;
        }

        //발신번호 목록 확인
        public List<SenderNumber> GetSenderNumberList(string CorpNum, string UserID = null)
        {
            return httpget<List<SenderNumber>>("/Message/SenderNumber", CorpNum, UserID);
        }

        #endregion


        #region Send API

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
        public List<MessageResult> GetMessage(string CorpNum, string receiptNum, string UserID = null)
        {
            if (string.IsNullOrEmpty(receiptNum))
                throw new PopbillException(-99999999, "접수번호가 입력되지 않았습니다.");

            return httpget<List<MessageResult>>("/Message/" + receiptNum, CorpNum, UserID);
        }

        //전송내역 확인 - 요청번호 할당
        public List<MessageResult> GetMessageRN(string CorpNum, string requestNum, string UserID = null)
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
            if (QString != null) uri += "&QString=" + QString;

            return httpget<MSGSearchResult>(uri, CorpNum, UserID);
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
            [DataMember] public Boolean adsYN = false;
        }

        [DataContract]
        public class ReceiptResponse
        {
            [DataMember] public string receiptNum;
        }
    }
}