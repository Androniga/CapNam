using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum GhostNodeStatesEnum
    {
        respawning,
        leftNode,
        rightNode,
        centerNode,
        startNode,
        movingInNodes
    }

    public GhostNodeStatesEnum ghostNodeState;
    public GhostNodeStatesEnum respawnState;

    public enum GhostType
    {
        red,
        blue,
        pink,
        orange
    }


    public GhostType ghostType;

    public GameObject ghostNodeLeft;
    public GameObject ghostNodeRight;
    public GameObject ghostNodeCenter;
    public GameObject ghostNodeStart;
    
    public MovementController movementController;

    public GameObject startingNode;
    public bool readyToLeaveHome = false;

    public GameManager gameManager;

    public bool testRespawn = false;

    public bool isFrightened = false;

    public GameObject[] scatterNodes;
    public int scatterNodeIndex;



    // Start is called before the first frame update
    void Awake()
    {   
        scatterNodeIndex = 0;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        movementController = GetComponent<MovementController>();
        if (ghostType == GhostType.red)
        {
            ghostNodeState = GhostNodeStatesEnum.startNode;
            respawnState = GhostNodeStatesEnum.centerNode;
            startingNode = ghostNodeStart;
            readyToLeaveHome = true;
        }
        else if (ghostType == GhostType.pink)
        {
            ghostNodeState = GhostNodeStatesEnum.centerNode;
            startingNode = ghostNodeCenter;
            respawnState = GhostNodeStatesEnum.centerNode;
            
        }
        else if (ghostType == GhostType.blue)
        {
            ghostNodeState = GhostNodeStatesEnum.leftNode;
            startingNode = ghostNodeLeft;
            respawnState = GhostNodeStatesEnum.leftNode;
        }
        else if (ghostType == GhostType.orange)
        {
            ghostNodeState = GhostNodeStatesEnum.rightNode;
            startingNode = ghostNodeRight;
            respawnState = GhostNodeStatesEnum.rightNode;
        }
        movementController.currentNode = startingNode;
        transform.position = startingNode.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (testRespawn == true)
        {
            readyToLeaveHome = false;
            ghostNodeState = GhostNodeStatesEnum.respawning;
            testRespawn = false;
        }
    }

    public void ReachedCenterofNode(NodeController nodeController)
    {
        if(ghostNodeState == GhostNodeStatesEnum.movingInNodes)
        {
            if (gameManager.currentGhostMode == GameManager.GhostMode.scatter)
            {
                DetermineGhostScatterModeDirection();
            }
            else if (isFrightened)
            {
                
            }
            else
            {
                if (ghostType == GhostType.red)
                {
                    DetermineRedGhostDirection();
                }
                else if (ghostType == GhostType.pink)
                {
                    DeterminePinkGhostDirection();
                }
                else if (ghostType == GhostType.blue)
                {
                    DetermineBlueGhostDirection();
                }
                
                else if (ghostType == GhostType.orange)
                {
                    DetermineOrangeGhostDirection();
                }

            }
        }
        else if (ghostNodeState == GhostNodeStatesEnum.respawning)
        {
            string direction = "";
            if (transform.position.x == ghostNodeStart.transform.position.x && transform.position.y == ghostNodeStart.transform.position.y)
            {
                direction = "down";
            }
            else if (transform.position.x == ghostNodeCenter.transform.position.x && transform.position.y == ghostNodeCenter.transform.position.y)
            {
               if (respawnState == GhostNodeStatesEnum.centerNode)
               {
                  ghostNodeState = respawnState;
               } 
               else if (respawnState == GhostNodeStatesEnum.leftNode)
               {
                 direction = "left";
               }
               else if (respawnState == GhostNodeStatesEnum.rightNode)
               {
                 direction = "right";
               }
            }
            else if (
                (transform.position.x == ghostNodeLeft.transform.position.x && transform.position.y == ghostNodeLeft.transform.position.y)
                ||(transform.position.x == ghostNodeRight.transform.position.x && transform.position.y == ghostNodeRight.transform.position.y))
            {
                ghostNodeState = respawnState;
            }
            else
            {
                direction = GetClosestDirection(ghostNodeStart.transform.position);
            }

            movementController.SetDirection(direction);
        }
        else 
        {
            if (readyToLeaveHome)
            {
                if (ghostNodeState == GhostNodeStatesEnum.leftNode)
                {
                    ghostNodeState = GhostNodeStatesEnum.centerNode;
                    movementController.SetDirection("right");
                }
                else if (ghostNodeState == GhostNodeStatesEnum.rightNode)
                {
                    ghostNodeState = GhostNodeStatesEnum.centerNode;
                    movementController.SetDirection("left");
                    
                }
                else if (ghostNodeState == GhostNodeStatesEnum.centerNode)
                {
                    ghostNodeState = GhostNodeStatesEnum.startNode;
                    movementController.SetDirection("up");
                    
                }
                else if (ghostNodeState == GhostNodeStatesEnum.startNode)
                {
                    ghostNodeState = GhostNodeStatesEnum.movingInNodes;
                    movementController.SetDirection("left");
                }
                

            }
        }

    }

    void DetermineGhostScatterModeDirection(){

        if (transform.position.x == scatterNodes[scatterNodeIndex].transform.position.x &&
                    transform.position.y == scatterNodes[scatterNodeIndex].transform.position.y )
                {   
                scatterNodeIndex++;
                }

                if (scatterNodeIndex == scatterNodes.Length - 1 )
                {
                    scatterNodeIndex = 0;
                }

                string direction = GetClosestDirection(scatterNodes[scatterNodeIndex].transform.position);
                movementController.SetDirection(direction);

    }


    void DetermineRedGhostDirection()
    {
        string direction = GetClosestDirection(gameManager.pacman.transform.position);
        movementController.SetDirection(direction);
    }
    void DeterminePinkGhostDirection()
    {
        string pacmansDirection = gameManager.pacman.GetComponent<MovementController>().lastMovingDirection;
        float distanceBetweenNodes = 0.35f;

        Vector2 target = gameManager.pacman.transform.position;
        if (pacmansDirection == "left")
        {
            target.x -= distanceBetweenNodes * 2;
        }
        else if (pacmansDirection == "right")
        {
            target.x += distanceBetweenNodes * 2;
        }
        else if (pacmansDirection == "up")
        {
            target.x += distanceBetweenNodes * 2;
        }
        else if (pacmansDirection == "down")
        {
            target.x -= distanceBetweenNodes * 2;
        }

        string direction = GetClosestDirection(target);
        movementController.SetDirection(direction);
     }
    void DetermineBlueGhostDirection()
    {
        string pacmansDirection = gameManager.pacman.GetComponent<MovementController>().lastMovingDirection;
        float distanceBetweenNodes = 0.35f;

        Vector2 target = gameManager.pacman.transform.position;
        if (pacmansDirection == "left")
        {
            target.x -= distanceBetweenNodes * 2;
        }
        else if (pacmansDirection == "right")
        {
            target.x += distanceBetweenNodes * 2;
        }
        else if (pacmansDirection == "up")
        {
            target.x += distanceBetweenNodes * 2;
        }
        else if (pacmansDirection == "down")
        {
            target.x -= distanceBetweenNodes * 2;
        }
        GameObject redGhost = gameManager.redGhost;
        float xDistance = target.x - redGhost.transform.position.x;
        float yDistance = target.y - redGhost.transform.position.y;

        Vector2 blueTarget = new Vector2(target.x + xDistance, target.y + yDistance);
        string direction = GetClosestDirection(blueTarget);
        movementController.SetDirection(direction);
    }
    void DetermineOrangeGhostDirection()
    {

        float distance = Vector2.Distance(gameManager.pacman.transform.position, transform.position);
        float distanceBetweenNodes = 0.35f;
        if (distance < 0)
        {
            distance *= -1;    
        }

        if(distance <= distanceBetweenNodes * 8)
        {
            DetermineRedGhostDirection();
        }
        else
        {
            DetermineGhostScatterModeDirection();
        }

    }

    string GetClosestDirection(Vector2 target)
    {
        float shortestDistance = 0;
        string lastMovingDirection = movementController.lastMovingDirection;
        string newDirection = "";
        NodeController nodeController = movementController.currentNode.GetComponent<NodeController>();

        if (nodeController.canMoveUp && lastMovingDirection != "down")
        {   
            GameObject nodeUp = nodeController.nodeUp;
            float distance = Vector2.Distance(nodeUp.transform.position, target);
            if (distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "up";
            }

        }
        if (nodeController.canMoveDown && lastMovingDirection != "up")
        {   
            GameObject nodeDown = nodeController.nodeDown;
            float distance = Vector2.Distance(nodeDown.transform.position, target);
            if (distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "down";
            }

        }
        if (nodeController.canMoveLeft && lastMovingDirection != "right")
        {   
            GameObject nodeLeft = nodeController.nodeLeft;
            float distance = Vector2.Distance(nodeLeft.transform.position, target);
            if (distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "left";
            }

        }
        if (nodeController.canMoveRight && lastMovingDirection != "left")
        {   
            GameObject nodeRight = nodeController.nodeRight;
            float distance = Vector2.Distance(nodeRight.transform.position, target);
            if (distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "right";
            }

        }

        return newDirection;
    }
}

