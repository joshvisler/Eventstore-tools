﻿using AutoMapper;
using EventStoreTools.Core.Entities;
using EventStoreTools.DTO.Entities.Connection;
using System;

namespace EventStoreTools.Core.Containers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<InsertConnectionParameterDTO, Connection>().ConstructUsing(x=> new Connection(Guid.NewGuid(), x.Name, x.ConnectionString, x.IsDefault, x.RoleId, x.ServerAddress));
        }
    }
}
