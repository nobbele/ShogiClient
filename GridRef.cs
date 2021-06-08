using Microsoft.Xna.Framework;

namespace ShogiClient
{
    /// <summary>
    ///   A reference to a grid element, contains data and the position in the grid.
    /// </summary>
    public struct GridRef<T>
    {
        public T Data;
        public Point Position;
    }
}