using System;
using System.Collections.Generic;
using System.Text;

namespace HIOC
{
    public class FooService: IFooService
    {
        private IFoo _foo;

        public FooService(IFoo foo)
        {
            _foo = foo;
        }

        public string SayHi()
        {
            return _foo.SayHi();
        }
    }

    public interface IFooService
    {
        string SayHi();
    }
}
