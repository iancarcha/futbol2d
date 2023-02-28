using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI player1Score;
    [SerializeField] private TextMeshProUGUI player2Score;
    [SerializeField] private GameObject results;

    public void DisplayResults(int player1, int player2)
    {
        results.SetActive(true);
        player1Score.text = player1.ToString();
        player2Score.text = player2.ToString();
    }
}
