using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTile : MonoBehaviour
{
    public GameObject tile;


    // Start is called before the first frame update
    void Start()
    {
        NewTile();
    }

    public void NewTile()
    {
        Instantiate(tile, transform.position, Quaternion.identity);
    }
}
