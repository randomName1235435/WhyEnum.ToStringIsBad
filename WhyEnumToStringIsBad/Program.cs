using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;
using System;
using System.Net;
using FastEnumUtility;

namespace WhyEnumToStringIsBad
{
    class Program
    {
        static void Main(string[] args)
        {
            var benchmarkClasse = new BenchmarkClass();
           Console.WriteLine( benchmarkClasse.NativeSmallEnumToString());
            Console.WriteLine(benchmarkClasse.NativeBigEnumToString());
            Console.WriteLine(benchmarkClasse.NameOfSmallEnumToString());
            Console.WriteLine(benchmarkClasse.NameOfBigEnumToString());
            Console.WriteLine(benchmarkClasse.EnumGetNameSmallEnumToString());
            Console.WriteLine(benchmarkClasse.EnumGetNameBigEnumToString());
            Console.WriteLine(benchmarkClasse.FastEnumBigEnumToString());
            Console.WriteLine(benchmarkClasse.FastEnumSmallEnumToString());
            
            BenchmarkRunner.Run<BenchmarkClass>();
        }
    }
    public enum MyHttpConnectionState
    {
        OK
    }
    [MemoryDiagnoser]
    public class BenchmarkClass
    {
        [Benchmark]
        public string NativeBigEnumToString()
        {
            //enums get searched with binary search, so u dont get punished a lot for using big enums
            return HttpStatusCode.OK.ToString();
        }
        [Benchmark]
        public string NativeSmallEnumToString() {
            return MyHttpConnectionState.OK.ToString();
        }
        [Benchmark]
        public string NameOfSmallEnumToString()
        {
            return  nameof( MyHttpConnectionState.OK);
        }
        [Benchmark]
        public string NameOfBigEnumToString()
        {
            return nameof(HttpStatusCode.OK);
        }
        [Benchmark]
        public string EnumGetNameBigEnumToString()
        {
            return Enum.GetName(typeof(HttpStatusCode), HttpStatusCode.OK);
        }
        [Benchmark]
        public string EnumGetNameSmallEnumToString()
        {
            return Enum.GetName(typeof(MyHttpConnectionState), MyHttpConnectionState.OK);
        }
        [Benchmark]
        public string NameOfSwitchigEnumToString()
        {
            return ToStringSwitch(HttpStatusCode.OK);
        }

        private string ToStringSwitch(HttpStatusCode code)
        {
            //switch should be generated with t4 or smth else
            switch (code)
            {
                case HttpStatusCode.OK:
                    return nameof(HttpStatusCode.OK);
                default:
                    return "";
            }
        }
        private string ToStringSwitch(MyHttpConnectionState code)
        {
            //switch should be generated with t4 or smth else
            switch (code)
            {
                case MyHttpConnectionState.OK:
                    return nameof(MyHttpConnectionState.OK);
                default:
                    return "";
            }
        }

        [Benchmark]
        public string NameOfSwitchSmallEnumToString()
        {
            return ToStringSwitch(MyHttpConnectionState.OK); 
        }

        [Benchmark]
        public string FastEnumSmallEnumToString()
        {
            return ToStringFastEnum(MyHttpConnectionState.OK);
        }
        string ToStringFastEnum(MyHttpConnectionState code)
        {
            return FastEnum.GetName(code);
        }
        public string ToStringFastEnum(HttpStatusCode code)
        {
            return FastEnum.GetName(code);
        }
        [Benchmark]
        public string FastEnumBigEnumToString()
        {
            return ToStringFastEnum(HttpStatusCode.OK);
        }
    }
}
