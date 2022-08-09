using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Global")]
    public int width = 256;
    public int height = 256;
    public int depth = 20;
    public int seed = 114514;

    public enum TerrainType
    {
        Canyon,
        Desert,
        Hill,
        Moutain,
    }
    public TerrainType type;

    // crossoverable parameters
    public int numberOfSmoothAgent = 500;
    public int tokenOfSmoothAgent = 100;

    public int numberOfMoutainAgent = 5;

    public int brushWidth = 0;
    public int tokenOfMoutainAgent = 10;
    public bool postSmooth = false; // smooth after moutain created
    void Start()
    {

        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height);
        // terrainData.SetHeights(0, 0, GenerateHeights());
        if (type == TerrainType.Canyon)
        {
            terrainData.SetHeights(0, 0, CanyonGenerator());
        }
        else if (type == TerrainType.Desert)
        {
            terrainData.SetHeights(0, 0, DesertGenerator());
        }
        else if (type == TerrainType.Hill)
        {
            terrainData.SetHeights(0, 0, HillGenerator());
        }
        else if (type == TerrainType.Moutain)
        {
            terrainData.SetHeights(0, 0, MoutainGenerator());
        }
        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];
        // call agents
        System.Random rand = new System.Random(seed);

        InitAgent ia = new InitAgent(heights, rand);
        heights = ia.run();

        SmoothAgent sa = new SmoothAgent(numberOfSmoothAgent, heights, tokenOfSmoothAgent, rand);
        heights = sa.run();

        int count = 0;
        while (count < numberOfMoutainAgent)
        {
            MoutainAgent ma = new MoutainAgent(brushWidth, heights, tokenOfMoutainAgent, rand);
            heights = ma.run();
            count++;
        }

        if (postSmooth)
        {
            SmoothAgent sa1 = new SmoothAgent(10, heights, 50, rand);
            heights = sa1.run();
        }

        return heights;
    }

    private float[,] CanyonGenerator()
    {
        float[,] heights = new float[width, height];
        // call agents
        System.Random rand = new System.Random(seed);

        InitAgent ia = new InitAgent(heights, rand, 70, 100);
        heights = ia.run();

        SmoothAgent sa = new SmoothAgent(numberOfSmoothAgent, heights, tokenOfSmoothAgent, rand);
        heights = sa.run();

        CanyonAgent ca = new CanyonAgent(10, heights, 1, rand);
        heights = ca.run();

        if (postSmooth)
        {
            SmoothAgent sa1 = new SmoothAgent(10, heights, 50, rand);
            heights = sa1.run();
        }

        return heights;
    }

    private float[,] DesertGenerator()
    {
        float[,] heights = new float[width, height];
        // call agents
        System.Random rand = new System.Random(seed);

        InitAgent ia = new InitAgent(heights, rand);
        heights = ia.run();

        return heights;
    }

    private float[,] HillGenerator()
    {
        float[,] heights = new float[width, height];
        // call agents
        System.Random rand = new System.Random(seed);

        InitAgent ia = new InitAgent(heights, rand);
        heights = ia.run();

        SmoothAgent sa = new SmoothAgent(numberOfSmoothAgent, heights, tokenOfSmoothAgent, rand);
        heights = sa.run();

        int count = 0;
        while (count < numberOfMoutainAgent)
        {
            MoutainAgent ma = new MoutainAgent(brushWidth, heights, tokenOfMoutainAgent, rand);
            heights = ma.run();
            count++;
        }

        if (postSmooth)
        {
            SmoothAgent sa1 = new SmoothAgent(10, heights, 50, rand);
            heights = sa1.run();
        }

        return heights;
    }

    private float[,] MoutainGenerator()
    {
        float[,] heights = new float[width, height];
        // call agents
        System.Random rand = new System.Random(seed);

        InitAgent ia = new InitAgent(heights, rand);
        heights = ia.run();

        return heights;
    }
}
