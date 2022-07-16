using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public static void GenerateDice(int diceNum, GameObject dice)
    {
        for (int i = 0; i < diceNum; i++)
        {
            GameObject newDice = Instantiate(dice, new Vector3(Random.Range(12f, 18f), Random.Range(20.0f, 50.0f), Random.Range(2f, 8f)), dice.transform.rotation);
            var num = Random.Range(2, 20);
            newDice.name = num.ToString();
            newDice.GetComponent<Renderer>().material.SetFloat("_Number", num);
        }
    }

    public static void GenerateNat1(GameObject dice)
    {
        GameObject newDice = Instantiate(dice, new Vector3(Random.Range(12f, 18f), Random.Range(20.0f, 50.0f), Random.Range(2f, 8f)), dice.transform.rotation);
        newDice.GetComponent<Renderer>().material.SetFloat("_Number", "1");
    }

    public static void GenerateNat20(GameObject dice)
    {
        GameObject newDice = Instantiate(dice, new Vector3(Random.Range(12f, 18f), Random.Range(20.0f, 50.0f), Random.Range(2f, 8f)), dice.transform.rotation);
        newDice.GetComponent<Renderer>().material.SetFloat("_Number", "20");
    }
}
