using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.DTOs;
using TodoApp.Domain.Models;

namespace TodoApp.Application.MappingProfiles
{
    public class autoMapper : Profile
    {
        public autoMapper()
        {
            CreateMap<Todo, TodoDto>().ReverseMap();
        }
    }
}
