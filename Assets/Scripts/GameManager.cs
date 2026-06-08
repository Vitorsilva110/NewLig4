using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentPlayer = 1;

    public bool gameEnded = false;

    void Awake()
    {
        Instance = this;
    }

    public void ChangeTurn()
    {
        if(currentPlayer == 1)
        {
            currentPlayer = 2;
        }
        else
        {
            currentPlayer = 1;
        }

        UIManager.Instance.UpdateTurnText(currentPlayer);

        Debug.Log("Jogador atual: " + currentPlayer);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }
}