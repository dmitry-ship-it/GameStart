using AutoMapper;
using GameStart.IdentityService.Data.Models;
using IdentityModel;
using System.Security.Claims;

namespace GameStart.IdentityService.Data.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<List<Claim>, User>()
                .ForMember(user => user.UserName, options => options.MapFrom(claims => claims.Find(claim => claim.Type == ClaimTypes.Name).Value.Replace(' ', '_')))
                .ForMember(user => user.Email, options => options.MapFrom(claims => claims.Find(claim => claim.Type == ClaimTypes.Email).Value))
                .ForMember(user => user.EmailConfirmed, options => options.MapFrom(_ => true))
                .ForMember(user => user.ExternalProviderUserId, options => options.MapFrom(claims => claims.Find(claim => claim.Type == ClaimTypes.NameIdentifier || claim.Type == JwtClaimTypes.ClientId).Value));
        }
    }
}
