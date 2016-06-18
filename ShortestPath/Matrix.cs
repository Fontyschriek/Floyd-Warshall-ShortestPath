using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPath
{
    internal class Matrix
    {
        public List<string> Vertices { get; set; }
        public List<Node> Points { get; set; }
        public List<List<Edge>> Edges { get; set; }
        public string StartingPoint { get; set; }
        public string EndingPoint { get; set; }

        public Matrix(string vertices, string startingPoint, string endingPoint)
        {
            Edges = new List<List<Edge>>();
            StartingPoint = startingPoint;
            EndingPoint = endingPoint;

            Vertices = vertices.Split(',').ToList();
            int len = Vertices.Count;
            for (int i = 0; i < len; i++)
            {
                List<Edge> temp = new List<Edge>();
                for (int j = 0; j < len; j++)
                {
                    temp.Add(new Edge(Vertices[i], Vertices[j], -1));
                }
                Edges.Add(temp);
            }
        }

        public List<Edge> GetConnectionsFor(string x)
        {
            List<Edge> temp = new List<Edge>();
            for (int i = 0; i < Vertices.Count; i++)
            {
                if (Vertices[i] == x)
                {
                    for (int j = 0; j < Edges[i].Count; j++)
                    {
                        if (Edges[i][j].Weight > -1 && Edges[i][j].IsUsed == false)
                            temp.Add(Edges[i][j]);
                    }
                    break;
                }
            }
            return temp;
        }

        private Node GetPoint(string name)
        {
            foreach (Node point in Points)
            {
                if (point.Name == name)
                    return point;
            }
            return null;
        }

        public string GetShortestPath( )
        {
            //Creates a list with all the vertices
            Points = new List<Node>();
            foreach (string vertex in this.Vertices)
            {
                //Adds all the connections of the point
                Points.Add(new Node(vertex, GetConnectionsFor(vertex)));
            }
            Node endPoint = GetPoint(EndingPoint);

            Node currentPoint = GetPoint(StartingPoint);
            currentPoint.AccumulatedWeight = 0;
            currentPoint.PreviousPoint = currentPoint.Name;
            currentPoint.IsUsed = true;
            do
            {
                //Go through all the edges incident on the vertex
                foreach (Edge edge in currentPoint.Connections)
                {
                    //Get vertex on the other side of the current edge
                    Node nextPoint = GetPoint(edge.EndingPoint);
                    //Check if the label can be improved
                    if (nextPoint.AccumulatedWeight > currentPoint.AccumulatedWeight + edge.Weight)
                    {
                        nextPoint.AccumulatedWeight = currentPoint.AccumulatedWeight + edge.Weight;
                        nextPoint.PreviousPoint = currentPoint.Name;
                    }
                }
                //Select the vertex with the smallest weigth label
                currentPoint = getMin();
            } while (currentPoint.Name != EndingPoint); //Repeat until you reach the end point

            //Generating

            int weigth = int.MaxValue;
            Node current = endPoint;
            string msg = endPoint.Name + " ";
            while (weigth != 0)
            {
                if (current.AccumulatedWeight == 0)
                {
                    weigth = 0;
                    break;
                }
                msg = msg.Insert(0, current.PreviousPoint + " ");
                weigth = current.AccumulatedWeight;
                current = GetPoint(current.PreviousPoint);
            }
            msg = msg.Insert(0, "W: " + endPoint.AccumulatedWeight + "  ");
            return msg;
        }

        private Node getMin( )
        {
            int min = int.MaxValue;
            int index = -1;
            for (int i = 0; i < Points.Count; i++)
            {
                if (Points[i].IsUsed == false)
                {
                    if (Points[i].AccumulatedWeight < min)
                    {
                        min = Points[i].AccumulatedWeight;
                        index = i;
                    }
                }
            }
            try
            {
                Points[index].IsUsed = true;
                if (index != -1)
                    return Points[index];
                else
                    return null;
            }
            catch (System.ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        public List<string> GetAllPaths( )
        {
            List<string> message = new List<string>();
            foreach (string item in Vertices)
            {
                foreach (string inner in Vertices)
                {
                    if (item != inner)
                    {
                        string paths = "";
                        paths += item;
                        StartingPoint = item;
                        EndingPoint = inner;
                        paths += "-" + inner + "\t" + GetShortestPath();
                        message.Add(paths);
                    }
                }
            }
            return message;
        }
    }
}