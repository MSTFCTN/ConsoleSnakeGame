using System.Threading;
using System;
using System.Reflection.Metadata.Ecma335;
using System.Globalization;

namespace SnakeGame
{
    public struct Position
    {
        public int X;
        public int Y;
        public Position(int x, int y)
        {
            X = x; 
            Y = y;
        }
    }

    public static class SnakeSettings
    {
        public static int SnakeSize = 3;
        public static string Snake_Head = "0";
        public static string Snake_Tail = "*";
    }

    public static class FoodSettings
    {
        public static string Food = "@";
    }

    class Game
    {
        public static Random rnd = new Random();
        public const int tickRate = 100; // miliseconds
        public static bool gameOver = true;
        public static int HighestScore = 0;

        public static void Main()
        {
            while (gameOver)
            {
                
                //------- Console Settings -------//
                Console.CursorVisible = false;
                Console.Title = "Yılan Oyunu";

                int Map_Height = 30;
                int Map_Width = 81;

                Console.Clear();
                //------- Drawing Map -------//
                for (int i = 0; i < Map_Width; i++)
                {
                    for (int j = 0; j < Map_Height; j++)
                    {
                        Console.SetCursorPosition(i, j);
                        if (i == 0 || i == Map_Width - 1)
                        {
                            Console.Write("█");
                        }
                        else if (j == 0 || j == Map_Height - 1)
                        {
                            Console.Write("█");
                        }
                    }
                }

                // Console.SetCursorPosition(79, 28); // Bottom Right Corner
                // Console.SetCursorPosition(1, 1); // Top Left Corner




                //------------ Default Values--------------//

                Position[] Snake = new Position[2212];
                Snake[0] = new Position(3, 14); //Head
                Snake[1] = new Position(2, 14); //Tail
                Snake[2] = new Position(1, 14); //Tail

                ConsoleKeyInfo key = new ConsoleKeyInfo();
                string direction = "right";

                SnakeSettings.SnakeSize = 3;

                Position[] food = new Position[1];
                food[0] = new Position(10, 14);
                Console.SetCursorPosition(food[0].X, food[0].Y);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(FoodSettings.Food);
                Console.ResetColor();

                Console.SetCursorPosition(90, 14);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Highest Score: " + (HighestScore).ToString());


                bool restart = false;
                while (!restart)
                {

                    //------Score Mechanics------//
                    Console.SetCursorPosition(37, 0);
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.Write("Score: " + (SnakeSettings.SnakeSize - 3).ToString());
                    Console.ResetColor();

                    while (Console.KeyAvailable)
                    {
                        //This part allows me to retrieve the last key pressed
                        key = Console.ReadKey(true);
                    }


                    if ((key.Key == ConsoleKey.W || key.Key == ConsoleKey.UpArrow) && direction != "down")
                    {
                        direction = "up";
                    }
                    else if ((key.Key == ConsoleKey.A || key.Key == ConsoleKey.LeftArrow) && direction != "right")
                    {
                        direction = "left";
                    }
                    else if ((key.Key == ConsoleKey.S || key.Key == ConsoleKey.DownArrow) && direction != "up")
                    {
                        direction = "down";
                    }
                    else if ((key.Key == ConsoleKey.D || key.Key == ConsoleKey.RightArrow) && direction != "left")
                    {
                        direction = "right";
                    }
                    else if (key.Key == ConsoleKey.Q)
                    {
                        Console.Clear();
                        Console.SetCursorPosition(50, 13);
                        Console.WriteLine("Game Executed!");
                        Console.SetCursorPosition(50, 16);
                        Console.WriteLine("Highest Score : {0}", HighestScore);
                        Thread.Sleep(2000);
                        return;
                    }



                    Position[] temp = new Position[1];
                    temp[0] = new Position(Snake[SnakeSettings.SnakeSize - 1].X, Snake[SnakeSettings.SnakeSize - 1].Y);

                    //----------Following The Tail------------//
                    for (int i = SnakeSettings.SnakeSize - 1; i > 0; i--)
                    {
                        Snake[i] = Snake[i - 1];
                    }

                    /*
                    Snake[0] = (3, 14) --> Snake[0] = (3, 14)
                    Snake[1] = (2, 14) --> Snake[1] = (3, 14)
                    Snake[2] = (1, 14) --> Snake[2] = (2, 14)
                    */

                    if (direction == "up")
                    {
                        Snake[0].Y -= 1;
                    }
                    else if (direction == "down")
                    {
                        Snake[0].Y += 1;
                    }
                    else if (direction == "left")
                    {
                        Snake[0].X -= 1;
                    }
                    else if (direction == "right")
                    {
                        Snake[0].X += 1;
                    }

                    /*
                    (right)
                    Snake[0] = (3, 14) --> Snake[0] = (4, 14)
                    Snake[1] = (3, 14) --> Snake[1] = (3, 14)
                    Snake[2] = (2, 14) --> Snake[2] = (2, 14)
                    */



                    //-------- Food Mechanics --------//

                    bool control = true;
                    while (control)
                    {
                        int controlCounter = 0;
                        for (int i = 1; i < SnakeSettings.SnakeSize; i++)
                        {
                            if (food[0].X == Snake[i].X && food[0].Y == Snake[i].Y)
                            {
                                food[0] = new Position(rnd.Next(1, 79), rnd.Next(1, 28));
                            }
                        }
                        for (int i = 1; i < SnakeSettings.SnakeSize; i++)
                        {
                            if (food[0].X != Snake[i].X || food[0].Y != Snake[i].Y)
                            {
                                controlCounter += 1;
                            }
                            if (controlCounter == SnakeSettings.SnakeSize - 1)
                            {
                                control = false;
                            }
                        }
                    }

                    Console.SetCursorPosition(food[0].X, food[0].Y);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(FoodSettings.Food);
                    Console.ResetColor();




                    //--------Tail Eating Mechanics--------//
                    for (int i = 1; i < SnakeSettings.SnakeSize; i++)
                    {
                        if (Snake[0].X == Snake[i].X && Snake[0].Y == Snake[i].Y)
                        {
                            Console.Clear();
                            Console.SetCursorPosition(43, 14);
                            Console.WriteLine("Game Over!");
                            Console.SetCursorPosition(43, 16);
                            Console.WriteLine("Score: " + (SnakeSettings.SnakeSize - 3).ToString());
                            Thread.Sleep(1000);
                            Console.SetCursorPosition(40, 18);
                            Console.WriteLine("Press R key to try again (Q to quit)");

                            while (Console.KeyAvailable) Console.ReadKey(true); //to clean input buffer

                            key = Console.ReadKey(true);
                            if (key.Key == ConsoleKey.R)
                            {
                                restart = true;
                                if ((SnakeSettings.SnakeSize - 3) >= HighestScore)
                                {
                                    Game.HighestScore = SnakeSettings.SnakeSize - 3;
                                }
                            }
                            else
                            {
                                Console.Clear();
                                Console.SetCursorPosition(43, 14);
                                Console.WriteLine("Game Executed!");
                                Thread.Sleep(1000);
                                return;
                            }
                        }
                    }

                    if (restart)
                    {
                        break;
                    }

                    //-------- Wall-passing mechanics --------//
                    if (Snake[0].X == 0)
                    {
                        Snake[0].X = 79;
                    }
                    else if (Snake[0].X == 80)
                    {
                        Snake[0].X = 1;
                    }
                    else if (Snake[0].Y == 0)
                    {
                        Snake[0].Y = 28;
                    }
                    else if (Snake[0].Y == 29)
                    {
                        Snake[0].Y = 1;
                    }

                    //-------- Drawing the snake --------//
                    Console.SetCursorPosition(Snake[0].X, Snake[0].Y);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(SnakeSettings.Snake_Head);
                    for (int i = 1; i < SnakeSettings.SnakeSize; i++)
                    {
                        Console.SetCursorPosition(Snake[i].X, Snake[i].Y);
                        Console.Write(SnakeSettings.Snake_Tail);
                    }
                    Console.ResetColor();

                    //-------- Food And Tail Eating Mechanics--------//
                    if (food[0].X == Snake[0].X && food[0].Y == Snake[0].Y)
                    {
                        Snake[SnakeSettings.SnakeSize] = new Position(temp[0].X, temp[0].Y);
                        SnakeSettings.SnakeSize++;
                    }
                    else
                    {
                        Console.SetCursorPosition(temp[0].X, temp[0].Y);
                        Console.Write(" ");
                    }


                    Thread.Sleep(tickRate);
                }
                Console.Clear();
            }
            Console.ReadKey(true);
        }
    }
}