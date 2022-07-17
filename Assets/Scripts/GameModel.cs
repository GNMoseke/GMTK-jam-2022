using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class GameModel : MonoBehaviour
{
    // start at x, increase by 10 each day
    const int INITIAL_TICKETS = 15;
    const float ROUND_LENGTH = 90;
    const int VICTORY_COUNT = 100;

    public int round { get; set; }
    public float roundTimer { get; set; }

    public TextAsset ticketsCSV;
    public GameObject ticketPrefab;
    public GameObject dicePrefab;
    public List<TicketModel> tickets { get; set; }
    public float dailyTicketInterval { get; set; }
    private HashSet<GameObject> activeTickets;

    public int followerCount { get; set; }

    public GameObject ticketDispenser;
    public GameObject ticketSpawnPosition;
    public GameObject successParticles;
    public GameObject failParticles;
    public GameObject diceDestroySparks;
    public GameObject diceDestroySmoke;
    public Leaderboard leaderboard;
    public TMPro.TMP_Text followerText;
    public TMPro.TMP_Text ticketsRemainingText;

    public AudioSource printerClip;
    public GameObject victoryBoard;

    public float ticketTimer;

    int ticketIndex = 0;
    public float shootForce;

    private int roundTicketsRemaining;

    public bool betweenRounds;

    // Use this for initialization
    void Start()
    {
        activeTickets = new HashSet<GameObject>();
        tickets = TicketParser.ReadTickets(ticketsCSV);
        // randomize ticket order
        Shuffle(tickets);
        round = 0;
        followerCount = 3;
        UpdateUI();
        NextDay();
        this.victoryBoard.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!betweenRounds)
        {
            // there's still tickets in the queue, tick the timer to fire the next one
            if (roundTicketsRemaining > 0)
            {
                ticketTimer += Time.deltaTime;
                if (ticketTimer >= dailyTicketInterval)
                {
                    print($"using ticket at idx {ticketIndex}");
                    GenerateTicket(tickets[ticketIndex]);
                    ticketIndex++;
                    // ran out of tickets, reshuffle and start again
                    if (ticketIndex == tickets.Count) {
                        Shuffle(tickets);
                        ticketIndex = 0;
                    }
                    ticketTimer = 0;
                    roundTicketsRemaining--;
                    if (roundTicketsRemaining >= 0)
                    {
                        ticketsRemainingText.text = roundTicketsRemaining.ToString();
                    }
                }
            }
            else // ROUND FINISHED
            {
                // if there are still active tickets, wait for those to be done before we move on
                if (activeTickets.Count == 0) {
                betweenRounds = true;
                Camera.main.GetComponent<CameraRotate>().StartRotation(false, true);
                // check if the player just won
                if (followerCount > VICTORY_COUNT)
                {
                    leaderboard.gameObject.SetActive(false);
                    victoryBoard.SetActive(true);
                }
                ResetTable();
                }
            }
        }
    }

    public void ResetTable()
    {
        DeleteRemainingDiceAndVFX();
        // Activate scoreboard, pause game
        Time.timeScale = 0;
    }


    private void DeleteRemainingDiceAndVFX()
    {
        // who likes array concatenation anyways
        GameObject[] remainingDice = GameObject.FindGameObjectsWithTag("Die");
        GameObject[] remainingVFX = GameObject.FindGameObjectsWithTag("VFX");
        foreach (GameObject die in remainingDice)
        {
            GameObject.Destroy(die);
        }
        foreach (GameObject vfx in remainingVFX)
        {
            GameObject.Destroy(vfx);
        }
    }

    public void NextDay()
    {
        activeTickets.Clear();
        AudioSource musicBG = this.GetComponent<AudioSource>();
        musicBG.time = 0f;
        musicBG.Play();
        round++;
        roundTimer = ROUND_LENGTH;
        int specialDiceCounter = 4 + round;
        roundTicketsRemaining = (INITIAL_TICKETS + (10 * (round - 1)));
        int randomDice = roundTicketsRemaining - 2*specialDiceCounter;
        roundTimer = ROUND_LENGTH;
        dailyTicketInterval = ROUND_LENGTH / roundTicketsRemaining;
        ticketTimer = dailyTicketInterval;

        Time.timeScale = 1f;
        betweenRounds = false;
        DiceManager.GenerateDice(randomDice, dicePrefab, specialDiceCounter);
    }
    public void GenerateTicket(TicketModel ticket)
    {
        GameObject ticketGameObj = Instantiate(ticketPrefab, ticketSpawnPosition.transform.position, Quaternion.Euler(0, -35f, 0));
        ticketGameObj.GetComponent<Rigidbody>().AddForce(-ticketDispenser.transform.up * shootForce);
        ticketGameObj.AddComponent<TicketModel>();
        ticketGameObj.GetComponent<TicketModel>().InstantiateTicket(ticket);
        printerClip.Play();
        activeTickets.Add(ticketGameObj);

        ticketGameObj.GetComponent<TicketModel>().ticketCompletion += TicketComplete;
    }

    public void TicketComplete(bool success, int severity, Vector3 loc, GameObject completedTicket)
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
        activeTickets.Remove(completedTicket);
        Instantiate(diceDestroySmoke, loc, Quaternion.identity);
        Instantiate(diceDestroySparks, loc, Quaternion.identity);
        UpdateUI();
    }


    public void ReturnToMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    void UpdateUI()
    {
        leaderboard.UpdateLeaderboard(followerCount);
        followerText.text = followerCount.ToString();
    }

    private static System.Random rng = new System.Random();

    private void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}