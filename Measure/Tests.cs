using System;
using System.Collections.Generic;
using System.Text;

namespace Lattice1.Measure {
    partial class Manager {
        private Guid execution;
        private string outDir;

        public Manager(Guid execution, string outDir) {
            this.execution = execution;
            this.outDir = outDir;
        }
    }

    class TestResult {
        private List<string> segments;
        private List<long> ticks;

        private int segPos;

        public TestResult() {
            this.segments = new List<string>();
            this.ticks = new List<long>();

            segPos = 0;
        }

        public void AddSegment(String _segment, long _ticks) {
            segments.Add(_segment);
            ticks.Add(_ticks);
            segPos++;
        }

        public string SegmentString() {
            if (segPos == 0) return string.Empty;
            return segments[segPos-1] + "..." + Environment.NewLine +
                "  " + ticks[segPos-1].OfTen() + " ticks";
        }

        public List<string> Segments { get { return segments; } }
        public List<long> Ticks { get { return ticks; } }
    }

    internal static class TestsExtensions {
        internal static string OfTen(this long n) {
            string _n = n.ToString();
            double power = _n.Length - 1;
            double __n = Math.Round((double)n / Math.Pow(10, power), 2);
            return string.Format(new System.Globalization.CultureInfo("en-GB"), 
                "{1:0.00}*10^{2} = {0}", _n, __n, power);
        }
    }
}
