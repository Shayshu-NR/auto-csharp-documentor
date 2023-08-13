using System;
using System.Collections.Generic;
using System.IO;


namespace Test
{
    public class CustomAttribute : Attribute
    { }

    public class TestAttribute : Attribute
    {}

    public class Fn
    {
        /// <summary>
        /// This is a summary of method1
        /// </summary>
        public void Method1()
        {

        }

        /// <summary>
        /// This is another method but this time with an attribute and a return and input 
        /// </summary>
        /// <param name="input">Input parameter to method</param>
        /// <returns></returns>
        [Custom]
        [Test]
        public List<string> AnotherMethod(string input)
        {
            return new List<string>();
        }

        /// <summary>
        /// Static method
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double StaticMethod(List<int> input)
        {
            return 0.0;
        }

        public void ThisMethodHasNoComments()
        {

        }

        /// <summary>
        /// This is a private method and shouldn't show up
        /// </summary>
        private void Nothing()
        {

        }

        /// <summary>
        /// This is a protected method with some extra attributes
        /// </summary>
        /// <returns></returns>
        [Custom]
        [Test]
        protected static Dictionary<string, List<int>> AnotherPrivateMethod(int arg1, string arg2, Dictionary<List<Tuple<string, string>>, double> t)
        {
            return new Dictionary<string, List<int>>();
        }


        /// <summary>
        /// This is a protected method, and should show up in the output
        /// </summary>
        /// <remarks>
        /// Hey this is a remark
        /// </remarks>
        /// <returns></returns>
        protected int TestMethodProc()
        {
            return 0;
        }

    }
}