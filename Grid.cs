using System.Collections;
using System.Collections.Generic;

namespace ShogiClient
{
    public class Grid<T> : IEnumerable<(T Content, int X, int Y)>
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

        public Grid<T> Clone()
        {
            var clone = new Grid<T>(Width, Height);

            for (int y = 0; y < Height; y++)
            {
                clone.Data[y] = (T[])Data[y].Clone();
            }

            return clone;
        }

        public IEnumerator<(T Content, int X, int Y)> GetEnumerator()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    yield return (GetAt(x, y), x, y);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}