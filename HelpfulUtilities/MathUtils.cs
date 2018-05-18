using System;

namespace HelpfulUtilities
{
    /// <summary>
    /// Deal with this later.
    /// </summary>
    internal class MathUtils
    {
        public sbyte Sign(sbyte sign, sbyte number) => (sbyte)(Math.Sign(sign) * Math.Abs(number));
        public short Sign(short sign, short number) => (short)(Math.Sign(sign) * Math.Abs(number));

        public int Sign(int sign, int number) => Math.Sign(sign) * Math.Abs(number);
        public long Sign(long sign, long number) => Math.Sign(sign) * Math.Abs(number);

        public float Sign(float sign, float number) => Math.Sign(sign) * Math.Abs(number);
        public double Sign(double sign, double number) => Math.Sign(sign) * Math.Abs(number);
        public decimal Sign(decimal sign, decimal number) => Math.Sign(sign) * Math.Abs(number);


        public double Tanh(double value, double min, double max)
        {
            var multiplier = (max - min) / 2;
            var average = (max + min) / 2;
            return multiplier * Math.Tanh(value) + average;
        }
        
    }
}
