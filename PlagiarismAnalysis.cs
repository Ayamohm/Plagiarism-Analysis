using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    public static class PlagiarismAnalysis
    {
        #region YOUR CODE IS HERE
        //Your Code is Here:
        //==================
        /// <summary>
        /// Given an UNDIRECTED Graph of submission IDs and matching pairs with their similarity scores (%), calculate the average similarity percentage of the component containing the given "startVertex"
        /// </summary>
        /// <param name="vertices">array of submission IDs</param>
        /// <param name="edges">array of matching pairs and their similarity score</param>
        /// <param name="startVertex">start vertex to analyze its component</param>
        /// <returns>average similarity score (%) of the component containing the startVertex</returns>
        /// 


        public static float AnalyzeMatchingScore(string[] vertices, Tuple<string, string, float>[] edges, string startVertex)
        {
            // Create a dictionary to store the graph using adjacency lists
            var graph = new Dictionary<string, List<Tuple<string, double>>>();
            foreach (string vertex in vertices)
            {
                graph[vertex] = new List<Tuple<string, double>>();
            }
            foreach (var edge in edges)
            {
                string vertex1 = edge.Item1;
                string vertex2 = edge.Item2;
                double score = edge.Item3;
                graph[vertex1].Add(new Tuple<string, double>(vertex2, score));
                graph[vertex2].Add(new Tuple<string, double>(vertex1, score));
            }
            // Traverse the graph starting from the given startVertex using BFS
            var queue = new Queue<string>();
            var visited = new HashSet<string>();
            queue.Enqueue(startVertex);
            visited.Add(startVertex);
            var connectedVertices = new HashSet<string>();
            while (queue.Count > 0)
            {
                string vertex = queue.Dequeue();
                connectedVertices.Add(vertex);
                foreach (var neighbor in graph[vertex])
                {
                    string neighborVertex = neighbor.Item1;
                    if (!visited.Contains(neighborVertex))
                    {
                        visited.Add(neighborVertex);
                        queue.Enqueue(neighborVertex);
                    }
                }
            }
            // Calculate the average similarity score of the component containing the given startVertex
            double totalScore = 0;
            int numEdges = 0;
            foreach (string vertex in connectedVertices)
            {
                foreach (var neighbor in graph[vertex])
                {
                    string neighborVertex = neighbor.Item1;
                    double score = neighbor.Item2;
                    if (visited.Contains(neighborVertex))
                    {
                        totalScore += score;
                        numEdges++;
                    }
                }
            }
            return (float)(numEdges > 0 ? totalScore / numEdges : 0);
        }
        #endregion
    }
}
