namespace Power_Estimator
{
    partial class PowerEstimatorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series26 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series27 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series28 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series29 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series30 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.showCurvesButton = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.findBoostCurve = new System.Windows.Forms.Button();
            this.maxTemperatureInput = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ambientTemperatureInput = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.displacementInput = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.pressureDropTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(20, 55);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(60, 550);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "1000\n1500\n2000\n2500\n3000\n3500\n4000\n4500\n5000\n5500\n6000\n6500\n7500";
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(90, 55);
            this.richTextBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(78, 550);
            this.richTextBox2.TabIndex = 1;
            this.richTextBox2.Text = "1.08\n5.10\n10.06\n12.53\n13.77\n14.39\n14.39\n14.39\n13.46\n11.60\n9.75\n7.89\n3.87";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 26);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "RPM";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(86, 26);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Boost (psi)";
            // 
            // showCurvesButton
            // 
            this.showCurvesButton.Location = new System.Drawing.Point(18, 617);
            this.showCurvesButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.showCurvesButton.Name = "showCurvesButton";
            this.showCurvesButton.Size = new System.Drawing.Size(152, 57);
            this.showCurvesButton.TabIndex = 4;
            this.showCurvesButton.Text = "Show Curves";
            this.showCurvesButton.UseVisualStyleBackColor = true;
            this.showCurvesButton.Click += new System.EventHandler(this.showCurvesButton_Click);
            // 
            // chart1
            // 
            chartArea6.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea6);
            legend6.Name = "Legend1";
            this.chart1.Legends.Add(legend6);
            this.chart1.Location = new System.Drawing.Point(178, 103);
            this.chart1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chart1.Name = "chart1";
            series26.ChartArea = "ChartArea1";
            series26.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series26.Legend = "Legend1";
            series26.Name = "Power";
            series27.ChartArea = "ChartArea1";
            series27.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series27.Legend = "Legend1";
            series27.Name = "Torque";
            series28.ChartArea = "ChartArea1";
            series28.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series28.Legend = "Legend1";
            series28.Name = "Temperature";
            series29.ChartArea = "ChartArea1";
            series29.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series29.Legend = "Legend1";
            series29.Name = "Compressor Efficiency";
            series29.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            series30.ChartArea = "ChartArea1";
            series30.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series30.Legend = "Legend1";
            series30.Name = "Volumetric Efficiency";
            series30.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            this.chart1.Series.Add(series26);
            this.chart1.Series.Add(series27);
            this.chart1.Series.Add(series28);
            this.chart1.Series.Add(series29);
            this.chart1.Series.Add(series30);
            this.chart1.Size = new System.Drawing.Size(1004, 637);
            this.chart1.TabIndex = 5;
            this.chart1.Text = "chart1";
            // 
            // findBoostCurve
            // 
            this.findBoostCurve.Location = new System.Drawing.Point(18, 683);
            this.findBoostCurve.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.findBoostCurve.Name = "findBoostCurve";
            this.findBoostCurve.Size = new System.Drawing.Size(152, 57);
            this.findBoostCurve.TabIndex = 4;
            this.findBoostCurve.Text = "Find Best Boost Curve";
            this.findBoostCurve.UseVisualStyleBackColor = true;
            this.findBoostCurve.Click += new System.EventHandler(this.findBoostCurve_Click);
            // 
            // maxTemperatureInput
            // 
            this.maxTemperatureInput.Location = new System.Drawing.Point(1090, 15);
            this.maxTemperatureInput.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.maxTemperatureInput.Name = "maxTemperatureInput";
            this.maxTemperatureInput.Size = new System.Drawing.Size(55, 26);
            this.maxTemperatureInput.TabIndex = 6;
            this.maxTemperatureInput.Text = "300";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1156, 20);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "°F";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(832, 20);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(250, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "Maximum Air Charge Temperature";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(563, 20);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(163, 20);
            this.label5.TabIndex = 11;
            this.label5.Text = "Ambient Temperature";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(779, 20);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(24, 20);
            this.label6.TabIndex = 10;
            this.label6.Text = "°F";
            // 
            // ambientTemperatureInput
            // 
            this.ambientTemperatureInput.Location = new System.Drawing.Point(729, 15);
            this.ambientTemperatureInput.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ambientTemperatureInput.Name = "ambientTemperatureInput";
            this.ambientTemperatureInput.Size = new System.Drawing.Size(40, 26);
            this.ambientTemperatureInput.TabIndex = 9;
            this.ambientTemperatureInput.Text = "75";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(315, 20);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(106, 20);
            this.label7.TabIndex = 14;
            this.label7.Text = "Displacement";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(503, 20);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(25, 20);
            this.label8.TabIndex = 13;
            this.label8.Text = "cc";
            // 
            // displacementInput
            // 
            this.displacementInput.Location = new System.Drawing.Point(431, 15);
            this.displacementInput.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.displacementInput.Name = "displacementInput";
            this.displacementInput.Size = new System.Drawing.Size(61, 26);
            this.displacementInput.TabIndex = 12;
            this.displacementInput.Text = "2497";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(544, 53);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(191, 20);
            this.label9.TabIndex = 17;
            this.label9.Text = "Intercooler Pressure Drop";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(781, 58);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 20);
            this.label10.TabIndex = 16;
            this.label10.Text = "PSI";
            // 
            // pressureDropTextBox
            // 
            this.pressureDropTextBox.Location = new System.Drawing.Point(729, 53);
            this.pressureDropTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pressureDropTextBox.Name = "pressureDropTextBox";
            this.pressureDropTextBox.Size = new System.Drawing.Size(41, 26);
            this.pressureDropTextBox.TabIndex = 15;
            this.pressureDropTextBox.Text = "2";
            // 
            // PowerEstimatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 842);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.pressureDropTextBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.displacementInput);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.ambientTemperatureInput);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.maxTemperatureInput);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.findBoostCurve);
            this.Controls.Add(this.showCurvesButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.richTextBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(1222, 898);
            this.Name = "PowerEstimatorForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button showCurvesButton;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button findBoostCurve;
        private System.Windows.Forms.TextBox maxTemperatureInput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox ambientTemperatureInput;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox displacementInput;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox pressureDropTextBox;
    }
}

