using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Score : MonoBehaviour
{
    public Text[] playersScore;
    private int[] playerScoresInt = { 0, 0 };


    private int playerNumber = 0;

    void Start()
    {
        playersScore[0].text = "Player 1: " + playerScoresInt[0].ToString();
        playersScore[1].text = "Player 2: " + playerScoresInt[1].ToString();
    }

    public int cmdAddPlayer()
    {
        return ++playerNumber;
    }

    public void cmdPlayerDied(int player)
    {
        playerScoresInt[(player == 1) ? 1 : 0]++;
        playersScore[(player == 1) ? 1 : 0].text = "Player " + ((player == 1) ? 2 : 1) + ": " + playerScoresInt[(player == 1) ? 1 : 0].ToString();
    }
}
