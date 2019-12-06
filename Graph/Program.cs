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

        public Tuple<int, List<Node>> findShortestPath(string start, string end)
        {
            //New Dictionary that contains a the name of each node and corresponding shortest path length and node order
            Dictionary<string, Tuple<int, List<Node>>> shortestPaths = new Dictionary<string, Tuple<int, List<Node>>>();
            //List of remaining nodes
            List<string> remainingNodes = new List<string>();
            //Loop over nodes and add each node to remaining nodes and shortest path with initial values of -1
            foreach(Node node in this.Nodes)
            {
                remainingNodes.Add(node.name);
                shortestPaths[node.name] = new Tuple<int, List<Node>> (-1, new List<Node>());
            }

            //Set start node to path length of 0 and remove it from remaining nodes
            shortestPaths[start] = new Tuple<int, List<Node>>(0, new List<Node>());
            Node lastVisitedNode = getNodeByName(start);
            remainingNodes.Remove(start);

            List<Node> addNodeToPath(List<Node> oldPath, Node newNode)
            {
                //Create the new list of nodes on path
                List<Node> newPath = new List<Node>();
                foreach (Node node in oldPath)
                {
                    newPath.Add(node);
                }
                //Add new node to list
                newPath.Add(newNode);
                return newPath;
            }
            
            //Loop untill there are no remaining nodes
            while (remainingNodes.Count > 0) {
                //Loop over the connected Nodes of the last node removed
                foreach (KeyValuePair<string, int> node in lastVisitedNode.getConnectedNodes()) {
                    //calcuate the path length from its current shortest path + edge length
                    int path = shortestPaths[lastVisitedNode.name].Item1 + node.Value;
                    //Get the current shortest path to the node
                    int currentShortestPath = shortestPaths[node.Key].Item1;
                    //if the node as currentShortestPath of -1 it has not been visited yet
                    if (currentShortestPath == -1) {
                        //Add node to path
                        List<Node> newPath = addNodeToPath(shortestPaths[lastVisitedNode.name].Item2, getNodeByName(node.Key));
                        //Update shortest Paths
                        shortestPaths[node.Key] = new Tuple<int, List<Node>>(path, newPath);

                    } else if (path < shortestPaths[node.Key].Item1 && shortestPaths[node.Key].Item1 != -1) {
                        //Add node to path
                        List<Node> newPath = addNodeToPath(shortestPaths[lastVisitedNode.name].Item2, getNodeByName(node.Key));
                        //Update shortest Paths
                        shortestPaths[node.Key] = new Tuple<int, List<Node>>(path, newPath);
                    }
                }
                //After updating all shortest paths of connected nodes, check for the shortest one
                KeyValuePair<string, int> shortest = new KeyValuePair<string, int>("default", -1);
                foreach(string nodeName in remainingNodes)
                {
                    Node node = getNodeByName(nodeName);
                    //first nodes
                    if (shortest.Value == -1) {
                        //update shortest
                        shortest = new KeyValuePair<string, int>(nodeName, shortestPaths[nodeName].Item1);
                    
                    //check if the path to current node is shortest and make sure it has actually been visited
                    } else if (shortestPaths[nodeName].Item1 < shortest.Value && shortestPaths[nodeName].Item1 != -1)
                    {
                        //update shortest
                        shortest = new KeyValuePair<string, int>(nodeName, shortestPaths[nodeName].Item1);
                    }
                }

              
                //Shortest path found
                remainingNodes.Remove(shortest.Key);
                //update last visited Node
                lastVisitedNode = getNodeByName(shortest.Key);

            }

            //Shortest path to all nodes found
             
            return new Tuple<int, List<Node>>(shortestPaths[end].Item1, shortestPaths[end].Item2);
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
            graph.addNode("Harwich", new Dictionary<string, int> { { "Dunwich", 53 }, { "Blaxhall", 40 }, { "Tiptree", 31 }, { "Clacton", 17} });
            graph.addNode("Feering", new Dictionary<string, int> { { "Maldon", 11 }, { "Blaxhall", 46 }, { "Tiptree", 3 } });
            graph.addNode("Tiptree", new Dictionary<string, int> { { "Feering", 3 }, { "Maldon", 8 }, { "Harwich", 31 }, { "Clacton", 29 } });
            graph.addNode("Clacton", new Dictionary<string, int> { { "Harwich", 17 }, { "Maldon", 40 }, { "Tiptree", 29 } });
            graph.addNode("Maldon", new Dictionary<string, int> { { "Feering", 11 }, { "Tiptree", 8 }, { "Clacton", 40 } });
            //Console.WriteLine(graph.getNodes()[0].getConnectedNodes()["Harwich"]);

            Console.WriteLine("Please enter a node to find connected nodes: ");
            string selectedNode = Console.ReadLine();
            Console.WriteLine("Nodes connected to {0}: ", selectedNode);
            foreach(string node in graph.getConnectedNodes(selectedNode).Keys)
            {
                Console.WriteLine(node);
            }
            Console.WriteLine("Order of nodes: ");
            
            foreach(Node node in graph.getAlphabetOrder())
            {
                Console.WriteLine(node.name);
            }

            Console.WriteLine("Please enter a start node for shortest path: ");
            string startNode = Console.ReadLine();
            Console.WriteLine("Please enter an end node for shortest path: ");
            string endNode = Console.ReadLine();

            Tuple<int, List<Node>> shortestPath = graph.findShortestPath(startNode, endNode);
            Console.WriteLine("Shortest path between {0} and {1} has length: {2}", startNode, endNode, shortestPath.Item1);
            Console.WriteLine("Sequence of nodes starting at {0}", startNode);
            foreach (Node node in shortestPath.Item2)
                Console.WriteLine(node.name);
            Console.ReadLine();
        }
    }
}
