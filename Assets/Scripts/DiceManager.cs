using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public static Vector3 standardSpawnPosition = new Vector3(17f, Random.Range(50.0f, 80.0f), 3f);

    public static void GenerateDice(int diceNum, GameObject dice)
    {
        for (int i = 0; i < diceNum; i++)
        {
            GameObject newDice = Instantiate(dice, new Vector3(38.0f, 10.0f * i, 30.0f), dice.transform.rotation);
            int num = Random.Range(2, 20);
            newDice.GetComponent<DieModel>().value = num;
            newDice.name = num.ToString();
            newDice.GetComponent<Renderer>().material.SetFloat("_Number", num);
            newDice.GetComponent<Rigidbody>().velocity = new Vector3(-30.0f, 0.0f, -20.0f);
        }
        for (int i = 1; i < 6; i++)
        {
            GameObject twenty = Instantiate(dice, new Vector3(i * 3, 17, 10), dice.transform.rotation);
            twenty.GetComponent<DieModel>().value = 20;
            twenty.name = "20";
            twenty.GetComponent<Renderer>().material.SetFloat("_Number", 20);

            GameObject one = Instantiate(dice, new Vector3(-i * 3f, 17, 10), dice.transform.rotation);
            one.GetComponent<DieModel>().value = 1;
            one.name = "1";
            one.GetComponent<Renderer>().material.SetFloat("_Number", 1);
        }
    }
}
