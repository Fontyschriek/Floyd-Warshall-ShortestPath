using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPath
{
    internal class Edge
    {
        public string Name { get; set; }
        public string StartingPoint { get; set; }
        public string EndingPoint { get; set; }
        public int Weight { get; set; }

        //public int AcumulatedWeight { get; set; }
        //public string PreviousPoint { get; set; }
        public bool IsUsed { get; set; }

        public Edge(string startingPoint, string endingPoint, int weight)
        {
            StartingPoint = startingPoint;
            EndingPoint = endingPoint;
            Weight = weight;
            Name = startingPoint + "-" + endingPoint;
            IsUsed = false;
        }

        public override string ToString( )
        {
            return Name + "****weight:" + Weight;
        }
    }
}