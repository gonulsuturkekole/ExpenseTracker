using AutoMapper;
using ExpenseTracker.Persistence.Domain;

namespace ExpenseTracker.Business.Mapper;

    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<UserRequest, User>();
            CreateMap<User, UserResponse>();
        }
    }

