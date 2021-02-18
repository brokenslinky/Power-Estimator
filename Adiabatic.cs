using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Power_Estimator
{
    class Adiabatic
    {
        /* Adiabatic equations:
         * pressure * volume ^ adiabaticIndex                             = constant
         * pressure ^ (1 - adiabaticIndex) * temperature ^ adiabaticIndex = constant
         * temperature * volume ^ (adiabaticIndex - 1)                    = constant
         * 
         * From Ideal Gas Law
         * density * temperature / pressure                               = constant
         */

        static double adiabaticIndex = 1.4;

        public static double TemperatureRatio(double pressureRatio, double compressorEfficiency=1.0)
        {
            return Math.Pow(pressureRatio, 1.0 - 1.0 / adiabaticIndex) / compressorEfficiency + 1.0 - 1.0 / compressorEfficiency;
        }

        /// Factor by which density increases through the compressor
        public static double BoostMultiplier(double pressureRatio, double compressorEfficiency=1.0)
        {
            return DensityMultiplier(pressureRatio, TemperatureRatio(pressureRatio, compressorEfficiency));
        }

        public static double DensityMultiplier(double pressureRatio=1.0, double temperatureRatio=1.0)
        {
            return pressureRatio / temperatureRatio;
        }

    }
}
