using System.Text;
using Lab1;

namespace Lab4_server
{
    public static class Services
    {
        public static string Calculate(byte[] buffer, int range)
        {
            var data = ConvertByteArrayToIntArray(buffer);

            var result = data.GetModeAndMedianSingleThread(range);

            return $"Mode: {result.Item1}; Median: {result.Item2}";
        }

        static int[] ConvertByteArrayToIntArray(byte[] byteArray)
        {
            int[] intArray = new int[byteArray.Length / 4];

            for (int i = 0; i < intArray.Length; i++)
            {
                intArray[i] = BitConverter.ToInt32(byteArray, i * 4);
            }

            return intArray;
        }

        static byte[] ConvertIntArrayToByteArray(int[] intArray)
        {
            byte[] byteArray = new byte[intArray.Length * 4];

            for (int i = 0; i < intArray.Length; i++)
            {
                byte[] bytes = BitConverter.GetBytes(intArray[i]);
                Array.Copy(bytes, 0, byteArray, i * 4, 4);
            }

            return byteArray;
        }
    }
}
