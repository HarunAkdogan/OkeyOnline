using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameObject[,,] stones = new GameObject[2,5,13];
    public Vector3 startPoint; 
    public GameObject stonePrefab;
    public Sprite [] stonesAll;
    public SpriteMask mask;
    private ArrayList myStones = new ArrayList();

    void Start()
    {


        for (int k = 0; k < 2; k++)
        {
            int count = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 13; j++)
                {

                    stones[k, i, j] = Instantiate(stonePrefab, startPoint + new Vector3(j, (k * 10) + i * 1.5f + 2, 0), Quaternion.identity);
                    stones[k, i, j].GetComponent<SpriteRenderer>().sprite = stonesAll[count];
                    count++;
                }
            }
        }

        while (myStones.Count < 15)
        {
            int count2 = 0;
            int index = Random.Range(0, 103);

            for (int k = 0; k < 2; k++)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 13; j++)
                    {
                        if (count2 == index && !myStones.Contains(stones[k, i, j]))
                        {
                            myStones.Add(stones[k, i, j]);

                            i = 4;
                            j = 13;
                            k = 2;
                        }
                        else
                            count2++;
                    }
                }
            }
        }


      
            for (int i = 0; i < 7; i++)
            { 
                ((GameObject) myStones[i]).transform.position = startPoint + new Vector3(i, 0, 0);
            }

        for (int i = 7; i < 15; i++)
        {
            ((GameObject)myStones[i]).transform.position = startPoint + new Vector3(i - 7, -1.35f, 0);
        }

        Debug.Log(myStones.Count);






    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
