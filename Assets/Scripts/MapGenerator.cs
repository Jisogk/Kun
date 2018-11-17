using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

  private int[,] map;
  private int width_cnt = 66;
  private int height_cnt = 66;
  private float blocksize = 4;

  [Range(0, 100)]
  public int randomFillPercent;

  public GameObject[] wallTiles;
  public GameObject[] blockTiles;
  public GameObject player;

  public int smoothrounds;

  private List<GameObject> tiles = new List<GameObject>();

	// Use this for initialization
	void Start () {
    GenerateMap();
    InstantiateMap();
    //player = Instantiate(robot, new Vector3(blocksize, blocksize * (height_cnt - 2), 0), Quaternion.identity) as GameObject;
    Vector2 initpos = new Vector2(blocksize * 1.5f, blocksize * (height_cnt - 3));
    //player.GetComponent<Rigidbody2D>().MovePosition(initpos);
    player.transform.position = initpos;
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
    for(int i = height_cnt - 2; i >= height_cnt - 4; i --)
    {
      map[i, 1] = map[i, 2] = 0;
    }

    for(int i = 0; i < height_cnt; i ++)
    {
      for(int j = 0; j < width_cnt; j ++)
      {
        GameObject gobj = null;
        if(i == 0 || i == height_cnt - 1 || j == 0 || j == width_cnt - 1)
        {
          gobj = wallTiles[0];
        }
        else
        {
          if (map[i, j] == 1)
            gobj = blockTiles[0];
        }
        if (gobj != null) {
          GameObject inst = Instantiate(gobj, new Vector3(j * blocksize, i * blocksize, 0), Quaternion.identity) as GameObject;
          tiles.Add(inst);
          //Debug.Log(gobj.GetComponent<SpriteRenderer>().bounds.size);
        }
      }
    }
  }
}
