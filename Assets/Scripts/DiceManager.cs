using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public static Vector3 standardSpawnPosition = new Vector3(17f, Random.Range(20.0f, 30.0f), 3f);
    public static void GenerateDice(int diceNum, GameObject dice)
    {
        for (int i = 0; i < diceNum; i++)
        {
            GameObject newDice = Instantiate(dice, new Vector3(Random.Range(-2, 30), Random.Range(20.0f, 30.0f), Random.Range(-6, 12)), dice.transform.rotation);
            int num = Random.Range(2, 20);
            newDice.GetComponent<DieModel>().value = num;
            newDice.name = num.ToString();
            newDice.GetComponent<Renderer>().material.SetFloat("_Number", num);
        }
    }
}
