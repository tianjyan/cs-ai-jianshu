using System;

namespace JianShuCore.CryptoCore
{
    struct ABCDStruct
    {
        internal uint A;
        internal uint B;
        internal uint C;
        internal uint D;
    }

    internal sealed class MD5Core
    {
        internal static byte[] GetHash(byte[] input)
        {
            if (input == null)
            {
                throw new System.ArgumentNullException("input", "Unable to calculate hash over null input data");
            }

            //Intitial values defined in RFC 1321
            ABCDStruct abcd = new ABCDStruct();
            abcd.A = 0x67452301;
            abcd.B = 0xefcdab89;
            abcd.C = 0x98badcfe;
            abcd.D = 0x10325476;

            //We pass in the input array by block, the final block of data must be handled specialy for padding & length embeding
            int startIndex = 0;
            while (startIndex <= input.Length - 64)
            {
                MD5Core.GetHashBlock(input, ref abcd, startIndex);
                startIndex += 64;
            }
            // The final data block. 
            return MD5Core.GetHashFinalBlock(input, startIndex, input.Length - startIndex, abcd, (Int64)input.Length * 8);
        }

        private static byte[] GetHashFinalBlock(byte[] input, int ibStart, int cbSize, ABCDStruct ABCD, Int64 len)
        {
            byte[] working = new byte[64];
            byte[] length = BitConverter.GetBytes(len);

            //Padding is a single bit 1, followed by the number of 0s required to make size congruent to 448 modulo 512. Step 1 of RFC 1321  
            //The CLR ensures that our buffer is 0-assigned, we don't need to explicitly set it. This is why it ends up being quicker to just
            //use a temporary array rather then doing in-place assignment (5% for small inputs)
            Array.Copy(input, ibStart, working, 0, cbSize);
            working[cbSize] = 0x80;

            //We have enough room to store the length in this chunk
            if (cbSize <= 56)
            {
                Array.Copy(length, 0, working, 56, 8);
                GetHashBlock(working, ref ABCD, 0);
            }
            else  //We need an aditional chunk to store the length
            {
                GetHashBlock(working, ref ABCD, 0);
                //Create an entirely new chunk due to the 0-assigned trick mentioned above, to avoid an extra function call clearing the array
                working = new byte[64];
                Array.Copy(length, 0, working, 56, 8);
                GetHashBlock(working, ref ABCD, 0);
            }
            byte[] output = new byte[16];
            Array.Copy(BitConverter.GetBytes(ABCD.A), 0, output, 0, 4);
            Array.Copy(BitConverter.GetBytes(ABCD.B), 0, output, 4, 4);
            Array.Copy(BitConverter.GetBytes(ABCD.C), 0, output, 8, 4);
            Array.Copy(BitConverter.GetBytes(ABCD.D), 0, output, 12, 4);
            return output;
        }

