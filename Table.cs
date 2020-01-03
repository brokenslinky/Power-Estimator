using System;
using System.IO;

namespace Power_Estimator
{
    /// <summary>
    /// Represents a 2:1 lookup table with linear interpolation.
    /// </summary>
    public class Table
    {
        /// <summary>
        /// Values in the 'x' axis of the lookup table.
        /// </summary>
        public double[] x;
        /// <summary>
        /// Values in the 'y' axis of the lookup table.
        /// </summary>
        public double[] y;
        /// <summary>
        /// Output values at the input nodes [x,y].
        /// </summary>
        public double[,] value;

        /// <summary>
        /// Linear interpolation of this table for the given input values.
        /// Return the edge case if asked to extrapolate.
        /// </summary>
        /// <param name="xValue">The value of the 'x' input parameter.</param>
        /// <param name="yValue">The value of the 'y' input parameter.</param>
        /// <returns>The interpolated output value.</returns>
        public double Interpolate(double xValue, double yValue)
        {
            // Use edge case if asked to extrapolate.
            if (xValue > x[0] && xValue > x[x.Length - 1])
                xValue = (x[0] > x[x.Length - 1] ? x[0] : x[x.Length - 1]);
            if (xValue < x[0] && xValue < x[x.Length - 1])
                xValue = (x[0] < x[x.Length - 1] ? x[0] : x[x.Length - 1]);
            if (yValue > y[0] && yValue > y[y.Length - 1])
                yValue = (y[0] > y[y.Length - 1] ? y[0] : y[y.Length - 1]);
            if (yValue < y[0] && yValue < y[y.Length - 1])
                yValue = (y[0] < y[y.Length - 1] ? y[0] : y[y.Length - 1]);

            int xIndexLow = -1;
            int yIndexLow = -1;
            bool exactX = false, exactY = false;
            for (int xIndex = 0; xIndex < x.Length - 1; xIndex++)
            {
                if (xValue == x[xIndex])
                {
                    xIndexLow = xIndex;
                    exactX = true;
                    break;
                }
                if ((x[xIndex] < xValue && x[xIndex + 1] > xValue) || (
                    x[xIndex] > xValue && x[xIndex + 1] < xValue))
                    xIndexLow = xIndex;
            }
            for (int yIndex = 0; yIndex < y.Length - 1; yIndex++)
            {
                if (yValue == y[yIndex])
                {
                    yIndexLow = yIndex;
                    exactY = true;
                    break;
                }
                if ((y[yIndex] < yValue && y[yIndex + 1] > yValue) || (
                    y[yIndex] > yValue && y[yIndex + 1] < yValue))
                    yIndexLow = yIndex;
            }
            if (xIndexLow < 0)
            {
                xIndexLow = x.Length - 1;
                exactX = true;
            }
            if (yIndexLow < 0)
            {
                yIndexLow = y.Length - 1;
                exactY = true;
            }

            if (exactX)
            {
                if (exactY)
                    return value[xIndexLow, yIndexLow];
                double yspan = Math.Abs(y[yIndexLow + 1] - y[yIndexLow]);
                double interpolated = value[xIndexLow, yIndexLow] * (yspan - Math.Abs(yValue - y[yIndexLow]));
                interpolated += value[xIndexLow, yIndexLow + 1] * (yspan - Math.Abs(yValue - y[yIndexLow + 1]));
                interpolated /= yspan;
                return interpolated;
            }
            if (exactY)
            {
                double xspan = Math.Abs(x[xIndexLow + 1] - x[xIndexLow]);
                double interpolated = value[xIndexLow, yIndexLow] * (xspan - Math.Abs(xValue - x[xIndexLow]));
                interpolated += value[xIndexLow + 1, yIndexLow] * (xspan - Math.Abs(xValue - x[xIndexLow + 1]));
                interpolated /= xspan;
                return interpolated;
            }

            double interpolatedValue = 0.0;
            double xSpan = Math.Abs(x[xIndexLow + 1] - x[xIndexLow]);
            double ySpan = Math.Abs(y[yIndexLow + 1] - y[yIndexLow]);
            double enclosedArea = xSpan * ySpan;
            for (int xIndex = xIndexLow; xIndex < xIndexLow + 2; xIndex++)
                for (int yIndex = yIndexLow; yIndex < yIndexLow + 2; yIndex++)
                    interpolatedValue += value[xIndex, yIndex] * 
                        (xSpan - Math.Abs(xValue - x[xIndex])) * (ySpan - Math.Abs(yValue - y[yIndex]));
            interpolatedValue /= enclosedArea;
            return interpolatedValue;
        }

        /// <summary>
        /// Parse a Table from a tab delimited file.
        /// </summary>
        /// <param name="tableLocation">Path to the file to parse as a Table.</param>
        /// <returns>A new Table based on the input file.</returns>
        static public Table ReadTable(string tableLocation)
        {
            Table table = new Table();
            double[] xBuffer = new double[64];
            double[] yBuffer = new double[64];
            StreamReader reader = new StreamReader(tableLocation);
            while (reader.Peek() != '\t')
                reader.ReadLine();
            string word = string.Empty;
            int column = -1;
            string line = reader.ReadLine();
            foreach (char c in line)
            {
                if (c == '\t')
                {
                    if (column != -1)
                    {
                        xBuffer[column] = Convert.ToDouble(word);
                    }
                    word = string.Empty;
                    column++;
                }
                else
                    word += c;
            }
            if (word != string.Empty)
            {
                xBuffer[column] = Convert.ToDouble(word);
                column++;
                word = string.Empty;
            }
            table.x = new double[column];
            for (column = 0; column < table.x.Length; column++)
            {
                table.x[column] = xBuffer[column];
            }
            double[,] tableBuffer = new double[table.x.Length, 64];
            int row = 0;
            while ((line = reader.ReadLine()) != null)
            {
                column = -1;
                word = string.Empty;
                foreach (char c in line)
                {
                    if (c == '\t')
                    {
                        if (column == -1)
                            yBuffer[row] = Convert.ToDouble(word);
                        else
                        {
                            tableBuffer[column, row] = Convert.ToDouble(word);
                        }
                        column++;
                        word = string.Empty;
                    }
                    else
                        word += c;
                }
                if (word != string.Empty)
                {
                    tableBuffer[column, row] = Convert.ToDouble(word);
                    word = string.Empty;
                }
                row++;
            }

            reader.Close();

            table.y = new double[row];
            table.value = new double[table.x.Length, table.y.Length];
            for (row = 0; row < table.y.Length; row++)
            {
                table.y[row] = yBuffer[row];
                for (column = 0; column < table.x.Length; column++)
                    table.value[column, row] = tableBuffer[column, row];
            }
            return table;
        }
    }

}
