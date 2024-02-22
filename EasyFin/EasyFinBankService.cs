using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Runtime.Serialization;


namespace Popbill.EasyFin
{
    public class EasyFinBankService : BaseService
    {
        public EasyFinBankService(string LinkID, string SecretKey)
            : base(LinkID, SecretKey)
        {
            this.AddScope("180");
        }

        public ChargeInfo GetChargeInfo(string CorpNum, string UserID = null)
        {
            return httpget<ChargeInfo>("/EasyFin/Bank/ChargeInfo", CorpNum, UserID);
        }

        public Response RegistBankAccount(string CorpNum, EasyFinBankAccountForm info, string UserID = null)
        {
            if (info == null) throw new PopbillException(-99999999, "은행 계좌정보가 입력되지 않았습니다.");
            if (string.IsNullOrEmpty(info.BankCode)) throw new PopbillException(-99999999, "기관코드가 입력되지 않았습니다.");
            if (info.BankCode.Length != 4) throw new PopbillException(-99999999, "기관코드가 올바르지 않습니다.");
            if (string.IsNullOrEmpty(info.AccountNumber)) throw new PopbillException(-99999999, "은행 계좌번호가 입력되지 않았습니다.");
            if (string.IsNullOrEmpty(info.AccountPWD )) throw new PopbillException(-99999999, "계좌 비밀번호가 입력되지 않았습니다.");
            if (string.IsNullOrEmpty(info.AccountType)) throw new PopbillException(-99999999, "계좌유형이 입력되지 않았습니다.");
            if (info.AccountType != "개인" && info.AccountType != "법인") throw new PopbillException(-99999999, "계좌유형이 올바르지 않습니다.");
            if (string.IsNullOrEmpty(info.IdentityNumber)) throw new PopbillException(-99999999, "예금주 식별정보가 입력되지 않았습니다.");

            string PostData = toJsonString(info);

            string uri = "/EasyFin/Bank/BankAccount/Regist";

            if (!string.IsNullOrEmpty(info.UsePeriod)) uri += "?UsePeriod=" + info.UsePeriod;

            return httppost<Response>(uri, CorpNum, PostData, null, null, UserID);
            
        }

        public Response UpdateBankAccount(string CorpNum, EasyFinBankAccountForm info, string UserID = null)
        {
            if (info == null) throw new PopbillException(-99999999, "은행 계좌정보가 입력되지 않았습니다.");
            if (string.IsNullOrEmpty(info.BankCode)) throw new PopbillException(-99999999, "기관코드가 입력되지 않았습니다.");
            if (info.BankCode.Length != 4) throw new PopbillException(-99999999, "기관코드가 올바르지 않습니다.");
            if (string.IsNullOrEmpty(info.AccountNumber)) throw new PopbillException(-99999999, "은행 계좌번호가 입력되지 않았습니다.");
            if (string.IsNullOrEmpty(info.AccountPWD)) throw new PopbillException(-99999999, "계좌 비밀번호가 입력되지 않았습니다.");

            string uri = "/EasyFin/Bank/BankAccount/" + info.BankCode + "/" + info.AccountNumber + "/Update";

            string PostData = toJsonString(info);

            return httppost<Response>(uri, CorpNum, PostData, null, null, UserID);
        }

        public Response UpdateBankAccount(string CorpNum, string BankCode, string AccountNumber, UpdateEasyFinBankAccountForm info, string UserID = null)
        {
            if (info == null) throw new PopbillException(-99999999, "은행 계좌정보가 입력되지 않았습니다.");
            if (string.IsNullOrEmpty(BankCode)) throw new PopbillException(-99999999, "기관코드가 입력되지 않았습니다.");
            if (BankCode.Length != 4) throw new PopbillException(-99999999, "기관코드가 올바르지 않습니다.");
            if (string.IsNullOrEmpty(AccountNumber)) throw new PopbillException(-99999999, "은행 계좌번호가 입력되지 않았습니다.");
            if (string.IsNullOrEmpty(info.AccountPWD)) throw new PopbillException(-99999999, "계좌 비밀번호가 입력되지 않았습니다.");

            string uri = "/EasyFin/Bank/BankAccount/" + BankCode + "/" + AccountNumber + "/Update";

            string PostData = toJsonString(info);

            return httppost<Response>(uri, CorpNum, PostData, null, null, UserID);
        }

