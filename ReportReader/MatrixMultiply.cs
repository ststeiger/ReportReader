using System;
using System.Collections.Generic;
using System.Text;

namespace ReportReader
{
    class MatrixMultiplyTests 
    {


        public static void JaggedArrayVsMultiDim()
        {
            // JS: 
            // var matrix = [];
            // for (var i = 0; i < 9; i++)
            // {
            //     matrix[i] = new Array(9);
            // }
            // ... or:

            // var matrix = [];
            // for (var i = 0; i < 9; i++)
            // {
            //     matrix[i] = [];
            //     for (var j = 0; j < 9; j++)
            //     {
            //         matrix[i][j] = undefined;
            //     }
            // }

            // https://stackoverflow.com/questions/27205018/multiply-2-matrices-in-javascript


            // https://stackoverflow.com/questions/597720/what-are-the-differences-between-a-multidimensional-array-and-an-array-of-arrays
            double[][] jaggedArray2 = new double[][]
            {
                new double[] {1,2,3},
                new double[] {4,5,6},
                new double[] {7,8,9}
            };

            // Multidimensional array
            string[,] stringArray2Db = new string[3, 3] {
                 { "a", "c", "e" }
                ,{ "b", "d", "f" }
                ,{ "0", "0", "1" }
            };


            double[,] array2Db = new double[3, 3] {
                 { 0, 0, 0 }
                ,{ 0, 0, 0 }
                ,{ 0, 0, 0 }
            };

        }


        public class SvgPoint
        {
            public double x { get; set; }
            public double y { get; set; }
            public double z { get; set; } = 1;
        }


        public static void Test()
        {
            string json = @"
[
  {
    ""x"": 1024,
    ""y"": 391
  },
  {
    ""x"": -999,
    ""y"": 999
  },
  {
    ""x"": 121,
    ""y"": 121
  },
  {
    ""x"": 878,
    ""y"": -878
  },
  {
    ""x"": 878,
    ""y"": 878
  },
  {
    ""x"": 121,
    ""y"": -121
  }
]
";


            // https://developer.mozilla.org/en-US/docs/Web/SVG/Attribute/transform
            // matrix(a,b,c,d,e,f) 
            // transform="matrix(0.26458333,0,0,0.26458333,-281.51667,-133.02351)">
            double[,] matrix = new double[3, 3] {
                 { 0.26458333, 0, -281.51667 }
                ,{ 0, 0.26458333, -133.02351 }
                ,{ 0, 0, 1 }
            };

            // http://apike.ca/prog_svg_transform.html
            // https://stackoverflow.com/questions/15133977/how-to-calculate-svg-transform-matrix-from-rotate-translate-scale-values
            double tx = -44.298811;
            double ty = -46.175;

            // transform = "translate(-44.298811,-46.175)"
            double[,] translate = new double[3, 3] {
                 { 1, 0, tx }
                ,{ 0, 1, ty }
                ,{ 0, 0, 1 }
            };
            

            System.Collections.Generic.List<SvgPoint> ls = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<SvgPoint>>(json);

            for (int i = 0; i < ls.Count; ++i)
            {
                double[] p = new double[3] { ls[i].x, ls[i].y, ls[i].z };
                double[] p2 = Multiply(p, matrix);
                // double[,] p3 = MultiplyPoint1(p, matrix);

                // System.Console.WriteLine(p2);
                // System.Console.WriteLine(p3);
                //System.Console.WriteLine(p4);


                // ls[i].x = p2[0];
                // ls[i].y = p2[1];
                // ls[i].z = p2[2];

                p2[2] = 1;
                // var p2a = new double[] { p2[0] - 32, p2[1] - 32, p2[2] };

                double[] p2a = p2;
                // Aaaaarg, relative coordinates
                // Translate therefore needs only be applied to FIRST coordiante...
                // if it is applied to all, it's a major malfunction...
                if (i == 0) // We have relative coordinates, so we only need to translate the first one...
                {
                    // p2a = Multiply(p2, translate);
                    p2a[0] += tx;
                    p2a[1] += ty;
                }

                //// double[,] p3a = MultiplyPoint1(p2, translate);
                //// System.Console.WriteLine(p3a);

                ls[i].x = p2a[0];
                ls[i].y = p2a[1];
                ls[i].z = p2a[2];

                // ls[i].x = p3[0];
                // ls[i].y = p3[1];
                // ls[i].z = p3[2];

                // ls[i].x = p2[0] - tx;
                // ls[i].y = p2[1] - ty;
                // ls[i].z = p2[2] + 0;

                ls[i].x = Math.Round(ls[i].x, 0);
                ls[i].y = Math.Round(ls[i].y, 0);
                ls[i].z = Math.Round(ls[i].z, 0);
            }

            /*
            double minX = double.MaxValue;
            double minY = double.MaxValue;

            double maxX = double.MinValue;
            double maxY = double.MinValue;


            for (int i = 0; i < ls.Count; ++i)
            {
                if (ls[i].x < minX)
                    minX = ls[i].x;

                if (ls[i].y < minY)
                    minY = ls[i].y;


                if (ls[i].x > maxX)
                    maxX = ls[i].x;

                if (ls[i].y > maxY)
                    maxY = ls[i].y;
            }

            System.Console.WriteLine(minX);
            System.Console.WriteLine(minY);

            double deltaX = 0 - minX;
            double deltaY = 0 - minY;

            for (int i = 0; i < ls.Count; ++i)
            {
                ls[i].x = ls[i].x + deltaX;
                ls[i].y = ls[i].y + deltaY;
            }

            double w = maxX - minX;
            double h = maxY - minY;
            */


            System.Text.StringBuilder sb = new StringBuilder();

            // 0 0 139.7 78.316673
            sb.Append("<svg xmlns=\"http://www.w3.org/2000/svg\" xmlns:svg=\"http://www.w3.org/2000/svg\" version =\"1.1\" ");
            sb.Append("viewBox=\"0 0 528.6 296.3\" ");
            //sb.Append("viewBox=\"0 0 139.7 78.316673\" ");
            // sb.Append("viewBox=\"0 0 " + w.ToString() + " " + h.ToString() + "\" ");
            sb.AppendLine("width=\"528.6\" height=\"296.3\" >");
            sb.Append("    <path id=\"lol\" d=\"");

            sb.Append("m");

            for (int i = 0; i < ls.Count; ++i)
            {
                if(i!= 0)
                    sb.Append(" ");

                sb.Append(ls[i].x);
                sb.Append(" ");
                sb.Append(ls[i].y);
            }

            


                sb.Append("z");
            sb.AppendLine("\" fill=\"currentColor\" />");
            sb.AppendLine("</svg>");

            string path = sb.ToString();
            System.Console.WriteLine(path);
            System.IO.File.WriteAllText(@"D:\Stefan.Steiger\Desktop\test.svg", path);
            string res = Newtonsoft.Json.JsonConvert.SerializeObject(ls, Newtonsoft.Json.Formatting.Indented);
            System.Console.WriteLine(res);




        }



