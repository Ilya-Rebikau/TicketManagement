using Ninject;
using System;
using System.Collections.Generic;
using ThirdPartyEventEditor.Interfaces;
using ThirdPartyEventEditor.Models;

namespace ThirdPartyEventEditor.Repositories
{
    public class EventRepository : IRepository<ThirdPartyEvent>
    {
        protected static IKernel Kernel;
        public ThirdPartyEvent Create(ThirdPartyEvent obj)
        {
            throw new NotImplementedException();
        }

        public ThirdPartyEvent Delete(ThirdPartyEvent obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ThirdPartyEvent> GetAll()
        {
            throw new NotImplementedException();
        }

        public ThirdPartyEvent GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ThirdPartyEvent Update(ThirdPartyEvent obj)
        {
            throw new NotImplementedException();
        }
    }
}