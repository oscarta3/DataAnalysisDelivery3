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

    enum HeatmapType { None, Path, Jumped, Damaged, Death, EnemiesKilled }
    [SerializeField] HeatmapType heatType;

    List<Vector3> jumpPositionsList = new List<Vector3>();
    List<Vector3> pathPositionsList = new List<Vector3>();
    List<Vector3> damagedPositionsList = new List<Vector3>();
    List<Vector3> deathPositionsList = new List<Vector3>();
    List<Vector3> enemiesKilledPositionsList = new List<Vector3>();

    public List<GameObject> allCubes;
    public float searchRadius = 2.0f;
    public GameObject heatmapPointPrefab;
    public Gradient gradient;
    Color colorInicio = Color.green;
    Color colorFinal = Color.red;

    int gridWidth = 129;
    int gridHeight = 81;
    float pathMax = 0;
    float jumpMax = 0;
    float damagedMax = 0;
    float deathMax = 0;
    float killedMax = 0;
    public float max = 0;
    CubeCollision _cube;

    int[,] arrayName;

    void Start()
    {
        arrayName = new int[gridWidth, gridHeight];

        StartCoroutine(GetJumpedData());
        StartCoroutine(GetPathData());
        StartCoroutine(GetDamagedData());
        StartCoroutine(GetDeathData());
        StartCoroutine(GetEnemiesKilledData());

    }

    void GenerateGrid(List<Vector3> list)
    {
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                for (int k = 0; k < list.Count; k++)
                {
                    if (list[k].x >= i - 35 && list[k].x <= i - 31 && list[k].z >= j - 41 && list[k].z <= j - 37)
                    {
                        arrayName[i, j] += 1;
                        if (arrayName[i, j] > max)
                        {
                            max += 1;
                        }
                    }

                }
                j += 3;
            }
            i += 3;
        }

        if (max == 0)
        {
            max = 1;
        }

        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                if (arrayName[i, j] != 0)
                {
                    GameObject heatmapPoint = Instantiate(heatmapPointPrefab, new Vector3(i - 33, 20, j - 39), Quaternion.identity, transform);
                    float percentage = (arrayName[i,j] / max);
                    Color color = gradient.Evaluate(percentage);
                    heatmapPoint.GetComponent<Renderer>().material.color = color;
                    allCubes.Add(heatmapPoint);
                }
            }
        }

    }

    void OptionSelected()
    {
        if (heatType == HeatmapType.Path)
        {
            GenerateGrid(pathPositionsList);
            // EmptyGrid();
            // max = pathMax;
            // GenerateHeatmap(pathPositionsList);
        }
        if (heatType == HeatmapType.Jumped)
        {

            EmptyGrid();
            // max = jumpMax;
            // GenerateHeatmap(jumpPositionsList);
        }
        if (heatType == HeatmapType.Damaged)
        {
            EmptyGrid();
            max = damagedMax;
            GenerateHeatmap(damagedPositionsList);
        }
        if (heatType == HeatmapType.Death)
        {
            EmptyGrid();
            max = deathMax;
            GenerateHeatmap(deathPositionsList);
        }
        if (heatType == HeatmapType.EnemiesKilled)
        {
            EmptyGrid();
            max = killedMax;
            GenerateHeatmap(enemiesKilledPositionsList);
        }
        if (heatType == HeatmapType.None)
        {
            EmptyGrid();
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

    IEnumerator GetDamagedData()
    {
        WWW www = new WWW("https://citmalumnes.upc.es/~oscarta3/importdamaged.php");

        yield return www;
        string[] damagedData = www.text.Split("<br>");
        Vector3Int[] damagedDataInt = new Vector3Int[damagedData.Length - 3];

        for (int i = 2; i < (damagedData.Length - 1); i++)
        {
            string[] parts = damagedData[i].Split(" ");
            int x = int.Parse(parts[0]);
            int y = int.Parse(parts[1]);
            int z = int.Parse(parts[2]);

            Vector3Int vector = new Vector3Int(x, y, z);
            damagedDataInt[i - 2] = vector;
        }


        for (int i = 0; i < damagedDataInt.Length; i++)
        {
            damagedPositionsList.Add(damagedDataInt[i]);
        }
    }

    IEnumerator GetDeathData()
    {
        WWW www = new WWW("https://citmalumnes.upc.es/~oscarta3/importdeath.php");

        yield return www;
        string[] deathData = www.text.Split("<br>");
        Vector3Int[] deathDataInt = new Vector3Int[deathData.Length - 3];

        for (int i = 2; i < (deathData.Length - 1); i++)
        {
            string[] parts = deathData[i].Split(" ");
            int x = int.Parse(parts[0]);
            int y = int.Parse(parts[1]);
            int z = int.Parse(parts[2]);

            Vector3Int vector = new Vector3Int(x, y, z);
            deathDataInt[i - 2] = vector;
        }


        for (int i = 0; i < deathDataInt.Length; i++)
        {
            deathPositionsList.Add(deathDataInt[i]);
        }
    }

    IEnumerator GetEnemiesKilledData()
    {
        WWW www = new WWW("https://citmalumnes.upc.es/~oscarta3/importenkilled.php");

        yield return www;
        string[] enemiesKilledData = www.text.Split("<br>");
        Vector3Int[] enemiesKilledDataInt = new Vector3Int[enemiesKilledData.Length - 3];

        for (int i = 2; i < (enemiesKilledData.Length - 1); i++)
        {
            string[] parts = enemiesKilledData[i].Split(" ");
            int x = int.Parse(parts[0]);
            int y = int.Parse(parts[1]);
            int z = int.Parse(parts[2]);

            Vector3Int vector = new Vector3Int(x, y, z);
            enemiesKilledDataInt[i - 2] = vector;
        }


        for (int i = 0; i < enemiesKilledDataInt.Length; i++)
        {
            enemiesKilledPositionsList.Add(enemiesKilledDataInt[i]);
        }
    }

    void GenerateHeatmap(List<Vector3> positions)
    {
        foreach (Vector3 position in positions)
        {
            GameObject heatmapPoint = Instantiate(heatmapPointPrefab, new Vector3(position.x, 20, position.z), Quaternion.identity, transform);
            allCubes.Add(heatmapPoint);
        }
    }

    void EmptyGrid()
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

        if (max == 0)
        {
            max = 1;
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
