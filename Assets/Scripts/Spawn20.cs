using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn20 : MonoBehaviour
{
    public GameObject twentyPipe;
    public GameObject die;

    private Vector3 risingVel = new Vector3(0.0f, 10.0f, 0.0f);
    private int cnt = 0;
    public GameModel gameModel;

    public void OnMouseDown()
    {
        if (gameModel.nat20Counter > 0)
        {
            twentyPipe.GetComponent<Rigidbody>().useGravity = true;
            cnt++;
        }
    }

    public void FixedUpdate()
    {
        if (twentyPipe.transform.position.y < 120 && twentyPipe.GetComponent<Rigidbody>().useGravity)
        {
            twentyPipe.GetComponent<Rigidbody>().useGravity = false;
            twentyPipe.GetComponent<Rigidbody>().velocity = risingVel;
            for (int i = 0; i < cnt; i++)
                SpawnDie();
            cnt = 0;
        }
        if (twentyPipe.transform.position.y > 150)
            twentyPipe.GetComponent<Rigidbody>().velocity = Vector3.zero;

    }

    private void SpawnDie()
    {
        if (gameModel.nat20Counter > 0)
        {
            GameObject newDice = Instantiate(die, new Vector3(5.0f, 25.0f, 30.0f), die.transform.rotation);
            newDice.GetComponent<Renderer>().material.SetFloat("_Number", 20);
            newDice.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0.0f, 15000.0f), 0.0f, -15000.0f));
        }
        gameModel.nat20Counter--;
    }
}
