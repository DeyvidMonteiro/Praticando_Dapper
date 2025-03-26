using AutoMapper;
using CrudDapperApi.Dto;
using CrudDapperApi.Models;

namespace CrudDapperApi.Profiles
{
    public class ProfileAutomapper : Profile
    {
        public ProfileAutomapper()
        {
            CreateMap<Usuario, UsuarioListarDto>();
        }
    }
}
