using GDAPI.Application;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace GDAPITest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var gdPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GeometryDash");
            Console.WriteLine("Hello World!");

            string gameManagerPath = Path.Join(gdPath, "CCGameManager.dat"); // the path of the CCGameManager.dat file which contains everything else
            string localLevelsPath = Path.Join(gdPath, "CCLocalLevels.dat"); // the path of the CCLocalLevels.dat file which contains the levels you can edit
            Database database = new Database(gameManagerPath, localLevelsPath);


            database.LevelsRetrieved += async () =>
            {
                var lcount = database.UserLevels.Count;
                var lcountWidth = lcount.ToString().Length;
                for (var i = 0; i < lcount; i++)
                {
                    var userLevel = database.UserLevels[i];
                    Console.WriteLine($"\n{i.ToString().PadLeft(lcountWidth)})\nName: {userLevel.Name}\nDescription:");
                }
                var picked = -1;
                while (picked < 0 || picked >= lcount)
                {
                    Console.Write("Pick a Level: ");
                    var line = Console.ReadLine();
                    if (!int.TryParse(line, out picked) || picked < 0 || picked >= lcount)
                    {
                        Console.WriteLine("Please enter a valid number.");
                    }
                }
                var pickedLevel = database.UserLevels[picked];
                Console.WriteLine("You picked " + pickedLevel.Name);

                await pickedLevel.InitializeLoadingLevelString();

                // print out the first 10 objects of the level
                for(int i = 0; i < Math.Min(pickedLevel.LevelObjects.Count, 10); i++)
                {
                    var el = pickedLevel.LevelObjects[i];
                    Console.WriteLine($"object {el.ObjectID} position {el.Location}");
                }
                Environment.Exit(0);
            };
            while (true)
            {
                Thread.Sleep(10000);
            }
        }
    }
}
