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
        List<ShapeAnimation> _shapeList;
        Random _random;

        string[] _animTypes = { "vert", "hor", "circ", "box" };
        string[] _shapeTypes = { "square", "tri", "circ", "hex" };

        /// <summary>
        /// Initialises the main Canvas, but doesn't run anything yet
        /// </summary>
        public GraphicsController(Canvas canvas)
        {
            _canvas = canvas;
            _storyboard = new Storyboard();
            _shapeList = new List<ShapeAnimation>();
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

                    if (IsAnimation(line))
                    {
                        
                    } else if (IsShape(line))
                    {
                        _shapeList.Add(new ShapeAnimation(line));
                    }
                    else if (IsMisc(line))
                    {

                    }
                    else
                    {
                        stream.Close();
                        return false;
                    }
                }

                stream.Close();

                return true;
            } catch
            {
                return false;
            }
        }

        //Checking validity of string

        bool IsAnimation(string testString)
        {
            var stringArray = testString.ToLower().Split(new char[] { ' ' });

            if (stringArray[0] != "Anim")
                return false;

            //Here we need to parse stuff

            //if(stringArray[1].)

            return false;
        }

        bool IsShape(string testString)
        {
            return true;
        }

        bool IsMisc(string testString)
        {
            return false;
        }

        //Dealing with canvas

        public void DrawShapes()
        {
            for (int i = 0; i < _shapeList.Count; i++)
            {
                var shape = _shapeList[i].Shape;

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



        class ShapeAnimation {

            public Shape Shape
            { get; private set; }

            public bool IsValid
            { get; private set; }

            public ShapeAnimation(string shapeString)
            {
                Shape = new Rectangle();
                Shape.Stroke = new SolidColorBrush(Colors.Red);
                Shape.Fill = new SolidColorBrush(Colors.White);
                Shape.Width = 20;
                Shape.Height = 20;
            }
        }
    }
}
