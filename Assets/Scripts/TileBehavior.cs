
namespace Assets.Scripts
{
    using UnityEngine;

    
    /// The tile behavior class.
    /// Represents the visuals of tiles.
    
    public class TileBehavior : MonoBehaviour
    {
        
        /// The node reference.
        
        private Node node;

        
        /// Gets or sets the node.
        
        public Node Node
        {
            get
            {
                return this.node;
            }

            set
            {
                this.node = value;
            }
        }

        
        /// The start function.
        
        private void Start()
        {
            // If the node is not walkable..update the sprite renderer to show it as black
            if(!this.node.IsWalkable)
                this.gameObject.GetComponent<SpriteRenderer>().material.color = Color.black;
        }
    }
}
