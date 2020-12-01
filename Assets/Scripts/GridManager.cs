
namespace Assets.Scripts
{
    using System.Collections.Generic;

    using UnityEngine;

    
    /// The grid manager class.
    
    public class GridManager : MonoBehaviour
    {
        
        /// The _instance reference to the grid manager.
        
        private static GridManager _instance;

        
        /// The tiles for reset.
        
        private readonly List<TileBehavior> tilesForReset = new List<TileBehavior>();

        
        /// The tiles dictionary.
        /// Holds reference for each TileBehavior for each Node
        
        private readonly Dictionary<Node, TileBehavior> tiles = new Dictionary<Node, TileBehavior>();

        
        /// The tile prefab.
        
        [SerializeField]
        private GameObject tilePrefab;

        
        /// The movement prefab reference.
        
        [SerializeField]
        private GameObject movementPrefab;

        
        /// The prefab instance that stores the instantiate prefab object.
        
        private GameObject prefabInstance;

        
        /// The start node.
        
        private Node startNode;

        
        /// The goal node.
        
        private Node goalNode;

        
        /// The grid reference.
        
        private Node[,] grid;

        
        /// Gets the instance of the grid manager.
        
        public static GridManager Instance
        {
            get
            {
                return _instance = (_instance == null) ? FindObjectOfType<GridManager>() : _instance;
            }
        }

        
        /// Gets the tiles.
        
        public Dictionary<Node, TileBehavior> Tiles
        {
            get
            {
                return this.tiles;
            }
        }

        
        /// Gets the Grid.
        
        public Node[,] Grid
        {
            get
            {
                return this.grid;
            }
        }

        
        /// Gets the goal node.
        
        public Node GoalNode
        {
            get
            {
                return this.goalNode;
            }
        }

        
        /// Gets the start node.
        
        public Node StartNode
        {
            get
            {
                return this.startNode;
            }
        }

        
        /// The get node function.
        
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="y">
        /// The y.
        /// </param>
        /// <returns>
        /// The <see cref="Node"/>.
        /// </returns>
        public Node GetNode(int x, int y)
        {
            // Return the node at this location
            return this.Grid[x, y];
        }

        
        /// The spawn function.
        
        /// <param name="spawnbutton">
        /// The spawn button reference.
        /// </param>
        public void Spawn(GameObject spawnbutton)
        {
            TileBehavior t;

            // If the startnode or goal node are null just return
            if (this.startNode == null || this.goalNode == null)
                return;

            // If the start node is in the dictionary
            if (this.tiles.TryGetValue(this.StartNode, out t))
            {
                // Spawn object
                this.prefabInstance = Instantiate(this.movementPrefab, t.transform.position, Quaternion.identity);
            }

            // Set the button active to false so it cant be used again yet
            spawnbutton.SetActive(false);
        }

        
        /// The spawn function.
        
        /// <param name="spawnbutton">
        /// The spawn button reference.
        /// </param>
        public void Clear(GameObject spawnbutton)
        {
            this.startNode = null;
            this.goalNode = null;

            // Destroy the prefab instance that was spawned
            Destroy(this.prefabInstance);

            // Loop through the list of tiles that are to be reset
            foreach (TileBehavior t in this.tilesForReset)
            {
                // Set the nodes back to walkable
                t.Node.IsWalkable = true;

                // Set the spriterenderer to white to visually show the node is walkable
                t.GetComponent<SpriteRenderer>().material.color = Color.white;
            }

            // Set the spawn button back to active
            spawnbutton.SetActive(true);
        }

        
        /// The start function.
        
        private void Start()
        {
            // Set the instance to this
            _instance = this;

            // Set up the grid
            this.SetUpGrid();
        }

        
        /// The update function.
        
        private void Update()
        {
            // Get the right mouse button
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;

                // Perform ray cast
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // If ray cast hit something
                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    // Check if it was a tile
                    TileBehavior tile = hit.collider.GetComponent<TileBehavior>();

                    // If the start node is null
                    if (this.startNode == null)
                    {
                        // If the tile is not null and the node is walkable
                        if (tile != null && tile.Node.IsWalkable)
                        {
                            // Set the start node to that node
                            this.startNode = tile.Node;

                            // Change sprite renderer to green
                            hit.collider.gameObject.GetComponent<SpriteRenderer>().material.color = Color.green;

                            // Add the tile to the list to be reset
                            this.tilesForReset.Add(tile);
                        }
                    } // Else if the goal node is null
                    else if (this.goalNode == null)
                    {
                        // If the tile is not null and the node is walkable and the node is not the start node
                        if (tile != null && tile.Node.IsWalkable && tile.Node != this.startNode)
                        {
                            // Set the node to the goal node
                            this.goalNode = tile.Node;

                            // Change sprite renderer to red
                            hit.collider.gameObject.GetComponent<SpriteRenderer>().material.color = Color.red;

                            // Add the tile to the list to be reset
                            this.tilesForReset.Add(tile);
                        }
                    }
                }
            }

            // Get the right mouse button
            if (Input.GetMouseButtonDown(1))
            {
                RaycastHit hit;

                // Perform ray cast
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                // If ray cast hit something
                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    // If clicked on an enemy then just return
                    if (hit.collider.GetComponent<Enemy>())
                        return;

                    // Check if it was a tile
                    TileBehavior tile = hit.collider.GetComponent<TileBehavior>();

                    // If the tile clicked on is the start node or goal node then just return
                    if (tile.Node == this.startNode || tile.Node == this.goalNode)
                        return;

                    // Set the node to not walkable
                    tile.Node.IsWalkable = false;

                    // Change sprite renderer to black
                    hit.collider.gameObject.GetComponent<SpriteRenderer>().material.color = Color.black;

                    // Run the algorithm again because the moving objects path might need to be changed
                    EventManager.Publish("Run");

                    // Add the tile to the list to be reset
                    this.tilesForReset.Add(tile);
                }
            }
        }

        
        /// The set up grid function.
        
        private void SetUpGrid()
        {
            // Offset for spacing tiles
            float offset = 0.5f;

            // Get the screens position in world
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

            // Bool reference
            bool walkable;

            // Initialize grid with 7, 7
            this.grid = new Node[7, 7];

            // Loop through the grid
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    // Instantiate a tile prefab
                    GameObject go = Instantiate(this.tilePrefab);

                    // Set its position to the world position with the appropriate offset
                    go.transform.position = new Vector3(worldPos.x + i + 0.5f, worldPos.y - j + -0.5f, 0);

                    // Set walkable to true or false
                    walkable = (i % 7 == 0 || j % 7 == 0 || i == 7 - 1 || j == 7 - 1) ? false : true;

                    // Create a node
                    Node n = new Node(i, j, walkable);

                    // Get the TileBehavior component
                    TileBehavior tile = go.GetComponent<TileBehavior>();

                    // Set its node to the new created node
                    tile.Node = n;

                    // Add the node to the grid
                    this.grid[i, j] = n;

                    // Add the node and tile to the dictionary
                    this.tiles.Add(n, tile);
                }
            }
        }
    }

}
