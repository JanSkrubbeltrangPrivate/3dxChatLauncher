using BLL.Enums;
using BLL.Interfaces;

namespace BLL.Controllers
{
    public class DisplayController : IDisplayController
    {
        public bool LauncApplicationOnNoLoginFound()
        {
            Console.WriteLine("No logins found! (q to quit / l to launch the game)");

            while (true)
            {
                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.Q:
                        return false;

                    case ConsoleKey.L:
                        return true;
                }
            }
        }

        public SelectLoginChoice SelectLogin(string[] LoginKeys, string currentLoginKey, out string Login)
        {
            PrepareScreen();
            Console.WriteLine("Select the login you want to use");
            Console.WriteLine("====================================");
            for (int i = 0; i < LoginKeys.Length && i < 10; i++)
            {
                if (LoginKeys[i] == currentLoginKey)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine($"{i + 1}:\t{LoginKeys[i]}");
                Console.ForegroundColor = ConsoleColor.Gray;

            }
            Console.WriteLine("====================================");
            Console.WriteLine("Enter the number of the login you want to use (q to quit / l to start without changing the login)");

            while (true)
            {
                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.Q:
                        Login = "";
                        return SelectLoginChoice.Quit;

                    case ConsoleKey.L:
                        Login = "";
                        return SelectLoginChoice.Launch;

                    default:
                        if (int.TryParse(key.KeyChar.ToString(), out int index))
                        {
                            if (index > 0 && index <= LoginKeys.Length)
                            {
                                Login = LoginKeys[index - 1];
                                return SelectLoginChoice.Found;
                            }
                        }
                        break;
                }
            }
        }

        public bool StoreNewLogin(out string NewName)
        {
            PrepareScreen();
            NewName = "";
            Console.WriteLine("New Login found. Do you want to store it? (y/n)");
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Y)
            {

                Console.WriteLine("\n\rEnter a name for this login");
                string name = Console.ReadLine() ?? "Default";
                NewName = name;
                return true;
            }
            return false;
        }

        private void PrepareScreen()
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Title = "3DXChat Login Selector";
            Console.WriteLine("      3DXChat Login Selector");
            Console.WriteLine("====================================");
        }
    }
}
