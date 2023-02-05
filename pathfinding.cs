    /// <summary>
    /// Calculate a path inside a grid
    /// Author: CustomDev
    /// Date: 05/02/2023
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="nodes"></param>
    /// <returns></returns>
    public List<PathFindingNode> CalculateGridPath(Vector3 start, Vector3 end, PathFindingNode[,] nodes)
    {
        List<PathFindingNode> path = new List<PathFindingNode>();
        HashSet<PathFindingZone> closed = new HashSet<PathFindingZone>();

        // Get the start node
        PathFindingNode startNode = MapNode(start, nodes);
        // Get the end node
        PathFindingNode endNode = MapNode(end, nodes);

        PathFindingNode currentNode = startNode;

        while (currentNode.position != endNode.position)
        {
            Dictionary<PathFindingNode, float> distances = new Dictionary<PathFindingNode, float>();
            // Check the pre-computed successors
            foreach(PathFindingNode node in currentNode.childrens)
            {
                // node can be null in this case because of the grid boundaries
                if(closed.Contains(node) || node == null)
                {
                    continue;
                }
                // Calculate heuristics distance and add to the dictionary
                distances.Add(node, CalculateDistance(node, endNode));
            }

            PathFindingNode bestNode = currentNode;
            float bestDistance = Mathf.Infinity;

            // Check which node is the nearest to the end node
            foreach(KeyValuePair<PathFindingNode, float> kvp in distances)
            {
                if (!closed.Contains(kvp.Key))
                {
                    closed.Add(kvp.Key);
                }

                if(kvp.Key.walkable && kvp.Value < bestDistance)
                {
                    bestNode = kvp.Key;
                    bestDistance = kvp.Value;
                }
            }
            
            // if the node is not reachable interrupt
            if(bestDistance == Mathf.Infinity)
            {
                break;
            }
            
            // add the best find node to the path list
            path.Add(bestNode);

            currentNode = bestNode;

        }

        return path;
    }
