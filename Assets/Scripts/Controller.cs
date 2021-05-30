using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameObject[,] stones1 = new GameObject[5,13];
    public GameObject[,] stones2 = new GameObject[5, 13];
    public Vector3 startPoint; 
    public GameObject stonePrefab;
    public Sprite [] stonesAll;
    public SpriteMask mask;

    void Start()
    {
        int index = 0;

        for (int i=0; i<4; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                
                stones1[i, j] = Instantiate(stonePrefab, startPoint + new Vector3(j,i*2,0), Quaternion.identity);
                stones1[i, j].GetComponent<SpriteRenderer>().sprite = stonesAll[index];
                index++;
            }
        }

        index = 0;

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 13; j++)
            {

                stones2[i, j] = Instantiate(stonePrefab, startPoint + new Vector3(j + 20, i * 2, 0), Quaternion.identity);
                stones2[i, j].GetComponent<SpriteRenderer>().sprite = stonesAll[index];
                index++;
            }
        }




    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
