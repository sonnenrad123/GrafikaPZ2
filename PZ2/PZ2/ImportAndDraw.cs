using PZ2.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;

namespace PZ2
{
    public class ImportAndDraw
    {
        public static List<PowerEntity> AllResources;
        private List<LineEntity> lines = new List<LineEntity>();
        private List<SubstationEntity> substations = new List<SubstationEntity>();
        private List<SwitchEntity> switches = new List<SwitchEntity>();
        private List<NodeEntity> nodes = new List<NodeEntity>();
        private double xs;
        private double ys;
        private double minX;
        private double minY;
        private PZ2.Models.Grid drawingGrid;
        private int blockSize = 10;
        private List<Point> usedPoints = new List<Point>();
        public ImportAndDraw()
        {
           AllResources = new List<PowerEntity>();
        }

        public void LoadAndParseXML()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("Geographic.xml");
            XmlNodeList lineNodes = xmlDoc.SelectNodes("/NetworkModel/Lines/LineEntity");
            XmlNodeList substationNodes = xmlDoc.SelectNodes("/NetworkModel/Substations/SubstationEntity");
            XmlNodeList switchNodes = xmlDoc.SelectNodes("/NetworkModel/Switches/SwitchEntity");
            XmlNodeList nodeNodes = xmlDoc.SelectNodes("/NetworkModel/Nodes/NodeEntity");
            lines = createLineEntities(lineNodes);
            substations = createSubstationEntities(substationNodes);
            switches = createSwitchEntities(switchNodes);
            nodes = createNodeEntities(nodeNodes);
            
        }

        private static List<LineEntity> createLineEntities(XmlNodeList lineNodes)
        {
            List<LineEntity> ret = new List<LineEntity>();

            foreach(XmlNode node in lineNodes)
            {
                LineEntity line = new LineEntity();
                line.ConductorMaterial = node.SelectSingleNode("ConductorMaterial").InnerText;
                line.FirstEnd = long.Parse(node.SelectSingleNode("FirstEnd").InnerText);
                line.Id = long.Parse(node.SelectSingleNode("Id").InnerText);
                line.IsUnderground = bool.Parse(node.SelectSingleNode("IsUnderground").InnerText);
                line.LineType = node.SelectSingleNode("LineType").InnerText;
                line.Name = node.SelectSingleNode("Name").InnerText;
                line.R = float.Parse(node.SelectSingleNode("R").InnerText);
                line.SecondEnd = long.Parse(node.SelectSingleNode("SecondEnd").InnerText);
                line.ThermalConstantHeat = long.Parse(node.SelectSingleNode("ThermalConstantHeat").InnerText);

                bool postoji_dupla_suprotni_smer = false;

                foreach(LineEntity templine in ret)
                {
                    if(templine.FirstEnd == line.SecondEnd && templine.SecondEnd == line.FirstEnd)
                    {
                        postoji_dupla_suprotni_smer = true;
                    }
                }

                if(postoji_dupla_suprotni_smer == false)
                {
                    ret.Add(line);
                }
                
            }

            return ret;
        }


        private static List<SubstationEntity> createSubstationEntities(XmlNodeList substationNodes)
        {
            List<SubstationEntity> ret = new List<SubstationEntity>();
            foreach(XmlNode node in substationNodes)
            {
               
                SubstationEntity sub = new SubstationEntity();
                sub.Id = long.Parse(node.SelectSingleNode("Id").InnerText, CultureInfo.InvariantCulture);
                sub.Name = node.SelectSingleNode("Name").InnerText;
                sub.X = double.Parse(node.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture);
                sub.Y = double.Parse(node.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture);


                double noviX, noviY;
                ToLatLon(sub.X, sub.Y, 34, out noviX, out noviY);
                sub.X = noviX;
                sub.Y = noviY;
                ret.Add(sub);

            }
            return ret;
        }

