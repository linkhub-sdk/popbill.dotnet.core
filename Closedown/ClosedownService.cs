using System;
using System.Collections.Generic;

namespace Popbill.Closedown
{
    public class ClosedownService : BaseService
    {
        public ClosedownService(string LinkID, string SecretKey) : base(LinkID, SecretKey)
        {
            this.AddScope("170");
        }

        #region CheckCorp API

        //단건조회
        public CorpState checkCorpNum(string MemberCorpNum, string CheckCorpNum, string UserID = null)
        {
            if (CheckCorpNum == null || CheckCorpNum == "")
            {
                throw new PopbillException(-99999999, "조회할 사업자번호가 입력되지 않았습니다");
            }

            return httpget<CorpState>("/CloseDown?CN=" + CheckCorpNum, MemberCorpNum, UserID);
        }

        //대량조회
        public List<CorpState> checkCorpNums(string MemberCorpNum, List<string> CorpNumList, string UserID = null)
        {
            if (CorpNumList == null || CorpNumList.Count == 0)
                throw new PopbillException(-99999999, "조회할 사업자번호 목록이 입력되지 않았습니다.");

            string PostData = toJsonString(CorpNumList);

            return httppost<List<CorpState>>("/CloseDown", MemberCorpNum, PostData, null, null, UserID);
        }

        #endregion

        #region Point API

        //조회단가 확인
        public Single GetUnitCost(string CorpNum, string UserID = null)
        {
            UnitCostResponse response = httpget<UnitCostResponse>("/CloseDown/UnitCost", CorpNum, UserID);

            return response.unitCost;
        }

        //과금정보 확인
        public ChargeInfo GetChargeInfo(string CorpNum, string UserID = null)
        {
            ChargeInfo response = httpget<ChargeInfo>("/CloseDown/ChargeInfo", CorpNum, UserID);

            return response;
        }

        #endregion
    }
}