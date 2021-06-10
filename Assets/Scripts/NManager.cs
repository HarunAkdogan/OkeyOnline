using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class NManager : NetworkManager
{
    public ArrayList players = new ArrayList();


    public ArrayList stones = new ArrayList();
    public Vector3[] slots;
    public Vector3 startPoint;
    
    
    public Text remainTxt;

    public GameObject stonePrefab, slotPrefab;
    public Sprite[] stonesAll;
    
  


    public int remainIndex = 105;

    public void StartGame()
    {
        createStones();
        reShuffle(stones);
        setIndicatorAndFakeOkeys();


        DistibuteStones();

        //giveMyTestStonesColorsNumbers();
        //giveMyTestStonesDoubles();

        

        //makeReceivableStone();
        //makeReceivablePublicStone();

       

        remainTxt.text = (remainIndex + 1).ToString();

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

        for (int i = 0; i < 2; i++)
        {

            GameObject fake = Instantiate(stonePrefab, slots[2], Quaternion.identity);
            fake.GetComponent<SpriteRenderer>().sprite = stonesAll[52];
            fake.GetComponent<Stone>().original = stonesAll[52];
            fake.GetComponent<Stone>().type = "fake";
            fake.GetComponent<Stone>().number = -2;
            fake.SetActive(false);

            stones.Add(fake);
        }



    }




    public void setIndicatorAndFakeOkeys()
    {

        GameObject ind = (GameObject)stones[remainIndex];
        GameObject okeyObj = null;
        ind.SetActive(true);
        remainIndex--;
        ind.transform.position = slots[1];
        ind.GetComponent<Stone>().takeable = false;

        Debug.Log("Indicator ->" + ind.GetComponent<Stone>().color + ", " + ind.GetComponent<Stone>().number);

        foreach (GameObject go in stones)
        {
            if (((ind.GetComponent<Stone>().number == 13 && go.GetComponent<Stone>().number == 1) || (ind.GetComponent<Stone>().number == go.GetComponent<Stone>().number - 1)) && ind.GetComponent<Stone>().color == go.GetComponent<Stone>().color)
            {
                go.GetComponent<Stone>().type = "okey";
                okeyObj = go;
                Debug.Log("Okey ->" + go.GetComponent<Stone>().color + ", " + go.GetComponent<Stone>().number);
            }
        }


        foreach (GameObject go in stones)
        {
            if (go.GetComponent<Stone>().type == "fake")
            {
                go.GetComponent<Stone>().number = okeyObj.GetComponent<Stone>().number;
                go.GetComponent<Stone>().color = okeyObj.GetComponent<Stone>().color;

                // Debug.Log("Fake " + i + " -> " + ((GameObject)stones[fakes[i]]).GetComponent<Stone>().color + ", " + ((GameObject)stones[fakes[i]]).GetComponent<Stone>().number);
            }
        }

    }




    public void DistibuteStones()
    {
        for (int k = 0; k < players.Count; k++)
        {
            for (int i = 0; i < 14; i++)
            {
                ((GameObject)players[k]).GetComponent<Player>().myStones.Add(stones[remainIndex]);
                ((GameObject)stones[remainIndex]).GetComponent<Stone>().stock = false;
                ((GameObject)stones[remainIndex]).SetActive(true);
                remainIndex--;
            }
        }
    }

    /*
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

        ((GameObject)stones[46]).GetComponent<Stone>().type = "okey";
        ((GameObject)stones[47]).GetComponent<Stone>().type = "okey";


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

        ((GameObject)stones[46]).GetComponent<Stone>().type = "okey";
        ((GameObject)stones[47]).GetComponent<Stone>().type = "okey";

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
        */

  


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
