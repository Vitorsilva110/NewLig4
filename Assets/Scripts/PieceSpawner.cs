using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    public GameObject redPiece;
    public GameObject yellowPiece;

    GridData gridData;

    int width = 7;
    int height = 6;

    void Start()
    {
        gridData = new GridData(width, height);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)
            && !GameManager.Instance.gameEnded)
        {
            DetectColumn();
        }
    }

    void DetectColumn()
    {
        Vector3 mousePosition =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        int column =
            Mathf.RoundToInt(mousePosition.x);

        DropPiece(column);
    }

    void DropPiece(int column)
    {
        if (column < 0 || column >= width)
            return;

        for (int y = 0; y < height; y++)
        {
            if (gridData.grid[column, y] == 0)
            {
                SpawnPiece(column, y);

                CheckWin();

                CheckDraw();

                if(!GameManager.Instance.gameEnded)
                {
                    GameManager.Instance.ChangeTurn();
                }

                break;
            }
        }
    }

    void SpawnPiece(int x, int y)
    {
        GameObject prefab;

        int player =
            GameManager.Instance.currentPlayer;

        if (player == 1)
        {
            prefab = redPiece;

            gridData.grid[x, y] = 1;
        }
        else
        {
            prefab = yellowPiece;

            gridData.grid[x, y] = 2;
        }

        GameObject piece = Instantiate(
            prefab,
            new Vector3(x, height + 1, 0),
            Quaternion.identity
        );

        FallingPiece fallingPiece =
            piece.GetComponent<FallingPiece>();

        fallingPiece.targetPosition =
            new Vector3(x, y, 0);
    }

    void CheckWin()
    {
        int[,] board = gridData.grid;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int player = board[x, y];

                if (player == 0)
                    continue;

                // Horizontal
                if (x + 3 < width)
                {
                    if (board[x + 1, y] == player &&
                        board[x + 2, y] == player &&
                        board[x + 3, y] == player)
                    {
                        Win(player);
                    }
                }

                // Vertical
                if (y + 3 < height)
                {
                    if (board[x, y + 1] == player &&
                        board[x, y + 2] == player &&
                        board[x, y + 3] == player)
                    {
                        Win(player);
                    }
                }

                // Diagonal direita
                if (x + 3 < width && y + 3 < height)
                {
                    if (board[x + 1, y + 1] == player &&
                        board[x + 2, y + 2] == player &&
                        board[x + 3, y + 3] == player)
                    {
                        Win(player);
                    }
                }

                // Diagonal esquerda
                if (x - 3 >= 0 && y + 3 < height)
                {
                    if (board[x - 1, y + 1] == player &&
                        board[x - 2, y + 2] == player &&
                        board[x - 3, y + 3] == player)
                    {
                        Win(player);
                    }
                }
            }
        }
    }

    void CheckDraw()
    {
        if(GameManager.Instance.gameEnded)
            return;

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                if(gridData.grid[x, y] == 0)
                {
                    return;
                }
            }
        }

        Draw();
    }

    void Draw()
    {
        GameManager.Instance.gameEnded = true;

        UIManager.Instance.ShowDraw();
    }

    void Win(int player)
    {
        GameManager.Instance.gameEnded = true;

        UIManager.Instance.ShowWinner(player);
    }
}