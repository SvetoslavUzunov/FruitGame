using System;
using System.Web.Mvc;

namespace FruitGame.Controllers
{
    public class HomeController : Controller
    {
        static int countRows = 3;
        static int countCols = 9;
        static string[,] fruits = GenerateRandomFruits();
        static int scores = 0;
        static bool gameOver = false;
        static string firedFruits = string.Empty;

        public ActionResult Index()
        {
            ViewBag.countRows = countRows;
            ViewBag.countCols = countCols;
            ViewBag.fruits = fruits;
            ViewBag.scores = scores;
            ViewBag.gameOver = gameOver;
            ViewBag.firedFruits = firedFruits;
            return View();
        }

        static string[,] GenerateRandomFruits()
        {
            var rand = new Random();
            fruits = new string[countRows, countCols];
            for (int row = 0; row < countRows; row++)
            {
                for (int col = 0; col < countCols; col++)
                {
                    var rnd = rand.Next(9);
                    if (rnd < 2) fruits[row, col] = "apple";
                    else if (rnd < 4) fruits[row, col] = "banana";
                    else if (rnd < 6) fruits[row, col] = "orange";
                    else if (rnd < 8) fruits[row, col] = "kiwi";
                    else fruits[row, col] = "dynamite";
                }
            }
            return fruits;
        }

        public ActionResult Reset()
        {
            fruits = GenerateRandomFruits();
            gameOver = false;
            scores = 0;
            firedFruits = string.Empty;
            return RedirectToAction("Index");
        }

        public ActionResult FireTop(int position)
        {
            return Fire(position, 0, 1);
        }

        public ActionResult FireBottom(int position)
        {
            return Fire(position, countRows - 1, -1);
        }

        private ActionResult Fire(int position, int startRow, int step)
        {
            var col = position * (countCols - 1) / 100;
            var row = startRow;

            while (row >= 0 && row < countRows)
            {
                var fruit = fruits[row, col];
                if (fruit != "dynamite" && fruit != "empty")
                {
                    if (fruit == "apple") { scores += 5; firedFruits += " apple"; }
                    else if (fruit == "banana") { scores += 10; firedFruits += " banana "; }
                    else if (fruit == "kiwi") { scores += 15; firedFruits += " kiwi"; }
                    else { scores += 20; firedFruits += " orange"; }
                    fruits[row, col] = "empty";
                    break;
                }
                else if (fruit == "dynamite") { gameOver = true; break; }
                row += step;
            }
            return RedirectToAction("Index");
        }
    }
}