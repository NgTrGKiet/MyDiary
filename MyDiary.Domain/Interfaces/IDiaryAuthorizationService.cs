using MyDiary.Domain.Constants;
using MyDiary.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDiary.Domain.Interfaces
{
    public interface IDiaryAuthorizationService
    {
        bool Authorize(DiaryEntity diary , ResourceOperation resourceOperation);
    }
}
