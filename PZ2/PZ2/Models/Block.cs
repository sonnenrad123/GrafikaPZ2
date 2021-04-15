using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PZ2.Models
{
    public enum BlockType { EMPTY, NODE,SWITCH,SUBSTATION,LINE,HLINE,VLINE,CROSS_LINE}
    class Block
    {
        private PowerEntity bObject;
        private double xCoo;
        private double yCoo;
        private BlockType bType;
        private Brush bColor = Brushes.Black;
        private Shape bShape;
        private int approx_X;
        private int approx_Y;

        public Block(PowerEntity bObject, double xCoo, double yCoo, BlockType bType, Brush bColor, Shape bShape, int approx_X, int approx_Y)
        {
            this.bObject = bObject;
            this.xCoo = xCoo;
            this.yCoo = yCoo;
            this.bType = bType;
            this.bColor = bColor;
            this.bShape = bShape;
            this.approx_X = approx_X;
            this.approx_Y = approx_Y;
        }

        public Block(double xCoo, double yCoo, BlockType bType, Shape bShape, int approx_X, int approx_Y)
        {
           
            this.xCoo = xCoo;
            this.yCoo = yCoo;
            this.bType = bType;
          
            this.bShape = bShape;
            this.approx_X = approx_X;
            this.approx_Y = approx_Y;
        }
        public Block(double xCoo, double yCoo, int approx_X, int approx_Y)
        {

            this.xCoo = xCoo;
            this.yCoo = yCoo;
            this.approx_X = approx_X;
            this.approx_Y = approx_Y;
            this.BShape = null;
            this.bType = BlockType.EMPTY;
        }

        public Block()
        {
            this.approx_X = -1;
            this.approx_Y = -1;
            this.bColor = Brushes.Green;
            this.bObject = null;
            this.bType = BlockType.EMPTY;
            this.xCoo = -1;
            this.yCoo = -1;
            
        }

        public PowerEntity BObject { get => bObject; set => bObject = value; }
        public double XCoo { get => xCoo; set => xCoo = value; }
        public double YCoo { get => yCoo; set => yCoo = value; }
        public BlockType BType { get => bType; set => bType = value; }
        public Brush BColor { get => bColor; set => bColor = value; }
        public Shape BShape { get => bShape; set => bShape = value; }
        public int Approx_X { get => approx_X; set => approx_X = value; }
        public int Approx_Y { get => approx_Y; set => approx_Y = value; }

        
    }
}
