using Derify.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derify.Core.Repository
{
    public interface IBaseRepository
    {
        public List<RelationEntity> GetAll();
    }
}