        private static List<SwitchEntity> createSwitchEntities(XmlNodeList switchNodes)
        {
            List<SwitchEntity> ret = new List<SwitchEntity>();
            foreach(XmlNode node in switchNodes)
            {
                SwitchEntity swtch = new SwitchEntity();
                swtch.Id = long.Parse(node.SelectSingleNode("Id").InnerText, CultureInfo.InvariantCulture);
                swtch.Name = node.SelectSingleNode("Name").InnerText;
                swtch.Status = node.SelectSingleNode("Status").InnerText;
                swtch.X = double.Parse(node.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture);
                swtch.Y = double.Parse(node.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture);
                double noviX, noviY;
                ToLatLon(swtch.X, swtch.Y, 34, out noviX, out noviY);
                swtch.X = noviX;
                swtch.Y = noviY;
                ret.Add(swtch);
            }


            return ret;
        }

        private static List<NodeEntity> createNodeEntities(XmlNodeList nodeNodes)
        {
            List<NodeEntity> ret = new List<NodeEntity>();
            foreach(XmlNode node in nodeNodes)
            {
                NodeEntity nEntity = new NodeEntity();
                nEntity.Id = long.Parse(node.SelectSingleNode("Id").InnerText, CultureInfo.InvariantCulture);
                nEntity.Name = node.SelectSingleNode("Name").InnerText;
                nEntity.X = double.Parse(node.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture);
                nEntity.Y = double.Parse(node.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture);
                double noviX, noviY;
                ToLatLon(nEntity.X, nEntity.Y, 34, out noviX, out noviY);
                nEntity.X = noviX;
                nEntity.Y = noviY;
                ret.Add(nEntity);
            }



            return ret;
        }



        public void ScaleFromLatLonToCanvas(double canvasWidth,double canvasHeight)
        {
            //da bi lepo se skalirale tacke na ceo kanvas
            double substationMinX, substationMinY, nodeMinX, nodeMinY, switchMinX, switchMinY, substationNodeMinX, substationNodeMinY;
            substationMinX = substations.Min((obj) => obj.X);
            nodeMinX = nodes.Min((obj) => obj.X);
            switchMinX = switches.Min((obj) => obj.X);
            substationMinY = substations.Min((obj) => obj.Y);
            nodeMinY = nodes.Min((obj) => obj.Y);
            switchMinY = switches.Min((obj) => obj.Y);
            substationNodeMinX = Math.Min(substationMinX, nodeMinX);
            minX = Math.Min(substationNodeMinX, switchMinX);

            substationNodeMinY = Math.Min(substationMinY, nodeMinY);
            minY = Math.Min(substationNodeMinY, switchMinY);

            double substationMaxX, substationMaxY, nodeMaxX, NodeMaxY, switchMaxX, switchMaxY, substationNodeMaxX, substationNodeMaxY,maxX,maxY;

            substationMaxX = substations.Max((obj) => obj.X);
            nodeMaxX = nodes.Max((obj) => obj.X);
            switchMaxX = switches.Max((obj) => obj.X); 

            substationMaxY = substations.Max((obj) => obj.Y);
            NodeMaxY = nodes.Max((obj) => obj.Y);
            switchMaxY = switches.Max((obj) => obj.Y);
            substationNodeMaxX = Math.Min(substationMaxX, nodeMaxX);
            substationNodeMaxY = Math.Min(substationMaxY, NodeMaxY);
            maxX = Math.Max(substationNodeMaxX, switchMaxX);
            maxY = Math.Max(substationNodeMaxY, switchMaxY);
            xs = (canvasWidth/2) / (maxX - minX); //podeoci po x
            ys = (canvasHeight / 2) / (maxY - minY); //podeoci po y




            CreateNewDrawingGrid();

        }

        private void CreateNewDrawingGrid()
        {
            drawingGrid = new PZ2.Models.Grid(401, 401);
            for (int x = 0; x <= 400; x++)
            {
                for (int y = 0; y <= 400; y++)//init blocks
                {
                   drawingGrid.BlockMatrix[x, y] = new Block(x * blockSize, y * blockSize, BlockType.EMPTY, null, x, y); //4000x4000 je kanvas, 400 blokova,block size je 10, znaci grid coord je 10*x i 10*y
                    

                }
            }
        }

