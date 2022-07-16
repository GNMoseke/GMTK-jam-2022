using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDice : MonoBehaviour
{
    public GameObject dice;
    public int numDice;

    void Start()
    {
        //GenerateDice(Random.Range(10, 20));
        GenerateDice(numDice);
    }

    void GenerateDice(int diceNum)
    {
        for (int i = 0; i < diceNum; i++)
        {
            GameObject newDice = Instantiate(dice, new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(20.0f, 50.0f), Random.Range(-3.0f, 3.0f)), dice.transform.rotation);
            newDice.name = Random.Range(1, 20).ToString();
            //newDice.GetComponent<Renderer>().material
        }
    }
}
