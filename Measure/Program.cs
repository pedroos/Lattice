using System;
using System.IO;

namespace Lattice1.Measure {

    class Program
    {
        static Manager manager;
        static Guid execution;
        static string csvDir;

        static int Main(string[] args)
        {
            if (!(args.Length > 0)) return 1;
            Type testType = Type.GetType(typeof(Test).Namespace + "." + args[0]);
            if (testType == null) return 1;

            Test test = Activator.CreateInstance(testType) as Test;

            execution = Guid.NewGuid();

            csvDir = System.IO.Path.Combine(Environment.CurrentDirectory, 
                "csv");
            if (!Directory.Exists(csvDir)) {
                Directory.CreateDirectory(csvDir);
            }

            manager = new Manager(execution, csvDir);
            bool ok;

            ok = RunTest(test);
            if (!ok) {
                Console.WriteLine("not ok. quit.");
                Console.Read();
                return 1;
            }

            Console.WriteLine(Environment.NewLine + "quit.");
            Console.Read();
            return 0;
        }

        static bool RunTest(Test test) {
            Console.WriteLine("=={0}==", test);
            TestResult tr = test.Run();
            Console.WriteLine("done.");

            string csvPath = Path.Combine(csvDir, execution + ".csv");
            File.WriteAllText(csvPath, test.ToCsv(tr));
            Console.WriteLine(csvPath);

            return true;
        }
    }
}
