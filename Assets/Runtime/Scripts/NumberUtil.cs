using System.Collections.Generic;
using UnityEngine;

namespace Leniton.NumberUtilities
{
    public static class NumberUtil
    {
        public static bool ContainsBytes(int value, int comparingValue) => (comparingValue ^ value) == (comparingValue - value);
        public static int Invert(int value, int maxValue) => Mathf.Abs(value - maxValue);
        public static float Invert(float value, float maxValue) => Mathf.Abs(value - maxValue);
        public static bool ContainsAnyBits(int value, int comparingValue)
        {
            int[] bits = SeparateBits(value);
            for (int i = 0; i < bits.Length; i++)
                if (ContainsBytes(bits[i], comparingValue))
                    return true;
            return false;
        }
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

        float NormalizedDistance(float value, float target, float range)
        {
            return Mathf.Clamp01(1 - Mathf.Abs(value - target) / range);
        }
        float WithinTargetDistance(float value, float min, float max)
        {
            return (value - min) / (max - min);
        }
        float DetermineAlphaValue(float value, float min, float max, float range)
        {
            float alphaValue = NormalizedDistance(value, min, range) +
                    Mathf.Clamp01(NormalizedDistance(value, max, range) +
                    Mathf.Ceil(NormalizedDistance(value, min + ((max - min) / 2), (max - min) / 2))) *
                    Mathf.Ceil(Mathf.Clamp01(WithinTargetDistance(value, min, max)));
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