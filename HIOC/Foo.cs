using System;
using System.Collections.Generic;
using System.Text;

namespace HIOC
{
    public class Foo:IFoo
    {
        public string Random { get; set; }
        public string SayHi()
        {
            return Random = Random ?? new Random().Next(0,1000).ToString();
        }
    }
}
