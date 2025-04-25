using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class LeaderboardManager : MonoBehaviour
{
    private const string API_URL = "https://192.168.1.168:7215/Leaderboard";

    public Text[] usernameTexts;    // Array de 5 textos para nombres
    public Text[] pointsTexts;      // Array de 5 textos para puntos
    public Text currentPositionText; // Texto para mostrar la posición actual del jugador
    private int userId;             // ID del usuario actual

    void Start()
    {
        //userId = PlayerPrefs.GetInt("UserId");
        userId = 1;
        StartCoroutine(LoadLeaderboard());
    }

    public void OnCLick()
    {
        StartCoroutine(LoadLeaderboard());
    }

    private IEnumerator LoadLeaderboard()
    {
        UnityWebRequest web = UnityWebRequest.Get(API_URL);
        web.certificateHandler = new ForceAcceptAll();
        yield return web.SendWebRequest();

        if(web.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error API: " + web.error);
        }
        else
        {
            var entries = JsonConvert.DeserializeObject<List<LeaderboardEntry>>(web.downloadHandler.text);
            UpdateLeaderboardUI(entries);
        }
    }

    private void UpdateLeaderboardUI(List<LeaderboardEntry> entries)
    {
        // Actualizar top 5
        for (int i = 0; i < entries.Count && i < 5; i++)
        {
            usernameTexts[i].text = $"{entries[i].Username}";
            pointsTexts[i].text = entries[i].Points.ToString("F0");

        }

        // Buscar y mostrar posición del jugador actual
        for (int i = 0; i < entries.Count; i++)
        {
            if (entries[i].UserId == userId)
            {
                currentPositionText.text = $"Tu posición actual es: #{i + 1}";
                return;
            }
        }
        currentPositionText.text = "Tu posición: Sin clasificar";
    }

}

[System.Serializable]
public class LeaderboardEntry
{
    public int UserId;
    public string Username;
    public float Points;
}
