using System;
using System.Collections.Generic;
using System.Text;

namespace Lattice1.Measure
{
    abstract class Test
    {
        public abstract TestResult Run();
        public abstract string ToCsv(TestResult tr);
    }
}
