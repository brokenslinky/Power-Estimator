using System;

namespace Power_Estimator
{
    public class Curve
    {
        public double[] x;
        public double[] y;
        public double Interpolate(double x_value)
        {
            int index_low = 0;
            bool exact_x = false;
            for (int index = 0; index < x.Length - 1; index++)
            {
                if (x_value == x[index])
                {
                    exact_x = true;
                    index_low = index;
                    break;
                }
                if ((x[index] < x_value && x[index + 1] > x_value) || (x[index] > x_value && x[index + 1] < x_value))
                    index_low = index;
            }
            double dydx = (y[index_low + 1] - y[index_low]) / (x[index_low + 1] - x[index_low]);
            double y_value = y[index_low];
            if (!exact_x)
                y_value += dydx * (x_value - x[index_low]);
            return y_value;
        }

        static public Curve Read_curve(string x, string y)
        {
            Curve curve = new Curve();
            double[] buffer = new double[64];
            string word = string.Empty;
            int index = 0;
            foreach (char c in x)
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
