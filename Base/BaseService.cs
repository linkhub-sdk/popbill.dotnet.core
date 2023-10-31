using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Security.Cryptography;

using Linkhub;

namespace Popbill
{
    public class BaseService
    {
        private const string ServiceID_REAL = "POPBILL";
        private const string ServiceID_TEST = "POPBILL_TEST";
        private const string ServiceURL_REAL = "https://popbill.linkhub.co.kr";
        private const string ServiceURL_TEST = "https://popbill-test.linkhub.co.kr";

        private const string ServiceURL_REAL_Static = "https://static-popbill.linkhub.co.kr";
        private const string ServiceURL_TEST_Static = "https://static-popbill-test.linkhub.co.kr";

        private const string ServiceURL_REAL_GA = "https://ga-popbill.linkhub.co.kr";
        private const string ServiceURL_TEST_GA = "https://ga-popbill-test.linkhub.co.kr";

        private const string APIVersion = "1.0";
        private const string CRLF = "\r\n";

        private Dictionary<string, Token> _tokenTable = new Dictionary<string, Token>();
        private List<string> _Scopes = new List<string>();
        private bool _IsTest;
        private bool _IPRestrictOnOff;
        private bool _UseStaticIP;
        private bool _UseGAIP;
        private Authority _LinkhubAuth;
        private bool _UseLocalTimeYN;

        private bool _ProxyYN;
        private String _ProxyAddress;
        private String _ProxyUserName;
        private String _ProxyPassword;

        public bool IsTest
        {
            set { _IsTest = value; }
            get { return _IsTest; }
        }
        public bool IPRestrictOnOff
        {
            set { _IPRestrictOnOff = value; }
            get { return _IPRestrictOnOff; }
        }

        public bool UseStaticIP
        {
            set { _UseStaticIP = value; }
            get { return _UseStaticIP; }
        }

        public bool UseGAIP
        {
            set { _UseGAIP = value; }
            get { return _UseGAIP; }
        }

        public bool UseLocalTimeYN
        {
            set { _UseLocalTimeYN = value;  }
            get { return _UseLocalTimeYN;  }
        }

        public BaseService(string LinkID, string SecretKey)
        {
            _LinkhubAuth = new Authority(LinkID, SecretKey);
            _Scopes.Add("member");
            _IPRestrictOnOff = true;
            _ProxyYN = false;
        }

        public BaseService(string LinkID, string SecretKey, bool ProxyYN, string ProxyAddress, string ProxyUserName, string ProxyPassword)
        {
            _LinkhubAuth = new Authority(LinkID, SecretKey, ProxyYN, ProxyAddress, ProxyUserName, ProxyPassword);
            _Scopes.Add("member");
            _IPRestrictOnOff = true;
            _ProxyYN = ProxyYN;
            _ProxyAddress = ProxyAddress;
            _ProxyUserName = ProxyUserName;
            _ProxyPassword = ProxyPassword;
        }

        public void AddScope(string scope)
        {
            _Scopes.Add(scope);
        }

        protected string ServiceID
        {
            get { return _IsTest ? ServiceID_TEST : ServiceID_REAL; }
        }

        protected string ServiceURL
        {
            get {
                if(UseGAIP)
                {
                    return _IsTest ? ServiceURL_TEST_GA : ServiceURL_REAL_GA;
                } else if (UseStaticIP)
                {
                    return _IsTest ? ServiceURL_TEST_Static : ServiceURL_REAL_Static;
                } else
                {
                    return _IsTest ? ServiceURL_TEST : ServiceURL_REAL;
                }

            }
        }

