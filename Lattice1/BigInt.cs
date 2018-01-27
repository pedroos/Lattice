using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lattice1
{
    public static class BigInt
    {
        public static byte[] Factorial(byte[] n)
        {
            byte[] count = new byte[n.Length];
            n.CopyTo(count, 0);
            SubtractOneInPlace(count);

            byte[] mult = new byte[n.Length];
            n.CopyTo(mult, 0);
            while (true)
            {
                mult = LatticeMultiplication(count, mult);
                SubtractOneInPlace(count);
                if (IsZero(count)) break;
            }
            return mult;
        }

        public static byte[] Factorial(uint n)
        {
            uint count = n - 1;

            byte[] mult = MakeByteArray(n);
            while (true)
            {
                byte[] countByte = MakeByteArray(count);
                mult = LatticeMultiplication(countByte, mult);
                count--;
                if (count == 0) break;
            }
            return mult;
        }

        public static void SubtractOneInPlace(byte[] n)
        {
            if (n[n.Length - 1] >= 1)
            {
                n[n.Length - 1] = (byte)(n[n.Length - 1] - 1);
                return;
            }
            n[n.Length - 1] = 9;
            for (int i = n.Length - 1; i > 0; i--)
            {
                if (!(n[i - 1] > 0))
                {
                    n[i - 1] = 9;
                    continue;
                }
                n[i - 1] = (byte)(n[i - 1] - 1);
                break;
            }
        }

        public static void MultiplyByLessOneInPlace(byte[] n)
        {
            byte[] result = new byte[n.Length * n.Length * 2];
            for (int y = n.Length; y > 0; y--)
            {
                for (int x = n.Length; x > 0; x--)
                {
                    byte mx = n[x - 1];
                    byte my;
                    if (y == n.Length)
                    {
                        if (n[y - 1] >= 1)
                        {
                            my = (byte)(n[y - 1] - 1);
                        }
                        else
                        {
                            my = 9;
                        }
                    }
                    else
                    {
                        if (n[y] >= 1)
                        {
                            my = n[y - 1];
                        }
                        else
                        {
                            if (n[y - 1] == 0)
                            {
                                my = 9;
                            }
                            else
                            {
                                my = (byte)(n[y - 1] - 1);
                            }
                        }
                    }
                    byte mult = (byte)(mx * my);

                    int resultCell = ((y - 1) * n.Length * 2) + ((x - 1) * 2);
                    if (mult < 10)
                    {
                        result[resultCell] = 0;
                        result[resultCell + 1] = mult;
                    }
                    else
                    {
                        string _mult = mult.ToString();
                        result[resultCell] = byte.Parse(_mult[0].ToString());
                        result[resultCell + 1] = byte.Parse(_mult[1].ToString());
                    }
                }
            }
        }

        private static bool IsZero(byte[] a)
        {
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != 0) return false;
            }
            return true;
        }

        [Flags]
        enum LatticeOrientation
        {
            None = 0,
            Vertical = 1,
            Horizontal = 2
        }

        public static byte[] LatticeMultiplication(byte[] a, byte[] b)
        {
            long d1 = a.Length;
            long d2 = b.Length;
            long x = d1;
            long y = d2;
            bool odd = false;
            byte phase = 1;
            LatticeOrientation orientation = LatticeOrientation.None;

            byte[] digits = new byte[d1 + d2];
            long digitPos = digits.Length;
            long sum = 0;
            byte acc = 0;

            while (true)
            {
                // compute the multiplication
                byte ma = a[x - 1];
                byte mb = b[y - 1];
                byte mult = (byte)(ma * mb);
                string _mult = mult.ToString();
                byte digit;
                if (mult < 10)
                {
                    digit = odd ? (byte)0 : mult;
                }
                else
                {
                    digit = byte.Parse(_mult[odd ? 0 : 1].ToString());
                }
                sum += digit;

                // check line ends
                bool lineEndPhase1 = phase == 1 && y == d2 && !odd;
                bool lineEndPhase2V = phase == 2 && orientation == LatticeOrientation.Vertical && x == 1 && odd;
                bool lineEndPhase2H = phase == 2 && orientation == LatticeOrientation.Horizontal && y == d2 && !odd;
                bool lineEndPhase3 = phase == 3 && x == 1 && odd;

                if (lineEndPhase1 || lineEndPhase2V || lineEndPhase2H || lineEndPhase3)
                {
                    // finalize sum
                    if (acc != 0)
                    {
                        sum += acc;
                        acc = 0;
                    }
                    if (sum >= 10)
                    {
                        string _sum = sum.ToString();
                        acc = byte.Parse(_sum[0].ToString());
                        byte sumDigit = byte.Parse(_sum[1].ToString());
                        digits[digitPos - 1] = sumDigit;
                    }
                    else
                    {
                        digits[digitPos - 1] = (byte)sum;
                    }
                    digitPos--;
                    sum = 0;
                }

                if (lineEndPhase1)
                {
                    // check phase change
                    if (x == d1 + 1 - d2)
                    {
                        orientation = orientation | LatticeOrientation.Horizontal;
                    }
                    if (x == 1)
                    {
                        orientation = orientation | LatticeOrientation.Vertical;
                    }

                    // phase changing
                    if (orientation != LatticeOrientation.None)
                    {
                        if (orientation.HasFlag(LatticeOrientation.Horizontal) &&
                            orientation.HasFlag(LatticeOrientation.Vertical))
                        {
                            phase = 3;
                            // use the new phase jump, but:
                            x = y;
                            y = 1;
                            odd = !odd; // and odd toggles to true
                        }
                        else
                        {
                            phase = 2;
                            if (orientation == LatticeOrientation.Horizontal)
                            {
                                // use the new phase jump
                                y = 1;
                                x += d2 - 1;
                                odd = !odd; // odd toggles to true
                            }
                            else if (orientation == LatticeOrientation.Vertical)
                            {
                                // don't use the new phase jump, use this phase's jump
                                y -= d1 + 1 - x;
                                x = d1;
                                // odd stays false
                            }
                        }
                    }
                    else
                    {
                        y -= d1 + 1 - x;
                        x = d1;
                        // odd stays false
                    }
                }
                else if (lineEndPhase2V)
                {
                    // check phase change
                    if (y == d1 + 1)
                    {
                        phase = 3;
                        // use the new phase jump
                        x = y - 1;
                        y = 1;
                    }
                    else
                    {
                        x = d1;
                        y -= d1 + 1;
                        odd = !odd; // odd toggles to false
                    }
                }
                else if (lineEndPhase2H)
                {
                    // check phase change
                    if (x == 1)
                    {
                        phase = 3;
                        // don't use the new phase jump, use this phase's jump
                        y = 1;
                        x += d2 - 1;
                        odd = !odd; // odd toggles to true
                    }
                    else
                    {
                        y = 1;
                        x += d2 - 1;
                        odd = !odd; // odd toggles to true
                    }
                }
                else if (lineEndPhase3)
                {
                    // check end
                    if (y == 1)
                    {
                        break;
                    }
                    else
                    {
                        x = y - 1;
                        y = 1;
                        // odd stays true
                    }
                }
                else
                {
                    // no end of line
                    if (!odd)
                    {
                        y += 1;
                    }
                    else
                    {
                        x -= 1;
                    }
                    odd = !odd;
                }
            }

            return digits;
        }

        public static byte[] MakeByteArray(ulong n)
        {
            if (n < 10) return new byte[] { (byte)n };
            string _n = n.ToString();
            byte[] nb = new byte[_n.Length];
            for (int i = 0; i < _n.Length; i++)
            {
                nb[i] = byte.Parse(_n[i].ToString());
            }
            return nb;
        }
    }
}
