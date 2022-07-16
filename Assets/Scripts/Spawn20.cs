using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn20 : MonoBehaviour
{
    public GameObject pipe;
    public GameObject die;

    private Vector3 risingVel = new Vector3(0.0f, 10.0f, 0.0f);
    private int cnt = 0;
    public GameModel gameModel;

    private float timeHold = 1.0f;
    private float timeDump = 0.5f;
    private string state = "";

    public void OnMouseDown()
    {
        if (gameModel.nat20Counter > 0)
        {
            pipe.SetActive(true);
            pipe.GetComponent<Rigidbody>().useGravity = true;
            state = "falling";
            cnt++;
        }
    }

    public void FixedUpdate()
    {
        switch (state)
        {
            case "falling":
                if (pipe.transform.position.y < 135)
                {
                    state = "held";
                }
                break;
            case "held":
                pipe.GetComponent<Rigidbody>().useGravity = false;
                pipe.GetComponent<Rigidbody>().velocity = Vector3.zero;
                if (timeDump > 0.0f)
                {
                    timeDump -= Time.deltaTime;
                }
                else
                {
                    print("dump");

                    for (int i = 0; i < cnt; i++)
                        SpawnDie();
                    cnt = 0;
                }
                if (timeHold > 0)
                {
                    timeHold -= Time.deltaTime;
                }
                else
                {
                    pipe.GetComponent<Rigidbody>().velocity = risingVel;
                    state = "rising";
                    timeDump = 0.5f;
                    timeHold = 1.0f;
                }
                break;
            case "rising":
                if (pipe.transform.position.y > 150)
                {
                    pipe.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    state = "";
                    pipe.SetActive(false);
                }
                break;
            case "":
                break;
        }
    }

    private void SpawnDie()
    {
        if (gameModel.nat20Counter > 0)
        {
            GameObject newDice = Instantiate(die, new Vector3(Random.Range(13.0f, 17.0f), 34.0f, 5.0f), die.transform.rotation);
            newDice.GetComponent<Renderer>().material.SetFloat("_Number", 20);
            newDice.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0.0f, 1000.0f), 0, 0));
        }
        gameModel.nat20Counter--;
    }
}
