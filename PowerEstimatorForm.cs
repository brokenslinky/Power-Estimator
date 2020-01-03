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

        private void showCurvesButton_Click(object sender, EventArgs e)
        {
            // Prepare plotting area.
            chart1.Series["Power"].Points.Clear();
            chart1.Series["Torque"].Points.Clear();
            chart1.Series["Temperature"].Points.Clear();
            chart1.Series["Compressor Efficiency"].Points.Clear();
            chart1.Series["Volumetric Efficiency"].Points.Clear();

            // Collect engine and environmental parameters.
            string compressorMapLocation = System.Environment.CurrentDirectory + "\\..\\..\\compressor map.txt";
            string VEMapLocation = System.Environment.CurrentDirectory + "\\..\\..\\VE map.txt";
            string outputFile = System.Environment.CurrentDirectory + @"\..\..\output.csv";
            double displacement = Convert.ToDouble(displacementInput.Text);
            double ambientTemperature = Convert.ToDouble(ambientTemperatureInput.Text);
            Curve boostCurve = Curve.ReadCurve(richTextBox1.Text, richTextBox2.Text);
            Table compressorMap = Table.ReadTable(compressorMapLocation);
            Table VEMap = Table.ReadTable(VEMapLocation);

            // Performance parameters.
            double rpm = boostCurve.x[0];
            if (rpm > VEMap.y[0])
                rpm = VEMap.y[0];
            double boost = boostCurve.Interpolate(rpm);
            double VE = VEMap.Interpolate(boost + 14.7, rpm) / 100.0;
            double CFM = VE * 0.5 * displacement * rpm * 3.531 / 100000.0;
            double compressorEfficiency = compressorMap.Interpolate(CFM, boost / 14.7 + 1.0) / 100.0;

            // Output file header.
            StreamWriter writer = new StreamWriter(outputFile);
            writer.WriteLine("RPM, boost, power, torque, temperature, compressorEfficiency, volumetricEfficiency");
            
            // Sweep RPM values to estimate performace parameters.
            for (rpm = VEMap.y[0]; rpm < VEMap.y[VEMap.y.Length - 1]; rpm += 100.0)
            {
                if (rpm >= boostCurve.x[boostCurve.x.Length - 1])
                    break;
                boost = boostCurve.Interpolate(rpm);
                VE = VEMap.Interpolate(boost + 14.7, rpm) / 100.0;
                CFM = VE * 0.5 * displacement * rpm * (1.0 + boost / 14.7) * (compressorEfficiency / (
                    compressorEfficiency + Math.Pow(1.0 + boost / 14.7, 0.4 / 1.4) - 1.0)) * 3.531 / 100000.0;
                if (CFM < compressorMap.x[0])
                    continue;
                double power = CFM * 0.6;
                double torque = power * 5252.0 / rpm;
                compressorEfficiency = compressorMap.Interpolate(CFM, boost / 14.7 + 1.0) / 100.0;
                double temperature = (459.7 + ambientTemperature) * ((Math.Pow((boost + 14.7) / 14.7, 0.4 /
                    1.4) - 1.0) / compressorEfficiency + 1.0) - 459.7;

                chart1.Series["Power"].Points.AddXY(rpm, power);
                chart1.Series["Torque"].Points.AddXY(rpm, torque);
                chart1.Series["Temperature"].Points.AddXY(rpm, temperature);
                chart1.Series["Compressor Efficiency"].Points.AddXY(rpm, compressorEfficiency);
                chart1.Series["Volumetric Efficiency"].Points.AddXY(rpm, VE);
                writer.WriteLine($"{rpm},{boost:N1},{power:N0},{torque:N0},{temperature:N0},{compressorEfficiency:N3},{VE:N3}");
            }
            writer.Close();
            chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0.0;
            chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Interval = 1000.0;
            this.Refresh();
        }
        
        private void findBoostCurve_Click(object sender, EventArgs e)
        {
            // Prepare plotting area.
            chart1.Series["Power"].Points.Clear();
            chart1.Series["Torque"].Points.Clear();
            chart1.Series["Temperature"].Points.Clear();
            chart1.Series["Compressor Efficiency"].Points.Clear();
            chart1.Series["Volumetric Efficiency"].Points.Clear();

            // Collect engine and environmental parameters.
            string compressorMapLocation = System.Environment.CurrentDirectory + "\\..\\..\\compressor map.txt";
            string VEMapLocation = System.Environment.CurrentDirectory + "\\..\\..\\VE map.txt";
            Curve boostCurve = Curve.ReadCurve(richTextBox1.Text, richTextBox2.Text);
            double displacement = Convert.ToDouble(displacementInput.Text);
            double ambientTemperature = Convert.ToDouble(ambientTemperatureInput.Text);
            Table compressorMap = Table.ReadTable(compressorMapLocation);
            Table VEMap = Table.ReadTable(VEMapLocation);

            // Sweep through RPM range.
            richTextBox2.Text = string.Empty;
            foreach (double rpm in boostCurve.x)
            {
                double bestFitness = 0.0;
                double bestBoost = 0.0;
                double bestPower = 0.0;
                double bestTemperature = 0.0;
                double bestCompressorEfficiency = 0.0;
                double bestVE = 0.0;
                double boostIncrement = 0.125;
                double boostMax = VEMap.x[VEMap.x.Length - 1] > VEMap.x[0] ?
                    VEMap.x[VEMap.x.Length - 1] : VEMap.x[0];
                boostMax -= 14.7;

                // sweep through boost range to find the ideal boost at this RPM
                for (double boost = 0.0; boost < boostMax; boost += boostIncrement)
                {
                    double VE = VEMap.Interpolate(boost + 14.7, rpm) / 100.0;
                    if (VE < 0)
                        MessageBox.Show("VE is negative!");
                    double CFM = VE * 0.5 * displacement * rpm * (1.0 + boost / 14.7) * (0.7 / (0.7 + Math.Pow(1.0 + boost /
                        14.7, 0.4 / 1.4) - 1.0)) * 3.531 / 100000.0;
                    if (CFM < 0)
                        MessageBox.Show("CFM is negative!");
                    double compressorEfficiency = compressorMap.Interpolate(CFM, boost / 14.7 + 1.0) / 100.0; ;
                    if (compressorEfficiency < 0)
                        MessageBox.Show($"Compressor efficiency is negative!" +
                            $"\nCFM: {CFM}\nPressure Ratio: {1.0 + boost / 14.7}"); 
                    CFM = VE * 0.5 * displacement * rpm * (1.0 + boost / 14.7) * (compressorEfficiency / 
                        (compressorEfficiency + Math.Pow(1.0 + boost / 14.7, 0.4 / 1.4) - 1.0)) * 3.531 / 100000.0;
                    double power = CFM * 0.6;
                    double torque = power * 5252.0 / rpm;
                    compressorEfficiency = compressorMap.Interpolate(CFM, boost / 14.7 + 1.0) / 100.0;
                    double temperature = (459.7 + ambientTemperature) * ((Math.Pow((boost + 14.7) / 14.7, 0.4 /
                        1.4) - 1.0) / compressorEfficiency + 1.0) - 459.7;
                    if (temperature > Convert.ToDouble(maxTemperatureInput.Text))
                        break;

                    double fitness = power * compressorEfficiency;// / (temperature + 459.7);

                    if (fitness > bestFitness)
                    {
                        /*if ((power - bestPower) / (temperature - bestTemperature) < 0.2)
                            continue;*/
                        bestFitness = fitness;
                        bestBoost = boost;
                        bestPower = power;
                        bestTemperature = temperature;
                        bestCompressorEfficiency = compressorEfficiency;
                        bestVE = VE;
                    }
                }
                chart1.Series["Power"].Points.AddXY(rpm, bestPower);
                chart1.Series["Torque"].Points.AddXY(rpm, bestPower * 5252.0 / rpm);
                chart1.Series["Temperature"].Points.AddXY(rpm, bestTemperature);
                chart1.Series["Compressor Efficiency"].Points.AddXY(rpm, bestCompressorEfficiency);
                chart1.Series["Volumetric Efficiency"].Points.AddXY(rpm, bestVE);
                richTextBox2.Text += bestBoost.ToString("N1") + "\n";
            }
            chart1.ChartAreas["ChartArea1"].AxisX.Minimum = 0.0;
            //chart1.ChartAreas["ChartArea1"].AxisX.Maximum = 7000.0;
            chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Interval = 1000.0;
            this.Refresh();
        }
    }
}
