using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        TextBlock _notification;

        string _filePath = Environment.CurrentDirectory;
        string _fileName = "Text.txt";     

        public MainWindow()
        {
            InitializeComponent();
            InitialiseNotification();

            _mainCanvas = MainCanvas;
            _graphicsController = new GraphicsController(_mainCanvas);

            Run();
        }

        
        void Run()
        {
            CreateFileWatcher(_filePath, _fileName);
            SetGraphicsController(_fileName);
        }

        /// <summary>
        /// Initialises the main Canvas
        /// </summary>
        /// 

        void InitialiseNotification()
        {
            _notification = new TextBlock();
            _notification.FontSize = 30;
            _notification.VerticalAlignment = VerticalAlignment.Center;
            _notification.HorizontalAlignment = HorizontalAlignment.Center;

            RootGrid.Children.Add(_notification);

            ChangeNotification("", false);
        }

        void ChangeNotification(string notification, bool isVisible)
        {
            _notification.Text = notification;
            _notification.Visibility = isVisible?Visibility.Visible:Visibility.Hidden;
        }

        void SetGraphicsController(string fileName)
        {
            if (File.Exists(fileName))
            {
                if (_graphicsController.LoadShapeFile(fileName))
                {
                    _graphicsController.DrawShapes();
                }
                else
                {
                    ChangeNotification("Error reading text file", true);
                }

            }
            else
            {
                ChangeNotification("No file at location", true);
            }
        }

        void ResetGraphicsController()
        {
            _graphicsController.ClearCanvas();
            SetGraphicsController(_filePath);
        }

        // Handling text file reloading

        void CreateFileWatcher(string path, string file)
        {
            // Create a new FileSystemWatcher and set its properties.
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = path;
            /* Watch for changes in LastAccess and LastWrite times, and 
               the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.
            watcher.Filter = file;

            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);

            // Begin watching.
            watcher.EnableRaisingEvents = true;
        }

        void OnChanged(object source, FileSystemEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                ReloadFile();
            });
        }

        void ReloadFile()
        {
            _graphicsController.ClearCanvas();
            ChangeNotification("", false);
            SetGraphicsController(_fileName);
        }

        void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
        }
    }
}
