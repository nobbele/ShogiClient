namespace ShogiClient
{
    public class Grid<T>
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        private T[][] Data;

        public Grid(int width, int height)
        {
            Width = width;
            Height = height;
            Data = new T[height][];
            for (int i = 0; i < height; i++)
            {
                Data[i] = new T[width];
            }
        }

        public T GetAt(int x, int y) => Data[y][x];
        public void SetAt(int x, int y, T data) => Data[y][x] = data;

        // Ignores checking if either is 0
        public bool AreIndicesWithinBounds(int x, int y) =>
            x >= 0 && x < Width
            && y >= 0 && y < Height;
    }
}