using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitAgent
{
    private float[,] heightmap;
    private System.Random rand;
    private int minHeight;
    private int maxHeight;
    // minHeight and maxHeight should between 0 and 30
    public InitAgent(float[,] heightmap, System.Random rand, int minHeight = 0, int maxHeight = 30)
    {
        this.heightmap = heightmap;
        this.rand = rand;
        this.minHeight = minHeight;
        this.maxHeight = maxHeight;
    }

    public float[,] run()
    {
        for (int i = 0; i < heightmap.GetLength(0); i++)
        {
            for (int j = 0; j < heightmap.GetLength(1); j++)
            {
                heightmap[i, j] = rand.Next(minHeight, maxHeight) / 100.0f;
            }
        }
        return heightmap;
    }
}
