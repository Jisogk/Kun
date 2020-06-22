using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

  private int[,] map;
  private int width_cnt = 31;
  private int height_cnt = 31;
  private float blocksize = 4;

  [Range(0, 100)]
  public int randomFillPercent;

  public GameObject[] wallTiles;
  public GameObject[] blockTiles;
  public GameObject doorTile;
  public GameObject player;
  public GameObject enemy;

  public int smoothrounds;

  public int entryWidth = 4;   //player刷新点的宽度,必须为偶数
  public int entryHeight = 4;  //player刷新点的高度,必须为偶数

    private List<GameObject> tiles = new List<GameObject>();

  struct Pos {
    public int x;
    public int y;
  }
  private List<Pos> emptyPosList = new List<Pos>();

	// Use this for initialization
	void Start () {
    GenerateMap();
    InstantiateMap();
    //player = Instantiate(robot, new Vector3(blocksize, blocksize * (height_cnt - 2), 0), Quaternion.identity) as GameObject;
    //Vector2 initpos = new Vector2(blocksize * 1.5f, blocksize * (height_cnt - 3));
    Vector2 initpos;
    switch(GameManager.instance.entryDirection)
    {
      case GameManager.Direction.Up:
        initpos = new Vector2(blocksize * (width_cnt / 2), blocksize * (height_cnt - 2));
        break;
      case GameManager.Direction.Right:
        initpos = new Vector2(blocksize * (width_cnt - 2), blocksize * (height_cnt / 2));
        break;
      case GameManager.Direction.Down:
        initpos = new Vector2(blocksize * (width_cnt / 2), blocksize * 2);
        break;
      case GameManager.Direction.Left:
        initpos = new Vector2(blocksize * 2, blocksize * (height_cnt / 2));
        break;
      default:  //should not be accessed
        initpos = new Vector2(0f, 0f);
        break;
    }
    //player.GetComponent<Rigidbody2D>().MovePosition(initpos);
    player.transform.position = initpos;
    InstansiateEnemys(0);
	}
	
	// Update is called once per frame
	void Update () {
    /*
		if(Input.GetKeyDown(KeyCode.Z))
    {
      DestoryAllTiles();
      SmoothMap(true);
      InstantiateMap();
    }
    else if(Input.GetKeyDown(KeyCode.X))
    {
      DestoryAllTiles();
      SmoothMap(false);
      InstantiateMap();
    }
    */
	}

  void DestoryAllTiles()
  {
    foreach (GameObject i in tiles)
    {
      Destroy(i);
    }
    tiles.Clear();
  }

  void GenerateMap()
  {
    map = new int[height_cnt, width_cnt];
    RandomFillMap();
    for(int i = 0; i < smoothrounds; i ++)
    {
      SmoothMap(true);
    }
    for(int i = 0; i < 3; i ++)
    {
      SmoothMap(false);
    }
  }

  void RandomFillMap()
  {
    int seed = 2018;
    System.Random rand = new System.Random();
    for(int i = 0; i < height_cnt; i ++)
    {
      for(int j = 0; j < width_cnt; j ++)
      {
        if (i == 0 || i == height_cnt - 1 || j == 0 || j == width_cnt - 1)
          map[i, j] = 1;
        else
          map[i, j] = (rand.Next(0, 100) < randomFillPercent)? 1: 0;
      }
    }
  }

  void SmoothMap(bool improved)
  {
    for(int i = 1; i < height_cnt - 1; i ++)
    {
      for(int j = 1; j < width_cnt - 1; j ++)
      {
        int neighbourBlockCnt = GetSurroundingCnt(i, j);
        if (improved)
        {
          if (GetSurroundingCnt2(i, j) <= 2)
          {
            map[i, j] = 1;
            continue;
          }
        }
        if (neighbourBlockCnt > 4)
          map[i, j] = 1;
        else if (neighbourBlockCnt < 3)
          map[i, j] = 0;
      }
    }
  }

  int GetSurroundingCnt(int x, int y)
  {
    int blockcnt = 0;
    for(int i = x - 1; i <= x + 1; i ++)
    {
      for(int j = y - 1; j <= y + 1; j ++)
      {
        if(i != x || j != y)
        {
          blockcnt += map[i, j];
        }
      }
    }
    return blockcnt;
  }

  int GetSurroundingCnt2(int x, int y)
  {
    int blockcnt = 0;
    for (int i = x - 2; i <= x + 2; i++)
    {
      for (int j = y - 2; j <= y + 2; j++)
      {
        if (0 <= i && i < height_cnt && 0 <= j && j < width_cnt)
        {
          if (i == x - 2 || i == x + 2 || j == y - 2 || j == y + 2)
          {
            blockcnt += map[i, j];
          }
        }
      }
    }
    return blockcnt;
  }

  void InstantiateMap()
  {
    /*
    for(int i = height_cnt - 2; i >= height_cnt - 4; i --)
    {
      map[i, 1] = map[i, 2] = 0;
    }
    */

    for (int i = 0; i < entryHeight; i ++)
    {
      for(int j = 0; j < entryWidth; j ++)
      {
        map[1 + i, width_cnt / 2 - entryWidth / 2 + j] = 2;// + (int)GameManager.Direction.Down;
        map[height_cnt - 2 - i, width_cnt / 2 - entryWidth / 2 + j] = 2;// + (int)GameManager.Direction.Up;
        map[height_cnt / 2 - entryHeight / 2 + i, 1 + j] = 2;// + (int)GameManager.Direction.Left;
        map[height_cnt / 2 - entryHeight / 2 + i, width_cnt - 2 - j] = 2;// + (int)GameManager.Direction.Right;
      }
    }

    for(int i = 0; i < height_cnt; i ++)
    {
      for(int j = 0; j < width_cnt; j ++)
      {
        GameObject gobj = null;
        bool isDoor = false;
        if(i == 0 || i == height_cnt - 1 || j == 0 || j == width_cnt - 1)
        {
          gobj = wallTiles[0];
        }
        else
        {
          if (map[i, j] == 1)
            gobj = blockTiles[0];
          else if (map[i, j] >= 2)
          {
            continue;
          }
        }
        if (gobj != null) {
          GameObject inst = Instantiate(gobj, new Vector3(j * blocksize, i * blocksize, 0), Quaternion.identity) as GameObject;
          tiles.Add(inst);
          if(isDoor)
          {
            Door doorScript = inst.GetComponent<Door>();
            doorScript.direction = (GameManager.Direction)(map[i, j] - 2);
          }
          //Debug.Log(gobj.GetComponent<SpriteRenderer>().bounds.size);
        }
        else //if (i < height_cnt - 4 || j > 2) 
        {
          Pos newpos;
          newpos.x = i;
          newpos.y = j;
          emptyPosList.Add(newpos);
        }
      }
    }

    //instantiate doors
    float[] doorY = { 1.5f, height_cnt / 2 - 0.5f, height_cnt / 2 - 0.5f, height_cnt - 2.5f };
    float[] doorX = { width_cnt / 2 - 0.5f, 1.5f, width_cnt - 2.5f, width_cnt / 2 - 0.5f };
    GameManager.Direction[] doorDir = {
      GameManager.Direction.Down, GameManager.Direction.Left,
      GameManager.Direction.Right, GameManager.Direction.Up
    };
    for(int i = 0; i < 4; i ++)
    {
      GameObject inst = Instantiate(doorTile, new Vector3(doorX[i] * blocksize, doorY[i] * blocksize, 0), Quaternion.identity) as GameObject;
      tiles.Add(inst);
      Door doorScript = inst.GetComponent<Door>();
      doorScript.direction = doorDir[i];
    }
  }

  void InstansiateEnemys(int enemyCount)
  {
    ShuffleList(emptyPosList, enemyCount);
    for(int i = 0; i < enemyCount; i ++)
    {
      Pos pos = emptyPosList[i];

      Instantiate(enemy, new Vector3(pos.y * blocksize, pos.x * blocksize, 0), Quaternion.identity);
    }
  }

  void ShuffleList<T>(List<T> list, int firstN = 0)
  {
    if (firstN == 0 || firstN > list.Count)
      firstN = list.Count;
    for(int i = 0; i < firstN; i ++)
    {
      T temp = list[i];
      int randomIndex = Random.Range(i, list.Count);
      list[i] = list[randomIndex];
      list[randomIndex] = temp;
    }
  }
}
