using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveData : MonoBehaviour
{
    private int movedX;
    private int movedY;
    private int movedZ;
    private string baseUrl = "citmalumnes.upc.es/~oscarta3";
    private string phpurl = "/path.php";
    private string url;

    public MoveData (int x, int y, int z)
    {
      this.movedX = x;
      this.movedY = y;
      this.movedZ = z;
      
      string dataUrl = "?posX=" + movedX + "&posY=" + movedY + "&posZ=" + movedZ; //PHP friendly string
      
      this.url = baseUrl + phpurl + dataUrl;
    }

    public string GetUrl()
    {
        return url;  
    }
}
