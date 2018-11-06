using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Popbill.Fax
{
    public class FaxService : BaseService
    {
        public FaxService(string LinkID, string SecretKey) : base(LinkID, SecretKey)
        {
            this.AddScope("160");
        }

        #region 발신번호 API

        //발신번호 관리 팝업 URL, 팩스 전송내역 팝업 URL
        public string GetURL(string CorpNum, string TOGO, string UserID = null)
        {
            URLResponse response = httpget<URLResponse>("/FAX/?TG=" + TOGO, CorpNum, UserID);

            return response.url;
        }

        //발신번호 목록 확인
        public List<SenderNumber> GetSenderNumberList(string CorpNum, string UserID = null)
        {
            return httpget<List<SenderNumber>>("/FAX/SenderNumber", CorpNum, UserID);
        }

        #endregion

        #region Send API

        //팩스 단건 전송
        public string SendFAX(string CorpNum, string snd, string sndnm, string rcv, string rcvnm,
            List<string> filePaths, string title = null, DateTime? reserveDT = null, bool adsYN = false,
            string requestNum = null, string UserID = null)
        {
            List<FaxReceiver> receivers = new List<FaxReceiver>();
            FaxReceiver receiver = new FaxReceiver();
            receiver.receiveNum = rcv;
            receiver.receiveName = rcvnm;
            receivers.Add(receiver);

            return SendFAX(CorpNum, snd, sndnm, receivers, filePaths, title, reserveDT, adsYN, requestNum, UserID);
        }

        //팩스 동보 전송
        public string SendFAX(string CorpNum, string snd, string sndnm, List<FaxReceiver> receivers,
            List<string> filePaths, string title = null, DateTime? reserveDT = null, bool adsYN = false,
            string requestNum = null, string UserID = null)
        {
            if (filePaths == null || filePaths.Count == 0)
                throw new PopbillException(-99999999, "전송할 파일정보가 입력되지 않았습니다.");
            if (receivers == null || receivers.Count == 0)
                throw new PopbillException(-99999999, "수신처 정보가 입력되지 않았습니다.");

            List<UploadFile> UploadFiles = new List<UploadFile>();

            foreach (string filePath in filePaths)
            {
                UploadFile uf = new UploadFile();

                uf.FieldName = "file";
                uf.FileName = System.IO.Path.GetFileName(filePath);
                uf.FileData = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                UploadFiles.Add(uf);
            }

            sendRequest request = new sendRequest();

            request.snd = snd;
            request.sndnm = sndnm;
            request.title = title;
            request.fCnt = filePaths.Count;
            request.sndDT = reserveDT == null ? null : reserveDT.Value.ToString("yyyyMMddHHmmss");
            request.adsYN = adsYN;
            request.requestNum = requestNum;
            request.rcvs = receivers;

            string PostData = toJsonString(request);

            ReceiptResponse response;
            response = httppostFile<ReceiptResponse>("/FAX", CorpNum, PostData, UploadFiles, null, UserID);

            return response.receiptNum;
        }

        //팩스 단건 재전송
        public string ResendFAX(string CorpNum, string receiptNum, string snd, string sndnm, string rcv, string rcvnm,
            string title = null, DateTime? reserveDT = null, string requestNum = null, string UserID = null)
        {
            List<FaxReceiver> receivers = null;

            if ((rcv.Length != 0) && (rcvnm.Length != 0))
            {
                receivers = new List<FaxReceiver>();
                FaxReceiver receiver = new FaxReceiver();
                receiver.receiveNum = rcv;
                receiver.receiveName = rcvnm;
                receivers.Add(receiver);
            }

            return ResendFAX(CorpNum, receiptNum, snd, sndnm, receivers, title, reserveDT, requestNum, UserID);
        }

        //팩스 동보 재전송
        public string ResendFAX(string CorpNum, string receiptNum, string snd, string sndnm,
            List<FaxReceiver> receivers, string title = null, DateTime? reserveDT = null, string requestNum = null,
            string UserID = null)
        {
            if (receiptNum == "") throw new PopbillException(-99999999, "팩스접수번호(receiptNum)가 입력되지 않았습니다.");

            sendRequest request = new sendRequest();

            if (snd != "") request.snd = snd;
            if (sndnm != "") request.sndnm = sndnm;
            if (receivers != null) request.rcvs = receivers;
            if (title != null) request.title = title;
            if (reserveDT != null) reserveDT.Value.ToString("yyyyMMddHHmmss");
            if (requestNum != null) request.requestNum = requestNum;

            string PostData = toJsonString(request);

            ReceiptResponse response;
            response = httppost<ReceiptResponse>("/FAX/" + receiptNum, CorpNum, PostData, null, null, UserID);

            return response.receiptNum;
        }

        //팩스 단건 재전송 - 요청번호 할당
        public string ResendFAXRN(string CorpNum, string orgRequestNum, string snd, string sndnm, string rcv,
            string rcvnm, string assignRequestNum = null, string title = null, DateTime? reserveDT = null,
            string UserID = null)
        {
            List<FaxReceiver> receivers = null;

            if ((rcv.Length != 0) && (rcvnm.Length != 0))
            {
                receivers = new List<FaxReceiver>();
                FaxReceiver receiver = new FaxReceiver();
                receiver.receiveNum = rcv;
                receiver.receiveName = rcvnm;
                receivers.Add(receiver);
            }

            return ResendFAXRN(CorpNum, orgRequestNum, snd, sndnm, receivers, assignRequestNum, title, reserveDT,
                UserID);
        }

        //팩스 동보 재전송 - 요청번호 할당
        public string ResendFAXRN(string CorpNum, string orgRequestNum, string snd, string sndnm,
            List<FaxReceiver> receivers, string assignRequestNum = null, string title = null,
            DateTime? reserveDT = null, string UserID = null)
        {
            if (orgRequestNum == "") throw new PopbillException(-99999999, "원본 팩스요청번호(requestNum)가 입력되지 않았습니다.");

            sendRequest request = new sendRequest();

            if (snd != "") request.snd = snd;
            if (sndnm != "") request.sndnm = sndnm;
            if (receivers != null) request.rcvs = receivers;
            if (assignRequestNum != "") request.requestNum = assignRequestNum;
            if (title != null) request.title = title;
            if (reserveDT != null) reserveDT.Value.ToString("yyyyMMddHHmmss");


            string PostData = toJsonString(request);

            ReceiptResponse response;
            response = httppost<ReceiptResponse>("/FAX/Resend/" + orgRequestNum, CorpNum, PostData, null, null, UserID);

            return response.receiptNum;
        }

        //예약전송 취소
        public Response CancelReserve(string CorpNum, string receiptNum, string UserID = null)
        {
            if (string.IsNullOrEmpty(receiptNum)) throw new PopbillException(-99999999, "접수번호가 입력되지 않았습니다.");

            return httpget<Response>("/FAX/" + receiptNum + "/Cancel", CorpNum, UserID);
        }

        //예약전송 취소 - 요청번호 할당
        public Response CancelReserveRN(string CorpNum, string requestNum, string UserID = null)
        {
            if (string.IsNullOrEmpty(requestNum))
                throw new PopbillException(-99999999, "요청번호(requestNum)가 입력되지 않았습니다.");

            return httpget<Response>("/FAX/Cancel/" + requestNum, CorpNum, UserID);
        }

        #endregion

        #region Info API

        //전송내역 및 전송상태 확인
        public List<FaxResult> GetFaxResult(string CorpNum, string receiptNum, string UserID = null)
        {
            if (string.IsNullOrEmpty(receiptNum))
                throw new PopbillException(-99999999, "접수번호가 입력되지 않았습니다.");

            return httpget<List<FaxResult>>("/FAX/" + receiptNum, CorpNum, UserID);
        }

        //전송내역 및 전송상태 확인 - 요청번호 할당
        public List<FaxResult> GetFaxResultRN(string CorpNum, string requestNum, string UserID = null)
        {
            if (string.IsNullOrEmpty(requestNum))
                throw new PopbillException(-99999999, "요청번호(requestNum)가 입력되지 않았습니다.");

            return httpget<List<FaxResult>>("/FAX/Get/" + requestNum, CorpNum, UserID);
        }

        //전송내역 목록 조회
        public FAXSearchResult Search(string CorpNum, string SDate, string EDate, string[] State = null,
            bool? ReserveYN = null, bool? SenderOnly = null, int? Page = null, int? PerPage = null, string Order = null,
            string Qstring = null, string UserID = null)
        {
            if (string.IsNullOrEmpty(SDate)) throw new PopbillException(-99999999, "시작일자가 입력되지 않았습니다.");
            if (string.IsNullOrEmpty(EDate)) throw new PopbillException(-99999999, "종료일자가 입력되지 않았습니다.");

            string uri = "/FAX/Search";
            uri += "?SDate=" + SDate;
            uri += "&EDate=" + EDate;
            if (State != null) uri += "&State=" + string.Join(",", State);
            if (ReserveYN != null && (bool) ReserveYN) uri += "&ReserveYN=1";
            if (SenderOnly != null && (bool) SenderOnly) uri += "&SenderOnly=1";
            if (Page != null) uri += "&Page=" + Page.ToString();
            if (PerPage != null) uri += "&PerPage=" + PerPage.ToString();
            if (Order != null) uri += "&Order=" + Order;
            if (Qstring != null) uri += "&Qstring=" + Qstring;

            return httpget<FAXSearchResult>(uri, CorpNum, UserID);
        }

        //팩스 미리보기 팝업 URL
        public string GetPreviewURL(string corpNum, string receiptNum, string userID = null)
        {
            if (string.IsNullOrEmpty(receiptNum)) throw new PopbillException(-99999999, "접수번호가 입력되지 않았습니다.");

            URLResponse response = httpget<URLResponse>("/FAX/Preview/" + receiptNum, corpNum, userID);

            return response.url;
        }

        #endregion

        #region Point API

        //전송단가 확인
        public Single GetUnitCost(string CorpNum, string UserID = null)
        {
            UnitCostResponse response = httpget<UnitCostResponse>("/FAX/UnitCost", CorpNum, UserID);

            return response.unitCost;
        }

        //과금정보 확인
        public ChargeInfo GetChargeInfo(string CorpNum, string UserID = null)
        {
            ChargeInfo response = httpget<ChargeInfo>("/FAX/ChargeInfo", CorpNum, UserID);

            return response;
        }

        #endregion

        [DataContract]
        private class sendRequest
        {
            [DataMember] public string snd = null;
            [DataMember] public string sndnm = null;
            [DataMember] public string requestNum = null;
            [DataMember(IsRequired = false)] public bool? adsYN = null;
            [DataMember(IsRequired = false)] public string sndDT = null;
            [DataMember] public int fCnt = 0;
            [DataMember] public string title = null;
            [DataMember] public List<FaxReceiver> rcvs = null;
        }

        [DataContract]
        public class ReceiptResponse
        {
            [DataMember] public string receiptNum;
        }
    }
}