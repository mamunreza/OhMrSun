using API.Features.Activities;
using AutoMapper;

namespace API.Domains.Activities;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Activity, ActivitiesBaseResult>(MemberList.None);
        CreateMap<Activity, GetActivities.ActivityRecord>(MemberList.None);
    }
}
