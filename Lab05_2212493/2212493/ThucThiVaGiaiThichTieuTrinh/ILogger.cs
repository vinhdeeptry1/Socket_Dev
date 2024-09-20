using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThucThiVaGiaiThichTieuTrinh
{
    internal interface ILogger
    {
        void writeEntry(ArrayList entry);
        void writeEntry(String entry);
    }
}
