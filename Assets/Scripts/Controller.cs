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
    private bool moving = false;
    private GameObject stoneMoving;

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
    void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            
            Touch touch = Input.GetTouch(0);
            

            if (touch.phase == TouchPhase.Began && !moving)
            {
                moving = true;

                RaycastHit hit;
                Vector3 point = Camera.main.ScreenToWorldPoint(touch.position);
                Physics.Raycast(point, Vector3.forward * 100, out hit);

                //Debug.DrawRay(point2, Vector3.forward * 100, Color.red);

                if (hit.collider != null && hit.collider.tag == "stone")
                    stoneMoving = hit.collider.gameObject;


            }
            else if (touch.phase == TouchPhase.Moved && moving && stoneMoving != null)
            {
                Vector3 point = Camera.main.ScreenToWorldPoint(touch.position);
                stoneMoving.transform.position = new Vector3(point.x, point.y, stoneMoving.transform.position.z);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                stoneMoving = null;
                moving = false;
            }

        }
    }
}
