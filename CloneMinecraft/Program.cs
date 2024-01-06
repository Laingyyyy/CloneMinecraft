namespace CloneMinecraft
{
    class Program
    {
        static void Main(string[] args)
        {

            // creates a game object and disposes it after leaving the scope
            using(Game game = new Game(500, 500))
            {
                game.Run();
            }
        }
    }
}