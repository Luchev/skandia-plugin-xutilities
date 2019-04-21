using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace xUtilities.Other
{
    public class NPC
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string Guild { get; set; }
        [XmlAttribute]
        public uint Id { get; set; }
        public NPC(string _name, string _guild, uint _id)
        {
            Name = _name;
            Guild = _guild;
            Id = _id;
        }
        public NPC()
        {

        }
    }
}
