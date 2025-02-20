﻿using System;
using System.Web;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Popbill.HomeTax
{
    public class HTTaxinvoiceService : BaseService
    {
        public HTTaxinvoiceService(string LinkID, string SecretKey) : base(LinkID, SecretKey)
        {
            this.AddScope("111");
        }

        #region Job API

        //수집요청
        public string RequestJob(string CorpNum, KeyType tiType, string DType, string SDate, string EDate, string UserID = null)
        {
            if (string.IsNullOrEmpty(DType)) throw new PopbillException(-99999999, "검색일자 유형이 입력되지 않았습니다.");
            if (string.IsNullOrEmpty(SDate)) throw new PopbillException(-99999999, "시작일자가 입력되지 않았습니다.");
            if (string.IsNullOrEmpty(EDate)) throw new PopbillException(-99999999, "종료일자가 입력되지 않았습니다.");

            string uri = "/HomeTax/Taxinvoice/" + tiType.ToString();
            uri += "?DType=" + DType;
            uri += "&SDate=" + SDate;
            uri += "&EDate=" + EDate;

            JobIDResponse response;

            response = httppost<JobIDResponse>(uri, CorpNum, null, null, null, UserID);

            return response.jobID;
        }

        //수집 상태 확인
        public HTTaxinvoiceJobState GetJobState(string CorpNum, string JobID, string UserID = null)
        {
            if (JobID.Length != 18) throw new PopbillException(-99999999, "작업아이디(jobID)가 올바르지 않습니다.");

            return httpget<HTTaxinvoiceJobState>("/HomeTax/Taxinvoice/" + JobID + "/State", CorpNum, UserID);
        }

        //수집 상태 목록 확인
        public List<HTTaxinvoiceJobState> ListActiveJob(string CorpNum, string UserID = null)
        {
            return httpget<List<HTTaxinvoiceJobState>>("/HomeTax/Taxinvoice/JobList", CorpNum, UserID);
        }

        #endregion

        #region Search API

        //수집 결과 조회
        public HTTaxinvoiceSearch Search(string CorpNum, string JobID, string[] Type = null, string[] TaxType = null,
            string[] PurposeType = null, string TaxRegIDYN = null, string TaxRegIDType = null, string TaxRegID = null,
            int? Page = null, int? PerPage = null, string Order = null, string UserID = null, string SearchString = null)
        {
            if (JobID.Length != 18) throw new PopbillException(code: -99999999, Message: "작업아이디(jobID)가 올바르지 않습니다.");

            string uri = "/HomeTax/Taxinvoice/" + JobID + "?Type=";
            if (Type != null) uri += string.Join(",", Type);
            if (TaxType != null) uri += "&TaxType=" + string.Join(",", TaxType);
            if (PurposeType != null) uri += "&PurposeType="  + string.Join(",", PurposeType);
            if (TaxRegIDYN != null && TaxRegIDYN != "") uri += "&TaxRegIDYN=" + TaxRegIDYN;
            if (TaxRegIDType != null && TaxRegIDType != "") uri += "&TaxRegIDType=" + TaxRegIDType;
            if (TaxRegID != null && TaxRegID != "") uri += "&TaxRegID=" + TaxRegID;
            if (Page > 0) uri += "&Page=" + Page.ToString();
            if (PerPage > 0 && PerPage <= 1000) uri += "&PerPage=" + PerPage.ToString();
            if (Order != null && Order != "") uri += "&Order=" + Order;
            if (SearchString != null && SearchString != "") uri += "&SearchString=" + HttpUtility.UrlEncode(SearchString);

            return httpget<HTTaxinvoiceSearch>(uri, CorpNum, UserID);
        }

        //수집 결과 요약정보 조회
        public HTTaxinvoiceSummary Summary(string CorpNum, string JobID, string[] Type = null, string[] TaxType = null,
            string[] PurposeType = null, string TaxRegIDYN = null, string TaxRegIDType = null, string TaxRegID = null,
            string UserID = null, string SearchString = null)
        {
            if (JobID.Length != 18) throw new PopbillException(-99999999, "작업아이디(jobID)가 올바르지 않습니다.");

            string uri = "/HomeTax/Taxinvoice/" + JobID + "/Summary" + "?Type=";
            if (Type != null) uri += string.Join(",", Type);
            if (TaxType != null) uri += "&TaxType=" + string.Join(",", TaxType);
            if (PurposeType != null) uri += "&PurposeType=" + string.Join(",", PurposeType);
            if (TaxRegIDYN != null && TaxRegIDYN != "") uri += "&TaxRegIDYN=" + TaxRegIDYN;
            if (TaxRegIDType != null && TaxRegIDType != "") uri += "&TaxRegIDType=" + TaxRegIDType;
            if (TaxRegID != null && TaxRegID != "") uri += "&TaxRegID=" + TaxRegID;
            if (SearchString != null && SearchString != "") uri += "&SearchString=" + HttpUtility.UrlEncode(SearchString);

            return httpget<HTTaxinvoiceSummary>(uri, CorpNum, UserID);
        }


        //상세정보 확인 - JSON
        public HTTaxinvoice GetTaxinvoice(string CorpNum, string ntsconfirmNum, string UserID = null)
        {
            if (ntsconfirmNum.Length != 24) throw new PopbillException(-99999999, "국세청승인번호가 올바르지 않습니다.");

            return httpget<HTTaxinvoice>("/HomeTax/Taxinvoice/" + ntsconfirmNum, CorpNum, UserID);
        }

        //상세정보 확인 - XML
        public HTTaxinvoiceXML GetXML(string CorpNum, string ntsconfirmNum, string UserID = null)
        {
            if (ntsconfirmNum.Length != 24) throw new PopbillException(-99999999, "국세청승인번호가 올바르지 않습니다.");

            return httpget<HTTaxinvoiceXML>("/HomeTax/Taxinvoice/" + ntsconfirmNum + "?T=xml", CorpNum, UserID);
        }

        //홈택스 전자세금계산서 보기 팝업 URL
        public string GetPopUpURL(string CorpNum, string ntsconfirmNum, string UserID = null)
        {
            if (ntsconfirmNum.Length != 24) throw new PopbillException(-99999999, "국세청승인번호가 올바르지 않습니다.");

            URLResponse response =
                httpget<URLResponse>("/HomeTax/Taxinvoice/" + ntsconfirmNum + "/PopUp", CorpNum, UserID);

            return response.url;
        }

        public string GetPrintURL(string CorpNum, string ntsconfirmNum, string UserID = null)
        {
            if (ntsconfirmNum.Length != 24) throw new PopbillException(-99999999, "국세청승인번호가 올바르지 않습니다.");

            URLResponse response =
                httpget<URLResponse>("/HomeTax/Taxinvoice/" + ntsconfirmNum + "/Print", CorpNum, UserID);

            return response.url;
        }

        #endregion

        #region Certificate API

        //홈택스수집 인증 관리 팝업 URL
        public string GetCertificatePopUpURL(string CorpNum, string UserID = null)
        {
            URLResponse response = httpget<URLResponse>("/HomeTax/Taxinvoice?TG=CERT", CorpNum, UserID);

            return response.url;
        }

        //홈택스수집 공인인증서 만료일자 확인
        public DateTime GetCertificateExpireDate(string CorpNum, string UserID = null)
        {
            CertResponse response = httpget<CertResponse>("/HomeTax/Taxinvoice/CertInfo", CorpNum, UserID);

            return DateTime.ParseExact(response.certificateExpiration, "yyyyMMddHHmmss", null);
        }

        //홈택스 공인인증서 로그인 테스트
        public Response CheckCertValidation(string CorpNum, string UserID = null)
        {
            if (string.IsNullOrEmpty(CorpNum)) throw new PopbillException(-99999999, "연동회원 사업자번호가 입력되지 않았습니다.");

            return httpget<Response>("/HomeTax/Taxinvoice/CertCheck", CorpNum, UserID);
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


            return httppost<Response>("/HomeTax/Taxinvoice/DeptUser", CorpNum, PostData, null, null, UserID);
        }

        //부서사용자 등록정보 확인
        public Response CheckDeptUser(string CorpNum, string UserID = null)
        {
            if (string.IsNullOrEmpty(CorpNum)) throw new PopbillException(-99999999, "연동회원 사업자번호가 입력되지 않았습니다.");

            return httpget<Response>("/HomeTax/Taxinvoice/DeptUser", CorpNum, UserID);
        }

        //부서사용자 로그인 테스트
        public Response CheckLoginDeptUser(string CorpNum, string UserID = null)
        {
            if (string.IsNullOrEmpty(CorpNum)) throw new PopbillException(-99999999, "연동회원 사업자번호가 입력되지 않았습니다.");

            return httpget<Response>("/HomeTax/Taxinvoice/DeptUser/Check", CorpNum, UserID);
        }

        //부서사용자 등록정보 삭제
        public Response DeleteDeptUser(string CorpNum, string UserID = null)
        {
            if (string.IsNullOrEmpty(CorpNum)) throw new PopbillException(-99999999, "연동회원 사업자번호가 입력되지 않았습니다.");

            return httppost<Response>("/HomeTax/Taxinvoice/DeptUser", CorpNum, null, "DELETE", null, UserID);
        }

        #endregion

        #region Point API

        //과금정보 확인
        public ChargeInfo GetChargeInfo(string CorpNum, string UserID = null)
        {
            ChargeInfo response = httpget<ChargeInfo>("/HomeTax/Taxinvoice/ChargeInfo", CorpNum, UserID);

            return response;
        }

        //정액제 서비스 신청 URL
        public string GetFlatRatePopUpURL(string CorpNum, string UserID = null)
        {
            URLResponse response = httpget<URLResponse>("/HomeTax/Taxinvoice?TG=CHRG", CorpNum, UserID);

            return response.url;
        }

        //정액제 서비스 상태 확인
        public HTFlatRate GetFlatRateState(string CorpNum, string UserID = null)
        {
            return httpget<HTFlatRate>("/HomeTax/Taxinvoice/Contract", CorpNum, UserID);
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