        public Response RevokeCloseBankAccount(string CorpNum, string BankCode, string AccountNumber, string UserID = null)
        {
            if (string.IsNullOrEmpty(BankCode)) throw new PopbillException(-99999999, "기관코드가 입력되지 않았습니다.");
            if (BankCode.Length != 4) throw new PopbillException(-99999999, "기관코드가 올바르지 않습니다.");
            if (string.IsNullOrEmpty(AccountNumber)) throw new PopbillException(-99999999, "은행 계좌번호가 입력되지 않았습니다.");

            string uri = "/EasyFin/Bank/BankAccount/RevokeClose";
            uri += "?BankCode=" + BankCode;
            uri += "&AccountNumber=" + AccountNumber;

            return httppost<Response>(uri, CorpNum, null, null, null, UserID);
        }

        public Response CloseBankAccount(string CorpNum, string BankCode, string AccountNumber, string CloseType, string UserID = null)
        {
            if (string.IsNullOrEmpty(BankCode)) throw new PopbillException(-99999999, "기관코드가 입력되지 않았습니다.");
            if (BankCode.Length != 4) throw new PopbillException(-99999999, "기관코드가 올바르지 않습니다.");
            if (string.IsNullOrEmpty(AccountNumber)) throw new PopbillException(-99999999, "은행 계좌번호가 입력되지 않았습니다.");
            if (string.IsNullOrEmpty(CloseType)) throw new PopbillException(-99999999, "정액제 해지유형이 입력되지 않았습니다.");
            if (CloseType != "중도" && CloseType != "일반") throw new PopbillException(-99999999, "정액제 해지유형이 올바르지 않습니다.");

            string uri = "/EasyFin/Bank/BankAccount/Close";
            uri += "?BankCode=" + BankCode;
            uri += "&AccountNumber=" + AccountNumber;
            uri += "&CloseType=" + CloseType;

            return httppost<Response>(uri, CorpNum, null, null, null, UserID);
        }

        // 종량제 계좌 삭제
        public Response DeleteBankAccount(string CorpNum, string BankCode, string AccountNumber, string UserID = null)
        {
            if (string.IsNullOrEmpty(BankCode)) throw new PopbillException(-99999999, "기관코드가 입력되지 않았습니다.");
            if (BankCode.Length != 4) throw new PopbillException(-99999999, "기관코드가 올바르지 않습니다.");
            if (string.IsNullOrEmpty(AccountNumber)) throw new PopbillException(-99999999, "은행 계좌번호가 입력되지 않았습니다.");

            string PostData = "{'BankCode':" + BankCode + ", 'AccountNumber':" + AccountNumber + "}";

            string uri = "/EasyFin/Bank/BankAccount/Delete";
            
           return  httppost<Response>(uri, CorpNum, PostData, null, null, UserID);
        }

        public EasyFinBankAccount GetBankAccountInfo(string CorpNum, string BankCode, string AccountNumber, string UserID = null)
        {
            if (string.IsNullOrEmpty(BankCode)) throw new PopbillException(-99999999, "기관코드가 입력되지 않았습니다.");
            if (BankCode.Length != 4) throw new PopbillException(-99999999, "기관코드가 올바르지 않습니다.");
            if (string.IsNullOrEmpty(AccountNumber)) throw new PopbillException(-99999999, "은행 계좌번호가 입력되지 않았습니다.");

            string uri = "/EasyFin/Bank/BankAccount/" + BankCode + "/" + AccountNumber;

            return httpget<EasyFinBankAccount>(uri, CorpNum, UserID);
        }

        public string GetBankAccountMgtURL(string CorpNum, string UserID = null)
        {
            URLResponse response = httpget<URLResponse>("/EasyFin/Bank?TG=BankAccount", CorpNum, UserID);

            return response.url;
        }

        public List<EasyFinBankAccount> ListBankAccount(string CorpNum, string UserID = null)
        {
            return httpget<List<EasyFinBankAccount>>("/EasyFin/Bank/ListBankAccount", CorpNum, UserID);
        }

