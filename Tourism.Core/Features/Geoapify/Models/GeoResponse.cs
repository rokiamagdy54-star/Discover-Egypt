using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tourism.Core.Features.Geoapify.Models
{
    public class GeoResponse
    {
        public List<Feature> features { get; set; }
    }

    public class Feature
    {
        public Properties properties { get; set; }
    }

    public class Properties
    {
        public string name { get; set; }
        public string formatted { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public List<string> categories { get; set; }
    }
}
