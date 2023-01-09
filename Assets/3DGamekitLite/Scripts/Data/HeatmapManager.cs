using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Networking;

public class HeatmapManager : MonoBehaviour
{

    List<Vector3Int> playerPositions = new List<Vector3Int>();
    public GameObject cubePrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(GetJumpedData());
        }
        if (Input.GetKeyDown(KeyCode.H))
        {

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
            playerPositions.Add(jumpDataInt[i]);
        }

        for (int i = 0; i < playerPositions.Count; i++)
        {
            // Creamos una instancia del cubo en la posición actual
            GameObject cube = Instantiate(cubePrefab, playerPositions[i], Quaternion.identity);
            cube.transform.localPosition = new Vector3(cube.transform.localPosition.x, 0.5f, cube.transform.localPosition.z);

            // Le asignamos un color cyan
            cube.GetComponent<Renderer>().material.color = Color.cyan;
        }

        

    }
}
