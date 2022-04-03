using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2work
{
    public class Language
    {
        public string ID { get; set; }
        public string NameofLanguage { get; set; }
        public string Level { get; set; }
        public List<Person> People { get; set; }
    }
}
