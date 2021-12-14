using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnigmaSvgCore
{
    public class Matrix
    {
        public float A { get; set; } = 0.0f;
        public float B { get; set; } = 0.0f;
        public float C { get; set; } = 0.0f;
        public float D { get; set; } = 0.0f;
        public float E { get; set; } = 0.0f;
        public float F { get; set; } = 0.0f;

        public Matrix()
        { 
        
        }

        public Matrix(string a, string b, string c, string d, string e, string f)
        {
            A = float.Parse(a);
            B = float.Parse(b);
            C = float.Parse(c);
            D = float.Parse(d);
            E = float.Parse(e);
            F = float.Parse(f);
        }

        public Matrix(float a, float b, float c, float d, float e, float f)
        {
            A = a;
            B = b;
            C = c;
            D = d;
            E = e;
            F = f;
        }

        public Matrix(Node node)
        {
            if (node.IsAttributeExist("transform"))
            {
                string transformValue = node.Attributes["transform"];
                string[] matrixValues = transformValue.Substring(transformValue.IndexOf('(') + 1, transformValue.IndexOf(')') - transformValue.IndexOf('(') - 1).Split(' ');
                A = float.Parse(matrixValues[0]);
                B = float.Parse(matrixValues[1]);
                C = float.Parse(matrixValues[2]);
                D = float.Parse(matrixValues[3]);
                E = float.Parse(matrixValues[4]);
                F = float.Parse(matrixValues[5]);
            }
        }

        public int GetRotation(float maxDeviation = 0.02f)
        {
            int textRotation = 0;
            float max1 = float.MinValue;
            int maxNumber1 = 0;
            float max2 = float.MinValue;
            int maxNumber2 = 0;

            List<float> abcd = new List<float>()
            {
                Math.Abs(A),
                Math.Abs(B),
                Math.Abs(C),
                Math.Abs(D)
            };

            for (int i = 1; i <= 4; i++)
            {
                if (abcd[i - 1] > max1)
                {
                    maxNumber1 = i;
                    max1 = abcd[i - 1];
                }
            }

            for (int i = 1; i <= 4; i++)
            {
                if (i != maxNumber1)
                {
                    if (abcd[i - 1] > max2)
                    {
                        maxNumber2 = i;
                        max2 = abcd[i - 1];
                    }
                }
            }

            List<int> maxNumbers = new List<int>() { maxNumber1, maxNumber2 };

            if (maxNumbers.Contains(2) && maxNumbers.Contains(3)) // B и C
            {
                if (B > maxDeviation && C < -maxDeviation)
                {
                    textRotation = 90;
                }
                else if (B < -maxDeviation && C > maxDeviation)
                {
                    textRotation = 270;
                }
            }
            else if (maxNumbers.Contains(1) && maxNumbers.Contains(4)) // A и D
            {
                if (A < -maxDeviation && D < -maxDeviation)
                {
                    textRotation = 180;
                }
                else if (A > maxDeviation && D > maxDeviation)
                {
                    textRotation = 0;
                }
            }

            return textRotation;
        }

        // 2 + 3 => B и C
        // 1 + 4 => A и D
        public Tuple<int, int> GetWidthAndHeightPositions()
        {
            float max1 = float.MinValue;
            int maxNumber1 = 0;
            float max2 = float.MinValue;
            int maxNumber2 = 0;

            List<float> abcd = new List<float>()
            {
                Math.Abs(A),
                Math.Abs(B),
                Math.Abs(C),
                Math.Abs(D)
            };

            for (int i = 1; i <= 4; i++)
            {
                if (abcd[i - 1] > max1)
                {
                    maxNumber1 = i;
                    max1 = abcd[i - 1];
                }
            }

            for (int i = 1; i <= 4; i++)
            {
                if (i != maxNumber1)
                {
                    if (abcd[i - 1] > max2)
                    {
                        maxNumber2 = i;
                        max2 = abcd[i - 1];
                    }
                }
            }

            Tuple<int, int> maxPositions = new Tuple<int, int>(maxNumber1 < maxNumber2 ? maxNumber1 : maxNumber2, maxNumber2 > maxNumber1 ? maxNumber2 : maxNumber1);
            
            return maxPositions;
        }

        public float GetMatrixValueByPositionNumber(int position)
        {
            float mValue;
            switch (position)
            {
                case 1:
                    mValue = A;
                    break;
                case 2:
                    mValue = B;
                    break;
                case 3:
                    mValue = C;
                    break;
                case 4:
                    mValue = D;
                    break;
                case 5:
                    mValue = E;
                    break;
                case 6:
                    mValue = F;
                    break;
                default:
                    mValue = 0.0f;
                    break;
            }

            return mValue;
        }
    }
}
