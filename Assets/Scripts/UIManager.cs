using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject winnerTextObject;

    public GameObject restartButton;

    public TextMeshProUGUI winnerText;

    public TextMeshProUGUI turnText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateTurnText(1);
    }

    public void ShowWinner(int player)
    {
        winnerTextObject.SetActive(true);

        restartButton.SetActive(true);

        winnerText.text =
            "Jogador " + player + " venceu!";
    }

    public void ShowDraw()
    {
        winnerTextObject.SetActive(true);

        restartButton.SetActive(true);

        winnerText.text = "Empate!";
    }

    public void UpdateTurnText(int player)
    {
        turnText.text =
            "Vez do Jogador " + player;
    }
}