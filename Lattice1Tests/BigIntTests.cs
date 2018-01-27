using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lattice1;
using System.Linq;
using System.Numerics;

namespace Lattice1Tests
{
    [TestClass]
    public class BigIntTests
        {

        [TestMethod]
        public void SubtractOneInPlace1()
        {
            byte[] a = new byte[] { 1 };
            BigInt.SubtractOneInPlace(a);
            Assert.IsTrue(Enumerable.SequenceEqual(new byte[] { 0 }, a));
        }

        [TestMethod]
        public void SubtractOneInPlace2()
        {
            byte[] a = new byte[] { 1, 2, 3, 4 };
            BigInt.SubtractOneInPlace(a);
            Assert.IsTrue(Enumerable.SequenceEqual(new byte[] { 1, 2, 3, 3 }, a));
        }

        [TestMethod]
        public void SubtractOneInPlace3()
        {
            byte[] a = new byte[] { 1, 2, 3, 0 };
            BigInt.SubtractOneInPlace(a);
            Assert.IsTrue(Enumerable.SequenceEqual(new byte[] { 1, 2, 2, 9 }, a));
        }

        [TestMethod]
        public void SubtractOneInPlace4()
        {
            byte[] a = new byte[] { 1, 8, 0, 0 };
            BigInt.SubtractOneInPlace(a);
            Assert.IsTrue(Enumerable.SequenceEqual(new byte[] { 1, 7, 9, 9 }, a));
        }

        [TestMethod]
        public void SubtractOneInPlace5()
        {
            byte[] a = new byte[] { 0, 2, 0, 0 };
            BigInt.SubtractOneInPlace(a);
            Assert.IsTrue(Enumerable.SequenceEqual(new byte[] { 0, 1, 9, 9 }, a));
        }

        [TestMethod]
        public void SubtractOneInPlace6()
        {
            byte[] a = new byte[] { 1, 0, 0 };
            BigInt.SubtractOneInPlace(a);
            Assert.IsTrue(Enumerable.SequenceEqual(new byte[] { 0, 9, 9 }, a));
        }

        [TestMethod]
        public void SubtractOneInPlace7()
        {
            byte[] a = new byte[] { 1, 1, 0, 0 };
            BigInt.SubtractOneInPlace(a);
            Assert.IsTrue(Enumerable.SequenceEqual(new byte[] { 1, 0, 9, 9 }, a));
        }

        public static ulong PlainFactorial(ulong n)
        {
            ulong count = n - 1;

            ulong mult = n;
            while (true)
            {
                mult = count * mult;
                count--;
                if (count == 0) break;
            }
            return mult;
        }

        [TestMethod]
        public void PlainFactorial19()
        {
            ulong result = PlainFactorial(19);
            BigInteger r = NumericsFactorial(19);
            Console.WriteLine("plain factorial result: " + result);
            Assert.AreEqual(r.ToString(), result.ToString());
        }

        [TestMethod]
        public void PlainFactorial29()
        {
            ulong result = PlainFactorial(29);
            BigInteger r = NumericsFactorial(29);
            Console.WriteLine("plain factorial result: " + result);
            Assert.AreNotEqual(r.ToString(), result.ToString());
        }

        public static BigInteger NumericsFactorial(ulong n)
        {
            ulong count = n - 1;

            BigInteger mult = new BigInteger(n);
            while (true)
            {
                mult = BigInteger.Multiply(count, mult);
                count--;
                if (count == 0) break;
            }
            return mult;
        }

        public string ClearZeroes(byte[] result)
        {
            for (int i = 0; i < result.Length; i++)
            {
                if (result[i] > 0)
                {
                    byte[] r = new byte[result.Length - i];
                    Array.ConstrainedCopy(result, i, r, 0, result.Length - i);
                    return string.Join("", r);
                }
            }
            return string.Join("", result);
        }

        [TestMethod]
        public void Factorial19()
        {
            TestFactorial(19);
        }

        [TestMethod]
        public void Factorial29()
        {
            TestFactorial(29);
        }

        [TestMethod]
        public void Factorial99()
        {
            TestFactorial(99);
        }

        [TestMethod]
        void TestFactorial(ulong n)
        {
            byte[] nByte = BigInt.MakeByteArray(n);
            byte[] result = BigInt.Factorial(nByte);
            string r = ClearZeroes(result);
            BigInteger result2 = NumericsFactorial(n);
            Assert.AreEqual(r, result2.ToString());
        }

        [TestMethod]
        public void Factorial199()
        {
            TestFactorial(199);
        }

        [TestMethod]
        public void Factorial999()
        {
            TestFactorial(999);
        }

        [TestMethod]
        public void Factorial1999()
        {
            TestFactorial(1999);
        }

        [TestMethod]
        public void Factorial2999()
        {
            TestFactorial(2999);
        }

        [TestMethod]
        public void Factorial4999()
        {
            TestFactorial(4999);
        }

        [TestMethod]
        public void Factorial8999()
        {
            TestFactorial(8999);
        }

        [TestMethod]
        public void LatticeMultiplication1()
        {
            byte[] a = new byte[] { 1, 2, 3, 4, 5 };
            byte[] b = new byte[] { 1, 2, 3 };
            byte[] result = BigInt.LatticeMultiplication(a, b);
            Assert.IsTrue(Enumerable.SequenceEqual(new byte[] { 0, 1, 5, 1, 8, 4, 3, 5 }, result));
        }

        [TestMethod]
        public void LatticeMultiplication2()
        {
            byte[] a = new byte[] { 1, 2, 3 };
            byte[] b = new byte[] { 1, 2, 3, 4, 5 };
            byte[] result = BigInt.LatticeMultiplication(a, b);
            Assert.IsTrue(Enumerable.SequenceEqual(new byte[] { 0, 1, 5, 1, 8, 4, 3, 5 }, result));
        }

        [TestMethod]
        public void LatticeMultiplication3()
        {
            byte[] a = new byte[] { 1, 2, 3, 4 };
            byte[] b = new byte[] { 1, 2, 3, 4 };
            byte[] result = BigInt.LatticeMultiplication(a, b);
            Assert.IsTrue(Enumerable.SequenceEqual(new byte[] { 0, 1, 5, 2, 2, 7, 5, 6 }, result));
        }

        [TestMethod]
        public void LatticeMultiplication4()
        {
            byte[] a = new byte[] { 9, 9, 9, 9, 9, 9, 9, 9, 9 };
            byte[] b = new byte[] { 9, 9, 9, 9, 9, 9, 9, 9, 9 };
            byte[] result = BigInt.LatticeMultiplication(a, b);
            Assert.IsTrue(Enumerable.SequenceEqual(new byte[] { 9, 9, 9, 9, 9, 9, 9, 9, 8,
            0, 0, 0, 0, 0, 0, 0, 0, 1}, result));
        }
    }
}
