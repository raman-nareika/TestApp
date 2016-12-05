using AutoMapper;
using Students.App.ViewModel;
using Students.Data.Data;

namespace Students.App.Profiles
{
    public class StudentsProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<StudentViewModel, Student>()
                .ForMember(dest => dest.Last, opt => opt.MapFrom(src => src.LastName));
            CreateMap<Student, StudentViewModel>()
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Last));
        }
    }
}
