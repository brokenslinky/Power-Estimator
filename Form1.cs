using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Power_Estimator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

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
        }

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
        }

        public Curve Read_curve(string x, string y)
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

        public Table Read_table(string table_location)
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

        private void show_curves_button_Click(object sender, EventArgs e)
        {
            chart1.Series["Power"].Points.Clear();
            chart1.Series["Torque"].Points.Clear();
            chart1.Series["Temperature"].Points.Clear();
            chart1.Series["Compressor Efficiency"].Points.Clear();
            chart1.Series["Volumetric Efficiency"].Points.Clear();
            string compressor_map_location = System.Environment.CurrentDirectory + "\\..\\..\\compressor map.txt";
            string VE_map_location = System.Environment.CurrentDirectory + "\\..\\..\\VE map.txt";
            double displacement = Convert.ToDouble(displacement_input.Text);
            double ambient_temperature = Convert.ToDouble(ambient_temperature_input.Text);
            Curve boost_curve = Read_curve(richTextBox1.Text, richTextBox2.Text);
            Table compressor_map = Read_table(compressor_map_location);
            Table VE_map = Read_table(VE_map_location);
            double rpm = boost_curve.x[0];
            if (rpm > VE_map.y[0])
                rpm = VE_map.y[0];
            double boost = boost_curve.Interpolate(rpm);
            double VE = VE_map.Interpolate(boost + 14.7, rpm) / 100.0;
            double CFM = VE * 0.5 * displacement * rpm * 3.531 / 100000.0;
            double compressor_efficiency = compressor_map.Interpolate(CFM, boost / 14.7 + 1.0) / 100.0;
            for (rpm = VE_map.y[0]; rpm < VE_map.y[VE_map.y.Length - 1]; rpm += 100.0)
            {
                if (rpm >= boost_curve.x[boost_curve.x.Length - 1])
                    break;
                boost = boost_curve.Interpolate(rpm);
                VE = VE_map.Interpolate(boost + 14.7, rpm) / 100.0;
                CFM = VE * 0.5 * displacement * rpm * (1.0 + boost / 14.7) * (compressor_efficiency / (
                    compressor_efficiency + Math.Pow(1.0 + boost / 14.7, 0.4 / 1.4) - 1.0)) * 3.531 / 100000.0;
                if (CFM < compressor_map.x[0])
                    continue;
                double power = CFM * 0.7;
                double torque = power * 5252.0 / rpm;
                compressor_efficiency = compressor_map.Interpolate(CFM, boost / 14.7 + 1.0) / 100.0;
                double temperature = (459.7 + ambient_temperature) * ((Math.Pow((boost + 14.7) / 14.7, 0.4 /
                    1.4) - 1.0) / compressor_efficiency + 1.0) - 459.7;
                chart1.Series["Power"].Points.AddXY(rpm, power);
                chart1.Series["Torque"].Points.AddXY(rpm, torque);
                chart1.Series["Temperature"].Points.AddXY(rpm, temperature);
                chart1.Series["Compressor Efficiency"].Points.AddXY(rpm, compressor_efficiency);
                chart1.Series["Volumetric Efficiency"].Points.AddXY(rpm, VE);
            }
            chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0.0;
            chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Interval = 1000.0;
            this.Refresh();
        }
        
        private void find_boost_curve_Click(object sender, EventArgs e)
        {
            chart1.Series["Power"].Points.Clear();
            chart1.Series["Torque"].Points.Clear();
            chart1.Series["Temperature"].Points.Clear();
            chart1.Series["Compressor Efficiency"].Points.Clear();
            chart1.Series["Volumetric Efficiency"].Points.Clear();
            string compressor_map_location = System.Environment.CurrentDirectory + "\\..\\..\\compressor map.txt";
            string VE_map_location = System.Environment.CurrentDirectory + "\\..\\..\\VE map.txt";
            Curve boost_curve = Read_curve(richTextBox1.Text, richTextBox2.Text);
            double displacement = Convert.ToDouble(displacement_input.Text);
            double ambient_temperature = Convert.ToDouble(ambient_temperature_input.Text);
            Table compressor_map = Read_table(compressor_map_location);
            Table VE_map = Read_table(VE_map_location);
            richTextBox2.Text = string.Empty;
            foreach (double rpm in boost_curve.x)
            {
                double best_boost = 0.0;
                double best_power = 0.0;
                double best_temperature = 0.0;
                double best_compressor_efficiency = 0.0;
                double best_VE = 0.0;
                double boost_increment = 0.1;
                double boost_max = 18.0;
                for (double boost = 0.0; boost < boost_max; boost += boost_increment)
                {
                    double VE = VE_map.Interpolate(boost + 14.7, rpm) / 100.0; ;
                    double CFM = VE * 0.5 * displacement * rpm * (1.0 + boost / 14.7) * (0.7 / (0.7 + Math.Pow(1.0 + boost /
                        14.7, 0.4 / 1.4) - 1.0)) * 3.531 / 100000.0;
                    double compressor_efficiency = compressor_map.Interpolate(CFM, boost / 14.7 + 1.0) / 100.0; ;
                    CFM = VE * 0.5 * displacement * rpm * (1.0 + boost / 14.7) * (compressor_efficiency / 
                        (compressor_efficiency + Math.Pow(1.0 + boost / 14.7, 0.4 / 1.4) - 1.0)) * 3.531 / 100000.0;
                    double power = CFM * 0.7;
                    double torque = power * 5252.0 / rpm;
                    compressor_efficiency = compressor_map.Interpolate(CFM, boost / 14.7 + 1.0) / 100.0;
                    double temperature = (459.7 + ambient_temperature) * ((Math.Pow((boost + 14.7) / 14.7, 0.4 /
                        1.4) - 1.0) / compressor_efficiency + 1.0) - 459.7;
                    if (temperature > Convert.ToDouble(max_temperature_input.Text))
                        continue;
                    if (power > best_power)
                    {
                        best_boost = boost;
                        best_power = power;
                        best_temperature = temperature;
                        best_compressor_efficiency = compressor_efficiency;
                        best_VE = VE;
                    }
                }
                chart1.Series["Power"].Points.AddXY(rpm, best_power);
                chart1.Series["Torque"].Points.AddXY(rpm, best_power * 5252.0 / rpm);
                chart1.Series["Temperature"].Points.AddXY(rpm, best_temperature);
                chart1.Series["Compressor Efficiency"].Points.AddXY(rpm, best_compressor_efficiency);
                chart1.Series["Volumetric Efficiency"].Points.AddXY(rpm, best_VE);
                richTextBox2.Text += best_boost.ToString("N1") + "\n";
            }
            chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0.0;
            //chart1.ChartAreas["ChartArea1"].AxisX.Maximum = 7000.0;
            chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Interval = 1000.0;
            this.Refresh();
        }
    }
}
