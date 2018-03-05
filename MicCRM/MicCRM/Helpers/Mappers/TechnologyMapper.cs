using MicCRM.Data.Entities;
using MicCRM.Models.TechnologyViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicCRM.Helpers.Mappers
{
    static class TechnologyMapper
    {
        public static TechnologyInfoViewModel Mapping(Technology technology)
        {
            return new TechnologyInfoViewModel()
            {
                Id = technology.Id,
                Name = technology.Name
            };
        }
    }
}
