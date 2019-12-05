using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{

    class Node
    {
        public string name;
        private Dictionary<string, int> connectedNodes = new Dictionary<string, int>();

        public Node(string name)
        {
            this.name = name;
        }

        public void defineEdges(Dictionary<string, int> edges)
        {
            this.connectedNodes = edges;
        }

        public Dictionary<string, int> getConnectedNodes()
        {
            return this.connectedNodes;
        }
    }
    
    class Graph
    {
        private List<Node> Nodes = new List<Node>();

        public void addNode(string name, Dictionary<string, int> edges)
        {
            Node newNode = new Node(name);
            newNode.defineEdges(edges);
            this.Nodes.Add(newNode);
        }

        public Node getNodeByName(string name)
        {
            return this.Nodes.Where(node => node.name == name).FirstOrDefault();
        }

        public Dictionary<string, int> getConnectedNodes(string target)
        {
            Node targetNode = getNodeByName(target);
            return targetNode.getConnectedNodes();
        }

        public List<Node> getAlphabetOrder()
        {
            List<Node> nodes = this.Nodes;
            nodes = nodes.OrderBy(node => node.name).ToList();
            return nodes;
        }

        public (int, List<Node>) findShortestPath(string start, string end)
        {
            Dictionary<string, Tuple<int, List<Node>>> shortestPaths = new Dictionary<string, Tuple<int, List<Node>>>();
            List<string> remainingNodes = new List<string>();
            foreach(Node node in this.Nodes)
            {
                remainingNodes.Add(node.name);
                shortestPaths[node.name] = new Tuple<int, List<Node>> (-1, new List<Node>());
            }

            shortestPaths[start] = new Tuple<int, List<Node>>(0, new List<Node>());
            Node lastVisitedNode = getNodeByName(start);
            remainingNodes.Remove(start);
            
            
            while (remainingNodes.Count > 0) {
                foreach (KeyValuePair<string, int> node in lastVisitedNode.getConnectedNodes()) {
                    int path = shortestPaths[lastVisitedNode.name].Item1 + node.Value;

                }
            }
             
            return (1, new List<Node>());
        }

        public List<Node> getNodes()
        {
            return this.Nodes;
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            //Create graph with nodes
            Graph graph = new Graph();
            graph.addNode("Dunwich", new Dictionary<string, int> { { "Blaxhall", 15 }, { "Harwich", 53 } });
            graph.addNode("Blaxhall", new Dictionary<string, int> { { "Dunwich", 15 }, { "Harwich", 40 }, { "Feering", 46 } });
            graph.addNode("Harwich", new Dictionary<string, int> { { "Dunwich", 53 }, { "Blaxhill", 40 }, { "Tiptree", 31 }, { "Clacton", 17} });
            graph.addNode("Feering", new Dictionary<string, int> { { "Maldon", 11 }, { "Blaxhill", 46 }, { "Tiptree", 3 } });
            graph.addNode("Tiptree", new Dictionary<string, int> { { "Feering", 3 }, { "Maldon", 8 }, { "Harwich", 31 }, { "Clacton", 29 } });
            graph.addNode("Clacton", new Dictionary<string, int> { { "Harwich", 17 }, { "Maldon", 40 }, { "Tiptree", 29 } });
            graph.addNode("Maldon", new Dictionary<string, int> { { "Feering", 11 }, { "Tiptree", 8 }, { "Clacton", 40 } });
            //Console.WriteLine(graph.getNodes()[0].getConnectedNodes()["Harwich"]);

            Console.WriteLine("Nodes connected to Tiptree: ");
            foreach(string node in graph.getConnectedNodes("Tiptree").Keys)
            {
                Console.WriteLine(node);
            }
            Console.WriteLine("Order of nodes: ");
            
            foreach(Node node in graph.getAlphabetOrder())
            {
                Console.WriteLine(node.name);
            }

            graph.findShortestPath("Dunwich", "Maldon");
            
            Console.ReadLine();
        }
    }
}
