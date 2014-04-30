using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Swerl.Referee.NerdDinnerSample.Models;
using Swerl.Referee.NerdDinnerSample.Models.EditModels;

namespace Swerl.Referee.NerdDinnerSample.App_Start
{
    public class AutomapConfig
    {
        public static void Configure()
        {
            Mapper.CreateMap<DinnerEditModel, Dinner>().ReverseMap();
        }
    }
}