using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PZ2.Models
{
    class SubstationEntity:PowerEntity
    {
        public SubstationEntity()
        {

        }
        public override void SetElementColor()
        {
            PowerEntityShape.Fill = Brushes.Green;
        }
    }
}
