using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PZ2.Models
{
    public class PowerEntity
    {
        private long id;
        private string name;
        private double x;
        private double y;
        private Ellipse powerEntityShape;
        

        public PowerEntity()
        {

        }

        public long Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public double X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }

        public double Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
            }
        }

        public Ellipse PowerEntityShape { get => powerEntityShape; set => powerEntityShape = value; }

        virtual public void SetElementColor()
        {

        }

        public void OnClick(object sender,EventArgs e)
        {
            powerEntityShape.Fill = Brushes.Yellow;
        }
    }
}