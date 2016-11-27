using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.IO;

namespace TestGraphicalProject {

    class GraphicsController {

        Canvas _canvas;
        Storyboard _storyboard;
        List<Shape> _shapeList;
        Random _random;

        /// <summary>
        /// Initialises the main Canvas, but doesn't run anything yet
        /// </summary>
        public GraphicsController(Canvas canvas)
        {
            _canvas = canvas;
            _storyboard = new Storyboard();
            _shapeList = new List<Shape>();
            _random = new Random();
        }

        /// <summary>
        /// Returns false if fails
        /// </summary>
        public bool LoadShapeFile(string filePath)
        {
            _shapeList.Clear();

            try
            {
                var stream = File.OpenText(filePath);

                while (!stream.EndOfStream)
                {
                    var line = stream.ReadLine();
                    Console.WriteLine(line);

                    if(line == "Gunch")
                    {
                        throw new Exception("ghost");
                    }

                    var shape = new Rectangle();
                    shape.Stroke = new SolidColorBrush(Colors.Red);
                    shape.Fill = new SolidColorBrush(Colors.White);
                    shape.Width = _random.Next(5, 60);
                    shape.Height = _random.Next(5, 60);
                    
                    _shapeList.Add(shape);
                }

                stream.Close();

                return true;
            } catch
            {
                return false;
            }



        }

        public void DrawShapes()
        {
            for (int i = 0; i < _shapeList.Count; i++)
            {
                var shape = _shapeList[i];

                Canvas.SetLeft(shape, _random.Next(0, 100)) ;
                Canvas.SetTop(shape, _random.Next(0, 100));

                _canvas.Children.Add(shape);
            }


            //return true;
        }

        public void ClearCanvas()
        {
            _canvas.Children.Clear();
        }

    }
}