        public void ConvertFromLatLonToCanvasCoord()
        {
            foreach(SubstationEntity substation in substations)
            {
                double x = Math.Round((substation.X - minX) * xs / blockSize) * blockSize;
                double y = Math.Round((substation.Y - minY) * ys / blockSize) * blockSize;
                Point ret = FindClosestUnusedPoint(x, y);
                substation.X = ret.X;
                substation.Y = ret.Y;
                drawingGrid.AddToGrid(ret.X, ret.Y, BlockType.SUBSTATION);


            }

            foreach(NodeEntity node in nodes)
            {
                double x = Math.Round((node.X - minX) * xs / blockSize) * blockSize;
                double y = Math.Round((node.Y - minY) * ys / blockSize) * blockSize;
                Point ret = FindClosestUnusedPoint(x, y);
                drawingGrid.AddToGrid(ret.X, ret.Y, BlockType.NODE);
                node.X = ret.X;
                node.Y = ret.Y;
            }

            foreach(SwitchEntity sw in switches)
            {
                double x = Math.Round((sw.X - minX) * xs / blockSize) * blockSize;
                double y = Math.Round((sw.Y - minY) * ys / blockSize) * blockSize;
                Point ret = FindClosestUnusedPoint(x, y);
                drawingGrid.AddToGrid(ret.X, ret.Y, BlockType.SWITCH);
                sw.X = ret.X;
                sw.Y = ret.Y;
            }
        }


        public Point FindClosestUnusedPoint(double x,double y)
        {
            bool pointUsed = IsPointUsed(x, y);

            if(pointUsed == false)
            {
                Point p = new Point();
                p.X = x;
                p.Y = y;
                usedPoints.Add(p);
                Point ret = new Point();
                //promenjeno da svaka tacka ima duplirane koordinate i
                //scale factor smanjen za duplo tako da tacke nikad nisu jedna pored druge
                //jer nam treba prostor za vodove
                ret.X = x * 2;
                ret.Y = y * 2;
                return ret;
            }
            double closestX = x - blockSize;
            closestX = (closestX < 0) ? closestX + blockSize : closestX; // ne smemo izaci van canvasa
            double closestY = y - blockSize;
            closestY = (closestY < 0) ? closestY + blockSize : closestY;

            while (IsPointUsed(closestX, closestY)){ 
                for(int j = 0;j< 2; j++)//obilazimo svugde oko tacke
                {
                    for(int i = 0; i < 2; i++)
                    {
                        if (IsPointUsed(closestX, closestY))
                        {
                            usedPoints.Add(new Point() { X = closestX, Y = closestY });
                            return new Point() { X = closestX * 2, Y = closestY * 2 };
                        }
                        closestY = closestY + blockSize;
                    }
                    if (IsPointUsed(closestX, closestY))
                    {
                        usedPoints.Add(new Point() { X = closestX, Y = closestY });
                        return new Point() { X = closestX * 2, Y = closestY * 2 };
                    }
                    closestX = closestX + blockSize;
                    closestY = closestY - 2 * blockSize;
                }
                
            }

            usedPoints.Add(new Point() { X = closestX, Y = closestY });
            return new Point() { X = closestX * 2, Y = closestY * 2 };
        }


        public bool IsPointUsed(double x,double y)
        {
            foreach (Point ptemp in usedPoints)
            {
                if (ptemp.X == x && ptemp.Y == y)
                {
                    return true;
                }
            }
            return false;
        }

        public void DrawElements(Canvas drawingCanvas, MouseButtonEventHandler e)
        {
            DrawSubstations(drawingCanvas, e);
            DrawNodes(drawingCanvas, e);
            DrawSwitches(drawingCanvas, e);
        }


        public void DrawSubstations(Canvas drawingCanvas, MouseButtonEventHandler e)
        {
            foreach(SubstationEntity ent in substations)
            {
                Ellipse shape = new Ellipse() { Height = 6, Width = 6, Fill = Brushes.Yellow };
                shape.ToolTip = "Substation: \n" + "ID:" + ent.Id + "\nName: " + ent.Name;
                shape.MouseLeftButtonDown += e;
                Canvas.SetLeft(shape, ent.X + 2);
                Canvas.SetTop(shape, ent.Y + 2);
                ent.PowerEntityShape = shape;
                AllResources.Add(ent);
                drawingCanvas.Children.Add(shape);
                
            }
        }

