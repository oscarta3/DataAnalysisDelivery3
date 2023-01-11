using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading;

public class HeatmapManager : MonoBehaviour
{

    enum HeatmapType { None, Position, Jumped, Damaged, Death, EnemiesKilled, Path, Paths, Grid }
    [SerializeField] HeatmapType heatType;


    //[SerializeField] 
    //PathNumber pathNum;

    List<Vector3> jumpPositionsList = new List<Vector3>();
    List<Vector3> positionList = new List<Vector3>();
    List<Vector3> damagedPositionsList = new List<Vector3>();
    List<Vector3> deathPositionsList = new List<Vector3>();
    List<Vector3> enemiesKilledPositionsList = new List<Vector3>();
    List<Vector4> PathPositionsList = new List<Vector4>();

    public List<GameObject> allCubes;
    public List<float> colorPer;
    public GameObject heatmapPointPrefab;
    public Gradient gradient;
    public Gradient _gradient = null;

    int gridWidth = 129;
    int gridHeight = 81;
    int numberPaths = 0;
    float pathMax = 0;
    float jumpMax = 0;
    float damagedMax = 0;
    float deathMax = 0;
    float killedMax = 0;
    public float max = 0;
    bool gradientdif = false;

    private Coroutine coroutine;


    int[,] arrayName;

    void Start()
    {
        arrayName = new int[gridWidth, gridHeight];

        StartCoroutine(GetJumpedData());
        StartCoroutine(GetPositionData());
        StartCoroutine(GetDamagedData());
        StartCoroutine(GetDeathData());
        StartCoroutine(GetEnemiesKilledData());
        StartCoroutine(GetPathData());

    }

    void Update()
    {

        if (gradientdif)
        {
            int i = 0;
            foreach (GameObject instance in allCubes)
            {
                Color color = gradient.Evaluate(colorPer[i]);
                instance.GetComponent<Renderer>().material.color = color;
                i++;
            }
        }
    }

