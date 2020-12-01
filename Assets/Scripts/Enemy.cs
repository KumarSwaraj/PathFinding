
namespace Assets.Scripts
{
    using System.Collections.Generic;

    using UnityEngine;

    
    /// The enemy class.
    
    public class Enemy : MonoBehaviour
    {
        
        /// The path the object will take to reach the goal
        
        private Stack<TileBehavior> path;

        
        /// The destination reference.
        
        private Vector3 destination;

        
        /// The current tile reference.
        
        private TileBehavior currentTile;

        
        /// The previous tile reference.
        
        private TileBehavior previousTile;

        
        /// The start function
        
        private void Start()
        {
            // Set to new stack of tilebehvaior objects
            this.path = new Stack<TileBehavior>();

            // Run algorithm and get pathfrom the start node that was clicked by the user
            List<Node> finalPath = AStar.RunAStar(GridManager.Instance.StartNode, GridManager.Instance.GoalNode);

            // Set the destination to the current position to prevent movement
            this.destination = this.transform.position;

            // If the path count is equal to 0...cant reach the goal
            if (finalPath.Count == 0)
                return;

            TileBehavior t;

            // Loop through the return list of nodes
            foreach (Node n in finalPath)
            {
                // Check if the node is in the dictionary
                if (GridManager.Instance.Tiles.TryGetValue(n, out t))
                {
                    // Push the node on to the stack
                    this.path.Push(t);
                }
            }

            // Set the destination to the current position to prevent movement
            this.destination = this.transform.position;

            // If the path has nodes in it
            if (this.path.Count > 0)
            {
                // Pop the first tile behavior and set it to the current tile
                this.currentTile = this.path.Pop();

                // Now set the current tile position to the destination
                this.destination = this.currentTile.transform.position;
                
            }

            // Subscribe and listen for the "Run" event
            EventManager.Subscribe("Run", this.CalcNewPath);
        }

        
        /// The update function
        
        private void Update()
        {
            // If the current position of the enemy is not the destination
            if (this.transform.position != this.destination)
            {
                // If the current Tile node has become unwalkable
                if (!this.currentTile.Node.IsWalkable)
                {
                    // Calculate a new path and return
                    this.CalcNewPath();
                    return;
                }

                // Move towards the destination at a speed of 2 * time.deltaTime
                this.transform.position = Vector3.MoveTowards(
                    this.transform.position,
                    this.destination,
                    2 * Time.deltaTime);
            }
            // If the object has reached its destination
            else
            {
                // If the the path count is greater than 0
                if (path.Count > 0)
                {
                    // Get the first object in the stack
                    TileBehavior t = path.Pop();

                    // If the node is walkable
                    if (t.Node.IsWalkable)
                    {
                        // Set the current tile to the previous tile as reference
                        this.previousTile = this.currentTile;

                        // Set the object to the current tile
                        this.currentTile = t;

                        // Set the current tiles position to the destination
                        this.destination = this.currentTile.transform.position;
                    }
                }
            }
        }

        
        /// The on destroy function.
        
        private void OnDestroy()
        {
            // Unsubscribe the event when the object is destroyed
            EventManager.UnSubscribe("Run", this.CalcNewPath);
        }

        
        /// The calculate new path function.
        /// Gets the new AStar path
        
        private void CalcNewPath()
        {
            // Create new list of nodes for final path
            List<Node> finalPath = new List<Node>();

            // Set the path to a new stack of tile behavior objects
            this.path = new Stack<TileBehavior>();

            // If the current Tile is not equal to null
            if (this.currentTile != null)
            {
                // If the current tile node is not walkable - use the previous tile node else use the current tile node
                Node startNode = (!this.currentTile.Node.IsWalkable) ? this.previousTile.Node : this.currentTile.Node;

                // Run algorithm and get path using the proper starting tile
                finalPath = AStar.RunAStar(startNode, GridManager.Instance.GoalNode);

                TileBehavior t;

                // Loop through the nodes in the final path
                foreach (Node n in finalPath)
                {
                    // If the dictionary contains it
                    if (GridManager.Instance.Tiles.TryGetValue(n, out t))
                    {
                        // Push it onto the stack
                        this.path.Push(t);
                    }
                }

                // If path count is greater than 0
                if (this.path.Count > 0)
                {
                    // Pop node off the stack and set it as the current tile
                    this.currentTile = this.path.Pop();

                    // Set the destination to the current tile position
                    this.destination = this.currentTile.transform.position;
                }
            }
        }
    }
}
