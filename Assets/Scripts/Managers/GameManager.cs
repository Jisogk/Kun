using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;

    private int horiPos = 0;
    private int vertPos = 0;

    private bool isTouchingDoor = false;
    private DoorEventArgs curDoorEventArgs;

    public int posX, posY;
    private int[] xChange = { 0, 1, 0, -1 };
    private int[] yChange = { -1, 0, 1, 0 };

    public enum Direction
    {
        Up, Right, Down, Left
    }

    [HideInInspector]
    public Direction entryDirection;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        entryDirection = Direction.Up;
        posX = posY = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //detect whether player is going to leave map
        if (isTouchingDoor)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (posY <= 1 && curDoorEventArgs.direction == Direction.Up
                  || posY >= 5 && curDoorEventArgs.direction == Direction.Down)
                {
                    Debug.Log("can't goto next floor!");
                    return;
                }
                posX += xChange[(int)curDoorEventArgs.direction];
                posY += yChange[(int)curDoorEventArgs.direction];
                entryDirection = (Direction)(((int)curDoorEventArgs.direction + 2) % 4);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    public void NextLevel(string levelName)
    {
        if (!string.IsNullOrEmpty(levelName))
            SceneManager.LoadScene(levelName);
        else
            Debug.Log("Empty level name!");
    }

    public void TouchDoor(object sender, DoorEventArgs e)
    {
        Debug.Log("TouchDoor()");
        isTouchingDoor = true;
        curDoorEventArgs = e;
    }

    public void LeaveDoor(object sender, DoorEventArgs e)
    {
        Debug.Log("LeaveDoor()");
        isTouchingDoor = false;
    }
}
