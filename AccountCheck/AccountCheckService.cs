using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Popbill.AccountCheck
{
    public class AccountCheckService : BaseService
    {
        public AccountCheckService(string LinkID, string SecretKey) : base(LinkID, SecretKey)
        {
            this.AddScope("182");
            this.AddScope("183");
        }

        public ChargeInfo GetChargeInfo(String CorpNum)
        {
            return GetChargeInfo(CorpNum, null, null);
        }

        public ChargeInfo GetChargeInfo(String CorpNum, String UserID, String ServiceType = null)
        {
            string url = "/EasyFin/AccountCheck/ChargeInfo?serviceType=" + ServiceType;

            ChargeInfo response = httpget<ChargeInfo>(url, CorpNum, UserID);

            return response;
        }

        public Single GetUnitCost(String CorpNum, String ServiceType = null, String UserID = null)
        {
            string url = "/EasyFin/AccountCheck/UnitCost?serviceType=" + ServiceType;

            UnitCostResponse response = httpget<UnitCostResponse>(url, CorpNum, UserID);

            return response.unitCost;
        }

        public AccountCheckInfo CheckAccountInfo(String MemberCorpNum, String BankCode, String AccountNumber, String UserID = null)
        {
            if (BankCode == null || BankCode == "")
            {
                throw new PopbillException(-99999999, "계좌의 기관코드가 입력되지 않았습니다");
            }

            if (AccountNumber == null || AccountNumber == "")
            {
                throw new PopbillException(-99999999, "조회할 계좌번호가 입력되지 않았습니다");
            }

            String url = "/EasyFin/AccountCheck";
            url += "?c=" + BankCode;
            url += "&n=" + AccountNumber;

            return httppost<AccountCheckInfo>(url, MemberCorpNum, null, null, null, UserID);
        }

        public DepositorCheckInfo CheckDepositorInfo(String MemberCorpNum, String BankCode, String AccountNumber, String IdentityNumType, String IdentityNum, String UserID = null)
        {
            if (BankCode == null || BankCode == "")
            {
                throw new PopbillException(-99999999, "계좌의 기관코드가 입력되지 않았습니다");
            }

            if (AccountNumber == null || AccountNumber == "")
            {
                throw new PopbillException(-99999999, "조회할 계좌번호가 입력되지 않았습니다");
            }

            if (IdentityNumType == null || IdentityNumType == "")
            {
                throw new PopbillException(-99999999, "등록번호 유형이 입력되지 않았습니다.");
            }

            Regex reg = new Regex(@"^[PB]$");
            if (reg.IsMatch(IdentityNumType) == false)
            {
                throw new PopbillException(-99999999, "등록번호 유형이 유효하지 않습니다.");
            }

            if (IdentityNum == null || IdentityNum == "")
            {
                throw new PopbillException(-99999999, "등록번호가 입력되지 않았습니다.");
            }

            reg = new Regex(@"^\d+$");
            if (reg.IsMatch(IdentityNum) == false)
            {
                throw new PopbillException(-99999999, "등록번호는 숫자만 입력할 수 있습니다.");
            }

            String url = "/EasyFin/DepositorCheck";
            url += "?c=" + BankCode;
            url += "&n=" + AccountNumber;
            url += "&t=" + IdentityNumType;
            url += "&p=" + IdentityNum;

            return httppost<DepositorCheckInfo>(url, MemberCorpNum, null, null, null, UserID);
        }
    }
}
