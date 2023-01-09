using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
//using UnityEngine.Networking;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class MyClass
{
    public string name;
    public int value;
}

public class HeatmapManager : MonoBehaviour
{
    

    public List<Vector3> jumpPositionsList = new List<Vector3>();
    public List<Vector3> pathPositionsList = new List<Vector3>();
    public List<GameObject> allCubes;
    public float searchRadius = 2.0f;
    public GameObject heatmapPointPrefab;
    public Gradient gradient;
    Color colorInicio = Color.green;
    Color colorFinal = Color.red;
    public float max = 0;
    CubeCollision _cube;

    [HideInInspector]
    public int arrayIdx = 0;

    [HideInInspector]
    public string[] MyArray = new string[]{"Path", "Jumped"};


    void OnGUI()
    {
        // Display the dropdown menu
        arrayIdx = EditorGUILayout.Popup(arrayIdx, MyArray);

        // Call a function when the selection changes
        if (GUI.changed)
        {
            OnSelectionChanged(arrayIdx);
        }
    }

    void OnSelectionChanged(int index)
    {
        // Do something when the selection changes
        Debug.Log("Selection changed to: " + MyArray[index]);
    }

    void Start()
    {
        StartCoroutine(GetJumpedData());
        StartCoroutine(GetPathData());
    }

    void OptionSelected()
    {
        
    }

    void OnValidate()
    {
        OptionSelected();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GenerateHeatmap(pathPositionsList);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            GenerateHeatmap(jumpPositionsList);
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