        public void DrawNodes(Canvas drawingCanvas, MouseButtonEventHandler e)
        {
            foreach (NodeEntity ent in nodes)
            {
                Ellipse shape = new Ellipse() { Height = 6, Width = 6, Fill = Brushes.Blue };
                shape.ToolTip = "Node: \n" + "ID:" + ent.Id + "\nName: " + ent.Name;
                shape.MouseLeftButtonDown += e;
                Canvas.SetLeft(shape, ent.X + 2);
                Canvas.SetTop(shape, ent.Y + 2);
                ent.PowerEntityShape = shape;
                AllResources.Add(ent);
                drawingCanvas.Children.Add(shape);

            }
        }

        public void DrawSwitches(Canvas drawingCanvas, MouseButtonEventHandler e)
        {
            foreach (SwitchEntity ent in switches)
            {
                Ellipse shape = new Ellipse() { Height = 6, Width = 6, Fill = Brushes.Red };
                shape.ToolTip = "Switch: \n" + "ID:" + ent.Id + "\nName: " + ent.Name + "\nStatus: "+ent.Status;
                shape.MouseLeftButtonDown += e;
                Canvas.SetLeft(shape, ent.X + 2);
                Canvas.SetTop(shape, ent.Y + 2);
                ent.PowerEntityShape = shape;
                AllResources.Add(ent);
                drawingCanvas.Children.Add(shape);

            }
        }


        public static void ToLatLon(double utmX, double utmY, int zoneUTM, out double latitude, out double longitude)
        {
            bool isNorthHemisphere = true;

            var diflat = -0.00066286966871111111111111111111111111;
            var diflon = -0.0003868060578;

            var zone = zoneUTM;
            var c_sa = 6378137.000000;
            var c_sb = 6356752.314245;
            var e2 = Math.Pow((Math.Pow(c_sa, 2) - Math.Pow(c_sb, 2)), 0.5) / c_sb;
            var e2cuadrada = Math.Pow(e2, 2);
            var c = Math.Pow(c_sa, 2) / c_sb;
            var x = utmX - 500000;
            var y = isNorthHemisphere ? utmY : utmY - 10000000;

            var s = ((zone * 6.0) - 183.0);
            var lat = y / (c_sa * 0.9996);
            var v = (c / Math.Pow(1 + (e2cuadrada * Math.Pow(Math.Cos(lat), 2)), 0.5)) * 0.9996;
            var a = x / v;
            var a1 = Math.Sin(2 * lat);
            var a2 = a1 * Math.Pow((Math.Cos(lat)), 2);
            var j2 = lat + (a1 / 2.0);
            var j4 = ((3 * j2) + a2) / 4.0;
            var j6 = ((5 * j4) + Math.Pow(a2 * (Math.Cos(lat)), 2)) / 3.0;
            var alfa = (3.0 / 4.0) * e2cuadrada;
            var beta = (5.0 / 3.0) * Math.Pow(alfa, 2);
            var gama = (35.0 / 27.0) * Math.Pow(alfa, 3);
            var bm = 0.9996 * c * (lat - alfa * j2 + beta * j4 - gama * j6);
            var b = (y - bm) / v;
            var epsi = ((e2cuadrada * Math.Pow(a, 2)) / 2.0) * Math.Pow((Math.Cos(lat)), 2);
            var eps = a * (1 - (epsi / 3.0));
            var nab = (b * (1 - epsi)) + lat;
            var senoheps = (Math.Exp(eps) - Math.Exp(-eps)) / 2.0;
            var delt = Math.Atan(senoheps / (Math.Cos(nab)));
            var tao = Math.Atan(Math.Cos(delt) * Math.Tan(nab));

            longitude = ((delt * (180.0 / Math.PI)) + s) + diflon;
            latitude = ((lat + (1 + e2cuadrada * Math.Pow(Math.Cos(lat), 2) - (3.0 / 2.0) * e2cuadrada * Math.Sin(lat) * Math.Cos(lat) * (tao - lat)) * (tao - lat)) * (180.0 / Math.PI)) + diflat;
        }
    }

}
