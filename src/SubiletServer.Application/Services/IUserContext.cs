using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubiletServer.Application.Services
{
    public interface IUserContext
    {
        Guid GetUserId();
    }
}
