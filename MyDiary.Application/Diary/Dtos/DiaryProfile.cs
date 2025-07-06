using AutoMapper;
using MyDiary.Application.Diary.Commands.CreateDiary;
using MyDiary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDiary.Application.Diary.Dtos
{
    public class DiaryProfile : Profile
    {
        public DiaryProfile()
        {
            CreateMap<DiaryEntity, DiaryDtos>().ReverseMap();

            CreateMap<CreateDiaryCommand, DiaryEntity>();
        }
    }
}
