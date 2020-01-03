using System;

namespace Power_Estimator
{
    /// <summary>
    /// Represents a 1:1 lookup table with linear interpolation.
    /// </summary>
    public class Curve
    {
        /// <summary>
        /// Values of the lookup inputs.
        /// </summary>
        public double[] x;
        /// <summary>
        /// Values of the lookup outputs.
        /// </summary>
        public double[] y;

        /// <summary>
        /// Linear interpolation of this curve for the given input value.
        /// Gives the edge value if asked to extrapolate.
        /// </summary>
        /// <param name="xValue">The value of the input parameter.</param>
        /// <returns>The interpolated output value.</returns>
        public double Interpolate(double xValue)
        {
            // Return the edge value if asked to extrapolate.
            if (xValue > x[0] && xValue > x[x.Length - 1])
                return y[0] > y[y.Length - 1] ? y[0] : y[y.Length - 1];
            if (xValue < x[0] && xValue < x[x.Length - 1])
                return y[0] < y[y.Length - 1] ? y[0] : y[y.Length - 1];

            int indexLow = 0;
            bool exactX = false;
            for (int index = 0; index < x.Length - 1; index++)
            {
                if (xValue == x[index])
                {
                    exactX = true;
                    indexLow = index;
                    break;
                }
                if ((x[index] < xValue && x[index + 1] > xValue) || (x[index] > xValue && x[index + 1] < xValue))
                    indexLow = index;
            }
            double dydx = (y[indexLow + 1] - y[indexLow]) / (x[indexLow + 1] - x[indexLow]);
            double yValue = y[indexLow];
            if (!exactX)
                yValue += dydx * (xValue - x[indexLow]);
            return yValue;
        }

        /// <summary>
        /// Parse a Curve from two strings corresponding to the x and y arrays.
        /// </summary>
        /// <param name="x">List of input values (comma, space, or line separated).</param>
        /// <param name="y">List of output values (comma, space, or line separated).</param>
        /// <returns>A Curve based on the supplied values.</returns>
        static public Curve ReadCurve(string x, string y)
        {
            Curve curve = new Curve();
            double[] buffer = new double[64];
            string word = string.Empty;
            int index = 0;
            foreach (char c in x)
            {
                if (c == '\n' || c == ' ' || c == ',' || c == '\t')
                {
                    buffer[index] = Convert.ToDouble(word);
                    word = string.Empty;
                    index++;
                }
                else
                    word += c;
            }
            if (word != string.Empty)
            {
                buffer[index] = Convert.ToDouble(word);
                index++;
                word = string.Empty;
            }
            curve.x = new double[index];
            for (index = 0; index < curve.x.Length; index++)
                curve.x[index] = buffer[index];
            index = 0;
            foreach (char c in y)
            {
                if (c == '\n')
                {
                    buffer[index] = Convert.ToDouble(word);
                    word = string.Empty;
                    index++;
                }
                else
                    word += c;
            }
            if (word != string.Empty)
            {
                buffer[index] = Convert.ToDouble(word);
                index++;
            }
            curve.y = new double[index];
            for (index = 0; index < curve.y.Length; index++)
                curve.y[index] = buffer[index];
            return curve;
        }
    }

}
