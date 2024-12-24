using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{


    public GameObject pacman;

    public GameObject ghostNodeLeft;
    public GameObject ghostNodeRight;
    public GameObject ghostNodeCenter;
    public GameObject ghostNodeStart;

    public GameObject redGhost;
    public GameObject pinkGhost;
    public GameObject blueGhost;
    public GameObject orangeGhost;
    // Start is called before the first frame update

    public enum GhostMode
    {
        chase,scatter
    }

    public GhostMode currentGhostMode;

    public int[] ghostModeTimers = new int[] {7,20 ,7,20,5,20,5};
    public int ghostModeTimerIndex = 0;
    public float ghostModeTimer = 0;
    public bool runningTimer = true;
    public bool completedTimer = false;

    void Awake()
    {
        currentGhostMode = GhostMode.chase;
        ghostNodeStart.GetComponent<NodeController>().isGhostStartingNode = true ;
    }


    // Update is called once per frame

    void Update()
    {
        if (!completedTimer && runningTimer)
        {
            ghostModeTimer += Time.deltaTime;
            if (ghostModeTimer >= ghostModeTimers[ghostModeTimerIndex])
            {
                ghostModeTimer = 0;
                ghostModeTimerIndex++;
                if (currentGhostMode == GhostMode.chase)
                {
                    currentGhostMode = GhostMode.scatter;
                }
                else
                {
                    currentGhostMode = GhostMode.chase;
                }
                if (ghostModeTimerIndex == ghostModeTimers.Length)
                {
                    completedTimer = true;
                    runningTimer = false;
                    currentGhostMode = GhostMode.chase; 
                }
            }
        }
    }
}
