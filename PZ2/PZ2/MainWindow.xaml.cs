using Microsoft.Win32;
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
        string location = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenFileBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = "xml";
            openFileDialog.Filter = "XML Files|*.xml";

            if (openFileDialog.ShowDialog()==true)
            {
                try
                {
                    if (GridCanvas.Children.Count > 0)
                    {
                        string message = "Are you sure? Old data will be lost.";
                        string caption = "Confirmation";
                        MessageBoxButton buttons = MessageBoxButton.YesNo;
                        MessageBoxImage icon = MessageBoxImage.Question;
                        if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
                        {
                            using (new WaitCursor())
                            {
                                mainImportDraw = new ImportAndDraw();
                                GridCanvas.Children.Clear();
                                location = openFileDialog.FileName;
                                mainImportDraw.LoadAndParseXML(location);
                                mainImportDraw.ScaleFromLatLonToCanvas(GridCanvas.Width, GridCanvas.Height);
                                mainImportDraw.ConvertFromLatLonToCanvasCoord();
                                mainImportDraw.DrawElements(this.GridCanvas);
                            }
                        }
                    }
                    else
                    {
                        using (new WaitCursor())
                        {
                            mainImportDraw = new ImportAndDraw();
                            GridCanvas.Children.Clear();
                            location = openFileDialog.FileName;
                            mainImportDraw.LoadAndParseXML(location);
                            mainImportDraw.ScaleFromLatLonToCanvas(GridCanvas.Width, GridCanvas.Height);
                            mainImportDraw.ConvertFromLatLonToCanvasCoord();
                            mainImportDraw.DrawElements(this.GridCanvas);
                        }

                        
                    }
                }
                catch(Exception exc)
                {
                    MessageBox.Show("Please, provide a valid xml file.", "Invalid file", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

        }
    }

    public class WaitCursor : IDisposable
    {
        private Cursor _previousCursor;

        public WaitCursor()
        {
            _previousCursor = Mouse.OverrideCursor;

            Mouse.OverrideCursor = Cursors.Wait;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Mouse.OverrideCursor = _previousCursor;
        }

        #endregion
    }
}
