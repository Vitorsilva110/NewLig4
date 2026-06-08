using UnityEngine;

public class Board : MonoBehaviour
{
    public int width = 7;
    public int height = 6;

    public GameObject cellPrefab;

    void Start()
    {
        GenerateBoard();
    }

    void GenerateBoard()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2 position = new Vector2(x, y);

                Instantiate(
                    cellPrefab,
                    position,
                    Quaternion.identity,
                    transform
                );
            }
        }
    }
}