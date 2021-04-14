using PZ2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PZ2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ImportAndDraw mainImportDraw = new ImportAndDraw();

        public MainWindow()
        {
            InitializeComponent();
            mainImportDraw.LoadAndParseXML();
            mainImportDraw.ScaleFromLatLonToCanvas(GridCanvas.Width,GridCanvas.Height);
            mainImportDraw.ConvertFromLatLonToCanvasCoord();
            mainImportDraw.DrawElements(this.GridCanvas,DrawingElementMouseDown);
        }


        public void DrawingElementMouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
