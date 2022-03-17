using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Popbill.Cashbill
{
    public class CashbillService : BaseService
    {
        public CashbillService(string LinkID, string SecretKey) : base(LinkID, SecretKey)
        {
            this.AddScope("140");
        }

        #region Issue API

        //문서번호 확인
        public bool CheckMgtKeyInUse(string CorpNum, string MgtKey, string UserID = null)
        {
            if (string.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            try
            {
                CashbillInfo response = httpget<CashbillInfo>("/Cashbill/" + MgtKey, CorpNum, UserID);

                return string.IsNullOrEmpty(response.itemKey) == false;
            }
            catch (PopbillException pe)
            {
                if (pe.code == -14000003) return false;

                throw pe;
            }
        }

        //즉시발행
        public CBIssueResponse RegistIssue(string CorpNum, Cashbill cashbill, string Memo, string UserID = null, string EmailSubject = null)
        {
            if (cashbill == null) throw new PopbillException(-99999999, "현금영수증 정보가 입력되지 않았습니다.");

            cashbill.memo = Memo;

            if (EmailSubject != null) cashbill.emailSubject = EmailSubject;

            string PostData = toJsonString(cashbill);

            return httppost<CBIssueResponse>("/Cashbill", CorpNum, PostData, "ISSUE", null, UserID);
        }

        //초대량 발행 접수
        public BulkResponse BulkSubmit(string CorpNum, string SubmitID, List<Cashbill> cashbillList)
        {
            return BulkSubmit(CorpNum, SubmitID, cashbillList, null);
        }

        public BulkResponse BulkSubmit(string CorpNum, string SubmitID, List<Cashbill> cashbillList, string UserID)
        {
            if (string.IsNullOrEmpty(SubmitID)) throw new PopbillException(-99999999, "제출아이디(SubmitID)가 입력되지 않았습니다.");
            if (cashbillList == null || cashbillList.Count <= 0) throw new PopbillException(-99999999, "현금영수증 정보가 입력되지 않았습니다.");

            BulkCashbillSubmit cb = new BulkCashbillSubmit();
            cb.cashbills = cashbillList;

            String PostData = toJsonString(cb);

            return httpBulkPost<BulkResponse>("/Cashbill/", CorpNum, SubmitID, PostData, UserID, "BULKISSUE");

        }

        public BulkCashbillResult GetBulkResult(string CorpNum, string SubmitID)
        {
            return GetBulkResult(CorpNum, SubmitID, null);
        }
        public BulkCashbillResult GetBulkResult(string CorpNum, string SubmitID, string UserID)
        {
            if (string.IsNullOrEmpty(SubmitID)) throw new PopbillException(-99999999, "제출아이디(SubmitID)가 입력되지 않았습니다.");

            return httpget<BulkCashbillResult>("/Cashbill/BULK/" + SubmitID + "/State", CorpNum, UserID);
        }

        //임시저장
        public Response Register(string CorpNum, Cashbill cashbill, string UserID = null)
        {
            if (cashbill == null) throw new PopbillException(-99999999, "현금영수증 정보가 입력되지 않았습니다.");

            string PostData = toJsonString(cashbill);

            return httppost<Response>("/Cashbill", CorpNum, PostData, null, null, UserID);
        }

        //수정
        public Response Update(string CorpNum, string MgtKey, Cashbill cashbill, string UserID = null)
        {
            if (string.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            if (cashbill == null) throw new PopbillException(-99999999, "현금영수증 정보가 입력되지 않았습니다.");

            string PostData = toJsonString(cashbill);

            return httppost<Response>("/Cashbill/" + MgtKey, CorpNum, PostData, "PATCH", null, UserID);
        }

        //발행
        public CBIssueResponse Issue(string CorpNum, string MgtKey, string Memo, string UserID = null)
        {
            if (string.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            MemoRequest request = new MemoRequest();

            request.memo = Memo;

            string PostData = toJsonString(request);

            return httppost<CBIssueResponse>("/Cashbill/" + MgtKey, CorpNum, PostData, "ISSUE", null, UserID);
        }

        //발행취소
        public Response CancelIssue(string CorpNum, string MgtKey, string Memo, string UserID = null)
        {
            if (string.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            MemoRequest request = new MemoRequest();

            request.memo = Memo;

            string PostData = toJsonString(request);

            return httppost<Response>("/Cashbill/" + MgtKey, CorpNum, PostData, "CANCELISSUE", null, UserID);
        }

        //삭제
        public Response Delete(string CorpNum, string MgtKey, string UserID = null)
        {
            if (string.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            return httppost<Response>("/Cashbill/" + MgtKey, CorpNum, null, "DELETE", null, UserID);
        }

        //취소현금영수증 즉시발행
        public CBIssueResponse RevokeRegistIssue(string CorpNum, string mgtKey, string orgConfirmNum, string orgTradeDate,
            bool smssendYN = false, string memo = null, bool isPartCancel = false, int? cancelType = null,
            string totalAmount = null, string supplyCost = null, string tax = null, string serviceFee = null,
            string UserID = null)
        {
            RevokeRequest request = new RevokeRequest();
            request.mgtKey = mgtKey;
            request.orgConfirmNum = orgConfirmNum;
            request.orgTradeDate = orgTradeDate;
            request.smssenYN = smssendYN;
            request.memo = memo;
            request.isPartCancel = isPartCancel;
            request.cancelType = cancelType;
            request.totalAmount = totalAmount;
            request.supplyCost = supplyCost;
            request.tax = tax;
            request.serviceFee = serviceFee;

            string PostData = toJsonString(request);

            return httppost<CBIssueResponse>("/Cashbill", CorpNum, PostData, "REVOKEISSUE", null, UserID);
        }

        //취소현금영수증 임시저장
        public Response RevokeRegister(string CorpNum, string mgtKey, string orgConfirmNum, string orgTradeDate,
            bool smssendYN = false, bool isPartCancel = false, int? cancelType = null, string totalAmount = null,
            string supplyCost = null, string tax = null, string serviceFee = null, string UserID = null)
        {
            RevokeRequest request = new RevokeRequest();
            request.mgtKey = mgtKey;
            request.orgConfirmNum = orgConfirmNum;
            request.orgTradeDate = orgTradeDate;
            request.smssenYN = smssendYN;
            request.isPartCancel = isPartCancel;
            request.cancelType = cancelType;
            request.totalAmount = totalAmount;
            request.supplyCost = supplyCost;
            request.tax = tax;
            request.serviceFee = serviceFee;

            string PostData = toJsonString(request);

            return httppost<Response>("/Cashbill", CorpNum, PostData, "REVOKE", null, UserID);
        }

        #endregion

        #region Info API

        //상태 확인
        public CashbillInfo GetInfo(string CorpNum, string MgtKey, string UserID = null)
        {
            if (string.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            return httpget<CashbillInfo>("/Cashbill/" + MgtKey, CorpNum, UserID);
        }

        //상태 대량 확인
        public List<CashbillInfo> GetInfos(string CorpNum, List<string> MgtKeyList, string UserID = null)
        {
            if (MgtKeyList == null || MgtKeyList.Count == 0)
                throw new PopbillException(-99999999, "문서번호 목록이 입력되지 않았습니다.");

            string PostData = toJsonString(MgtKeyList);

            return httppost<List<CashbillInfo>>("/Cashbill/States", CorpNum, PostData, null, null, UserID);
        }

        //상세정보 확인
        public Cashbill GetDetailInfo(string CorpNum, string MgtKey, string UserID = null)
        {
            if (string.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            return httpget<Cashbill>("/Cashbill/" + MgtKey + "?Detail", CorpNum, UserID);
        }

        //목록 조회
        public CBSearchResult Search(string CorpNum, string DType, string SDate, string EDate, string[] State = null,
            string[] TradeType = null, string[] TradeUsage = null, string[] TradeOpt = null,
            string[] TaxationType = null, int? Page = null, int? PerPage = null, string Order = null,
            string QString = null, string UserID = null, string FranchiseTaxRegID = null)
        {
            if (string.IsNullOrEmpty(DType)) throw new PopbillException(-99999999, "검색일자 유형이 입력되지 않았습니다.");
            if (string.IsNullOrEmpty(SDate)) throw new PopbillException(-99999999, "시작일자가 입력되지 않았습니다.");
            if (string.IsNullOrEmpty(EDate)) throw new PopbillException(-99999999, "종료일자가 입력되지 않았습니다.");

            string uri = "/Cashbill/Search";
            uri += "?DType=" + DType;
            uri += "&SDate=" + SDate;
            uri += "&EDate=" + EDate;
            if (State != null) uri += "&State=" + string.Join(",", State);
            if (TradeType != null) uri += "&TradeType=" + string.Join(",", TradeType);
            if (TradeUsage != null) uri += "&TradeUsage=" + string.Join(",", TradeUsage);
            if (TradeOpt != null) uri += "&TradeOpt=" + string.Join(",", TradeOpt);
            if (TaxationType != null) uri += "&TaxationType=" + string.Join(",", TaxationType);
            if (Page != null) uri += "&Page=" + Page.ToString();
            if (PerPage != null) uri += "&PerPage=" + PerPage.ToString();
            if (Order != null) uri += "&Order=" + Order;
            if (QString != null) uri += "&QString=" + QString;
            if (FranchiseTaxRegID != null) uri += "&FranchiseTaxRegID=" + FranchiseTaxRegID;

            return httpget<CBSearchResult>(uri, CorpNum, UserID);
        }

        public Response AssignMgtKey(string CorpNum, string ItemKey, string MgtKey,
            string UserID = null)
        {
            if (string.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "할당할 문서번호가 입력되지 않았습니다.");

            string PostData = "MgtKey=" + MgtKey;

            return httppost<Response>("/Cashbill/" + ItemKey, CorpNum, PostData, null,
                "application/x-www-form-urlencoded; charset=utf-8", UserID);
        }


        //상태 변경이력 확인
        public List<CashbillLog> GetLogs(string CorpNum, string MgtKey, string UserID = null)
        {
            if (string.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            return httpget<List<CashbillLog>>("/Cashbill/" + MgtKey + "/Logs", CorpNum, UserID);
        }

        //현금영수증 문서함 관련 URL
        public string GetURL(string CorpNum, string TOGO, string UserID = null)
        {
            URLResponse response = httpget<URLResponse>("/Cashbill?TG=" + TOGO, CorpNum, UserID);

            return response.url;
        }

        #endregion

        #region PopUp/Print API

        //현금영수증 보기 URL
        public string GetPopUpURL(string CorpNum, string MgtKey, string UserID = null)
        {
            if (string.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            URLResponse response = httpget<URLResponse>("/Cashbill/" + MgtKey + "?TG=POPUP", CorpNum, UserID);

            return response.url;
        }

        //현금영수증 보기 URL [메뉴/버튼 제외]
        public string GetViewURL(string CorpNum, string MgtKey, string UserID = null)
        {
            if (string.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            URLResponse response = httpget<URLResponse>("/Cashbill/" + MgtKey + "?TG=VIEW", CorpNum, UserID);

            return response.url;
        }

        //현금영수증 PDF 다운로드 URL
        public string GetPDFURL(string CorpNum, string MgtKey, string UserID = null)
        {
            if (string.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            URLResponse response = httpget<URLResponse>("/Cashbill/" + MgtKey + "?TG=PDF", CorpNum, UserID);

            return response.url;
        }

        //현금영수증 인쇄 URL
        public string GetPrintURL(string CorpNum, string MgtKey, string UserID = null)
        {
            if (string.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            URLResponse response = httpget<URLResponse>("/Cashbill/" + MgtKey + "?TG=PRINT", CorpNum, UserID);

            return response.url;
        }

        public string GetEPrintURL(string CorpNum, string MgtKey, string UserID = null)
        {
            if (string.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            URLResponse response = httpget<URLResponse>("/Cashbill/" + MgtKey + "?TG=EPRINT", CorpNum, UserID);

            return response.url;
        }

        //현금영수증 대량 인쇄 URL
        public string GetMassPrintURL(string CorpNum, List<string> MgtKeyList, string UserID = null)
        {
            if (MgtKeyList == null || MgtKeyList.Count == 0)
                throw new PopbillException(-99999999, "문서번호 목록이 입력되지 않았습니다.");

            string PostData = toJsonString(MgtKeyList);

            URLResponse response = httppost<URLResponse>("/Cashbill/Prints", CorpNum, PostData, null, null, UserID);

            return response.url;
        }

        //현금영수증 메일링크 URL
        public string GetMailURL(string CorpNum, string MgtKey, string UserID = null)
        {
            if (string.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            URLResponse response = httpget<URLResponse>("/Cashbill/" + MgtKey + "?TG=MAIL", CorpNum, UserID);

            return response.url;
        }

        #endregion

        #region Add Ons API

        //메일 전송
        public Response SendEmail(string CorpNum, string MgtKey, string Receiver, string UserID = null)
        {
            if (string.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            ResendRequest request = new ResendRequest();

            request.receiver = Receiver;

            string PostData = toJsonString(request);

            return httppost<Response>("/Cashbill/" + MgtKey, CorpNum, PostData, "EMAIL", null, UserID);
        }

        //문자 전송
        public Response SendSMS(string CorpNum, string MgtKey, string Sender, string Receiver, string Contents,
            string UserID = null)
        {
            if (string.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            ResendRequest request = new ResendRequest();

            request.sender = Sender;
            request.receiver = Receiver;
            request.contents = Contents;

            string PostData = toJsonString(request);

            return httppost<Response>("/Cashbill/" + MgtKey, CorpNum, PostData, "SMS", null, UserID);
        }

        //팩스 전송
        public Response SendFAX(string CorpNum, string MgtKey, string Sender, string Receiver, string UserID = null)
        {
            if (string.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            ResendRequest request = new ResendRequest();

            request.sender = Sender;
            request.receiver = Receiver;

            string PostData = toJsonString(request);

            return httppost<Response>("/Cashbill/" + MgtKey, CorpNum, PostData, "FAX", null, UserID);
        }

        //알림메일 전송목록 조회
        public List<EmailConfig> ListEmailConfig(string CorpNum, string UserID = null)
        {
            return httpget<List<EmailConfig>>("/Cashbill/EmailSendConfig", CorpNum, UserID);
        }

        //알림메일 전송설정 수정
        public Response UpdateEmailConfig(string CorpNum, string EmailType, bool SendYN, string UserID = null)
        {
            if (string.IsNullOrEmpty(EmailType)) throw new PopbillException(-99999999, "메일전송 타입이 입력되지 않았습니다.");

            string uri = "/Cashbill/EmailSendConfig?EmailType=" + EmailType + "&SendYN=" + SendYN;

            return httppost<Response>(uri, CorpNum, null, null, null, UserID);
        }

        #endregion

        #region Point API

        //발행단가 확인
        public Single GetUnitCost(string CorpNum, string UserID = null)
        {
            UnitCostResponse response = httpget<UnitCostResponse>("/Cashbill?cfg=UNITCOST", CorpNum, UserID);

            return response.unitCost;
        }

        //과금정보 확인
        public ChargeInfo GetChargeInfo(string CorpNum, string UserID = null)
        {
            ChargeInfo response = httpget<ChargeInfo>("/Cashbill/ChargeInfo/", CorpNum, UserID);

            return response;
        }

        #endregion

        [DataContract]
        private class MemoRequest
        {
            [DataMember] public string memo;
        }

        [DataContract]
        private class ResendRequest
        {
            [DataMember] public string receiver;
            [DataMember] public string sender = null;
            [DataMember] public string contents = null;
        }

        [DataContract]
        private class RevokeRequest
        {
            [DataMember] public string mgtKey;
            [DataMember] public string orgTradeDate;
            [DataMember] public string orgConfirmNum;
            [DataMember] public bool? smssenYN = false;
            [DataMember] public string memo;
            [DataMember] public bool? isPartCancel = false;
            [DataMember] public int? cancelType;
            [DataMember] public string totalAmount;
            [DataMember] public string supplyCost;
            [DataMember] public string tax;
            [DataMember] public string serviceFee;
        }
    }
}