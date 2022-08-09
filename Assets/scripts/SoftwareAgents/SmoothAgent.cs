using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothAgent
{
    private float[,] heightMap;
    private int token;
    private System.Random rand;

    private int[] position;
    private int mapWidth;
    private int mapHeight;
    private int numberOfAgent;
    public SmoothAgent(int numberOfAgent, float[,] heightMap, int token, System.Random rand)
    {
        this.heightMap = heightMap;
        this.token = token;

        this.rand = rand;
        this.position = new int[2];
        this.mapWidth = heightMap.GetLength(0);
        this.mapHeight = heightMap.GetLength(1);
        this.numberOfAgent = numberOfAgent;
    }

    public float[,] run()
    {
        int tempToken;
        while (numberOfAgent > 0)
        {
            tempToken = this.token;
            // generate random position
            position[0] = rand.Next(0, mapWidth);
            position[1] = rand.Next(0, mapHeight);

            while (tempToken > 0) // this agent alive
            {
                smooth();
                move();
                tempToken--;
            }
            numberOfAgent--;
        }
        return heightMap;
    }

    private void smooth()
    {
        // do the smooth
        float value = 0;
        int count = 0;
        int[] temp_pos = new int[2];
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                temp_pos[0] = position[0] + i;
                temp_pos[1] = position[1] + j;
                if (isInBound(temp_pos)) // in the height map boundary
                {
                    value += heightMap[temp_pos[0], temp_pos[1]];
                    count += 1;
                }
            }
        }
        heightMap[position[0], position[1]] = (float)value / count;
    }

    private void move()
    {
        // move to the next random location
        int[] temp_pos = new int[2];
        temp_pos[0] = position[0];
        temp_pos[1] = position[1];
        int choice = rand.Next(0, 2);
        if (choice == 0) // horizontal
        {
            choice = rand.Next(0, 2);
            if (choice == 0)
            {
                temp_pos[1] += 1;
            }
            else
            {
                temp_pos[1] -= 1;
            }
        }
        else
        {
            choice = rand.Next(0, 2);
            if (choice == 0)
            {
                temp_pos[0] += 1;
            }
            else
            {
                temp_pos[0] -= 1;
            }
        }
        while (!isInBound(temp_pos))
        {
            temp_pos[0] = position[0];
            temp_pos[1] = position[1];
            choice = rand.Next(0, 2);
            if (choice == 0) // horizontal
            {
                choice = rand.Next(0, 2);
                if (choice == 0)
                {
                    temp_pos[1] += 1;
                }
                else
                {
                    temp_pos[1] -= 1;
                }
            }
            else
            {
                choice = rand.Next(0, 2);
                if (choice == 0)
                {
                    temp_pos[0] += 1;
                }
                else
                {
                    temp_pos[0] -= 1;
                }
            }
        }
        position[0] = temp_pos[0];
        position[1] = temp_pos[1];
    }

    private bool isInBound(int[] pos)
    {
        return pos[0] >= 0 && pos[0] < mapWidth && pos[1] >= 0 && pos[1] < mapHeight;
    }
}
