using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PZ2.Models
{
    class SwitchEntity:PowerEntity
    {
        private string status;

        public string Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
            }
        }
        public override void SetElementColor()
        {
            PowerEntityShape.Fill = Brushes.Red;
        }
    }
}
