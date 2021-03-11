using System;
using System.Collections.Generic;
using System.Text;

namespace CovidApi.model
{
    public class Data
    {
        public DateTime date { get; set; }
        public string deces { get; set; }
        public string hospitalises { get; set; }
        public string reanimation { get; set; }
        public string gueris { get; set; }
        public string nom { get; set; }
        public string code { get; set; }
    }
}
