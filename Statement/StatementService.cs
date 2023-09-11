using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Runtime.Serialization;


namespace Popbill.Statement
{
    public class StatementService : BaseService
    {
        public StatementService(string LinkID, string SecretKey) : base(LinkID, SecretKey)
        {
            this.AddScope("121");
            this.AddScope("122");
            this.AddScope("123");
            this.AddScope("124");
            this.AddScope("125");
            this.AddScope("126");
        }

        #region Issue API

        //문서번호 확인
        public bool CheckMgtKeyInUse(string CorpNum, int itemCode, string mgtKey, string UserID = null)
        {
            if (string.IsNullOrEmpty(mgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            try
            {
                StatementInfo response =
                    httpget<StatementInfo>("/Statement/" + itemCode.ToString() + "/" + mgtKey, CorpNum, UserID);


                return string.IsNullOrEmpty(response.itemKey) == false;
            }
            catch (PopbillException pe)
            {
                if (pe.code == -12000004) return false;

                throw pe;
            }
        }

        //즉시 발행
        public STMIssueResponse RegistIssue(string CorpNum, Statement statement, string Memo = null, string UserID = null, string EmailSubject = null)
        {
            if (statement == null) throw new PopbillException(-99999999, "명세서 정보가 입력되지 않았습니다.");

            statement.memo = Memo;

            if (EmailSubject != null) statement.emailSubject = EmailSubject;

            string PostData = toJsonString(statement);

            return httppost<STMIssueResponse>("/Statement", CorpNum, PostData, "ISSUE", null, UserID);
        }

        //임시저장
        public Response Register(string CorpNum, Statement statement, string UserID = null)
        {
            if (statement == null) throw new PopbillException(-99999999, "명세서 정보가 입력되지 않았습니다.");


            string PostData = toJsonString(statement);


            return httppost<Response>("/Statement", CorpNum, PostData, null, null, UserID);
        }

        //수정
        public Response Update(string CorpNum, int itemCode, string mgtKey, Statement statement, string UserID = null)
        {
            if (string.IsNullOrEmpty(mgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            if (statement == null) throw new PopbillException(-99999999, "명세서 정보가 입력되지 않았습니다.");


            string PostData = toJsonString(statement);

            return httppost<Response>("/Statement/" + itemCode.ToString() + "/" + mgtKey, CorpNum, PostData, "PATCH",
                null, UserID);
        }

        //발행
        public Response Issue(string CorpNum, int itemCode, string mgtKey, string Memo = null, string UserID = null, string EamilSubject = null)
        {
            if (string.IsNullOrEmpty(mgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            MemoRequest request = new MemoRequest();

            request.memo = Memo;
            request.emailSubject = EamilSubject;

            string PostData = toJsonString(request);

            return httppost<Response>("/Statement/" + itemCode.ToString() + "/" + mgtKey, CorpNum, PostData, "ISSUE",
                null, UserID);
        }

        //발행취소
        public Response Cancel(string CorpNum, int itemCode, string mgtKey, string Memo = null, string UserID = null)
        {
            if (string.IsNullOrEmpty(mgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            MemoRequest request = new MemoRequest();

            request.memo = Memo;

            string PostData = toJsonString(request);

            return httppost<Response>("/Statement/" + itemCode.ToString() + "/" + mgtKey, CorpNum, PostData, "CANCEL",
                null, UserID);
        }

        //삭제
        public Response Delete(string CorpNum, int itemCode, string mgtKey, string UserID = null)
        {
            if (string.IsNullOrEmpty(mgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            return httppost<Response>("/Statement/" + itemCode.ToString() + "/" + mgtKey, CorpNum, "", "DELETE", null,
                UserID);
        }

        #endregion

        #region Info API

        //상태 확인
        public StatementInfo GetInfo(string CorpNum, int itemCode, string mgtKey, string UserID = null)
        {
            if (string.IsNullOrEmpty(mgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            return httpget<StatementInfo>("/Statement/" + itemCode.ToString() + "/" + mgtKey, CorpNum, UserID);
        }

        //상태 대량 확인
        public List<StatementInfo> GetInfos(string CorpNum, int itemCode, List<string> mgtKeyList, string UserID = null)
        {
            if (mgtKeyList == null || mgtKeyList.Count == 0) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            string PostData = toJsonString(mgtKeyList);

            return httppost<List<StatementInfo>>("/Statement/" + itemCode.ToString(), CorpNum, PostData, null, null,
                UserID);
        }

        //상세정보 확인
        public Statement GetDetailInfo(string CorpNum, int itemCode, string mgtKey, string UserID = null)
        {
            if (string.IsNullOrEmpty(mgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            return httpget<Statement>("/Statement/" + itemCode.ToString() + "/" + mgtKey + "?Detail", CorpNum, UserID);
        }

        //목록 조회
        public DocSearchResult Search(string CorpNum, string DType, string SDate, string EDate, string[] State = null,
            int[] ItemCode = null, int? Page = null, int? PerPage = null, string Order = null, string QString = null,
            string UserID = null)
        {
            if (string.IsNullOrEmpty(DType)) throw new PopbillException(-99999999, "검색일자 유형이 입력되지 않았습니다.");
            if (string.IsNullOrEmpty(SDate)) throw new PopbillException(-99999999, "시작일자가 입력되지 않았습니다.");
            if (string.IsNullOrEmpty(EDate)) throw new PopbillException(-99999999, "종료일자가 입력되지 않았습니다.");

            string uri = "/Statement/Search";
            uri += "?DType=" + DType;
            uri += "&SDate=" + SDate;
            uri += "&EDate=" + EDate;
            if (State != null) uri += "&State=" + string.Join(",", State);
            string[] ItemCodeArr = Array.ConvertAll(ItemCode, x => x.ToString());
            if (ItemCode != null) uri += "&ItemCode=" + string.Join(",", ItemCodeArr);
            if (Page != null) uri += "&Page=" + Page.ToString();
            if (PerPage != null) uri += "&PerPage=" + PerPage.ToString();
            if (Order != null) uri += "&Order=" + Order;
            if (QString != null) uri += "&QString=" + HttpUtility.UrlEncode(QString);

            return httpget<DocSearchResult>(uri, CorpNum, UserID);
        }

        //상태 변경이력 확인
        public List<StatementLog> GetLogs(string CorpNum, int itemCode, string mgtKey, string UserID = null)
        {
            if (string.IsNullOrEmpty(mgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            return httpget<List<StatementLog>>("/Statement/" + itemCode.ToString() + "/" + mgtKey + "/Logs", CorpNum,
                UserID);
        }

        //전자명세서 문서함 관련 URL
        public string GetURL(string CorpNum, string TOGO, string UserID = null)
        {
            URLResponse response = httpget<URLResponse>("/Statement?TG=" + TOGO, CorpNum, UserID);

            return response.url;
        }

        #endregion

        #region PopUp/Print API

        //전자명세서 보기 URL
        public string GetPopUpURL(string CorpNum, int itemCode, string mgtKey, string UserID = null)
        {
            if (string.IsNullOrEmpty(mgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            URLResponse response =
                httpget<URLResponse>("/Statement/" + itemCode.ToString() + "/" + mgtKey + "?TG=POPUP", CorpNum, UserID);

            return response.url;
        }

        public string GetViewURL(string CorpNum, int itemCode, string mgtKey, string UserID = null)
        {
            if (string.IsNullOrEmpty(mgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            URLResponse response =
                httpget<URLResponse>("/Statement/" + itemCode.ToString() + "/" + mgtKey + "?TG=VIEW", CorpNum, UserID);

            return response.url;
        }

        //전자명세서 인쇄 URL
        public string GetPrintURL(string CorpNum, int itemCode, string mgtKey, string UserID = null)
        {
            if (string.IsNullOrEmpty(mgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            URLResponse response =
                httpget<URLResponse>("/Statement/" + itemCode.ToString() + "/" + mgtKey + "?TG=PRINT", CorpNum, UserID);

            return response.url;
        }

        public string GetEPrintURL(string CorpNum, int itemCode, string mgtKey, string UserID = null)
        {
            if (string.IsNullOrEmpty(mgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            URLResponse response =
                httpget<URLResponse>("/Statement/" + itemCode.ToString() + "/" + mgtKey + "?TG=EPRINT", CorpNum,
                    UserID);

            return response.url;
        }

        //전자명세서 대량 인쇄 URL
        public string GetMassPrintURL(string CorpNum, int itemCode, List<string> mgtKeyList, string UserID = null)
        {
            if (mgtKeyList == null || mgtKeyList.Count == 0) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            string PostData = toJsonString(mgtKeyList);

            URLResponse response = httppost<URLResponse>("/Statement/" + itemCode.ToString() + "?Print", CorpNum,
                PostData, null, null, UserID);

            return response.url;
        }

        //전자명세서 메일링크 URL
        public string GetMailURL(string CorpNum, int itemCode, string mgtKey, string UserID = null)
        {
            if (string.IsNullOrEmpty(mgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            URLResponse response = httpget<URLResponse>("/Statement/" + itemCode.ToString() + "/" + mgtKey + "?TG=MAIL",
                CorpNum, UserID);

            return response.url;
        }

        #endregion

        #region Add Ons API

        //팝빌 인감 및 첨부문서 등록 URL
        public string GetSealURL(string CorpNum, string UserID)
        {
            URLResponse response = httpget<URLResponse>("/?TG=SEAL", CorpNum, UserID);

            return response.url;
        }

        //첨부파일 추가
        public Response AttachFile(string CorpNum, int itemCode, string mgtKey, string FilePath, string UserID = null)
        {
            if (string.IsNullOrEmpty(mgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");
            if (string.IsNullOrEmpty(FilePath)) throw new PopbillException(-99999999, "파일경로가 입력되지 않았습니다.");

            List<UploadFile> files = new List<UploadFile>();

            UploadFile file = new UploadFile();

            file.FieldName = "Filedata";
            file.FileName = System.IO.Path.GetFileName(FilePath);
            file.FileData = new FileStream(FilePath, FileMode.Open, FileAccess.Read);

            files.Add(file);

            return httppostFile<Response>("/Statement/" + itemCode.ToString() + "/" + mgtKey + "/Files", CorpNum, null,
                files, null, UserID);
        }

        //첨부파일 삭제
        public Response DeleteFile(string CorpNum, int itemCode, string MgtKey, string FileID, string UserID = null)
        {
            if (string.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");
            if (string.IsNullOrEmpty(FileID)) throw new PopbillException(-99999999, "파일 아이디가 입력되지 않았습니다.");

            return httppost<Response>("/Statement/" + itemCode.ToString() + "/" + MgtKey + "/Files/" + FileID, CorpNum,
                null, "DELETE", null, UserID);
        }

        //첨부파일 목록 확인
        public List<AttachedFile> GetFiles(string CorpNum, int itemCode, string MgtKey, string UserID = null)
        {
            if (string.IsNullOrEmpty(MgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            return httpget<List<AttachedFile>>("/Statement/" + itemCode.ToString() + "/" + MgtKey + "/Files", CorpNum,
                UserID);
        }

        //메일 전송
        public Response SendEmail(string CorpNum, int itemCode, string mgtKey, string Receiver, string UserID = null)
        {
            if (string.IsNullOrEmpty(mgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            ResendRequest request = new ResendRequest();

            request.receiver = Receiver;

            string PostData = toJsonString(request);

            return httppost<Response>("/Statement/" + itemCode.ToString() + "/" + mgtKey, CorpNum, PostData, "EMAIL",
                null, UserID);
        }

        //문자 전송
        public Response SendSMS(string CorpNum, int ItemCode, string mgtKey, string Sender, string Receiver,
            string Contents, string UserID = null)
        {
            if (string.IsNullOrEmpty(mgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            ResendRequest request = new ResendRequest();

            request.sender = Sender;
            request.receiver = Receiver;
            request.contents = Contents;

            string PostData = toJsonString(request);

            return httppost<Response>("/Statement/" + ItemCode.ToString() + "/" + mgtKey, CorpNum, PostData, "SMS",
                null, UserID);
        }

        //팩스 전송
        public Response SendFAX(string CorpNum, int itemCode, string mgtKey, string Sender, string Receiver,
            string UserID = null)
        {
            if (string.IsNullOrEmpty(mgtKey)) throw new PopbillException(-99999999, "문서번호가 입력되지 않았습니다.");

            ResendRequest request = new ResendRequest();

            request.sender = Sender;
            request.receiver = Receiver;

            string PostData = toJsonString(request);

            return httppost<Response>("/Statement/" + itemCode.ToString() + "/" + mgtKey, CorpNum, PostData, "FAX",
                null, UserID);
        }

        //선팩스 전송
        public string FAXSend(string CorpNum, Statement statement, string SendNum, string ReceiveNum,
            string UserID = null)
        {
            if (statement == null) throw new PopbillException(-99999999, "명세서 정보가 입력되지 않았습니다.");
            if (string.IsNullOrEmpty(SendNum)) throw new PopbillException(-99999999, "발신번호가 입력되지 않았습니다.");
            if (string.IsNullOrEmpty(ReceiveNum)) throw new PopbillException(-99999999, "수신번호가 입력되지 않았습니다.");

            statement.sendNum = SendNum;
            statement.receiveNum = ReceiveNum;

            string PostData = toJsonString(statement);
            ReceiptResponse response = null;

            response = httppost<ReceiptResponse>("/Statement", CorpNum, PostData, "FAX", null, UserID);

            return response.receiptNum;
        }

        //전자명세서 첨부
        public Response AttachStatement(string CorpNum, int itemCode, string mgtKey, int SubItemCode, string SubMgtKey,
            string UserID = null)
        {
            string uri = "/Statement/" + itemCode.ToString() + "/" + mgtKey + "/AttachStmt";

            DocRequest request = new DocRequest();
            request.ItemCode = SubItemCode;
            request.MgtKey = SubMgtKey;

            string PostData = toJsonString(request);

            return httppost<Response>(uri, CorpNum, PostData, null, null, UserID);
        }

        //전자명세서 첨부해제
        public Response DetachStatement(string CorpNum, int itemCode, string mgtKey, int SubItemCode, string SubMgtKey,
            string UserID = null)
        {
            string uri = "/Statement/" + itemCode.ToString() + "/" + mgtKey + "/DetachStmt";

            DocRequest request = new DocRequest();
            request.ItemCode = SubItemCode;
            request.MgtKey = SubMgtKey;

            string PostData = toJsonString(request);

            return httppost<Response>(uri, CorpNum, PostData, null, null, UserID);
        }

        //알림메일 전송목록 조회
        public List<EmailConfig> ListEmailConfig(string CorpNum, string UserID = null)
        {
            return httpget<List<EmailConfig>>("/Statement/EmailSendConfig", CorpNum, UserID);
        }

        //알림메일 전송설정 수정
        public Response UpdateEmailConfig(string CorpNum, string EmailType, bool SendYN, string UserID = null)
        {
            if (string.IsNullOrEmpty(EmailType)) throw new PopbillException(-99999999, "메일전송 타입이 입력되지 않았습니다.");

            string uri = "/Statement/EmailSendConfig?EmailType=" + EmailType + "&SendYN=" + SendYN;

            return httppost<Response>(uri, CorpNum, UserID, null, null);
        }

        #endregion

        #region Point API

        //발행단가 확인
        public Single GetUnitCost(string CorpNum, int itemCode, string UserID = null)
        {
            UnitCostResponse response =
                httpget<UnitCostResponse>("/Statement/" + itemCode.ToString() + "?cfg=UNITCOST", CorpNum, UserID);
            return response.unitCost;
        }

        //과금정보 확인
        public ChargeInfo GetChargeInfo(string CorpNum, int itemCode, string UserID = null)
        {
            ChargeInfo response = httpget<ChargeInfo>("/Statement/ChargeInfo/" + itemCode.ToString(), CorpNum, UserID);

            return response;
        }

        #endregion

        [DataContract]
        private class MemoRequest
        {
            [DataMember] public string memo;
            [DataMember] public string emailSubject;
        }

        [DataContract]
        private class ResendRequest
        {
            [DataMember] public string receiver;
            [DataMember] public string sender = null;
            [DataMember] public string contents = null;
        }

        [DataContract]
        public class ReceiptResponse
        {
            [DataMember] public string receiptNum;
        }

        [DataContract]
        private class DocRequest
        {
            [DataMember] public int ItemCode;
            [DataMember] public string MgtKey;
        }
    }
}