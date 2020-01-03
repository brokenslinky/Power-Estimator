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
        /// </summary>
        /// <param name="xValue">The value of the 'x' input parameter.</param>
        /// <param name="yValue">The value of the 'y' input parameter.</param>
        /// <returns>The interpolated output value.</returns>
        public double Interpolate(double xValue, double yValue)
        {
            int x_index_low = 0;
            int y_index_low = 0;
            bool exact_x = false;
            bool exact_y = false;
            for (int x_index = 0; x_index < x.Length - 1; x_index++)
            {
                if (xValue == x[x_index])
                {
                    exact_x = true;
                    x_index_low = x_index;
                    break;
                }
                if ((x[x_index] < xValue && x[x_index + 1] > xValue) || (
                    x[x_index] > xValue && x[x_index + 1] < xValue))
                    x_index_low = x_index;
            }
            for (int y_index = 0; y_index < y.Length - 1; y_index++)
            {
                if (yValue == y[y_index])
                {
                    exact_y = true;
                    y_index_low = y_index;
                    break;
                }
                if ((y[y_index] < yValue && y[y_index + 1] > yValue) || (
                    y[y_index] > yValue && y[y_index + 1] < yValue))
                    y_index_low = y_index;
            }
            if (yValue < y[0])
                yValue = y[0];
            if (yValue > y[y.Length - 1])
                yValue = y[y.Length - 1];
            if (xValue < x[0])
                xValue = x[0];
            if (xValue > x[x.Length - 1])
                xValue = x[x.Length - 1];
            double interpolated_value = 0.0;
            double x_span = Math.Abs(x[x_index_low + 1] - x[x_index_low]);
            double y_span = Math.Abs(y[y_index_low + 1] - y[y_index_low]);
            double enclosed_area = x_span * y_span;
            for (int x_index = x_index_low; x_index < x_index_low + 2; x_index++)
                for (int y_index = y_index_low; y_index < y_index_low + 2; y_index++)
                    interpolated_value += value[x_index, y_index] * 
                        (x_span - Math.Abs(xValue - x[x_index])) * (y_span - Math.Abs(yValue - y[y_index]));
            interpolated_value /= enclosed_area;
            return interpolated_value;
        }

        /// <summary>
        /// Parse a Table from a tab delimited file.
        /// </summary>
        /// <param name="tableLocation">Path to the file to parse as a Table.</param>
        /// <returns>A new Table based on the input file.</returns>
        static public Table Read_table(string tableLocation)
        {
            Table table = new Table();
            double[] x_buffer = new double[64];
            double[] y_buffer = new double[64];
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
                        x_buffer[column] = Convert.ToDouble(word);
                    }
                    word = string.Empty;
                    column++;
                }
                else
                    word += c;
            }
            if (word != string.Empty)
            {
                x_buffer[column] = Convert.ToDouble(word);
                column++;
                word = string.Empty;
            }
            table.x = new double[column];
            for (column = 0; column < table.x.Length; column++)
            {
                table.x[column] = x_buffer[column];
            }
            double[,] table_buffer = new double[table.x.Length, 64];
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
                            y_buffer[row] = Convert.ToDouble(word);
                        else
                        {
                            table_buffer[column, row] = Convert.ToDouble(word);
                        }
                        column++;
                        word = string.Empty;
                    }
                    else
                        word += c;
                }
                if (word != string.Empty)
                {
                    table_buffer[column, row] = Convert.ToDouble(word);
                    word = string.Empty;
                }
                row++;
            }

            table.y = new double[row];
            table.value = new double[table.x.Length, table.y.Length];
            for (row = 0; row < table.y.Length; row++)
            {
                table.y[row] = y_buffer[row];
                for (column = 0; column < table.x.Length; column++)
                    table.value[column, row] = table_buffer[column, row];
            }
            return table;
        }
    }

}
