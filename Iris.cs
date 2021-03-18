using System;
using System.Collections.Generic;
using System.Text;

namespace nai_p2
{
    class Iris
    {
        public string Nazwa { get; set; }
        public List<double> Atrybuty { get; set; }

        override
        public string ToString()
        {
            return this.Nazwa;
        }
    }
}
