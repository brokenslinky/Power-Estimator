using System;
using System.IO;

namespace Power_Estimator
{
        public class Table
        {
            public double[] x;
            public double[] y;
            public double[,] value;
            public double Interpolate(double x_value, double y_value)
            {
                int x_index_low = 0;
                int y_index_low = 0;
                bool exact_x = false;
                bool exact_y = false;
                for (int x_index = 0; x_index < x.Length - 1; x_index++)
                {
                    if (x_value == x[x_index])
                    {
                        exact_x = true;
                        x_index_low = x_index;
                        break;
                    }
                    if ((x[x_index] < x_value && x[x_index + 1] > x_value) || (
                        x[x_index] > x_value && x[x_index + 1] < x_value))
                        x_index_low = x_index;
                }
                for (int y_index = 0; y_index < y.Length - 1; y_index++)
                {
                    if (y_value == y[y_index])
                    {
                        exact_y = true;
                        y_index_low = y_index;
                        break;
                    }
                    if ((y[y_index] < y_value && y[y_index + 1] > y_value) || (
                        y[y_index] > y_value && y[y_index + 1] < y_value))
                        y_index_low = y_index;
                }
                if (y_value < y[0])
                    y_value = y[0];
                if (y_value > y[y.Length - 1])
                    y_value = y[y.Length - 1];
                if (x_value < x[0])
                    x_value = x[0];
                if (x_value > x[x.Length - 1])
                    x_value = x[x.Length - 1];
                double interpolated_value = 0.0;
                double x_span = Math.Abs(x[x_index_low + 1] - x[x_index_low]);
                double y_span = Math.Abs(y[y_index_low + 1] - y[y_index_low]);
                double enclosed_area = x_span * y_span;
                for (int x_index = x_index_low; x_index < x_index_low + 2; x_index++)
                    for (int y_index = y_index_low; y_index < y_index_low + 2; y_index++)
                        interpolated_value += value[x_index, y_index] * 
                            (x_span - Math.Abs(x_value - x[x_index])) * (y_span - Math.Abs(y_value - y[y_index]));
                interpolated_value /= enclosed_area;
                return interpolated_value;
            }


        static public Table Read_table(string table_location)
        {
            Table table = new Table();
            double[] x_buffer = new double[64];
            double[] y_buffer = new double[64];
            StreamReader reader = new StreamReader(table_location);
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
