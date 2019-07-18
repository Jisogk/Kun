using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTest : MonoBehaviour {

    public GameObject wallTile;

    public int Width = 66;
    public int Height = 66;

    private int[,] map;
    private float blocksize = 4.0f;
    private List<GameObject> tiles;

    // Use this for initialization
    void Start () {
        map = new int[Width, Height];
        for (int a = 0; a < Width; a++)
        {
            map[a, 0] = 1;
            map[a, Height] = 1;
        }
        for (int b = 0; b < Height; b++)
        {
            map[0, b] = 1;
            map[Width, b] = 1;
        }

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (map[i, j] == 1)
                {
                    GameObject inst = Instantiate(wallTile, new Vector3(i * blocksize, j * blocksize, 0), Quaternion.identity) as GameObject;
                    tiles.Add(inst);
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
