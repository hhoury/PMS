using PMS.Application.DTOs.Members;
using PMS.Application.DTOs.Policies;
using System;

namespace PMS.Application.DTOs.PolicyMembers
{
    public class CreatePolicyMemberDto
    {
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int PolicyId { get; set; }
        public PolicyDto PolicyDto { get; set; }
        public int MemberId { get; set; }
        public MemberDto MemberDto { get; set; }
    }
}
