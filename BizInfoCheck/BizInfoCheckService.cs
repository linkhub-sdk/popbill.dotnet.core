using System;

namespace Popbill.BizInfoCheck
{
    public class BizInfoCheckService : BaseService
    {
        public BizInfoCheckService(string LinkID, string SecretKey) : base(LinkID, SecretKey)
        {
            this.AddScope("171");
        }

        #region CheckBizInfo API

        //단건조회
        public BizCheckInfo checkBizInfo(string MemberCorpNum, string CheckCorpNum, string UserID = null)
        {
            if (CheckCorpNum == null || CheckCorpNum == "")
            {
                throw new PopbillException(-99999999, "조회할 사업자번호가 입력되지 않았습니다");
            }

            return httpget<BizCheckInfo>("/BizInfo/Check?CN=" + CheckCorpNum, MemberCorpNum, UserID);
        }

        #endregion

        #region Point API

        //조회단가 확인
        public Single GetUnitCost(string CorpNum, string UserID = null)
        {
            UnitCostResponse response = httpget<UnitCostResponse>("/BizInfo/UnitCost", CorpNum, UserID);

            return response.unitCost;
        }

        //과금정보 확인
        public ChargeInfo GetChargeInfo(string CorpNum, string UserID = null)
        {
            ChargeInfo response = httpget<ChargeInfo>("/BizInfo/ChargeInfo", CorpNum, UserID);

            return response;
        }

        #endregion
    }
}