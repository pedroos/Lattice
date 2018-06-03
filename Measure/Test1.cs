using System;
using System.Diagnostics;
using System.Text;

namespace Lattice1.Measure {
    class Test1 : Test {
        public override TestResult Run() {
            ulong n = 999;

            Stopwatch sw = new Stopwatch();

            TestResult tr = new TestResult();

            for (int i = 0; i < 10; i++) {
                sw.Restart();
                byte[] nByte = BigInt.MakeByteArray(n);
                sw.Stop();
                long t = sw.ElapsedTicks;
                tr.AddSegment("Make byte array", t);
                Console.WriteLine(tr.SegmentString());

                sw.Restart();
                byte[] result = BigInt.Factorial(nByte);
                sw.Stop();
                t = sw.ElapsedTicks;
                tr.AddSegment("Factorial", t);
                Console.WriteLine(tr.SegmentString());
            }

            return tr;
        }

        public override string ToCsv(TestResult tr) {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0};", GetType().Name);
            sb.Append(Environment.NewLine);
            sb.AppendFormat("{0};{1};{2}", "Order", tr.Segments[0], 
                tr.Segments[1]);
            for (int i = 0; i < tr.Ticks.Count; i++) {
                if (i % 2 == 0) {
                    sb.Append(Environment.NewLine);
                    sb.AppendFormat("{0};", (i / 2) + 1);
                }
                sb.AppendFormat("{0};", tr.Ticks[i]);
            }
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }
    }
}
