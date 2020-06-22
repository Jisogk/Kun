using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotDiagram
{
    public int[,] Diagram;
    public float FillPercent = 0.85f;

    //默认0.85填充率构造
    public RobotDiagram(int length, int width)
    {
        Diagram = new int[width, length];
        for (int a = 0; a < width; a++)
        {
            for (int b = 0; b < length; b++)
            {
                if (Random.Range(0f, 1f) < FillPercent)
                {
                    Diagram[a, b] = 1;
                }
                else
                {
                    Diagram[a, b] = 0;
                }
            }
        }
    }

    //可变填充率构造
    public RobotDiagram(int length, int width, float fillpercent)
    {
        FillPercent = fillpercent;
        Diagram = new int[width, length];
        for (int a = 0; a < width; a++)
        {
            for (int b = 0; b < length; b++)
            {
                if (Random.Range(0f, 1f) < FillPercent)
                {
                    Diagram[a, b] = 1;
                }
                else
                {
                    Diagram[a, b] = 0;
                }
            }
        }
    }

    //全填充构造
    public RobotDiagram(int length, int width, string cheatCode)
    {
        if (cheatCode.Equals("Perfect"))
        {
            Diagram = new int[width, length];
            for (int a = 0; a < width; a++)
            {
                for (int b = 0; b < length; b++)
                {
                    Diagram[a, b] = 1;
                }
            }
        }
    }
}


