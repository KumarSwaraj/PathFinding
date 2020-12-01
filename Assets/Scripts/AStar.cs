
namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    
    /// The A star class.
    
    public static class AStar
    {
        
        /// The run a star function.
        /// Runs the algorithm then returns the final path.
        /// If the path can not be found, then it will return an empty list.
        
        /// <param name="start">
        /// The start goal
        /// </param>
        /// <param name="goal">
        /// The goal node
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public static List<Node> RunAStar(Node start, Node goal)
        {
            // The path to return.
            List<Node> finalPath = new List<Node>();

            // The open list of nodes.
            List<Node> openList = new List<Node>();

            // The closed list of nodes.
            List<Node> closedList = new List<Node>();

            // The current node.
            Node currentNode;

            // 1. Add start node to open list for evaluation.
            openList.Add(start);

            // 1.1 While the open list has a count greater than 0..continue finding path.
            // If this ever reaches 0, then no path was found.
            while (openList.Count > 0)
            {
                // 2. Sort the openlist by fCost
                List<Node> sorted = openList.OrderByDescending(f => f.FCost).ToList();
                
                // Reverse the list so it will now be in Ascending order
                sorted.Reverse();

                // 7 If the goal node is in the openlist
                if (openList.Contains(goal))
                {
                    // Set the current node to the goal node
                    currentNode = goal;

                    finalPath.Add(currentNode);

                    // While the currentNode is not equal to the start node
                    while (currentNode != start)
                    {
                        // Add the currentNodes parent to the final path list
                        finalPath.Add(currentNode.Parent);

                        // Now set the currentNode to its parent
                        currentNode = currentNode.Parent;
                    }

                    // Now break the loop
                    break;
                }

                // 3. Set currentNode to the first element in the sorted list. (Has lowest fCost)
                currentNode = sorted[0];
                
                // Remove the currentNode from the open list
                openList.Remove(currentNode);

                // Add the currentNode to the closed list
                closedList.Add(currentNode);

                // 4. Get the currentNodes neighbors
                List<Node> neighbors = currentNode.GetNeighbors();

                // 5. Check the neighbors
                foreach (Node n in neighbors)
                {
                    // If the closed list contains the neighbor then its already been evaluated..
                    // Continue to next node.
                    if (closedList.Contains(n))
                        continue;


                    // Calculate H Cost
                    // Formula - Distance from goal node to neighbor node with absolute value
                    int x = Math.Abs(goal.XPosition - n.XPosition);
                    int y = Math.Abs(goal.YPosition - n.YPosition);

                    // The H cost is set multiplied by 10
                    n.HCost = (x + y) * 10;

                    // Get the distance between the neighbor and current node to determine its move cost
                    int xDiff = n.XPosition - currentNode.XPosition;
                    int yDiff = n.YPosition - currentNode.YPosition;

                    // If the xDiff or yDiff = 0..that means the neighbor is horizontal or vertical
                    // of the current node. Else it is one of the diagonal positions.
                    int moveCost = (xDiff == 0 || yDiff == 0) ? 10 : 14;
                    
                    // Add the currentNodes gCost to it.
                    moveCost += currentNode.GCost;

                    // 6. If the movecost is less than the neighbors gCost 
                    // OR the neighbor is not in the openlist.
                    if (moveCost < n.GCost || !openList.Contains(n))
                    {
                        // Set the gCost
                        n.GCost = moveCost;

                        // Set the parent to the currentNode
                        n.Parent = currentNode;

                        // If the neighbor is not in the open list then add it for evaluation
                        if (!openList.Contains(n))
                            openList.Add(n);
                    }
                }
            }
            
            // Return the final path
            return finalPath;
        }
    }
}
