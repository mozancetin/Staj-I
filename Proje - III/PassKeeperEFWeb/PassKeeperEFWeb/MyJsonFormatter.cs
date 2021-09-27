using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassKeeperEFWeb
{
    class MyJsonFormatter
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }

        public List<string> Data { get; set; }

        public string JsonData { get; set; }
    }
}
