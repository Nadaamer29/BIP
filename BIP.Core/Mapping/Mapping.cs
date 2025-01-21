using BIP.DataAccess.Dtos.User;
using BIP.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BIP.DataAccess.Mapping
{
    public class Mapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<RegisterRequestdto, ApplicationUser>()
          .Map(dest => dest.UserName, src => src.UserName)
                          .Map(dest => dest.Email, src => src.Email); 

        }
    }
}
