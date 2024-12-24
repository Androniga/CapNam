using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    // Start is called before the first frame update    public GameObject currentNode; 
    public GameManager gameManager;
    public GameObject currentNode; 
    public float speed = 4f;
    public string direction = "";
    public string lastMovingDirection = "";

    public bool isGhost = false;

    void Awake()
    {   
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void Update()
    {
        NodeController currentNodeController = currentNode.GetComponent<NodeController>();

        transform.position = Vector2.MoveTowards(transform.position, currentNode.transform.position, speed * Time.deltaTime);

        bool reverseDirection = false;

        if (
          (direction == "left" && lastMovingDirection == "right")  
          || (direction == "right" && lastMovingDirection == "left") 
          || (direction == "down" && lastMovingDirection == "up") 
          || (direction == "up" && lastMovingDirection == "down") )
        {
          reverseDirection = true ;  
        }


        if(transform.position.x == currentNode.transform.position.x && transform.position.y == currentNode.transform.position.y || reverseDirection)
        {
            if (isGhost)
            {
                GetComponent<EnemyController>().ReachedCenterofNode(currentNodeController);
            }

            if (currentNodeController.isGhostStartingNode && direction == "down"
                && (!isGhost || GetComponent <EnemyController>().ghostNodeState != EnemyController.GhostNodeStatesEnum.respawning))
                {
                    direction = lastMovingDirection;
                }

            GameObject newNode = currentNodeController.GetNodeFromDirection(direction);

            if (newNode != null)
            {
               currentNode = newNode;
               lastMovingDirection = direction; 
            }
            else
            {
                direction = lastMovingDirection;
                newNode = currentNodeController.GetNodeFromDirection(direction);
                if (newNode != null)
                {
                    currentNode = newNode;
                }
            }
            
        }
    }

    public void SetDirection(string newDirection)
    {
        direction = newDirection;
    }
}

