using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HS.Entities.Result
{
    public class TransferResult
    {
        public bool IsReleased { get; set; }
        public bool IsAdded { get; set; }

        public TransferResult()
        {
            
        }
        public TransferResult(bool Released, bool Added)
        {
            IsAdded = Added;
            IsReleased = Released;
        }
    }
}
