using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PZ2.Models
{
    class NodeEntity:PowerEntity
    {
        public NodeEntity()
        {

        }
        public override void SetElementColor()
        {
            PowerEntityShape.Fill = Brushes.Blue;
        }
    }
}
