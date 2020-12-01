
namespace Assets.Scripts
{
    using System.Collections.Generic;

    
    /// The node class.
    
    public class Node
    {
        
        /// The neighbors reference.
        
        private readonly List<Node> neighbors = new List<Node>();

        
        /// The x and y reference.
        
        private int x, y;

        
        /// The walkable reference.
        
        private bool walkable;

        
        /// The g and h cost references.
        
        private int gCost, hCost;

        
        /// The parent node reference.
        
        private Node parent;

        
        /// Initializes a new instance of the <see cref="Node"/> class.
        
        /// <param name="xPos">
        /// The x position.
        /// </param>
        /// <param name="yPos">
        /// The y position.
        /// </param>
        /// <param name="isWalkable">
        /// The is walkable.
        /// </param>
        public Node(int xPos, int yPos, bool isWalkable)
        {
            this.x = xPos;
            this.y = yPos;
            this.walkable = isWalkable;
        }

        
        /// Gets or sets the x position.
        
        public int XPosition
        {
            get
            {
                return this.x;
            }

            set
            {
                this.x = value;
            }
        }

        
        /// Gets or sets the y position.
        
        public int YPosition
        {
            get
            {
                return this.y;
            }

            set
            {
                this.y = value;
            }
        }

        
        /// Gets or sets a value indicating whether is walkable.
        
        public bool IsWalkable
        {
            get
            {
                return this.walkable;
            }

            set
            {
                this.walkable = value;
            }
        }

        
        /// Gets or sets the h cost.
        
        public int HCost
        {
            get
            {
                return this.hCost;
            }

            set
            {
                this.hCost = value;
            }
        }

        
        /// Gets or sets the g cost.
        
        public int GCost
        {
            get
            {
                return this.gCost;
            }

            set
            {
                this.gCost = value;
            }
        }

        
        /// Gets the f cost.
        
        public int FCost
        {
            get
            {
                return this.gCost + this.hCost;
            }
        }

        
        /// Gets or sets the parent.
        
        public Node Parent
        {
            get
            {
                return this.parent;
            }

            set
            {
                this.parent = value;
            }
        }

        
        /// The get neighbors function.
        /// Returns the list of neighbors.
        
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Node> GetNeighbors()
        {
            // Make sure list is already empty
            this.neighbors.Clear();

            // Loop through the neighbors
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    // If i and j = 0...then this is the current node trying to find its neighbors..
                    // Just continue or else it will check itself
                    if (i == 0 && j == 0)
                        continue;

                    // Get the node at the position from the grid
                    Node neighbor = GridManager.Instance.GetNode(this.x + i, this.y + j);

                    // If its not walkable then continue to next node
                    if (!neighbor.IsWalkable)
                        continue;

                    // Add the node to the neighbors list
                    this.neighbors.Add(neighbor);
                }
            }

            // Return the neighbors list
            return this.neighbors;
        }
    }
}
