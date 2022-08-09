using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanyonAgent
{
    // private int numberOfAgent;
    private float[,] heightmap;
    private int token;
    private int brushWidth; // the width of brush
    private int blurWidth; // the width of interplate
    private System.Random rand;

    private int mapWidth;
    private int mapHeight;
    private int[] position;
    private bool isVertical;
    private float canyonDepth = 0.3f;

    public CanyonAgent(int brushWidth, float[,] heightmap, int token, System.Random rand)
    {
        this.brushWidth = brushWidth;
        this.blurWidth = 5;
        this.heightmap = heightmap;
        this.token = token;
        this.rand = rand;

        mapWidth = heightmap.GetLength(0);
        mapHeight = heightmap.GetLength(1);
        position = new int[2];
        isVertical = false; // true if move forward to z axis
        // false forward to x axis
    }

    private bool isInMap(int[] position)
    {
        return position[0] > 0 && position[1] > 0 && position[0] < mapWidth && position[1] < mapHeight;
    }
    public float[,] run()
    {
        //position[0] = rand.Next(0, mapWidth);
        //position[1] = rand.Next(0, mapHeight);
        // position[0] = rand.Next(brushWidth, mapWidth - brushWidth);
        // position[1] = rand.Next(brushWidth, mapHeight - brushWidth);
        if (rand.Next(0, 2) == 0)
        {
            isVertical = true;
        }
        if(isVertical)
        {
            position[0] = rand.Next(brushWidth, mapWidth - brushWidth);
            position[1] = 1;
        }
        else
        {
            position[0] = 1;
            position[1] = rand.Next(brushWidth, mapHeight - brushWidth);
        }
        while (token > 0 && isInMap(position))
        {
            // Debug.Log("Token: " + token);
            createCanyon();
            moveForwards();
            token--;
        }
        return heightmap;
    }

    private void createCanyon()
    {
        if (isVertical)
        {
            int left = brushWidth / 2;
            int right = brushWidth - left;
            int[] tempPos = new int[2];
            tempPos[0] = position[0];
            tempPos[1] = position[1];
            // create left moutain
            while (isInMap(tempPos) && left > 0)
            {
                heightmap[tempPos[0], tempPos[1]] = canyonDepth; // canyon will lower the surface
                // Debug.Log("height map: " + heightmap[tempPos[0], tempPos[1]]);
                tempPos[0] -= 1;
                left -= 1;
            }
            // create moutain left slope
            int blurCount = blurWidth;
            float currentHeight = canyonDepth;
            float interval = (1.0f-canyonDepth) / blurWidth;
            while (isInMap(tempPos) && blurCount > 0)
            {
                currentHeight += interval + (rand.Next(0, 100) - 50) / 500.0f;
                // Debug.Log("Current Height: " + currentHeight);
                if (currentHeight < heightmap[tempPos[0], tempPos[1]])
                {
                    heightmap[tempPos[0], tempPos[1]] = currentHeight;
                    // Debug.Log("Current Height: " + heightmap[tempPos[0], tempPos[1]]);
                }
                    
                tempPos[0] -= 1;
                blurCount -= 1;
            }
            // create right moutain
            // back to start point
            tempPos[0] = position[0];
            tempPos[1] = position[1];
            while (isInMap(tempPos) && right > 0)
            {
                heightmap[tempPos[0], tempPos[1]] = canyonDepth; // moutain will rise 1.5
                tempPos[0] += 1;
                right -= 1;
            }
            // create right slope
            blurCount = blurWidth;
            currentHeight = canyonDepth;
            interval = (1.0f - canyonDepth) / blurWidth;
            while (isInMap(tempPos) && blurCount > 0)
            {
                // Debug.Log("Interval: " + interval);
                // Debug.Log("Random: " + ((rand.Next(0, 100) - 50) / 500.0f));
                currentHeight += interval + (rand.Next(0, 100) - 50) / 500.0f; // make the slope more natural
                if(currentHeight < heightmap[tempPos[0], tempPos[1]])
                    heightmap[tempPos[0], tempPos[1]] = currentHeight;
                tempPos[0] += 1;
                blurCount -= 1;
            }
        }
        else
        {
            int up = brushWidth / 2;
            int down = brushWidth - up;
            int[] tempPos = new int[2];
            tempPos[0] = position[0];
            tempPos[1] = position[1];
            // create up moutain
            while (isInMap(tempPos) && up > 0)
            {
                heightmap[tempPos[0], tempPos[1]] = canyonDepth; // moutain will rise 1.5
                tempPos[1] -= 1;
                up -= 1;
            }
            // create up slope
            int blurCount = blurWidth;
            float currentHeight = canyonDepth;
            float interval = canyonDepth / blurWidth;
            while (isInMap(tempPos) && blurCount > 0)
            {
                currentHeight += interval + (rand.Next(0, 100) - 50) / 500.0f;
                heightmap[tempPos[0], tempPos[1]] = currentHeight;
                tempPos[1] -= 1;
                blurCount -= 1;
            }

            tempPos[0] = position[0];
            tempPos[1] = position[1];
            // create down moutain
            while (isInMap(tempPos) && down > 0)
            {
                heightmap[tempPos[0], tempPos[1]] = canyonDepth;
                tempPos[1] += 1;
                down -= 1;
            }

            // create down slope
            blurCount = blurWidth;
            currentHeight = canyonDepth;
            interval = canyonDepth / blurWidth;
            while (isInMap(tempPos) && blurCount > 0)
            {
                // Debug.Log("Interval: " + interval);
                // Debug.Log("Random: " + ((rand.Next(0, 100) - 50) / 500.0f));
                currentHeight += interval + (rand.Next(0, 100) - 50) / 500.0f; // make the slope more natural
                heightmap[tempPos[0], tempPos[1]] = currentHeight;
                tempPos[1] += 1;
                blurCount -= 1;
            }
        }
    }

    private void moveForwards() // cannot move negativley
    {
        if (isVertical)
        {
            position[1] += 1;
        }
        else
        {
            position[0] += 1;
        }
    }
}
