using AutoMapper;
using GameStart.IdentityService.Data.Models;
using System.Security.Claims;

namespace GameStart.IdentityService.Common.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<List<Claim>, User>()

                .ForMember(user => user.UserName, options =>
                    options.MapFrom(claims => claims.Find(claim =>
                        claim.Type == ClaimTypes.Email).Value))

                .ForMember(user => user.Email, options =>
                    options.MapFrom(claims => claims.Find(claim =>
                        claim.Type == ClaimTypes.Email).Value))

                .ForMember(user => user.ExternalProviderUserId, options =>
                    options.MapFrom(claims => claims.Find(claim =>
                        claim.Type == ClaimTypes.NameIdentifier).Value))

                .ForMember(user => user.EmailConfirmed, options =>
                    options.MapFrom(_ => true));
        }
    }
}
