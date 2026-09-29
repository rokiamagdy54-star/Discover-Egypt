using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourism.Core.Features.Geoapify.Interfaces
{
    public interface IGeoapifyService
    {
        Task ImportPlacesAsync();
    }
}
