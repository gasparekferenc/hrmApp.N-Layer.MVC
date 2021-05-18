using AutoMapper;
using hrmApp.Core.PagedList;
using hrmApp.Web.DTO;
using hrmApp.Core.Models;
using hrmApp.Web.ViewModels;
using hrmApp.Web.ViewModels.ComponentsViewModels;

namespace hrmApp.Web.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Domain to Resource
            CreateMap<ApplicationUser, ApplicationUserDTO>().IncludeAllDerived().ReverseMap();
            CreateMap<Assignment, AssignmentDTO>().IncludeAllDerived().ReverseMap();
            CreateMap<DocType, DocTypeDTO>().IncludeAllDerived().ReverseMap();
            CreateMap<Document, DocumentDTO>().IncludeAllDerived().ReverseMap();
            CreateMap<Employee, EmployeeDTO>().IncludeAllDerived().ReverseMap();
            CreateMap<History, HistoryDTO>().IncludeAllDerived().ReverseMap();
            CreateMap<Job, JobDTO>().IncludeAllDerived().ReverseMap();
            CreateMap<Organization, OrganizationDTO>().IncludeAllDerived().ReverseMap();
            CreateMap<POEmployee, POEmployeeDTO>().IncludeAllDerived().ReverseMap();
            CreateMap<ProcessStatus, ProcessStatusDTO>().IncludeAllDerived().ReverseMap();
            CreateMap<Project, ProjectDTO>().IncludeAllDerived().ReverseMap();
            CreateMap<ProjectOrganization, ProjectOrganizationDTO>().IncludeAllDerived().ReverseMap();

            CreateMap<ApplicationUser, UserViewModel>().IncludeAllDerived().ReverseMap();
            CreateMap<Employee, EmployeeViewModel>().IncludeAllDerived().ReverseMap();

            // for server side pagination
            CreateMap(typeof(IPagedList<>), typeof(IPagedListDTO<>)).IncludeAllDerived();
        }
    }
}
