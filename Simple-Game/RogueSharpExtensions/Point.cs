using RogueSharp;

namespace SimpleGame.RogueSharpExtensions
{
    public static class PointExtensions
    {
        public static Point Clone(this Point point)
        {
            return new Point(point.X, point.Y);
        }
    }
}
