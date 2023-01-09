using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
//using UnityEngine.Networking;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class HeatmapManager : MonoBehaviour
{

    enum HeatmapType { None, Path, Jumped, Damaged }
    [SerializeField] HeatmapType heatType;

    public List<Vector3> jumpPositionsList = new List<Vector3>();
    public List<Vector3> pathPositionsList = new List<Vector3>();
    public List<GameObject> allCubes;
    public float searchRadius = 2.0f;
    public GameObject heatmapPointPrefab;
    public Gradient gradient;
    Color colorInicio = Color.green;
    Color colorFinal = Color.red;
    public float pathMax = 0;
    public float jumpMax = 0;
    public float max = 0;
    CubeCollision _cube;

    void Start()
    {
        StartCoroutine(GetJumpedData());
        StartCoroutine(GetPathData());
    }

    void OptionSelected()
    {
        if (heatType == HeatmapType.Path)
        {
            DestroyCurrentHeatmap();
            max = pathMax;
            GenerateHeatmap(pathPositionsList);
        }
        if (heatType == HeatmapType.Jumped)
        {
            DestroyCurrentHeatmap();
            max = jumpMax;
            GenerateHeatmap(jumpPositionsList);
        }
        if (heatType == HeatmapType.Damaged)
        {
            DestroyCurrentHeatmap();
        }
        if (heatType == HeatmapType.None)
        {
            DestroyCurrentHeatmap();
        }

    }

    void OnValidate()
    {
        OptionSelected();
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
            jumpPositionsList.Add(jumpDataInt[i]);
        }
    }

    IEnumerator GetPathData()
    {
        WWW www = new WWW("https://citmalumnes.upc.es/~oscarta3/importpath.php");

        yield return www;
        string[] pathData = www.text.Split("<br>");
        Vector3Int[] pathDataInt = new Vector3Int[pathData.Length - 3];

        for (int i = 2; i < (pathData.Length - 1); i++)
        {
            string[] parts = pathData[i].Split(" ");
            int x = int.Parse(parts[0]);
            int y = int.Parse(parts[1]);
            int z = int.Parse(parts[2]);

            Vector3Int vector = new Vector3Int(x, y, z);
            pathDataInt[i - 2] = vector;
        }


        for (int i = 0; i < pathDataInt.Length; i++)
        {
            pathPositionsList.Add(pathDataInt[i]);
        }
    }

    void GenerateHeatmap(List<Vector3> positions)
    {
        foreach (Vector3 position in positions)
        {
            GameObject heatmapPoint = Instantiate(heatmapPointPrefab, new Vector3(position.x, position.y, position.z), Quaternion.identity, transform);
            allCubes.Add(heatmapPoint);
        }
    }

    void DestroyCurrentHeatmap()
    {
        if (allCubes.Count > 0)
        {
            for (int i = 0; i < allCubes.Count; i++)
            {
                GameObject cube = allCubes[i];
                Destroy(cube);
            }
            allCubes.Clear();
        }
    }

    public void CompareCubes()
    {
        foreach (GameObject go in allCubes)
        {

            CubeCollision script = go.GetComponent<CubeCollision>();
            if (script.numCubes > max)
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

#if UNITY_EDITOR


#endif