        // Performs a single block transform of MD5 for a given set of ABCD inputs
        /* If implementing your own hashing framework, be sure to set the initial ABCD correctly according to RFC 1321:
        A = 0x67452301
        B = 0xefcdab89
        C = 0x98badcfe
        D = 0x10325476
        */
        private static void GetHashBlock(byte[] input, ref ABCDStruct ABCDValue, int ibStart)
        {
            uint[] temp = Converter(input, ibStart);
            uint a = ABCDValue.A;
            uint b = ABCDValue.B;
            uint c = ABCDValue.C;
            uint d = ABCDValue.D;

            a = FF(a, b, c, d, temp[0], 7, 0xd76aa478);
            d = FF(d, a, b, c, temp[1], 12, 0xe8c7b756);
            c = FF(c, d, a, b, temp[2], 17, 0x242070db);
            b = FF(b, c, d, a, temp[3], 22, 0xc1bdceee);
            a = FF(a, b, c, d, temp[4], 7, 0xf57c0faf);
            d = FF(d, a, b, c, temp[5], 12, 0x4787c62a);
            c = FF(c, d, a, b, temp[6], 17, 0xa8304613);
            b = FF(b, c, d, a, temp[7], 22, 0xfd469501);
            a = FF(a, b, c, d, temp[8], 7, 0x698098d8);
            d = FF(d, a, b, c, temp[9], 12, 0x8b44f7af);
            c = FF(c, d, a, b, temp[10], 17, 0xffff5bb1);
            b = FF(b, c, d, a, temp[11], 22, 0x895cd7be);
            a = FF(a, b, c, d, temp[12], 7, 0x6b901122);
            d = FF(d, a, b, c, temp[13], 12, 0xfd987193);
            c = FF(c, d, a, b, temp[14], 17, 0xa679438e);
            b = FF(b, c, d, a, temp[15], 22, 0x49b40821);

            a = GG(a, b, c, d, temp[1], 5, 0xf61e2562);
            d = GG(d, a, b, c, temp[6], 9, 0xc040b340);
            c = GG(c, d, a, b, temp[11], 14, 0x265e5a51);
            b = GG(b, c, d, a, temp[0], 20, 0xe9b6c7aa);
            a = GG(a, b, c, d, temp[5], 5, 0xd62f105d);
            d = GG(d, a, b, c, temp[10], 9, 0x02441453);
            c = GG(c, d, a, b, temp[15], 14, 0xd8a1e681);
            b = GG(b, c, d, a, temp[4], 20, 0xe7d3fbc8);
            a = GG(a, b, c, d, temp[9], 5, 0x21e1cde6);
            d = GG(d, a, b, c, temp[14], 9, 0xc33707d6);
            c = GG(c, d, a, b, temp[3], 14, 0xf4d50d87);
            b = GG(b, c, d, a, temp[8], 20, 0x455a14ed);
            a = GG(a, b, c, d, temp[13], 5, 0xa9e3e905);
            d = GG(d, a, b, c, temp[2], 9, 0xfcefa3f8);
            c = GG(c, d, a, b, temp[7], 14, 0x676f02d9);
            b = GG(b, c, d, a, temp[12], 20, 0x8d2a4c8a);

            a = HH(a, b, c, d, temp[5], 4, 0xfffa3942);
            d = HH(d, a, b, c, temp[8], 11, 0x8771f681);
            c = HH(c, d, a, b, temp[11], 16, 0x6d9d6122);
            b = HH(b, c, d, a, temp[14], 23, 0xfde5380c);
            a = HH(a, b, c, d, temp[1], 4, 0xa4beea44);
            d = HH(d, a, b, c, temp[4], 11, 0x4bdecfa9);
            c = HH(c, d, a, b, temp[7], 16, 0xf6bb4b60);
            b = HH(b, c, d, a, temp[10], 23, 0xbebfbc70);
            a = HH(a, b, c, d, temp[13], 4, 0x289b7ec6);
            d = HH(d, a, b, c, temp[0], 11, 0xeaa127fa);
            c = HH(c, d, a, b, temp[3], 16, 0xd4ef3085);
            b = HH(b, c, d, a, temp[6], 23, 0x04881d05);
            a = HH(a, b, c, d, temp[9], 4, 0xd9d4d039);
            d = HH(d, a, b, c, temp[12], 11, 0xe6db99e5);
            c = HH(c, d, a, b, temp[15], 16, 0x1fa27cf8);
            b = HH(b, c, d, a, temp[2], 23, 0xc4ac5665);

            a = II(a, b, c, d, temp[0], 6, 0xf4292244);
            d = II(d, a, b, c, temp[7], 10, 0x432aff97);
            c = II(c, d, a, b, temp[14], 15, 0xab9423a7);
            b = II(b, c, d, a, temp[5], 21, 0xfc93a039);
            a = II(a, b, c, d, temp[12], 6, 0x655b59c3);
            d = II(d, a, b, c, temp[3], 10, 0x8f0ccc92);
            c = II(c, d, a, b, temp[10], 15, 0xffeff47d);
            b = II(b, c, d, a, temp[1], 21, 0x85845dd1);
            a = II(a, b, c, d, temp[8], 6, 0x6fa87e4f);
            d = II(d, a, b, c, temp[15], 10, 0xfe2ce6e0);
            c = II(c, d, a, b, temp[6], 15, 0xa3014314);
            b = II(b, c, d, a, temp[13], 21, 0x4e0811a1);
            a = II(a, b, c, d, temp[4], 6, 0xf7537e82);
            d = II(d, a, b, c, temp[11], 10, 0xbd3af235);
            c = II(c, d, a, b, temp[2], 15, 0x2ad7d2bb);
            b = II(b, c, d, a, temp[9], 21, 0xeb86d391);

            ABCDValue.A = unchecked(a + ABCDValue.A);
            ABCDValue.B = unchecked(b + ABCDValue.B);
            ABCDValue.C = unchecked(c + ABCDValue.C);
            ABCDValue.D = unchecked(d + ABCDValue.D);
            return;
        }

        //Manually unrolling these equations nets us a 20% performance improvement
        private static uint FF(uint a, uint b, uint c, uint d, uint x, int s, uint t)
        {
            //                  (b + LSR((a + F(b, c, d) + x + t), s))
            //F(x, y, z)        ((x & y) | ((x ^ 0xFFFFFFFF) & z))
            return unchecked(b + LSR((a + ((b & c) | ((b ^ 0xFFFFFFFF) & d)) + x + t), s));
        }

        private static uint GG(uint a, uint b, uint c, uint d, uint x, int s, uint t)
        {
            //                  (b + LSR((a + G(b, c, d) + x + t), s))
            //G(x, y, z)        ((x & z) | (y & (z ^ 0xFFFFFFFF)))
            return unchecked(b + LSR((a + ((b & d) | (c & (d ^ 0xFFFFFFFF))) + x + t), s));
        }

        private static uint HH(uint a, uint b, uint c, uint d, uint x, int s, uint t)
        {
            //                  (b + LSR((a + H(b, c, d) + k + i), s))
            //H(x, y, z)        (x ^ y ^ z)
            return unchecked(b + LSR((a + (b ^ c ^ d) + x + t), s));
        }

        private static uint II(uint a, uint b, uint c, uint d, uint x, int s, uint t)
        {
            //                  (b + LSR((a + I(b, c, d) + k + i), s))
            //I(x, y, z)        (y ^ (x | (z ^ 0xFFFFFFFF)))
            return unchecked(b + LSR((a + (c ^ (b | (d ^ 0xFFFFFFFF))) + x + t), s));
        }

        // Implementation of left rotate
        // s is an int instead of a uint becuase the CLR requires the argument passed to >>/<< is of 
        // type int. Doing the demoting inside this function would add overhead.
        private static uint LSR(uint i, int s)
        {
            return ((i << s) | (i >> (32 - s)));
        }

        //Convert input array into array of UInts
        private static uint[] Converter(byte[] input, int ibStart)
        {
            if (input == null)
            {
                throw new System.ArgumentNullException("input", "Unable convert null array to array of uInts");
            }

            uint[] result = new uint[16];

            for (int i = 0; i < 16; i++)
            {
                result[i] = (uint)input[ibStart + i * 4];
                result[i] += (uint)input[ibStart + i * 4 + 1] << 8;
                result[i] += (uint)input[ibStart + i * 4 + 2] << 16;
                result[i] += (uint)input[ibStart + i * 4 + 3] << 24;
            }

            return result;
        }
    }
}


