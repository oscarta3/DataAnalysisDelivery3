using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DamagedData : MonoBehaviour
{
    private int damagedX;
    private int damagedY;
    private int damagedZ;
    private string baseUrl = "citmalumnes.upc.es/~oscarta3";
    private string phpurl = "/damaged.php";
    private string url;

    public DamagedData (int x, int y, int z)
    {
      this.damagedX = x;
      this.damagedY = y;
      this.damagedZ = z;
      
      string dataUrl = "?posX=" + damagedX + "&posY=" + damagedY + "&posZ=" + damagedZ; //PHP friendly string
      
      this.url = baseUrl + phpurl + dataUrl;
    }

    public string GetUrl()
    {
        return url;  
    }
}