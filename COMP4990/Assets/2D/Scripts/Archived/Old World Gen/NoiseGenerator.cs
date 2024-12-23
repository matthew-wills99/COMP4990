using UnityEngine;

public class NoiseGenerator : MonoBehaviour
{
    public bool enable = false;
    public bool useColour = false;

    public int width = 256;
    public int height = 256;

    public float scale = 20f;
    public float offsetX = 0f;
    public float offsetY = 0f;

    System.Random random;
    //public MapController mapController;

    // TODO fix this shit
    public Color[] biomes;
    [Range(0f, 1f)]
    public float[] biomeThresholds; // should be in ascending order and equal in length to biomes


    void Start()
    {
        random = new System.Random(/*mapController.GetWorldSeed()*/100);
        offsetX = random.Next() / 10000;
        offsetY = random.Next() / 10000;
    }

    void Update()
    {
        //renderer.material.mainTexture = GenerateTexture();
        //renderer.enabled = enable;
    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                Color colour = CalculateColour(x, y);
                texture.SetPixel(x, y, colour);
            }
        }

        texture.Apply();
        return texture;
    }

    public Color CalculateColour(int x, int y)
    {
        // Get Perlin noise value for (x, y)
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / height * scale + offsetY;

        float sample = Mathf.Clamp(Mathf.PerlinNoise(xCoord, yCoord), 0f, 1f);

        if (useColour)
        {
            // Ensure biome thresholds are in ascending order and match the biomes array
            for (int i = 0; i < biomeThresholds.Length; i++)
            {
                if (sample < biomeThresholds[i])
                {
                    return biomes[i];
                }
            }

            // If sample is greater than the last threshold, return the last biome color
            return biomes[biomes.Length - 1];
        }

        // Return grayscale if useColour is false
        return new Color(sample, sample, sample);
    }

    // return a noise value for a given tile coordinate
    public float GetTileNoise(int x, int y)
    {
        float t = Mathf.Clamp(Mathf.PerlinNoise((float)x / width * scale + offsetX, (float)y / height * scale + offsetY), 0f, 1f);
        return t;
    }

    

}
