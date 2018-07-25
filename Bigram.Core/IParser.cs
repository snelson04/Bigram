using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Bigram.Core
{
    public interface IParser
    {
        void Parse(ICounter counter);
    }
}
