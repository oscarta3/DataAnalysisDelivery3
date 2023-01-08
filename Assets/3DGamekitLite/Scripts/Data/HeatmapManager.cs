using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Networking;

public class HeatmapManager : MonoBehaviour
{

    Vector3Int[] playerPositions = new Vector3Int[26];
    int ykes;
    Texture2D heatmapTexture;
    public Gradient colorGradient;
    float minValue = float.MaxValue;
    float maxValue = float.MinValue;



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(GetJumpedData());
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            PrepareHeatmap();
        }

    }

    IEnumerator GetJumpedData()
    {
        WWW www = new WWW("https://citmalumnes.upc.es/~oscarta3/importjump.php");

        yield return www;
        string[] jumpData = www.text.Split("<br>");
        Vector3Int[] jumpDataInt = new Vector3Int[jumpData.Length - 3];

        for (int i = 2; i < (jumpData.Length - 1); i++)
        {
            //Debug.Log(jumpData[i]);
            string[] parts = jumpData[i].Split(" ");
            int x = int.Parse(parts[0]);
            int y = int.Parse(parts[1]);
            int z = int.Parse(parts[2]);

            Vector3Int vector = new Vector3Int(x, y, z);
            jumpDataInt[i - 2] = vector;
        }
        

        for (int i = 0; i < jumpDataInt.Length; i++)
        {
            playerPositions[i] = jumpDataInt[i];
        }
       
    }

    void PrepareHeatmap()
    {
        for (int i = 0; i < playerPositions.Length; i++)
        {
            minValue = Mathf.Min(minValue, playerPositions[i].magnitude);
            maxValue = Mathf.Max(maxValue, playerPositions[i].magnitude);
        }

        heatmapTexture = new Texture2D(512, 512);
        GetComponent<Renderer>().material.mainTexture = heatmapTexture;
        GenerateHeatmap();
    }

    void GenerateHeatmap()
    {
        for (int x = 0; x < heatmapTexture.width; x++)
        {
            for (int y = 0; y < heatmapTexture.height; y++)
            {
                // Calculate the pixel's position in the world
                Vector3 pixelPos = transform.position + new Vector3(x, y) * 0.1f;

                // Calculate the heat value at this pixel based on the player positions
                float heat = 0;
                for (int i = 0; i < playerPositions.Length; i++)
                {
                    heat += (1 - Vector3.Distance(pixelPos, playerPositions[i]) / 10f);
                }
                heat = Mathf.Clamp01(heat);

                // Map the heat value to a color using the color gradient
                Color color = colorGradient.Evaluate(heat);

                // Set the pixel's color on the texture
                heatmapTexture.SetPixel(x, y, color);
            }
        }

        // Apply the changes to the texture
        heatmapTexture.Apply();
    }
}
