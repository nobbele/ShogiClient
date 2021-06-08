using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace ShogiClient
{
    /// <summary>
    ///   A grid is a two dimensional collection type.
    /// </summary>
    [JsonObject]
    public class Grid<T> : IEnumerable<GridRef<T>>
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        [JsonProperty]
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

        public GridRef<T> GetAt(int x, int y) => GetAt(new Point { X = x, Y = y });
        public GridRef<T> GetAt(Point point) => new GridRef<T> { Data = Data[point.Y][point.X], Position = point };
        public void SetAt(int x, int y, T data) => Data[y][x] = data;
        public void SetAt(Point point, T data) => Data[point.Y][point.X] = data;

        // Ignores checking if either is 0
        public bool AreIndicesWithinBounds(int x, int y) =>
            x >= 0 && x < Width
            && y >= 0 && y < Height;
        public bool IsPointOnGrid(Point point) => AreIndicesWithinBounds(point.X, point.Y);

        public Grid<T> Clone()
        {
            var clone = new Grid<T>(Width, Height);

            for (int y = 0; y < Height; y++)
            {
                clone.Data[y] = (T[])Data[y].Clone();
            }

            return clone;
        }

        public IEnumerator<GridRef<T>> GetEnumerator()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    yield return GetAt(x, y);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}