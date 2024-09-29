using System.Collections.Generic;
using UnityEngine;

namespace Lenix.NumberUtilities
{
    public static class NumberUtil
    {
        public static int Invert(int value, int maxValue) => Mathf.Abs(value - maxValue);
        public static float Invert(float value, float maxValue) => Mathf.Abs(value - maxValue);

        /// <summary>
        /// Checks if given value contains given bits
        /// </summary>
        /// <param name="value">The value being checked</param>
        /// <param name="bytes">the bytes it must contain</param>
        /// <returns>true if value contains all bytes, otherwise false</returns>
        public static bool ContainsBytes(int value, int bytes) => (value ^ bytes) == (value - bytes);

        /// <summary>
        /// Checks if given value contains any of the given bits
        /// </summary>
        /// <param name="value">The value being checked</param>
        /// <param name="bytes">the bytes it must contain</param>
        /// <returns>true if value contains any of the bytes, otherwise false</returns>
        public static bool ContainsAnyBits(int value, int bytes)
        {
            int[] bits = SeparateBits(bytes);
            for (int i = 0; i < bits.Length; i++)
                if (ContainsBytes(value, bits[i]))
                    return true;
            return false;
        }
        /// <summary>
        /// Separate a value into its individual bits
        /// </summary>
        /// <param name="value">Value being divided</param>
        /// <returns>An array with each bit</returns>
        public static int[] SeparateBits(int value)
        {
            List<int> bits = new List<int>();
            int comparingValue = 1;
            for (int i = 0; i < 32; i++)
            {
                if ((comparingValue & value) != 0)
                    bits.Add(comparingValue);
                comparingValue <<= 1;
            }
            return bits.ToArray();
        }

        /// <summary>
        /// Gives the value of a point in a sine wave given the time
        /// </summary>
        /// <param name="time">The point in time</param>
        /// <param name="amplitude">The amplitude of the sine wave</param>
        /// <param name="frequency">The frequency of the wave</param>
        /// <param name="offset">Wave time offset</param>
        /// <returns>A value between amplitude and -amplitude based on the parameters</returns>
        public static float SineWave(float time, float amplitude, float frequency, float offset = 0) => amplitude * (Mathf.Sin(2 * Mathf.PI * frequency * time + offset));

        /// <summary>
        /// Normalizes value to given min/max range
        /// </summary>
        /// <param name="value">The value being normalized</param>
        /// <param name="min">The minimum value of range</param>
        /// <param name="max">The maximum value of range</param>
        /// <returns>A percentage representing where the original value falls within the given range.</returns>
        public static float NormalizeToRange(float value, float min, float max)
        {
            return (value - min) / (max - min);
        }

        /// <summary>
        /// Calculates how far the value is from target given the range
        /// </summary>
        /// <param name="value">The value you want to check</param>
        /// <param name="target">The target value to compare</param>
        /// <param name="range">The range in which the distance is verified</param>
        /// <returns>
        /// A value between 0 and 1 representing how far from the target is the original value, 
        /// where 1 means the value is equal to the target and 
        /// 0 the value distance from target is bigger or equal to the range.
        /// </returns>
        public static float DistanceFromTarget(float value, float target, float range)
        {
            return Mathf.Clamp01(1 - Mathf.Abs(value - target) / range);
        }

        /// <summary>
        /// Calculates how far the value is from the area between min and max given the range
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min">The minimum value of area</param>
        /// <param name="max">The maximum value of area</param>
        /// <param name="range">The range in which the distance is verified</param>
        /// <returns>
        /// A value between 0 and 1 representing how far from the area is the original value, 
        /// where 1 means the value is inside the area and 
        /// 0 the value distance from the area is bigger or equal to the range.
        /// Values in between represents the distance from the area based on the range
        /// </returns>
        public static float DistanceFromArea(float value, float min, float max, float range)
        {
            float alphaValue = DistanceFromTarget(value, min, range) +
                    Mathf.Clamp01(DistanceFromTarget(value, max, range) +
                    Mathf.Ceil(DistanceFromTarget(value, min + ((max - min) / 2), (max - min) / 2))) *
                    Mathf.Ceil(Mathf.Clamp01(NormalizeToRange(value, min, max)));
            return alphaValue;
        }
    }

    public struct bit
    {
        private byte value;
        public bit(bool on = false) => value = (byte)(on ? 1 : 0);
        private static byte ToBit(byte by) => (byte)(by > 0 ? 1 : 0);

        public static implicit operator byte(bit bit) => bit.value;
        public static implicit operator int(bit bit) => bit.value;
        public static explicit operator bit(int n) => new bit(n > 0);

        public static bit operator +(bit b, byte by)
        {
            b.value = ToBit((byte)(b.value + by));
            return b;
        }
        public static bit operator -(bit b, byte by)
        {
            b.value = ToBit((byte)(b.value - by));
            return b;
        }
        public static bit operator |(bit b, byte by)
        {
            b.value = (byte)(ToBit(by) | b.value);
            return b;
        }
        public static bit operator &(bit b, byte by)
        {
            b.value = (byte)(ToBit(by) & b.value);
            return b;
        }
        public static bit operator ^(bit b, byte by)
        {
            b.value = (byte)(ToBit(by) ^ b.value);
            return b;
        }
        public static bit operator ~(bit b) => b ^ 1;
        public static bit operator !(bit b) => ~b;

        public override string ToString() => value.ToString();
    }
}