using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betterloid.Wrappers
{
    public abstract class ObjectWrapper
    {
        public object Object { get; private set; }

        public ObjectWrapper(object o)
        {
            Object = o;
        }

    }
}
