namespace Snake
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    };

    public class Settings
    {
        public static int Width;
        public static int Height;
        public static int Speed;
        public static int Score;
        public static int Points;
        public static bool GameOver;
        public static Direction direction;

        public Settings()
        {
            Width = 16;
            Height = 16;
            Speed = 15;
            Score = 0;
            Points = 100;
            GameOver = false;
            direction = Direction.Down;
        }
    }


}
