using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPxGridViewAIIntegration.Models
{
    public class DictionaryEntry
    {
        public DictionaryEntry(int id, string name, string description)
        {
            ID = id;
            Name = name;
            Description = description;
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}