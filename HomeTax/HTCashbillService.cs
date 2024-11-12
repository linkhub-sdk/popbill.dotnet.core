using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace Popbill.HomeTax
{
    public class HTCashbillService : BaseService
    {
        public HTCashbillService(string LinkID, string SecretKey) : base(LinkID, SecretKey)
        {
            this.AddScope("141");
        }

        #region Job API

        //수집요청
        public string RequestJob(string CorpNum, KeyType cbType, string SDate, string EDate, string UserID = null)
        {
            if (string.IsNullOrEmpty(SDate)) throw new PopbillException(-99999999, "시작일자가 입력되지 않았습니다.");
            if (string.IsNullOrEmpty(EDate)) throw new PopbillException(-99999999, "종료일자가 입력되지 않았습니다.");

            string uri = "/HomeTax/Cashbill/" + cbType.ToString();
            uri += "?SDate=" + SDate;
            uri += "&EDate=" + EDate;

            JobIDResponse response;

            response = httppost<JobIDResponse>(uri, CorpNum, null, null, null, UserID);

            return response.jobID;
        }

        //수집 상태 확인
        public HTCashbillJobState GetJobState(string CorpNum, string JobID, string UserID = null)
        {
            if (JobID.Length != 18) throw new PopbillException(-99999999, "작업아이디(jobID)가 올바르지 않습니다.");

            return httpget<HTCashbillJobState>("/HomeTax/Cashbill/" + JobID + "/State", CorpNum, UserID);
        }

        //수집 상태 목록 확인
        public List<HTCashbillJobState> ListActiveJob(string CorpNum, string UserID = null)
        {
            return httpget<List<HTCashbillJobState>>("/HomeTax/Cashbill/JobList", CorpNum, UserID);
        }

        #endregion

        #region Search API

        //수집 결과 조회
        public HTCashbillSearch Search(string CorpNum, string JobID, string[] TradeType = null,
            string[] TradeUsage = null, int? Page = null, int? PerPage = null, string Order = null,
            string UserID = null)
        {
            if (JobID.Length != 18) throw new PopbillException(-99999999, "작업아이디(jobID)가 올바르지 않습니다.");

            string uri = "/HomeTax/Cashbill/" + JobID;
            if(TradeType != null)  uri += "?TradeType=" + string.Join(",", TradeType);
            if(TradeUsage != null) uri += "&TradeUsage=" + string.Join(",", TradeUsage);
            if(Page != null)  uri += "&Page=" + Page.ToString();
            if(PerPage != null)  uri += "&PerPage=" + PerPage.ToString();
            if(Order == "D" || Order == "A") uri += "&Order=" + Order;

            return httpget<HTCashbillSearch>(uri, CorpNum, UserID);
        }

        //수집 결과 요약정보 조회
        public HTCashbillSummary Summary(string CorpNum, string JobID, string[] TradeType = null,
            string[] TradeUsage = null, string UserID = null)
        {
            if (JobID.Length != 18) throw new PopbillException(-99999999, "작업아이디(jobID)가 올바르지 않습니다.");

            string uri = "/HomeTax/Cashbill/" + JobID + "/Summary";
            if (TradeType != null) uri += "?TradeType=" + string.Join(",", TradeType);
            if (TradeUsage != null) uri += "&TradeUsage=" + string.Join(",", TradeUsage);

            return httpget<HTCashbillSummary>(uri, CorpNum, UserID);
        }

        #endregion

        #region Certificate API

        //홈택스수집 인증 관리 팝업 URL
        public string GetCertificatePopUpURL(string CorpNum, string UserID = null)
        {
            URLResponse response = httpget<URLResponse>("/HomeTax/Cashbill?TG=CERT", CorpNum, UserID);

            return response.url;
        }

        //홈택스수집 공인인증서 만료일자 확인
        public DateTime GetCertificateExpireDate(string CorpNum, string UserID = null)
        {
            CertResponse response = httpget<CertResponse>("/HomeTax/Cashbill/CertInfo", CorpNum, UserID);

            return DateTime.ParseExact(response.certificateExpiration, "yyyyMMddHHmmss", null);
        }

        //홈택스 공인인증서 로그인 테스트
        public Response CheckCertValidation(string CorpNum, string UserID = null)
        {
            if (string.IsNullOrEmpty(CorpNum)) throw new PopbillException(-99999999, "연동회원 사업자번호가 입력되지 않았습니다.");

            return httpget<Response>("/HomeTax/Cashbill/CertCheck", CorpNum, UserID);
        }

        //부서사용자 계정등록
        public Response RegistDeptUser(string CorpNum, string deptUserID, string deptUserPWD, string UserID = null)
        {
            if (string.IsNullOrEmpty(CorpNum)) throw new PopbillException(-99999999, "연동회원 사업자번호가 입력되지 않았습니다.");
            if (string.IsNullOrEmpty(deptUserID)) throw new PopbillException(-99999999, "홈택스 부서사용자 계정 아이디가 입력되지 않았습니다.");
            if (string.IsNullOrEmpty(deptUserPWD)) throw new PopbillException(-99999999, "홈택스 부서사용자 계정 비밀번호가 입력되지 않았습니다.");

            RegistDeptUserRequest request = new RegistDeptUserRequest();

            request.id = deptUserID;
            request.pwd = deptUserPWD;

            string PostData = toJsonString(request);


            return httppost<Response>("/HomeTax/Cashbill/DeptUser", CorpNum, PostData, null, null, UserID);
        }

        //부서사용자 등록정보 확인
        public Response CheckDeptUser(string CorpNum, string UserID = null)
        {
            if (string.IsNullOrEmpty(CorpNum)) throw new PopbillException(-99999999, "연동회원 사업자번호가 입력되지 않았습니다.");

            return httpget<Response>("/HomeTax/Cashbill/DeptUser", CorpNum, UserID);
        }

        //부서사용자 로그인 테스트
        public Response CheckLoginDeptUser(string CorpNum, string UserID = null)
        {
            if (string.IsNullOrEmpty(CorpNum)) throw new PopbillException(-99999999, "연동회원 사업자번호가 입력되지 않았습니다.");

            return httpget<Response>("/HomeTax/Cashbill/DeptUser/Check", CorpNum, UserID);
        }

        //부서사용자 등록정보 삭제
        public Response DeleteDeptUser(string CorpNum, string UserID = null)
        {
            if (string.IsNullOrEmpty(CorpNum)) throw new PopbillException(-99999999, "연동회원 사업자번호가 입력되지 않았습니다.");

            return httppost<Response>("/HomeTax/Cashbill/DeptUser", CorpNum, null, "DELETE", null, UserID);
        }

        #endregion

        #region Point API

        //과금정보 확인
        public ChargeInfo GetChargeInfo(string CorpNum, string UserID = null)
        {
            ChargeInfo response = httpget<ChargeInfo>("/HomeTax/Cashbill/ChargeInfo", CorpNum, UserID);

            return response;
        }

        //정액제 서비스 신청 URL
        public string GetFlatRatePopUpURL(string CorpNum, string UserID = null)
        {
            URLResponse response = httpget<URLResponse>("/HomeTax/Cashbill?TG=CHRG", CorpNum, UserID);

            return response.url;
        }

        //정액제 서비스 상태 확인
        public HTFlatRate GetFlatRateState(string CorpNum, string UserID = null)
        {
            return httpget<HTFlatRate>("/HomeTax/Cashbill/Contract", CorpNum, UserID);
        }

        #endregion

        [DataContract]
        public class JobIDResponse
        {
            [DataMember] public string jobID;
        }

        [DataContract]
        public class CertResponse
        {
            [DataMember] public string certificateExpiration;
        }

        [DataContract]
        public class RegistDeptUserRequest
        {
            [DataMember] public string id;
            [DataMember] public string pwd;
        }
    }
}