using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        List<MyShape> _shapeList;
        Random _random;

        /// <summary>
        /// Initialises the main Canvas, but doesn't run anything yet
        /// </summary>
        public GraphicsController(Canvas canvas)
        {
            _canvas = canvas;
            _storyboard = new Storyboard();
            _shapeList = new List<MyShape>();
            _random = new Random();
        }

        /// <summary>
        /// Returns false if fails
        /// </summary>
        public bool LoadShapeFile(string fileName)
        {
            _shapeList.Clear();

            try
            {
                var stream = File.OpenText(fileName);

                while (!stream.EndOfStream)
                {
                    var line = stream.ReadLine();
                    //Console.WriteLine(line);

                    var stringArray = line.ToLower().Split(new char[] { ' ' });
                    var identifier = stringArray[0];

                    if (MyAnimation.Identifier ==identifier)
                    {
                        
                    } else if (MyShape.Identifier == identifier)
                    { 
                        var shape = new MyShape();

                        if (shape.TryParseLine(stringArray))
                        {
                            _shapeList.Add(shape);
                        }
                        else
                        {
                            return false;
                        }
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

        public void ReloadCanvas()
        {
            _canvas.Dispatcher.Invoke(() =>
            {
                _canvas.Children.Clear();
                DrawShapes();
            });
        }

        //Utils for Shape stuff

        static bool MatchesTypeArray(string testString, string[] typeArray)
        {
            for (int i = 0; i < typeArray.Length; i++)
            {
                if(testString == typeArray[i])
                {
                    return true;
                }
            }
            return false;
        }

        static bool MatchesAttributePattern(string testString, string[] attributes)
        {
            var array = testString.Split(new char[] { '=' });

            if (array.Length != 2)
                return false;



            return (MatchesTypeArray(array[0], attributes));
        }



        class MyShape {

            public static readonly string Identifier = "shape";
            static readonly string[] _shapeTypes = { "hexagon", "square", "circle", "triangle" };
            static readonly string[] _attributes = { "size" };

            public Shape Shape
            { get; private set; }

            

            public MyShape()
            {
            }

            public bool TryParseLine(string[] line)
            {

                if (line[0] != Identifier)
                {
                    return false;
                }

                if (MatchesTypeArray(line[1], _shapeTypes))
                {
                    SetShape(line[1]);
                }

                else
                {
                    return false;
                }

                //we start at 1 as first line section has been vetted

                for (int i = 2; i < line.Length; i++)
                {
                    if(MatchesAttributePattern(line[i], _attributes))
                    {
                        if (TrySetAttribute(line[i]))
                        {

                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            void SetShape(string code)
            {
                if(code == _shapeTypes[0]) //hexagon
                {
                    Shape = new Polygon();
                    //Add Corners
                }
                else if (code == _shapeTypes[1]) //square
                {
                    Shape = new Rectangle();
                }
                else if (code == _shapeTypes[2])//circle
                {
                    Shape = new Ellipse();
                }
                else if (code == _shapeTypes[3])//triangle
                {
                    Shape = new Polygon();
                }

                Shape.Stroke = new SolidColorBrush(Colors.Red);
                Shape.Fill = new SolidColorBrush(Colors.White);
                Shape.Width = 20;
                Shape.Height = 20;
            }

            bool TrySetAttribute(string code)
            {
                var array = code.Split(new char[] { '=' });

                string attribute = array[0];
                string value = array[1];


                if (attribute == _attributes[0]) //size
                {
                    int size;

                    if (!int.TryParse(value, out size))
                    {
                        return false;
                    }
                    Shape.Width = size;
                    Shape.Height = size;

                    return true;
                }
                return false;
            }

        }

        class MyAnimation {

            public static readonly string Identifier = "anim";
            static readonly string[] _shapeTypes = { "vertical", "horizontal", "circle", "box" };
            static readonly string[] _attributes = { "moverate" };



        }
    }
}
