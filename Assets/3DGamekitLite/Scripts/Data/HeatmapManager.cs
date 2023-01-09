using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Networking;

public class HeatmapManager : MonoBehaviour
{

    public List<Vector3> playerPositions = new List<Vector3>();
    public List<GameObject> allCubes;
    public float searchRadius = 2.0f;
    public GameObject heatmapPointPrefab;
    public Gradient gradient;
    Color colorInicio = Color.green;
    Color colorFinal = Color.red;
    public float max = 0;
    CubeCollision _cube;

    void Start()
    {
        StartCoroutine(GetJumpedData());

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(GetJumpedData());
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            GenerateHeatmap(playerPositions);
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
            string[] parts = jumpData[i].Split(" ");
            int x = int.Parse(parts[0]);
            int y = int.Parse(parts[1]);
            int z = int.Parse(parts[2]);

            Vector3Int vector = new Vector3Int(x, y, z);
            jumpDataInt[i - 2] = vector;
        }


        for (int i = 0; i < jumpDataInt.Length; i++)
        {
            playerPositions.Add(jumpDataInt[i]);
        }
    }

    void GenerateHeatmap(List<Vector3> positions)
    {
        foreach (Vector3 position in positions)
        {
            GameObject heatmapPoint = Instantiate(heatmapPointPrefab, new Vector3(position.x, 0.5f, position.z), Quaternion.identity, transform);
            allCubes.Add(heatmapPoint);
        }
    }

    public void CompareCubes()
    {
        foreach (GameObject go in allCubes)
        {
            CubeCollision script = go.GetComponent<CubeCollision>();
            if(script.numCubes > max)
            {
                max = script.numCubes;
            }
        }

        ColorCubes();
    }

    public void ColorCubes()
    {
        foreach (GameObject go in allCubes)
        {
            CubeCollision script = go.GetComponent<CubeCollision>();
            float percentage = (script.numCubes / max);
            Color color = gradient.Evaluate(percentage);
            script.GetComponent<Renderer>().material.color = color;
        }
    }
}
