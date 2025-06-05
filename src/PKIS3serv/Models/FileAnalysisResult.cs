using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programming_of_corporate_industrial_systems_Practice_2_server.Models
{
    public class FileAnalysisResult
    {
        public string FileName { get; set; }
        public int LineCount { get; set; }
        public int WordCount { get; set; }
        public int CharacterCount { get; set; }
    }
}
