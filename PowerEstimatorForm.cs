using System;
using System.Windows.Forms;
using System.IO;

namespace Power_Estimator
{
    public partial class PowerEstimatorForm : Form
    {

        public PowerEstimatorForm()
        {
            InitializeComponent();
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
            string outputFile = System.Environment.CurrentDirectory + @"\..\..\output.csv";
            double displacement = Convert.ToDouble(displacement_input.Text);
            double ambient_temperature = Convert.ToDouble(ambient_temperature_input.Text);
            Curve boost_curve = Curve.ReadCurve(richTextBox1.Text, richTextBox2.Text);
            Table compressor_map = Table.ReadTable(compressor_map_location);
            Table VE_map = Table.ReadTable(VE_map_location);
            double rpm = boost_curve.x[0];
            if (rpm > VE_map.y[0])
                rpm = VE_map.y[0];
            double boost = boost_curve.Interpolate(rpm);
            double VE = VE_map.Interpolate(boost + 14.7, rpm) / 100.0;
            double CFM = VE * 0.5 * displacement * rpm * 3.531 / 100000.0;
            double compressor_efficiency = compressor_map.Interpolate(CFM, boost / 14.7 + 1.0) / 100.0;
            StreamWriter writer = new StreamWriter(outputFile);
            writer.WriteLine("RPM, boost, power, torque, temperature, compressorEfficiency, volumetricEfficiency");
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
                double power = CFM * 0.6;
                double torque = power * 5252.0 / rpm;
                compressor_efficiency = compressor_map.Interpolate(CFM, boost / 14.7 + 1.0) / 100.0;
                double temperature = (459.7 + ambient_temperature) * ((Math.Pow((boost + 14.7) / 14.7, 0.4 /
                    1.4) - 1.0) / compressor_efficiency + 1.0) - 459.7;
                chart1.Series["Power"].Points.AddXY(rpm, power);
                chart1.Series["Torque"].Points.AddXY(rpm, torque);
                chart1.Series["Temperature"].Points.AddXY(rpm, temperature);
                chart1.Series["Compressor Efficiency"].Points.AddXY(rpm, compressor_efficiency);
                chart1.Series["Volumetric Efficiency"].Points.AddXY(rpm, VE);
                writer.WriteLine($"{rpm},{boost:N1},{power:N0},{torque:N0},{temperature:N0},{compressor_efficiency:N3},{VE:N3}");
            }
            writer.Close();
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
            Curve boost_curve = Curve.ReadCurve(richTextBox1.Text, richTextBox2.Text);
            double displacement = Convert.ToDouble(displacement_input.Text);
            double ambient_temperature = Convert.ToDouble(ambient_temperature_input.Text);
            Table compressor_map = Table.ReadTable(compressor_map_location);
            Table VE_map = Table.ReadTable(VE_map_location);
            richTextBox2.Text = string.Empty;
            foreach (double rpm in boost_curve.x)
            {
                double best_fitness = 0.0;
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
                    double power = CFM * 0.6;
                    double torque = power * 5252.0 / rpm;
                    compressor_efficiency = compressor_map.Interpolate(CFM, boost / 14.7 + 1.0) / 100.0;
                    double temperature = (459.7 + ambient_temperature) * ((Math.Pow((boost + 14.7) / 14.7, 0.4 /
                        1.4) - 1.0) / compressor_efficiency + 1.0) - 459.7;
                    if (temperature > Convert.ToDouble(max_temperature_input.Text))
                        continue;

                    double fitness = power * compressor_efficiency;// / (temperature + 459.7);

                    if (fitness > best_fitness)
                    {
                        /*if ((power - best_power) / (temperature - best_temperature) < 0.2)
                            continue;*/
                        best_fitness = fitness;
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