    void CreateGrid()
    {
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                GameObject gridCubes = Instantiate(heatmapPointPrefab, new Vector3(i - 33, 20, j - 39), Quaternion.identity, transform);
                allCubes.Add(gridCubes);
                j += 1;
            }
            i += 1;
        }
    }

    void GenerateGrid(List<Vector3> list)
    {
        for (int i = 0; i < gridWidth; i += 2)
        {
            for (int j = 0; j < gridHeight; j += 2)
            {
                for (int k = 0; k < list.Count; k++)
                {
                    if (list[k].x >= i - 34 && list[k].x <= i - 32 && list[k].z >= j - 40 && list[k].z <= j - 38)
                    {
                        arrayName[i, j] += 1;
                        if (arrayName[i, j] > max)
                        {
                            max += 1;
                        }
                    }
                }
            }
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
                    float percentage = (arrayName[i, j] / max);
                    Color color = gradient.Evaluate(percentage);
                    heatmapPoint.GetComponent<Renderer>().material.color = color;
                    allCubes.Add(heatmapPoint);
                    colorPer.Add(percentage);
                }
            }
        }

    }

    IEnumerator GeneratePath(List<Vector4> list)
    {

        List<Vector4> list2 = new List<Vector4>();
        for (int p = 1; p < list.Count; p++)
        {
            if (list[p - 1].w == p)
            {
                for (int i = 0; i < gridWidth; i += 2)
                {

                    for (int j = 0; j < gridHeight; j += 2)
                    {
                        if (list[p].x >= i - 34 && list[p].x <= i - 32 && list[p].z >= j - 40 && list[p].z <= j - 38)
                        {
                            Vector4 Test = new Vector4(i, 20, j, list[p].w);
                            list2.Add(Test);
                        }
                    }
                }
            }
        }
        for (int i = 0; i < list2.Count; i++)
        {
            yield return new WaitForSeconds(0.03f);
            GameObject heatmapPoint = Instantiate(heatmapPointPrefab, new Vector3(list2[i].x - 33, 20 + i * 0.001f, list2[i].z - 39), Quaternion.identity, transform);
            float percentage = (list2[i].w / list2[list2.Count - 1].w);
            Color color = gradient.Evaluate(percentage);
            heatmapPoint.GetComponent<Renderer>().material.color = color;
            allCubes.Add(heatmapPoint);
            colorPer.Add(percentage);
        }


        StopCoroutine(GeneratePath(list));

    }

    void EmptyGrid()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        if (allCubes.Count > 0)
        {
            for (int i = 0; i < allCubes.Count; i++)
            {
                GameObject cube = allCubes[i];
                Destroy(cube);
                arrayName = new int[gridWidth, gridHeight];
            }
            allCubes.Clear();
            colorPer.Clear();
        }
        max = 0;
    }

    void OptionSelected()
    {
        if (heatType == HeatmapType.Position)
        {
            EmptyGrid();
            GenerateGrid(positionList);
        }
        if (heatType == HeatmapType.Jumped)
        {
            EmptyGrid();
            GenerateGrid(jumpPositionsList);
        }
        if (heatType == HeatmapType.Damaged)
        {
            EmptyGrid();
            GenerateGrid(damagedPositionsList);
        }
        if (heatType == HeatmapType.Death)
        {
            EmptyGrid();
            GenerateGrid(deathPositionsList);
        }
        if (heatType == HeatmapType.EnemiesKilled)
        {
            EmptyGrid();
            GenerateGrid(enemiesKilledPositionsList);
        }
        if (heatType == HeatmapType.Path)
        {
            EmptyGrid();
            coroutine = StartCoroutine(GeneratePath(PathPositionsList));
        }
        if (heatType == HeatmapType.None)
        {
            EmptyGrid();
        }
        if (heatType == HeatmapType.Grid)
        {
            EmptyGrid();
            CreateGrid();
        }
        if (heatType != HeatmapType.Grid)
        {
            if (gradient != null)
            {
                for (int i = 0; i < gradient.colorKeys.Length; i++)
                {
                    if (gradient.colorKeys[i].color != _gradient.colorKeys[i].color || gradient.colorKeys[i].time != _gradient.colorKeys[i].time)
                    {
                        gradientdif = true;
                        ChangeGradient();
                    }
                }
            }
        }


    }

    void OnValidate()
    {
        OptionSelected();
    }

    void ChangeGradient()
    {
        _gradient.SetKeys(gradient.colorKeys, gradient.alphaKeys);
        gradientdif = false;
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

    IEnumerator GetPositionData()
    {
        WWW www = new WWW("https://citmalumnes.upc.es/~oscarta3/importposition.php");

        yield return www;
        string[] PositionData = www.text.Split("<br>");
        Vector3Int[] PositionDataInt = new Vector3Int[PositionData.Length - 3];

        for (int i = 2; i < (PositionData.Length - 1); i++)
        {
            string[] parts = PositionData[i].Split(" ");
            int x = int.Parse(parts[0]);
            int y = int.Parse(parts[1]);
            int z = int.Parse(parts[2]);

            Vector3Int vector = new Vector3Int(x, y, z);
            PositionDataInt[i - 2] = vector;
        }


        for (int i = 0; i < PositionDataInt.Length; i++)
        {
            positionList.Add(PositionDataInt[i]);
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
        WWW www = new WWW("https://citmalumnes.upc.es/~oscarta3/importenemy.php");

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

    IEnumerator GetPathData()
    {
        WWW www = new WWW("https://citmalumnes.upc.es/~oscarta3/importpath.php");

        yield return www;
        string[] pathData = www.text.Split("<br>");
        Vector4[] pathDataInt = new Vector4[pathData.Length - 3];

        for (int i = 2; i < (pathData.Length - 1); i++)
        {
            string[] parts = pathData[i].Split(" ");
            int x = int.Parse(parts[0]);
            int y = int.Parse(parts[1]);
            int z = int.Parse(parts[2]);
            int order = int.Parse(parts[3]);

            Vector4 vector = new Vector4(x, y, z, order);
            pathDataInt[i - 2] = vector;
        }


        for (int i = 0; i < pathDataInt.Length; i++)
        {
            PathPositionsList.Add(pathDataInt[i]);
        }
    }
}
