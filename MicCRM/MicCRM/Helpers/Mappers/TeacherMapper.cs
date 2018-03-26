using MicCRM.Data.Entities;
using MicCRM.Models.TeacherViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicCRM.Helpers.Mappers
{
    static class TeacherMapper
    {
        public static TeacherInfoViewModel Mapping(Teacher teacher)
        {
            return new TeacherInfoViewModel()
            {
                Id = teacher.Id,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                Phone1 = teacher.Phone,
                Email = teacher.Email
            };
        }
    }
}
