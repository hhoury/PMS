using PMS.Application.DTOs.Members;
using PMS.Application.DTOs.Policies;

namespace PMS.Application.DTOs.PolicyMembers
{
    public class PolicyMemberDto : BaseDto
    {
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int PolicyId { get; set; }
        public PolicyDto PolicyDto { get; set; }
        public int MemberId { get; set; }
        public MemberDto MemberDto { get; set; }

    }
}
