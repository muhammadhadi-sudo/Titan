using System.Globalization;

namespace Titan.Controllers;

static class Utilities
{
    public static bool ConvertHexStringToByteArray(string pHexString, byte[] pByteArray, int maxSize)
    {
        int iLen = pHexString.Length;
        bool rv = false;

        if (iLen > 0 && pHexString.Length % 2 == 0)
        {
            for (int index = 0; index < (iLen / 2) && index < maxSize; index++)
            {
                string byteValue = pHexString.Substring(index * 2, 2);
                byte byteVal = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                pByteArray[index] = byteVal;
            }
            rv = true;
        }
        return rv;
    }

    public static string ConvertHexStringToByteArray(byte[] pByteArray)
    {
        return BitConverter.ToString(pByteArray);
    }
}