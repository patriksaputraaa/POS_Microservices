using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Pos.Common
{
    public interface IEntity
    {
        public Guid Id { get; init; }
    }
}