        public string RequestJob(string CorpNum, string BankCode, string AccountNumber, string SDate, string EDate, string UserID = null)
        {
            if (string.IsNullOrEmpty(BankCode)) throw new PopbillException(-99999999, "기관코드가 입력되지 않았습니다.");
            if (string.IsNullOrEmpty(AccountNumber)) throw new PopbillException(-99999999, "은행 계좌번호가 입력되지 않았습니다.");
            if (string.IsNullOrEmpty(SDate)) throw new PopbillException(-99999999, "거래내역 조회 시작일자가 입력되지 않았습니다.");
            if (string.IsNullOrEmpty(EDate)) throw new PopbillException(-99999999, "거래내역 조회 종료일자가 입력되지 않았습니다.");


            string uri = "/EasyFin/Bank/BankAccount";
            uri += "?BankCode=" + BankCode;
            uri += "&AccountNumber=" + AccountNumber;
            uri += "&SDate=" + SDate;
            uri += "&EDate=" + EDate;

            return httppost<JobIDResponse>(uri, CorpNum, null, null, null, UserID).jobID;
        }

        public EasyFinBankJobState GetJobState(string CorpNum, string JobID, string UserID = null)
        {
            if (string.IsNullOrEmpty(JobID)) throw new PopbillException(-99999999, "작업아이디가 입력되지 않았습니다.");

            return httpget<EasyFinBankJobState>("/EasyFin/Bank/" + JobID + "/State", CorpNum, UserID);
        }

        public List<EasyFinBankJobState> ListActiveJob(string CorpNum, string UserID = null)
        {
            return httpget<List<EasyFinBankJobState>>("/EasyFin/Bank/JobList", CorpNum, UserID);
        }

        public EasyFinBankSearchResult Search(string CorpNum, string JobID, string[] TradeType = null, string SearchString = null, int? Page = null, int? PerPage = null, string Order = null, string UserID = null)
        {
            if (string.IsNullOrEmpty(JobID)) throw new PopbillException(-99999999, "작업아이디가 입력되지 않았습니다.");

            string uri = "/EasyFin/Bank/" + JobID;
            uri += "?TradeType=" + string.Join(",", TradeType);

            if (!string.IsNullOrEmpty(SearchString)) uri += "&SearchString=" + HttpUtility.UrlEncode(SearchString);

            if (Page != null) uri += "&Page=" + Page;
            if (PerPage != null) uri += "&PerPage=" + PerPage;
            if (Order == "D" || Order == "A") uri += "&Order=" + Order;

            return httpget<EasyFinBankSearchResult>(uri, CorpNum, UserID);

        }

        public EasyFinBankSummary Summary(string CorpNum, string JobID, string[] TradeType = null, string SearchString = null, string UserID = null)
        {
            if (string.IsNullOrEmpty(JobID)) throw new PopbillException(-99999999, "작업아이디가 입력되지 않았습니다.");

            string uri = "/EasyFin/Bank/" + JobID + "/Summary";
            uri += "?TradeType=" + string.Join(",", TradeType);

            if (!string.IsNullOrEmpty(SearchString)) uri += "&SearchString=" + HttpUtility.UrlEncode(SearchString);

            return httpget<EasyFinBankSummary>(uri, CorpNum, UserID);
        }

        public Response SaveMemo(string CorpNum, string TID, string Memo, string UserID = null)
        {
            if (string.IsNullOrEmpty(TID)) throw new PopbillException(-99999999, "거래내역 아이디가 입력되지 않았습니다.");
            if (string.IsNullOrEmpty(Memo)) throw new PopbillException(-99999999, "거래내역 메모가 입력되지 않았습니다.");

            string uri = "/EasyFin/Bank/SaveMemo";
            uri += "?TID=" + TID;
            uri += "&Memo=" + HttpUtility.UrlEncode(Memo);

            return httppost<Response>(uri, CorpNum, null, null, null, UserID);
        }

        public string GetFlatRatePopUpURL(string CorpNum, string UserID = null)
        {
            return httpget<URLResponse>("/EasyFin/Bank?TG=CHRG", CorpNum, UserID).url;
        }

        public EasyFinBankFlatRate GetFlatRateState(string CorpNum, string BankCode, string AccountNumber, string UserID = null)
        {
            if (string.IsNullOrEmpty(BankCode)) throw new PopbillException(-99999999, "기관코드가 입력되지 않았습니다.");
            if (string.IsNullOrEmpty(AccountNumber)) throw new PopbillException(-99999999, "은행 계좌번호가 입력되지 않았습니다.");

            string uri = "/EasyFin/Bank/Contract/" + BankCode + "/" + AccountNumber;

            return httpget<EasyFinBankFlatRate>(uri, CorpNum, UserID);
        }

        [DataContract]
        public class JobIDResponse
        {
            [DataMember]
            public string jobID;
        }
    }
}
