public class GridData
{
    public int[,] grid;

    public int width;
    public int height;

    public GridData(int width, int height)
    {
        this.width = width;
        this.height = height;

        grid = new int[width, height];
    }
}