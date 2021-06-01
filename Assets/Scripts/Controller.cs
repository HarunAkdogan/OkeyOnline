using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameObject[,,] stones = new GameObject[2,5,13];
    public Vector3 startPoint;
    private Vector3[,] istakaDivs = new Vector3[2,12];
    public Vector3[] slots;

    public GameObject stonePrefab, slotPrefab;
    public Sprite [] stonesAll;
    public SpriteMask mask;
    private ArrayList myStones = new ArrayList();
    private bool moving = false, myTurn, takeOrGive; //true->take stone
    private GameObject stoneMoving;
    private Vector3 stoneMovingFirstPos;

    void Start()
    {
       

        for (int i=0; i<2; i++)
        {
            for (int j = 0; j < 12; j++)
            {
                istakaDivs[i, j] = startPoint + new Vector3(j,-i* 1.35f,0);
            }
        }
        

        for (int k = 0; k < 2; k++)
        {
            int count = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 13; j++)
                {

                    stones[k, i, j] = Instantiate(stonePrefab, startPoint + new Vector3(j, (k * 10) + i * 1.5f + 10, 0), Quaternion.identity);
                    stones[k, i, j].GetComponent<SpriteRenderer>().sprite = stonesAll[count];
                    count++;
                }
            }
        }

        for (int i = 0; i < 4; i++)
        {
            Instantiate(slotPrefab, slots[i], Quaternion.identity);
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
                            stones[k, i, j].GetComponent<Stone>().takeable = false;

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


      
        for (int i = 0; i < 12; i++)
        { 
            ((GameObject) myStones[i]).transform.position = istakaDivs[0,i];
        }

        for (int i = 12; i < 15; i++)
        {
            ((GameObject)myStones[i]).transform.position = istakaDivs[1, i -12];
        }

        Debug.Log(myStones.Count);

        myTurn = true;
        takeOrGive = true;
        makeReceivableStone();
        makeReceivablePublicStone();



    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //myTurn = getFromServer...
        //takeOrGive = getFromServer...

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
                {
                    
                    if ((myTurn && takeOrGive && (inSlot(hit.collider.gameObject, slots[0]) || inSlot(hit.collider.gameObject, slots[2]) && hit.collider.gameObject.GetComponent<Stone>().takeable )) || myStones.Contains(hit.collider.gameObject))
                    {
                        stoneMoving = hit.collider.gameObject;
                        stoneMovingFirstPos = stoneMoving.transform.position;
                    }
                }

            }
            else if (touch.phase == TouchPhase.Moved && moving && stoneMoving != null)
            {
                Vector3 point = Camera.main.ScreenToWorldPoint(touch.position);
                stoneMoving.transform.position = new Vector3(point.x, point.y, stoneMoving.transform.position.z);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (stoneMoving != null)
                {
                    //stoneMoving.transform.position = stoneMovingFirstPos;
                    bool sticked = false, emptySlot = true;

                    if ((myTurn && !takeOrGive && inSlot(stoneMoving, slots[3])))
                    {
                        stoneMoving.transform.position = slots[3];
                        stoneMoving.GetComponent<Stone>().takeable = true;
                        myStones.Remove(stoneMoving);
                        sticked = true;
                        takeOrGive = true; // (ready to take) requestToServer...
                        myTurn = false; // requestToServer...
                    }
                    else
                    {

                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 12; j++)
                        {
                            if (inSlot(stoneMoving, istakaDivs[i, j]))
                            {
                                for (int k = 0; k < 15; k++)
                                {
                                    if (istakaDivs[i, j] == ((GameObject)myStones[k]).transform.position && ((GameObject)myStones[k]) != stoneMoving && myStones.Contains(stoneMoving))
                                    {
                                        stoneMoving.transform.position = istakaDivs[i, j];
                                        ((GameObject)myStones[k]).transform.position = stoneMovingFirstPos;
                                        emptySlot = false;
                                        sticked = true;
                                        break;
                                    }
                                    else if (istakaDivs[i, j] == ((GameObject)myStones[k]).transform.position && ((GameObject)myStones[k]) != stoneMoving && !myStones.Contains(stoneMoving))
                                    {

                                        emptySlot = false;
                                        sticked = false;
                                        break;
                                    }

                                }

                                if (emptySlot)
                                {
                                    sticked = true;
                                    stoneMoving.transform.position = istakaDivs[i, j];
                                        if (!myStones.Contains(stoneMoving))
                                        {
                                            myStones.Add(stoneMoving);
                                            takeOrGive = false; // (ready to give),  requestToServer...
                                        }
                                }


                                j = 12;
                                i = 2;
                            }
                        }
                    }

                }
                    if(!sticked)
                        stoneMoving.transform.position = stoneMovingFirstPos;



                }

                stoneMovingFirstPos = Vector3.zero;
                stoneMoving = null;
                moving = false;
            }

        }
    }
    
    public bool inSlot(GameObject go, Vector3 vc)
    {

        return (go.transform.position.x < vc.x + 0.5f && go.transform.position.x > vc.x - 0.5f && go.transform.position.y < vc.y + 0.5f && go.transform.position.y > vc.y - 0.5f);
    }

    public void makeReceivableStone()
    {

        bool found = false;

        while (!found)
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

                            
                            stones[k, i, j].transform.position = slots[0];

                            found = true;
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
    }

    public void makeReceivablePublicStone()
    {

        bool found = false;

        while (!found)
        {
            int count2 = 0;
            int index = Random.Range(0, 103);

            for (int k = 0; k < 2; k++)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 13; j++)
                    {
                        if (count2 == index && stones[k, i, j].GetComponent<Stone>().takeable)
                        {


                            stones[k, i, j].transform.position = slots[2];

                            found = true;
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
    }

}
