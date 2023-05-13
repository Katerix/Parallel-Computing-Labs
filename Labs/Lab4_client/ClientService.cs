
namespace Lab4_client
{
    public static class ClientService
    {
        public static int[] ConvertByteArrayToIntArray(byte[] byteArray)
        {
            int[] intArray = new int[byteArray.Length / 4];

            for (int i = 0; i < intArray.Length; i++)
            {
                intArray[i] = BitConverter.ToInt32(byteArray, i * 4);
            }

            return intArray;
        }

        public static byte[] ConvertIntArrayToByteArray(int[] intArray)
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
