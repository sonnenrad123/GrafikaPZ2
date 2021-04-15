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
        private readonly static List<int> dr = new List<int> { -1, +1, 0, 0 };
        private readonly static List<int> dc = new List<int> { 0, 0, +1, -1 };
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


        //BFS algoritam
        public List<Block> createLineUsingBFS(double X1,double Y1,double X2,double Y2,bool cross)
        {
            List<Block> shortestWay = new List<Block>();
            Queue<Block> searchQueue = new Queue<Block>();
            bool pathFound = false;
            Block[,] visited = new Block[blockMatrix.GetLength(0), BlockMatrix.GetLength(1)];


            //moramo pronaci tacno x,y indekse bloka na koji smo aproksimirali tj indekse u gridu..
             int approxX1 = -1, approxY1 = -1 , approxX2 = -1, approxY2 = -1;

             for(int x = 0; x < BlockMatrix.GetLength(0); x++)
             {
                 if(X1 == blockMatrix[x, 0].XCoo)
                 {
                     approxX1 = x;
                 }
             }

             for (int y = 0; y < BlockMatrix.GetLength(1); y++)
             {
                 if (Y1 == blockMatrix[0 , y].YCoo)
                 {
                     approxY1 = y;
                 }
             }

            for (int x = 0; x < BlockMatrix.GetLength(0); x++)
            {
                if (X2 == blockMatrix[x, 0].XCoo)
                {
                    approxX2 = x;
                }
            }

            for (int y = 0; y < BlockMatrix.GetLength(1); y++)
            {
                if (Y2 == blockMatrix[0, y].YCoo)
                {
                    approxY2 = y;
                }
            }



            Block starRouteBlock = new Block(X1, Y1, approxX1, approxY1);
            Block endRouteBlock = new Block(X2, Y2, approxX2, approxY2);
            visited[approxX1, approxY1] = starRouteBlock;
            searchQueue.Enqueue(starRouteBlock);



            while (searchQueue.Count > 0)
            {
                Block tBlock = searchQueue.Dequeue();
                if(tBlock.Approx_X == approxX2 && tBlock.Approx_Y == approxY2)// da li sam pronasao put do end tacke
                {
                    pathFound = true;
                    break;
                }
                for(int i = 0; i < 4; i++)//obidji svuda okolo bloka
                {
                    int nextRow = tBlock.Approx_X + dr[i];
                    int nextColumn = tBlock.Approx_Y + dc[i];

                    if (nextRow < 0 || nextColumn < 0 || nextRow >= BlockMatrix.GetLength(0) || nextColumn >= BlockMatrix.GetLength(1))//provera opsega
                    {
                        continue;
                    }

                    if (visited[nextRow, nextColumn] != null) //preskoci posecena
                    {
                        continue;
                    }

                    if(!(nextRow == endRouteBlock.Approx_X && nextColumn == endRouteBlock.Approx_Y) && (blockMatrix[nextRow,nextColumn].BType != BlockType.EMPTY) && cross == false)
                    {
                        continue;
                    }
                    

                    //ne zelimo ni linije da prolaze kroz polja sa elementima koje ne povezuju
                    if(!(nextRow == endRouteBlock.Approx_X && nextColumn == endRouteBlock.Approx_Y) && (blockMatrix[nextRow, nextColumn].BType == BlockType.NODE) && cross == true)
                    {
                        continue;
                    }

                    if (!(nextRow == endRouteBlock.Approx_X && nextColumn == endRouteBlock.Approx_Y) && (blockMatrix[nextRow, nextColumn].BType == BlockType.SWITCH) && cross == true)
                    {
                        continue;
                    }

                    if (!(nextRow == endRouteBlock.Approx_X && nextColumn == endRouteBlock.Approx_Y) && (blockMatrix[nextRow, nextColumn].BType == BlockType.SUBSTATION) && cross == true)
                    {
                        continue;
                    }

                    searchQueue.Enqueue(new Block(BlockMatrix[nextRow, nextColumn].XCoo, BlockMatrix[nextRow, nextColumn].YCoo, nextRow, nextColumn));
                    visited[nextRow, nextColumn] = tBlock; //stavljamo na sledeci x i y da bi nakon pronalaska puta mogli se vratiti kroz matricu do start bloka
                }
            }

            if (pathFound)
            {
                shortestWay.Add(endRouteBlock);
                Block previousBlock = visited[endRouteBlock.Approx_X, endRouteBlock.Approx_Y];
                while(previousBlock.Approx_X > 0 && !(previousBlock.XCoo == starRouteBlock.XCoo && previousBlock.YCoo == starRouteBlock.YCoo && previousBlock.Approx_X == starRouteBlock.Approx_X && previousBlock.Approx_Y == starRouteBlock.Approx_Y))
                {
                    //pravimo putanju
                    if(BlockMatrix[previousBlock.Approx_X,previousBlock.Approx_Y].BType == BlockType.EMPTY)
                    {
                        BlockMatrix[previousBlock.Approx_X, previousBlock.Approx_Y].BType = BlockType.LINE;
                        
                    }
                    shortestWay.Add(previousBlock);
                    previousBlock = visited[previousBlock.Approx_X, previousBlock.Approx_Y];
                }
                shortestWay.Add(previousBlock);
            }
            return shortestWay;

        }

        

        public void AddLineToGrid(double XC, double YC, BlockType lineType)
        {
            for(int x=0; x < BlockMatrix.GetLength(0); x++)
            {
                if(XC == BlockMatrix[x, 0].XCoo)
                {
                    for (int y = 0; y < BlockMatrix.GetLength(1); y++)
                    {
                        if(YC == blockMatrix[x, y].YCoo)
                        {
                            if(blockMatrix[x, y].BType == BlockType.NODE || blockMatrix[x, y].BType == BlockType.SUBSTATION || blockMatrix[x, y].BType == BlockType.SWITCH)//ako je element ne sme tuda linija
                            {
                                return;
                            }
                            if(blockMatrix[x, y].BType == BlockType.LINE || blockMatrix[x, y].BType == BlockType.EMPTY) //ako je linija ili prazno sme
                            {
                                blockMatrix[x, y].BType = lineType;

                            }
                            else if (blockMatrix[x, y].BType != lineType)
                            {
                                blockMatrix[x, y].BType = BlockType.CROSS_LINE;
                            }
                            return;
                        }
                    }
                }
            }
        }



    }
}
