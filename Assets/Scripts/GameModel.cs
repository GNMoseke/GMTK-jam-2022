using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class GameModel : MonoBehaviour
{
    const int TICKETS = 30;
    const float DAY_LENGTH = 180;

    public int day { get; set; }
    public float dayTimer { get; set; }

    public TextAsset ticketsCSV;
    public GameObject ticketPrefab;
    public GameObject dicePrefab;
    public List<TicketModel> tickets { get; set; }
    public float dailyTicketInterval { get; set; }

    public int followerCount { get; set; }

    public GameObject ticketDispenser;
    public GameObject ticketSpawnPosition;
    public GameObject successParticles;
    public GameObject failParticles;
    public Leaderboard leaderboard;
    public TMPro.TMP_Text followerText;
    public AudioSource printerClip;

    public float ticketTimer;

    public int numActiveTickets;
    public float shootForce;

    public int nat20Counter;
    public int nat1Counter;


    public bool betweenDays;

    // Use this for initialization
    void Start()
    {
        tickets = TicketParser.ReadTickets(ticketsCSV);
        day = 0;
        followerCount = 3;
        UpdateUI();
        NextDay();
    }

    // Update is called once per frame
    void Update()
    {
        if (!betweenDays)
        {
            if (dayTimer > 0)
            {
                this.dayTimer -= Time.deltaTime;
                ticketTimer += Time.deltaTime;
                if (ticketTimer >= dailyTicketInterval)
                {
                    GenerateTicket(tickets[numActiveTickets]);
                    numActiveTickets++;
                    ticketTimer = 0;
                }
            }
            else // DAY FINISHED
            {
                // 1) look up at leaderboard
                // 2) click continue
                betweenDays = true;
                Camera.main.GetComponent<CameraRotate>().StartRotation(false, true);
                ResetTable();
            }
        }
    }

    public void ResetTable()
    {
        DeleteRemainingDice();
        // Activate scoreboard, pause game
        Time.timeScale = 0;
    }


    private void DeleteRemainingDice()
    {
        GameObject[] remainingDice = GameObject.FindGameObjectsWithTag("Die");
        foreach (GameObject die in remainingDice)
        {
            GameObject.Destroy(die);
        }
    }

    public void NextDay()
    {
        day++;
        dayTimer = DAY_LENGTH;
        int modifier = CalculateAdditionalDiceModifier();
        nat20Counter = 5 * day + modifier;
        nat1Counter = 5 * day + modifier;
        int dailyTickets = (TICKETS * day);
        int randomDice = dailyTickets - nat1Counter - nat20Counter;
        dayTimer = DAY_LENGTH;
        dailyTicketInterval = DAY_LENGTH / dailyTickets;
        ticketTimer = dailyTicketInterval - 1f;
        numActiveTickets = 1;

        Time.timeScale = 1f;
        betweenDays = false;
        DiceManager.GenerateDice(randomDice, dicePrefab);
    }

    private int CalculateAdditionalDiceModifier()
    {
        // TODO: scale better
        print("GAINED " + (followerCount / 10) * (day - 1));
        return (followerCount / 10) * (day - 1);
    }

    public void GenerateTicket(TicketModel ticket)
    {
        GameObject ticketGameObj = Instantiate(ticketPrefab, ticketSpawnPosition.transform.position, Quaternion.Euler(0, -35f, 0));
        ticketGameObj.GetComponent<Rigidbody>().AddForce(-ticketDispenser.transform.up * shootForce);
        ticketGameObj.AddComponent<TicketModel>();
        ticketGameObj.GetComponent<TicketModel>().InstantiateTicket(ticket);
        printerClip.Play();

        ticketGameObj.GetComponent<TicketModel>().ticketCompletion += TicketComplete;
    }

    public void TicketComplete(bool success, int severity, Vector3 loc)
    {
        if (success)
        {
            GameObject.Instantiate(successParticles, loc, Quaternion.identity);
            followerCount += severity;
        }
        else
        {
            GameObject.Instantiate(failParticles, loc, Quaternion.identity);
            followerCount -= severity;
            if (followerCount < 0)
            {
                followerCount = 0;
            }
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        leaderboard.UpdateLeaderboard(followerCount);
        followerText.text = followerCount.ToString();
        // TODO: scale better
        //followerSlider.value = (float)followerCount / 100f;
    }
}