        protected string toJsonString(Object graph)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(graph.GetType());
                ser.WriteObject(ms, graph);
                ms.Seek(0, SeekOrigin.Begin);
                return new StreamReader(ms).ReadToEnd();
            }
        }

        protected T fromJson<T>(Stream jsonStream)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            return (T) ser.ReadObject(jsonStream);
        }

        //session_token
        private string getSession_Token(string CorpNum)
        {
            Token _token = null;

            if (_tokenTable.ContainsKey(CorpNum)) _token = _tokenTable[CorpNum];

            bool expired = true;

            if (_token != null)
            {
                DateTime now = DateTime.Parse(_LinkhubAuth.getTime(UseStaticIP, UseLocalTimeYN, UseGAIP));

                DateTime expiration = DateTime.Parse(_token.expiration);

                expired = expiration < now;
            }

            if (expired)
            {
                try
                {
                    if (_IPRestrictOnOff)
                    {
                        _token = _LinkhubAuth.getToken(ServiceID, CorpNum, _Scopes, null, UseStaticIP, UseLocalTimeYN, UseGAIP);
                    }
                    else
                    {
                        _token = _LinkhubAuth.getToken(ServiceID, CorpNum, _Scopes,"*", UseStaticIP, UseLocalTimeYN, UseGAIP);
                    }


                    if (_tokenTable.ContainsKey(CorpNum))
                    {
                        _tokenTable.Remove(CorpNum);
                    }

                    _tokenTable.Add(CorpNum, _token);
                }
                catch (LinkhubException le)
                {
                    throw new PopbillException(le);
                }
            }

            return _token.session_token;
        }

        #region http

        protected T httpget<T>(string url, string CorpNum, string UserID = null)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(ServiceURL + url);

            if (this._ProxyYN == true)
            {
                WebProxy proxyRequest = new WebProxy();

                Uri proxyURI = new Uri(this._ProxyAddress);
                proxyRequest.Address = proxyURI;
                proxyRequest.Credentials = new NetworkCredential(this._ProxyUserName, this._ProxyPassword);
                request.Proxy = proxyRequest;
            }

            if (string.IsNullOrEmpty(CorpNum) == false)
            {
                string bearerToken = getSession_Token(CorpNum);
                request.Headers.Add("Authorization", "Bearer" + " " + bearerToken);
            }

            request.Headers.Add("User-Agent", "DOTNETCORE POPBILL SDK");

            request.Headers.Add("x-lh-version", APIVersion);

            request.Headers.Add("Accept-Encoding", "gzip, deflate");

            request.AutomaticDecompression = DecompressionMethods.GZip;

            if (string.IsNullOrEmpty(UserID) == false)
            {
                request.Headers.Add("x-pb-userid", UserID);
            }

            request.Method = "GET";

            try
            {
                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                {
                    using (Stream stReadData = response.GetResponseStream())
                    {
                        return fromJson<T>(stReadData);
                    }
                }
            }
            catch (Exception we)
            {
                if (we is WebException && ((WebException) we).Response != null)
                {
                    using (Stream stReadData = ((WebException) we).Response.GetResponseStream())
                    {
                        Response t = fromJson<Response>(stReadData);
                        throw new PopbillException(t.code, t.message);
                    }
                }

                throw new PopbillException(-99999999, we.Message);
            }
        }
        protected T httpBulkPost<T>(string url, string CorpNum, string SubmitID, string PostData, string UserID, string Action)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ServiceURL + url);

            if (this._ProxyYN == true)
            {
                WebProxy proxyRequest = new WebProxy();

                Uri proxyURI = new Uri(this._ProxyAddress);
                proxyRequest.Address = proxyURI;
                proxyRequest.Credentials = new NetworkCredential(this._ProxyUserName, this._ProxyPassword);
                request.Proxy = proxyRequest;
            }

            request.ContentType = "application/json;";

            if (string.IsNullOrEmpty(CorpNum) == false)
            {
                string bearerToken = getSession_Token(CorpNum);
                request.Headers.Add("Authorization", "Bearer" + " " + bearerToken);
            }

            request.Headers.Add("User-Agent", "DOTNETCORE POPBILL SDK");

            request.Headers.Add("x-pb-version", APIVersion);

            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.AutomaticDecompression = DecompressionMethods.GZip;

            request.Headers.Add("x-pb-message-digest", Convert.ToBase64String(SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(PostData))));
            request.Headers.Add("x-pb-submit-id", SubmitID);

            if (string.IsNullOrEmpty(UserID) == false)
            {
                request.Headers.Add("x-pb-userid", UserID);
            }

            if (string.IsNullOrEmpty(Action) == false)
            {
                request.Headers.Add("X-HTTP-Method-Override", Action);
            }

            request.Method = "POST";

            if (string.IsNullOrEmpty(PostData)) PostData = "";

            byte[] btPostDAta = Encoding.UTF8.GetBytes(PostData);

            request.ContentLength = btPostDAta.Length;

            request.GetRequestStream().Write(btPostDAta, 0, btPostDAta.Length);

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stReadData = response.GetResponseStream())
                    {
                        return fromJson<T>(stReadData);
                    }
                }
            }
            catch (Exception we)
            {
                if (we is WebException && ((WebException)we).Response != null)
                {
                    using (Stream stReadData = ((WebException)we).Response.GetResponseStream())
                    {
                        Response t = fromJson<Response>(stReadData);
                        throw new PopbillException(t.code, t.message);
                    }
                }

                throw new PopbillException(-99999999, we.Message);
            }
        }
        protected T httppost<T>(string url, string CorpNum, string PostData, string httpMethod = null,
            string contentsType = null, string UserID = null)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(ServiceURL + url);

            if (this._ProxyYN == true)
            {
                WebProxy proxyRequest = new WebProxy();

                Uri proxyURI = new Uri(this._ProxyAddress);
                proxyRequest.Address = proxyURI;
                proxyRequest.Credentials = new NetworkCredential(this._ProxyUserName, this._ProxyPassword);
                request.Proxy = proxyRequest;
            }

            if (contentsType == null)
            {
                request.ContentType = "application/json;";
            }
            else
            {
                request.ContentType = contentsType;
            }


            if (string.IsNullOrEmpty(CorpNum) == false)
            {
                string bearerToken = getSession_Token(CorpNum);
                request.Headers.Add("Authorization", "Bearer" + " " + bearerToken);
            }

            request.Headers.Add("User-Agent", "DOTNETCORE POPBILL SDK");

            request.Headers.Add("x-lh-version", APIVersion);

            request.Headers.Add("Accept-Encoding", "gzip, deflate");

            request.AutomaticDecompression = DecompressionMethods.GZip;

            if (string.IsNullOrEmpty(UserID) == false)
            {
                request.Headers.Add("x-pb-userid", UserID);
            }

            if (string.IsNullOrEmpty(httpMethod) == false)
            {
                request.Headers.Add("X-HTTP-Method-Override", httpMethod);
            }

            request.Method = "POST";

            if (string.IsNullOrEmpty(PostData)) PostData = "";

            byte[] btPostDAta = Encoding.UTF8.GetBytes(PostData);

            request.ContentLength = btPostDAta.Length;

            request.GetRequestStream().Write(btPostDAta, 0, btPostDAta.Length);

            try
            {
                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                {
                    using (Stream stReadData = response.GetResponseStream())
                    {
                        return fromJson<T>(stReadData);
                    }
                }
            }
            catch (Exception we)
            {
                if (we is WebException && ((WebException) we).Response != null)
                {
                    using (Stream stReadData = ((WebException) we).Response.GetResponseStream())
                    {
                        Response t = fromJson<Response>(stReadData);
                        throw new PopbillException(t.code, t.message);
                    }
                }

                throw new PopbillException(-99999999, we.Message);
            }
        }

        protected T httppostFile<T>(string url, string CorpNum, string form, List<UploadFile> UploadFiles,
            string httpMethod, string UserID = null)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(ServiceURL + url);

            if (this._ProxyYN == true)
            {
                WebProxy proxyRequest = new WebProxy();

                Uri proxyURI = new Uri(this._ProxyAddress);
                proxyRequest.Address = proxyURI;
                proxyRequest.Credentials = new NetworkCredential(this._ProxyUserName, this._ProxyPassword);
                request.Proxy = proxyRequest;
            }

            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");

            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.KeepAlive = true;
            request.Method = "POST";

            if (string.IsNullOrEmpty(CorpNum) == false)
            {
                string bearerToken = getSession_Token(CorpNum);
                request.Headers.Add("Authorization", "Bearer" + " " + bearerToken);
            }

            request.Headers.Add("User-Agent", "DOTNETCORE POPBILL SDK");

            request.Headers.Add("x-lh-version", APIVersion);

            request.Headers.Add("Accept-Encoding", "gzip, deflate");

            request.AutomaticDecompression = DecompressionMethods.GZip;

            if (string.IsNullOrEmpty(UserID) == false)
            {
                request.Headers.Add("x-pb-userid", UserID);
            }

            if (string.IsNullOrEmpty(httpMethod) == false)
            {
                request.Headers.Add("X-HTTP-Method-Override", httpMethod);
            }

            Stream wstream = request.GetRequestStream();


            if (string.IsNullOrEmpty(form) == false)
            {
                string formBody = "--" + boundary + CRLF;
                formBody += "content-disposition: form-data; name=\"form\"" + CRLF;
                formBody += "content-type: Application/json" + CRLF + CRLF;
                formBody += form;
                byte[] btFormBody = Encoding.UTF8.GetBytes(formBody);

                wstream.Write(btFormBody, 0, btFormBody.Length);
            }

            foreach (UploadFile f in UploadFiles)
            {
                string fileHeader = CRLF + "--" + boundary + CRLF;
                fileHeader += "content-disposition: form-data; name=\"" + f.FieldName + "\"; filename=\"" + f.FileName +
                              "\"" + CRLF;
                fileHeader += "content-type: Application/octet-stream" + CRLF + CRLF;

                byte[] btFileHeader = Encoding.UTF8.GetBytes(fileHeader);

                wstream.Write(btFileHeader, 0, btFileHeader.Length);

                byte[] buffer = new byte[32768];
                int read;
                while ((read = f.FileData.Read(buffer, 0, buffer.Length)) > 0)
                {
                    wstream.Write(buffer, 0, read);
                }

                f.FileData.Close();
            }

            string boundaryFooter = CRLF + "--" + boundary + "--" + CRLF;
            byte[] btboundaryFooter = Encoding.UTF8.GetBytes(boundaryFooter);

            wstream.Write(btboundaryFooter, 0, btboundaryFooter.Length);

            wstream.Close();
            try
            {
                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                {
                    using (Stream stReadData = response.GetResponseStream())
                    {
                        return fromJson<T>(stReadData);
                    }
                }
            }
            catch (Exception we)
            {
                if (we is WebException && ((WebException) we).Response != null)
                {
                    using (Stream stReadData = ((WebException) we).Response.GetResponseStream())
                    {
                        Response t = fromJson<Response>(stReadData);
                        throw new PopbillException(t.code, t.message);
                    }
                }

                throw new PopbillException(-99999999, we.Message);
            }
        }

        #endregion

        #region Popbill API

        //팝빌 로그인
        public string GetAccessURL(string CorpNum, string UserID)
        {
            try
            {
                URLResponse response = httpget<URLResponse>("/?TG=LOGIN", CorpNum, UserID);

                return response.url;
            }
            catch (LinkhubException le)
            {
                throw new PopbillException(le);
            }
        }

        //연동회원 포인트충전 팝업 URL
        public string GetChargeURL(string CorpNum, string UserID)
        {
            try
            {
                URLResponse response = httpget<URLResponse>("/?TG=CHRG", CorpNum, UserID);

                return response.url;
            }
            catch (LinkhubException le)
            {
                throw new PopbillException(le);
            }
        }


        //연동회원 잔여포인트 확인
        public double GetBalance(string CorpNum)
        {
            try
            {
                return _LinkhubAuth.getBalance(getSession_Token(CorpNum), ServiceID, UseStaticIP, UseGAIP);
            }
            catch (LinkhubException le)
            {
                throw new PopbillException(le);
            }
        }

        //파트너 잔여포인트 확인
        public double GetPartnerBalance(string CorpNum)
        {
            try
            {
                return _LinkhubAuth.getPartnerBalance(getSession_Token(CorpNum), ServiceID, UseStaticIP, UseGAIP);
            }
            catch (LinkhubException le)
            {
                throw new PopbillException(le);
            }
        }

        //파트너 포인트충전 팝업 URL
        public string GetPartnerURL(string CorpNum, string TOGO)
        {
            try
            {
                return _LinkhubAuth.getPartnerURL(getSession_Token(CorpNum), ServiceID, TOGO, UseStaticIP, UseGAIP);
            }
            catch (LinkhubException le)
            {
                throw new PopbillException(le);
            }
        }

        //연동회원 포인트 결제내역 URL
        public string GetPaymentURL(string CorpNum, string UserID =null)
        {
            try
            {
                URLResponse response = httpget<URLResponse>("/?TG=PAYMENT", CorpNum, UserID);

                return response.url;
            }
            catch (LinkhubException le)
            {
                throw new PopbillException(le);
            }
        }

        //연동회원 포인트 사용내역 URL
        public string GetUseHistoryURL(string CorpNum, string UserID = null)
        {
            try
            {
                URLResponse response = httpget<URLResponse>("/?TG=USEHISTORY", CorpNum, UserID);

                return response.url;
            }
            catch (LinkhubException le)
            {
                throw new PopbillException(le);
            }
        }


            //연동회원 가입여부 확인
        public Response CheckIsMember(string CorpNum, string LinkID)
        {
            try
            {
                return httpget<Response>("/Join?CorpNum=" + CorpNum + "&LID=" + LinkID, null, null);
            }
            catch (LinkhubException le)
            {
                throw new PopbillException(le);
            }
        }

        //아이디 중복확인
        public Response CheckID(string ID)
        {
            try
            {
                return httpget<Response>("/IDCheck?ID=" + ID, "", "");
            }
            catch (LinkhubException le)
            {
                throw new PopbillException(le);
            }
        }

        //연동회원 신규가입
        public Response JoinMember(JoinForm joinInfo)
        {
            if (joinInfo == null) throw new PopbillException(-99999999, "No JoinForm");

            string postData = toJsonString(joinInfo);

            try
            {
                return httppost<Response>("/Join", "", postData, "", null, "");
            }
            catch (LinkhubException le)
            {
                throw new PopbillException(le);
            }
        }

        //회사정보 확인
        public CorpInfo GetCorpInfo(string CorpNum, string UserID = null)
        {
            try
            {
                return httpget<CorpInfo>("/CorpInfo", CorpNum, UserID);
            }
            catch (LinkhubException le)
            {
                throw new PopbillException(le);
            }
        }

        //회사정보 수정
        public Response UpdateCorpInfo(string CorpNum, CorpInfo corpInfo, string UserID = null)
        {
            if (corpInfo == null) throw new PopbillException(-99999999, "No CorpInfo data");

            string PostData = toJsonString(corpInfo);

            try
            {
                return httppost<Response>("/CorpInfo", CorpNum, PostData, "", null, UserID);
            }
            catch (LinkhubException le)
            {
                throw new PopbillException(le);
            }
        }

        //담당자 등록
        public Response RegistContact(string CorpNum, Contact contactInfo, string UserID = null)
        {
            if (contactInfo == null) throw new PopbillException(-99999999, "No ContactInfo form");

            string postData = toJsonString(contactInfo);

            try
            {
                return httppost<Response>("/IDs/New", CorpNum, postData, "", null, UserID);
            }
            catch (LinkhubException le)
            {
                throw new PopbillException(le);
            }
        }

        //담당자 정보확인
        public Contact GetContactInfo(string CorpNum, string ContactID, string UserID =null)
        {
            string postData = "{'id' :" + "'" + ContactID + "'}";

            try
            {
                return httppost<Contact>("/Contact", CorpNum, postData, null, null, UserID);
            }
            catch (LinkhubException le)
            {
                throw new PopbillException(le);
            }
        }

        //담당자 목록 확인
        public List<Contact> ListContact(string CorpNum, string UserID = null)
        {
            try
            {
                return httpget<List<Contact>>("/IDs", CorpNum, UserID);
            }
            catch (LinkhubException le)
            {
                throw new PopbillException(le);
            }
        }

        //담당자 정보 수정
        public Response UpdateContact(string CorpNum, Contact contactInfo, string UserID = null)
        {
            if (contactInfo == null) throw new PopbillException(-99999999, "No ContactInfo form");

            string PostData = toJsonString(contactInfo);

            try
            {
                return httppost<Response>("/IDs", CorpNum, PostData, "", null, UserID);
            }
            catch (LinkhubException le)
            {
                throw new PopbillException(le);
            }
        }

        // 연동회원 무통장 입금신청
        public PaymentResponse PaymentRequest(string CorpNum, PaymentForm paymentForm, string UserID = null)
        {
            if (paymentForm == null) throw new PopbillException(-99999999, "No PaymentForm form");
            string PostData = toJsonString(paymentForm);
            try
            {
                return httppost<PaymentResponse>("/Payment", CorpNum, PostData, UserID: UserID);
            }
            catch (LinkhubException le)
            {
                throw new PopbillException(le);
            }
        }

        // 연동회원 무통장 입금신청 정보확인
        public PaymentHistory GetSettleResult(string CorpNum, string settleCode, string UserID=null)
        {
            if (settleCode == null)
            {
                throw new PopbillException(-99999999, "settleCode가 입력되지 않았습니다.");
            }

            try
            {
                return httpget<PaymentHistory>("/Payment/" + settleCode, CorpNum, UserID );
            }
            catch (LinkhubException le)
            {
                throw new PopbillException(le);
            }
        }

        // 연동회원 포인트 사용내역 확인
        public UseHistoryResult GetUseHistory(string CorpNum, string SDate = null, string EDate = null, int Page = 1,
            int PerPage = 500, string Order = null, string UserID = null)
        {
            string url = "/UseHisotry";
            url += "?SDate=" +SDate;
            url += "&EDate=" +EDate;
            url += "&Page=" +Page;
            url += "&PerPage=" +PerPage;
            url += "&Order=" +Order;

            try
            {
                return httpget<UseHistoryResult>(url, CorpNum, UserID);
            }
            catch (LinkhubException le)
            {
                throw new PopbillException(le);
            }
        }

        // 연동회원 포인트 결제내역 확인
        public PaymentHistoryResult GetPaymentHistory(string CorpNum, string SDate=null, string EDate=null, int Page=1, int PerPage=500, string UserID=null)
        {
            string url = "/PaymentHistory";

            url += "?SDate" + SDate;
            url += "&EDate" + EDate;
            url += "&Page" + Page;
            url += "&PerPage" + PerPage;

            try
            {
                return httpget<PaymentHistoryResult>(url, CorpNum, UserID);
            }
            catch (LinkhubException le)
            {
                throw new PopbillException(le);
            }
        }

        // 연동회원 포인트 환불신청
        public RefundResponse Refund(string CorpNum, RefundForm refundForm, string UserID=null)
        {
            if (refundForm == null) throw new PopbillException(-99999999, "No RefundForm form");
            string PostData = toJsonString(refundForm);
            try
            {
                return httppost<RefundResponse>("/Refund", CorpNum, PostData, UserID: UserID);
            }
            catch (LinkhubException le)
            {
                throw new PopbillException(le);
            }
        }

        // 연동회원 포인트 환불내역 확인
        public RefundHistoryResult GetRefundHistory(string CorpNum, int Page=1, int PerPage=500, string UserID=null)
        {
            string url = "/RefundHistory";

            url += "?Page"+Page;
            url += "&PerPage"+PerPage;

            try
            {
                return httpget<RefundHistoryResult>(url, CorpNum, UserID);
            }
            catch (LinkhubException le)
            {
                throw new PopbillException(le);
            }
        }

        // 환불 가능 포인트 조회
        public double GetRefundableBalance(string CorpNum, string UserID=null)
        {
            try
            {
                return httpget<double>("/RefundPoint", CorpNum, UserID);
            }catch(LinkhubException le)
            {
                throw new PopbillException(le);
            }
        }

        // 환불 신청 상태 조회
        public RefundHistory GetRefundInfo(string CorpNum, string RefundCode, string UserID = null)
        {
            try
            {
                return httpget<RefundHistory>("/Refund/" + RefundCode, CorpNum, UserID);
            }catch(LinkhubException le)
            {
                throw new PopbillException(le);
            }
        }

        // 회원 탈퇴
        public Response QuitMember(string CorpNum, string QuitReason, String UserID = null)
        {
            string url = "/QuitRequest";

            string PostData = toJsonString("{'quitReason' :" + "'" + QuitReason + "'}");
            try
            {
                return httppost<Response>(url, CorpNum, PostData, UserID: UserID);
            }catch(LinkhubException le)
            {
                throw new PopbillException(le);
            }
        }


        #endregion

        [DataContract]
        public class UploadFile
        {
            [DataMember] public string FieldName;
            [DataMember] public string FileName;
            [DataMember] public Stream FileData;
        }

        [DataContract]
        public class URLResponse
        {
            [DataMember] public string url;
        }

        [DataContract]
        public class UnitCostResponse
        {
            [DataMember] public Single unitCost;
        }

    }
}