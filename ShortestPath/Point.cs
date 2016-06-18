using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPath
{
    internal class Node
    {
        public string Name { get; set; }
        public string PreviousPoint { get; set; }
        public int AccumulatedWeight { get; set; }
        public bool IsUsed { get; set; }
        public List<Edge> Connections { get; set; }

        public Node(string name, List<Edge> connections)
        {
            Name = name;
            PreviousPoint = "none";
            AccumulatedWeight = int.MaxValue;
            Connections = connections;
            IsUsed = false;
        }

        public override string ToString( )
        {
            return $"point: {Name} - previous: {PreviousPoint} - acumulated: {AccumulatedWeight}";
        }
    }
}