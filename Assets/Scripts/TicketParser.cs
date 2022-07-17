using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using System.IO;

public class TicketParser : MonoBehaviour
{
    public static List<TicketModel> ReadTickets(TextAsset ticketCSV)
    {
        List<TicketModel> tickets = new List<TicketModel>();
        List<string> ticketRows = new List<string>(ticketCSV.text.Split('\n'));
        // drop header row
        ticketRows.RemoveAt(0);
        foreach (string ticketRaw in ticketRows)
        {
            string[] components = ticketRaw.Split(',');

            int rollNeeded = int.Parse(components[1]);
            bool below = bool.Parse(components[2]);
            int severity = int.Parse(components[3]);

            string plea = components[0].Replace('_', '\n');
            StringBuilder sb = new StringBuilder(plea);

            // Loops are for dumb idiots anyway
            sb.Replace("NOT", @"<color=#FF0000>NOT</color>");
            sb.Replace("REALLY", @"<color=#0dabe5>REALLY</color>");
            sb.Replace("HIGH", @"<color=#0dabe5>HIGH</color>");
            sb.Replace("LOW", @"<color=#0dabe5>LOW</color>");
            sb.Replace("KILL", @"<color=#eaae15>KILL</color>");
            sb.Replace("DIE", @"<color=#eaae15>DIE</color>");
            sb.Replace("BOSS", @"<color=#eaae15>BOSS</color>");
            sb.Replace("EVERYTHING", @"<color=#eaae15>EVERYTHING</color>");
            sb.Replace("RESURRECT", @"<color=#eaae15>RESURRECT</color>");
            string pleaWithColor = sb.ToString();

            tickets.Add(new TicketModel(pleaWithColor, rollNeeded, below, severity));
        }

        return tickets;
    }
}
