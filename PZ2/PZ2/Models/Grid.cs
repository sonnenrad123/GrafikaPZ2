using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ2.Models
{
    class Grid
    {
        private Block[,] blockMatrix;

        internal Block[,] BlockMatrix { get => blockMatrix; set => blockMatrix = value; }

        public Grid(int xBlocks,int yBlocks)
        {
            BlockMatrix = new Block[xBlocks, yBlocks];
        }

        public void AddToGrid(double xC,double yC, BlockType bType)
        {
            for(int x = 0; x < BlockMatrix.GetLength(0); x++)
            {
                if(xC == blockMatrix[x, 0].XCoo)
                {
                    for (int y = 0; y < BlockMatrix.GetLength(1); y++)
                    {
                        if(yC == BlockMatrix[x, y].YCoo)
                        {
                            BlockMatrix[x, y].BType = bType;
                        }
                    }
                }
            }
        }
        
    }
}
