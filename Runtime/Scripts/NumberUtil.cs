using UnityEngine;

namespace Leniton.NumberUtilities
{
    public class NumberUtil
    {
        public static bool ContainsBytes(int value, int comparingValue) => (comparingValue ^ value) == (comparingValue - value);
        public static int Invert(int value, int maxValue) => Mathf.Abs(value - maxValue);
        public static float Invert(float value, float maxValue) => Mathf.Abs(value - maxValue);
    }
}