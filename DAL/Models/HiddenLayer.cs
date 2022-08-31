using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class HiddenLayer
    {
        public int HiddenlayerId { get; set; }
        public int ChatbotId { get; set; }
        public string LayerType { get; set; }
        public int LayerValue { get; set; }

        public HiddenLayer() { }
        public HiddenLayer(string layerType, int layerValue)
        {
            LayerType = layerType;
            LayerValue = layerValue;
        }
    }
}
