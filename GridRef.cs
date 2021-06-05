using Microsoft.Xna.Framework;

namespace ShogiClient
{
    public struct GridRef<T>
    {
        public T Data;
        public Point Position;
    }
}