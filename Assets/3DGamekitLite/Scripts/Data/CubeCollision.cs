using System.Collections.Generic;
using UnityEngine;

public class CubeCollision : MonoBehaviour
{
    float searchRadius;
    public HeatmapManager _puente;
    public List<GameObject> allCubes;
    public float numCubes;
    Color color;

    void Start()
    {
        allCubes = _puente.allCubes;
        searchRadius = _puente.searchRadius;
        CheckNearbyCubes(searchRadius, allCubes);
    }

    void Update()
    {
    }

    void CheckNearbyCubes(float radius, List<GameObject> cubes)
    {
        foreach (GameObject cube in cubes)
        {
            if (cube == gameObject) continue; 

            float distance = Vector3.Distance(gameObject.transform.position, cube.transform.position);
            if (distance <= radius)
            {
                numCubes += 1;
            }
        }
        _puente.CompareCubes();
    }
}