        public static double[,] MultiplyPoint(double[] p, double[,] A)
        {

            double[,] mp = new double[1, p.Length];

            for (int i = 0; i < p.Length; ++i)
            {
                mp[0, i] = p[i];
            }

            double[,] result = MatrixMultiply(mp, A);

            return result;
        }


        // WRONG ! 
        public static double[,] MultiplyPointWrongly(double[,] A, double[] p)
        {

            double[,] mp = new double[1, p.Length];

            for (int i = 0; i < p.Length; ++i)
            {
                mp[0, i] = p[i];
            }

            double[,] result = MatrixMultiply(A, mp);

            return result;
        }


        // https://gist.github.com/nadavrot/5b35d44e8ba3dd718e595e40184d03f0
        // https://slideplayer.com/slide/6957485/24/images/5/An+m+x+n+Matrix.jpg
        // a M-by-N matrix: // https://en.wikipedia.org/wiki/Matrix_(mathematics)
        // m: vertical, n: horizontal 
        public static double[,] MatrixMultiply(double[,] A, double[,] B)
        {
            int am = A.GetLength(0); // n  - numRows of A
            int an = A.GetLength(1); // m  - numColumns of A

            int bm = B.GetLength(0); // m - numRows of B
            int bn = B.GetLength(1); // p - numColumns of B

            if (an != bm)
            {
                throw new System.InvalidOperationException("Matrix cannot be multiplied !!! - Invalid dimensions.");
            }


            double[,] C = new double[am, bn];
            
            for (int i = 0; i < am; i++)
            {

                for (int j = 0; j < bn; j++)
                {
                    double sum = 0;

                    for (int k = 0; k < an; k++)
                    {
                        sum += A[i, k] * B[k, j];
                    }

                    C[i, j] = sum;
                } // Next j 

            } // Next i 

            return C;
        } // End Function MatrixMultiply 


        // https://stackoverflow.com/questions/6311309/how-can-i-multiply-two-matrices-in-c
        // https://stackoverflow.com/questions/18904153/matrix-multiplication-function
        public static double[] Multiply(double[] point, double[,] matrix)
        {
            int dim = point.Length;

            double[] aux = new double[dim];
            for (int i = 0; i < dim; ++i)
            {
                aux[i] = 0;
                for (int j = 0; j < dim; ++j)
                {
                    aux[i] += point[j] * matrix[j, i];
                }
            }
            return aux;
        }


    }
}
