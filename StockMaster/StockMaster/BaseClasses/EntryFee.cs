using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMaster.BaseClasses
{
    public class EntryFee: TBaseClass
    {
        public double Value { get; set; }
        public string Verbal { get; set; }

        public EntryFee(double value, string verbal ):this()
        {
            this.Value = value;
            this.Verbal = verbal;
        }

        public EntryFee()
        {

        }
    }
}
