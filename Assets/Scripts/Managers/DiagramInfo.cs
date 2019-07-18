using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiagramInfo : MonoBehaviour {

    //public static DiagramInfo instance = null;

    public List<RobotDiagram> DiagramList;

    //public RobotDiagram CurrentDiagram;

    // Use this for initialization
    void Awake () {
        /*if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }*/
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddDiagram(RobotDiagram diagram)
    {
        DiagramList.Add(diagram);
    }

    public void RemoveDiagram(int index)
    {
        DiagramList.RemoveAt(index);
    }

}
