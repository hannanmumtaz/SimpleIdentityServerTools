using System;

namespace SimpleIdentityServer.Eid.Helpers
{
    internal static class ByteCodeHelper
    {
        public static void ArrayCopy(Array src, int srcStart, Array dest, int destStart, int len)
        {
            Buffer.BlockCopy(src, srcStart, dest, destStart, len);
        }
    }
}
