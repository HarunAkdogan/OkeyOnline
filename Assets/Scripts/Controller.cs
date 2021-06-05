using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public ArrayList stones = new ArrayList();
    public Vector3 startPoint;
    private Vector3[,] cueDivs = new Vector3[2, 12];
    public Vector3[] slots;
    public Text remainTxt;

    public GameObject stonePrefab, slotPrefab;
    public Sprite[] stonesAll;
    public SpriteMask mask;
    private ArrayList myStones = new ArrayList();
    private bool moving = false, myTurn, takeOrGive; //true->take stone
    private GameObject stoneMoving;
    private Vector3 stoneMovingFirstPos;
    

    private bool doubledPer = true;
    private bool sameColorPer = false;
    private bool sameNumberPer = false;

    private bool released = false;

    private int remainIndex = 103;

    void Start()
    {
        createStones();

        reShuffle(stones);
        giveMyStones();

        //giveMyTestStonesColors();
        //giveMyTestStonesColorsNumbers();
        //giveMyTestStonesDoubles();

        setIndicatorAndFakeOkeys();
        createSlotsAndPlaceMyStones();


        //makeReceivableStone();
        //makeReceivablePublicStone();

        myTurn = true;
        takeOrGive = true;

        remainTxt.text = (remainIndex + 1).ToString();

    }

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
                released = false;

                RaycastHit hit;
                Vector3 point = Camera.main.ScreenToWorldPoint(touch.position);
                Physics.Raycast(point, Vector3.forward * 100, out hit);

                //Debug.DrawRay(point2, Vector3.forward * 100, Color.red);

                if (hit.collider != null && hit.collider.tag == "stone")
                {

                    if ((myTurn && takeOrGive && (inSlot(hit.collider.gameObject, slots[0]) || inSlot(hit.collider.gameObject, slots[2]) && hit.collider.gameObject.GetComponent<Stone>().takeable)) || myStones.Contains(hit.collider.gameObject))
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
                                if (inSlot(stoneMoving, cueDivs[i, j]))
                                {
                                    for (int k = 0; k < 14; k++)
                                    {
                                        if (cueDivs[i, j] == ((GameObject)myStones[k]).transform.position && ((GameObject)myStones[k]) != stoneMoving && myStones.Contains(stoneMoving))
                                        {
                                            stoneMoving.transform.position = cueDivs[i, j];
                                            ((GameObject)myStones[k]).transform.position = stoneMovingFirstPos;
                                            emptySlot = false;
                                            sticked = true;
                                            break;
                                        }
                                        else if (cueDivs[i, j] == ((GameObject)myStones[k]).transform.position && ((GameObject)myStones[k]) != stoneMoving && !myStones.Contains(stoneMoving))
                                        {
                                            myStones.Add(stoneMoving);
                                            emptySlot = false;
                                            sticked = false;
                                            break;
                                        }

                                    }

                                    if (emptySlot)
                                    {
                                        sticked = true;
                                        stoneMoving.transform.position = cueDivs[i, j];
                                        if (!myStones.Contains(stoneMoving))
                                        {
                                            myStones.Add(stoneMoving);

                                            if (stoneMoving.GetComponent<Stone>().stock)
                                            {
                                                stoneMoving.GetComponent<Stone>().setVisible(true);
                                                remainIndex--;
                                                ((GameObject)stones[remainIndex]).GetComponent<Stone>().setVisible(false);
                                                ((GameObject)stones[remainIndex]).SetActive(true);
                                                remainTxt.text = (remainIndex + 1).ToString();
                                            }

                                            takeOrGive = false; // (ready to give),  requestToServer...
                                        }
                                    }

                                    j = 12;
                                    i = 2;
                                }
                            }
                        }

                    }
                    if (!sticked)
                        stoneMoving.transform.position = stoneMovingFirstPos;



                }

                stoneMovingFirstPos = Vector3.zero;
                stoneMoving = null;
                moving = false;


                if (!released && checkSeries()) 
                {
                    Debug.Log("Congratulations!");
                    released = true;
                }
            }

        }
    }

    public void createSlotsAndPlaceMyStones()
    {
        for (int i = 0; i < 4; i++)
        {
            Instantiate(slotPrefab, slots[i], Quaternion.identity);
        }


        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 12; j++)
            {
                cueDivs[i, j] = startPoint + new Vector3(j, -i * 1.35f, 0);
            }
        }


        for (int i = 0; i < 7; i++)
        {
            ((GameObject)myStones[i]).transform.position = cueDivs[0, i];
        }

        for (int i = 7; i < 14; i++)
        {
            ((GameObject)myStones[i]).transform.position = cueDivs[1, i - 7];
        }

        ((GameObject)stones[remainIndex]).GetComponent<Stone>().setVisible(false);
        ((GameObject)stones[remainIndex]).SetActive(true);


    }


    public void setIndicatorAndFakeOkeys()
    {

        GameObject ind = (GameObject)stones[remainIndex];
        ind.SetActive(true);
        remainIndex--;
        ind.transform.position = slots[1];
        ind.GetComponent<Stone>().takeable = false;

        Debug.Log("Indicator ->" + ind.GetComponent<Stone>().color + ", " + ind.GetComponent<Stone>().number);

        foreach (GameObject go in stones)
        {
            if (((ind.GetComponent<Stone>().number == 13 && go.GetComponent<Stone>().number == 1) || (ind.GetComponent<Stone>().number == go.GetComponent<Stone>().number - 1)) && ind.GetComponent<Stone>().color == go.GetComponent<Stone>().color)
            {
                go.GetComponent<Stone>().okey = true;

                Debug.Log("Okey ->" + go.GetComponent<Stone>().color + ", " + go.GetComponent<Stone>().number);
            }
        }

    }




    public void giveMyStones()
    {
        for(int i=0; i<14; i++)
        {
            myStones.Add(stones[remainIndex]);
            ((GameObject)stones[remainIndex]).GetComponent<Stone>().stock = false;
            ((GameObject)stones[remainIndex]).SetActive(true);
            remainIndex--;
        }
    }

    public void giveMyTestStonesColors()
    {
        for (int i=0; i<11; i++)
        {
            myStones.Add(stones[i]);
        }

        myStones.Add(stones[13]);
        myStones.Add(stones[14]);
        myStones.Add(stones[15]);

    }

    public void giveMyTestStonesColorsNumbers()
    {
        myStones.Add(stones[0]);
        myStones.Add(stones[13]);
        myStones.Add(stones[26]);
        myStones.Add(stones[39]);

        myStones.Add(stones[2]);
        myStones.Add(stones[3]);
        myStones.Add(stones[4]);
        myStones.Add(stones[5]);

        myStones.Add(stones[1]);
        myStones.Add(stones[14]);
        myStones.Add(stones[27]);
        myStones.Add(stones[40]);
        //myStones.Add(stones[41]);

        //myStones.Add(stones[45]);
        myStones.Add(stones[46]);
        myStones.Add(stones[47]);

        ((GameObject)stones[46]).GetComponent<Stone>().okey = true;
        ((GameObject)stones[47]).GetComponent<Stone>().okey = true;


    }

    public void giveMyTestStonesDoubles()
    {
        myStones.Add(stones[0]);
        myStones.Add(stones[52]);

        myStones.Add(stones[1]);
        myStones.Add(stones[53]);

        myStones.Add(stones[2]);
        myStones.Add(stones[54]);

        myStones.Add(stones[3]);
        myStones.Add(stones[55]);

        myStones.Add(stones[4]);
        myStones.Add(stones[56]);

        myStones.Add(stones[5]);
        myStones.Add(stones[57]);

        myStones.Add(stones[46]);
        myStones.Add(stones[47]);

        ((GameObject)stones[46]).GetComponent<Stone>().okey = true;
        ((GameObject)stones[47]).GetComponent<Stone>().okey = true;

    }


    public void createStones()
    {

        for (int k = 0; k < 2; k++)
        {
            int count = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    GameObject go;
                    go = Instantiate(stonePrefab, slots[2], Quaternion.identity);
                    go.GetComponent<SpriteRenderer>().sprite = stonesAll[count];
                    go.GetComponent<Stone>().original = stonesAll[count];
                    go.GetComponent<Stone>().number = j + 1;
                    go.SetActive(false);

                    switch (i)
                    {
                        case 0:
                            go.GetComponent<Stone>().color = "blue";
                            break;
                        case 1:
                            go.GetComponent<Stone>().color = "black";
                            break;
                        case 2:
                            go.GetComponent<Stone>().color = "red";
                            break;
                        case 3:
                            go.GetComponent<Stone>().color = "green";
                            break;
                    }
                    stones.Add(go);

                    count++;
                }
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
            int index = Random.Range(0, 103);

            if (!myStones.Contains(stones[index]))
            {
                ((GameObject)stones[index]).transform.position = slots[0];
                found = true;
            }
        }
    }

    public void makeReceivablePublicStone()
    {

        bool found = false;

        while (!found)
        {
            int index = Random.Range(0, 103);


            if (((GameObject)stones[index]).GetComponent<Stone>().takeable)
            {
                ((GameObject)stones[index]).transform.position = slots[2];
                found = true;
            }

        }
    }

    public bool checkSeries()
    {
        sameColorPer = false;
        sameNumberPer = false;
        doubledPer = true;

        int checkRow0 = checkNext(0, 0, 0, 1);

        sameColorPer = false;
        sameNumberPer = false;

        int checkRow1 = checkNext(0, 1, 0, 1);

        sameColorPer = false;
        sameNumberPer = false;
        doubledPer = true;

        if (checkRow0 > -1 && checkRow1 > -1)
                return true;

        return false;
    }

    public int checkNext(int seriesCount, int row, int col0, int col1)
    {
        int sCount = seriesCount;

        if (col1 > 12)
            return sCount;

        GameObject stone = null;
        GameObject nextStone = null;

        foreach (GameObject go in myStones)
        {
            if (col0 < 12 &&  cueDivs[row, col0] == go.transform.position)
                stone = go;
        }

        foreach (GameObject go in myStones)
        {
            if (col1 < 12 && cueDivs[row, col1] == go.transform.position)
                nextStone = go;
        }

        if (stone == null)
        {
            return checkNext(0, row, col0 + 1, col1 + 1);
        }

        if (nextStone == null)
        {
            

            col0 = col1;


            if (!doubledPer && sCount < 2 || doubledPer && sCount !=1 || !doubledPer && stone.GetComponent<Stone>().okey || (!sameColorPer && !sameNumberPer && !doubledPer))
            {
                return -1;
            }
            else if(!sameNumberPer && !sameColorPer && sCount == 1)
            {
                
                return checkNext(1, row, col0, col1 + 1);
            }
            else
            {
                sameNumberPer = false;
                sameColorPer = false;
                return checkNext(0, row, col0, col1 + 1);
            }
        }

        if (stone.GetComponent<Stone>().okey && nextStone.GetComponent<Stone>().okey)
            return -1;

            if (stone.GetComponent<Stone>().okey)
        {
            return checkNext(sCount + 1, row, col0 + 1, col1 + 1);
        }

        if (nextStone.GetComponent<Stone>().okey)
        {
            return checkNext(sCount + 1, row, col0 , col1 + 1);
        }

        

        
        if (stone.GetComponent<Stone>().number == nextStone.GetComponent<Stone>().number && stone.GetComponent<Stone>().color == nextStone.GetComponent<Stone>().color && !sameColorPer && !sameNumberPer)
        {
            col0 = col1;
            doubledPer = true;
            return checkNext(1, row, col0, col1 + 1);

        }
        else if (((nextStone.GetComponent<Stone>().number == 1 && stone.GetComponent<Stone>().number == 13) || (nextStone.GetComponent<Stone>().number == stone.GetComponent<Stone>().number + 1)) && nextStone.GetComponent<Stone>().color == stone.GetComponent<Stone>().color)
        {
            doubledPer = false;
            col0 = col1;
            if (sameNumberPer && sCount > 0)
                return -1;
            else
            {
                sameColorPer = true;
                return checkNext(sCount + 1, row, col0, col1 + 1);
            }
        }
        else if (nextStone.GetComponent<Stone>().number == stone.GetComponent<Stone>().number && nextStone.GetComponent<Stone>().color != stone.GetComponent<Stone>().color)
        {
            doubledPer = false;
            col0 = col1;
            if (sameColorPer && sCount > 0)
                return -1;
            else
            {
                sameNumberPer = true;
                return checkNext(sCount + 1, row, col0, col1 + 1);
            }
        }


        return -1;
    }

    

    public void reShuffle(ArrayList list)
    {
        int n = list.Count - 1;
        while (n > 1)
        {
            int k = Random.Range(0, n);
            GameObject value = (GameObject)list[k];
            list[k] = list[n];
            list[n] = value;
            n--;
        }
        
    }



}
