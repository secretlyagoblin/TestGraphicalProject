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
using System.IO;

namespace TestGraphicalProject {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window {

        Canvas _mainCanvas;
        GraphicsController _graphicsController;         

        string _filePath;

        

        public MainWindow()
        {
            InitializeComponent();

            _mainCanvas = MainCanvas;
            _graphicsController = new GraphicsController(_mainCanvas);

            if (SetTextFile("Text.txt"))
            {
                if (_graphicsController.LoadShapeFile(_filePath))
                {
                    _graphicsController.DrawShapes();
                }
                else
                {
                    ShowNotification("Error reading text file");
                }

            }
            else
            {
                ShowNotification("No file at location");
            }      

        }

        /// <summary>
        /// Initialises the main Canvas
        /// </summary>

        bool SetTextFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                _filePath = filePath;
                return true;
            }
            
            return false;
        }

        void ShowNotification(string notification)
        {
            _graphicsController.ClearCanvas();

            var textBlock = new TextBlock();
            textBlock.Text = notification;
            textBlock.FontSize = 30;
            textBlock.Text = notification;
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.HorizontalAlignment = HorizontalAlignment.Center;

            RootGrid.Children.Add(textBlock);
        }
    }
}
