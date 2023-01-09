using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCollision : MonoBehaviour
{
    // Número de colisiones que ha sufrido el cubo
    public int numCollisions = 0;

    void OnCollisionEnter(Collision collision)
        {
            // Si el cubo colisiona con otro cubo, cambiamos su color
            if (collision.gameObject.CompareTag("Cube"))
            {
                // Obtenemos la cantidad de cubos con los que ha colisionado
                int numCollisions = collision.gameObject.GetComponent<CubeCollision>().numCollisions;

                // Mostramos el valor de numCollisions en la consola de Unity
                Debug.Log("numCollisions: " + numCollisions);

                // Según el número de colisiones, cambiamos el color del cubo
                if (numCollisions == 1)
                {
                    collision.gameObject.GetComponent<Renderer>().material.color = Color.green;
                }
                else if (numCollisions == 2)
                {
                    collision.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                }
                else
                {
                    collision.gameObject.GetComponent<Renderer>().material.color = Color.red;
                }
            }
        }
}
