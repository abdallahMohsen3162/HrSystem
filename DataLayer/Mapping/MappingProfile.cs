using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Mapping
{
    using AutoMapper;
    using DataLayer.Data;
    using DataLayer.dto.Departments;
    using DataLayer.dto.Employee;
    using DataLayer.dto.Roles;
    using DataLayer.dto.Users;
    using DataLayer.Entities;
    using DataLayer.ViewModels;
    using Microsoft.AspNetCore.Identity;

    public class MappingProfile : Profile
    {
        private readonly ApplicationDbContext context;
        public MappingProfile(ApplicationDbContext context)
        {
            this.context = context;

        }
        public MappingProfile()
        {
            CreateMap<Entities.Employee, EmployeeDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.EmployeeName))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.DepartmentName))
                .ReverseMap();

            CreateMap<Entities.Department, DepartmentDto>()
                .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.DepartmentName))
                .ForMember(dest => dest.NumberOFEmployees, opt => opt.MapFrom(src => src.Employees.Count()))
                .ReverseMap();

            CreateMap<IdentityRole, Rolesdto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));


        }
    }

}
