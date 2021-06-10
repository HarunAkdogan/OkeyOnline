using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    NManager nManager;
    public ArrayList myStones = new ArrayList();
    private Vector3[,] cueDivs = new Vector3[2, 12];
    private int offset = 10;

    int playerId;

    //true->take stone
    private GameObject stoneMoving;
    private Vector3 stoneMovingFirstPos;

    private bool moving = false, myTurn=true, takeOrGive=true;
    private bool doubledPer = true;
    private bool sameColorPer = false;
    private bool sameNumberPer = false;
    private bool released = false;

    void Start()
    {
        nManager = GameObject.Find("NetworkManager").GetComponent<NManager>();
        
        if (!isLocalPlayer)
        {
            Debug.Log("Yerelim");
            return;
        }
        CmdAddPlayer(gameObject);

        createSlotsAndPlaceMyStones();
        //myTurn = true;
        //takeOrGive = true;


    }

    
    [Command]
    void CmdAddPlayer(GameObject player)
    {
        if (nManager.players.Count < 4 && !nManager.players.Contains(player))
        {
            nManager.players.Add(player);
            playerId = nManager.players.Count;

            //Debug.Log(nManager.players.Count);
            if (nManager.players.Count == 2)
                nManager.StartGame();
        }
        
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
                released = false;

                RaycastHit hit;
                Vector3 point = Camera.main.ScreenToWorldPoint(touch.position);
                Physics.Raycast(point, Vector3.forward * 100, out hit);

                //Debug.DrawRay(point2, Vector3.forward * 100, Color.red);

                if (hit.collider != null && hit.collider.tag == "stone")
                {

                    if ((myTurn && takeOrGive && (inSlot(hit.collider.gameObject, nManager.slots[0]) || inSlot(hit.collider.gameObject, nManager.slots[2]) && hit.collider.gameObject.GetComponent<Stone>().takeable)) || myStones.Contains(hit.collider.gameObject))
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

                    if ((myTurn && !takeOrGive && inSlot(stoneMoving, nManager.slots[3])))
                    {
                        stoneMoving.transform.position = nManager.slots[3];
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
                                                nManager.remainIndex--;
                                                ((GameObject)nManager.stones[nManager.remainIndex]).GetComponent<Stone>().setVisible(false);
                                                ((GameObject)nManager.stones[nManager.remainIndex]).SetActive(true);
                                                nManager.remainTxt.text = (nManager.remainIndex + 1).ToString();
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

    public bool inSlot(GameObject go, Vector3 vc)
    {

        return (go.transform.position.x < vc.x + 0.5f && go.transform.position.x > vc.x - 0.5f && go.transform.position.y < vc.y + 0.5f && go.transform.position.y > vc.y - 0.5f);
    }

    public void createSlotsAndPlaceMyStones()
    {
        for (int i = 0; i < 4; i++)
        {
            Instantiate(nManager.slotPrefab, nManager.slots[i] + new Vector3(0, offset * playerId, 0), Quaternion.identity);
        }


        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 12; j++)
            {
                cueDivs[i, j] = nManager.startPoint + new Vector3(j, -i * 1.35f, 0) + new Vector3(0, offset * playerId, 0);
            }
        }


        for (int i = 0; i < 7; i++)
        {
            ((GameObject)myStones[i]).transform.position = cueDivs[0, i];
        }

        for (int i = 7; i < myStones.Count - 7; i++)
        {
            ((GameObject)myStones[i]).transform.position = cueDivs[1, i - 7];
        }

       ((GameObject)nManager.stones[nManager.remainIndex]).GetComponent<Stone>().setVisible(false);
        ((GameObject)nManager.stones[nManager.remainIndex]).SetActive(true);


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
            if (col0 < 12 && cueDivs[row, col0] == go.transform.position)
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


            if (!doubledPer && sCount < 2 || doubledPer && sCount != 1 || doubledPer && stone.GetComponent<Stone>().type != "okey" || (!sameColorPer && !sameNumberPer && !doubledPer))
            {
                return -1;
            }
            else if (!sameNumberPer && !sameColorPer && sCount == 1)
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

        if (stone.GetComponent<Stone>().type == "okey" && nextStone.GetComponent<Stone>().type == "okey")
            return -1;

        if (stone.GetComponent<Stone>().type == "okey")
        {
            return checkNext(sCount + 1, row, col0 + 1, col1 + 1);
        }

        if (nextStone.GetComponent<Stone>().type == "okey")
        {
            return checkNext(sCount + 1, row, col0, col1 + 1);
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



}
