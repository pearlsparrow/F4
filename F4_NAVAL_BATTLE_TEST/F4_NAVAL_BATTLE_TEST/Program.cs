using System;
using System.Collections.Generic;

namespace F4_NavalBattle
{
    class Program
    {
        static void Main(string[] args)
        {


            Play();
            Console.WriteLine("Thanks for playing");
            Console.ReadLine();

            

        }



        public static void Play()
        {
            Player pearl = new Player("Panos");
            Player sancroth = new Player("Nikos");

            int counterPearl = 0;
            int counterSancroth = 0;

            pearl.PrintNavalBoard();
            pearl.ChooseMyShips();
            Console.WriteLine("Press Enter to give controll to {0}!", sancroth.name);
            Console.ReadLine();
            Console.Clear();

            sancroth.PrintNavalBoard();
            sancroth.ChooseMyShips();
            Console.WriteLine("Press Enter to give controll to {0}!", pearl.name);
            Console.ReadLine();
            Console.Clear();




            while (true)
            {

                pearl.PrintNavalBoard();
                counterPearl = counterPearl + pearl.MarkShips(sancroth);
                if (counterPearl == 2)
                {
                    Console.Clear();
                    pearl.PrintNavalBoard();
                    Console.WriteLine("The game is Over! {0} Won!", pearl.name);
                    break;
                }
                Console.Clear();
                pearl.PrintNavalBoard();
                Console.WriteLine("Press Enter to give controll to {0}!", sancroth.name);
                Console.ReadLine();
                Console.Clear();

                sancroth.PrintNavalBoard();
                counterSancroth = counterSancroth + sancroth.MarkShips(pearl);
                if (counterSancroth == 2)
                {
                    Console.Clear();
                    sancroth.PrintNavalBoard();
                    Console.WriteLine("The game is Over! {0} Won!", sancroth.name);
                    break;
                }
                Console.Clear();
                sancroth.PrintNavalBoard();
                Console.WriteLine("Press Enter to give controll to {0}!", pearl.name);
                Console.ReadLine();
                Console.Clear();
            }

        }


        class Player
        {

            public string[] destroyer { get; } = new string[2];
            public string[] battleShip { get; } = new string[3];
            public string[] subMarine { get; } = new string[4];


            public string name { get; set; }




            public string[,] markedShips = new string[10, 10];
            public string[,] ownShips = new string[10, 10];


            public Player(string name)
            {

                this.name = name;
                SetNavalBattle(this.markedShips);
                SetNavalBattle(this.ownShips);

            }

            public void ChooseMyShips()
            {
                int x;
                int y;
                string pos;
                int offsetX = 0;
                int offsetY = 0;
                bool isCollision = true;
                List<string> collisions = new List<string>();
                string direction;
                bool inBound = false;

                //-----------------------------------------------------------DESTROYER

                while (inBound == false)
                {
                    (offsetX, offsetY, direction) = GetOffsetPos("DESTROYER");
                    Console.WriteLine("Please Choose your Destroyer starting Coordinations(2 positions)");
                    (x, y) = FindCoordinates();
                    inBound = ShipOutOfBounds(x, y, destroyer.Length, direction);
                    Console.Clear();
                    destroyer[0] = getNavalx(x) + "" + (y + 1);
                    destroyer[1] = getNavalx(x + offsetX * 1) + "" + (y + 1 + offsetY * 1);
                    PrintNavalBoard();

                    if (inBound == true)
                    {
                        Console.Clear();
                        SetCoordinators(x, y, "D");
                        SetCoordinators(x + offsetX * 1, y + offsetY * 1, "D");

                        PrintNavalBoard();
                    }
                    else
                    {
                        Console.WriteLine("Out Of bound");
                    }
                }
                inBound = false;

                //------------------------------------------------------BATTLESHIP

                while (isCollision == true && inBound == false)
                {

                    (offsetX, offsetY, direction) = GetOffsetPos("BATTLESHIP");

                    Console.WriteLine("Please Choose your Battleship starting Coordinations(3 positions)");
                    (x, y) = FindCoordinates();
                    inBound = ShipOutOfBounds(x, y, battleShip.Length, direction);
                    Console.Clear();
                    if (inBound == false)
                    {
                        PrintNavalBoard();
                        Console.WriteLine("Out of bounds");
                    }
                    else
                    {
                        int count = 0;
                        for (int i = 0; i < battleShip.Length; i++)
                        {
                            battleShip[i] = getNavalx(x + offsetX * i) + "" + (y + 1 + offsetY * i);
                            for (int j = 0; j < destroyer.Length; j++)
                            {
                                if (battleShip[i] == destroyer[j])
                                {
                                    collisions.Add(battleShip[i]);
                                    count++;
                                }

                            }
                        }
                        if (count == 0)
                        {
                            isCollision = false;
                            SetCoordinators(x, y, "B");
                            SetCoordinators(x + offsetX * 1, y + offsetY * 1, "B");
                            SetCoordinators(x + offsetX * 2, y + offsetY * 2, "B");
                        }



                        PrintNavalBoard();
                        if (count > 0)
                        {
                            inBound = false;
                            string[,] tmpOwnShips = (string[,])ownShips.Clone();
                            int xTmp, yTmp;
                            SetCoordinators(x, y, "B");
                            SetCoordinators(x + offsetX * 1, y + offsetY * 1, "B");
                            SetCoordinators(x + offsetX * 2, y + offsetY * 2, "B");
                            Console.Clear();
                            for (int i = 0; i < collisions.Count; i++)
                            {

                                (xTmp, yTmp) = FindCoordinatesNoInput(collisions[i]);
                                SetCoordinators(xTmp, yTmp, "X");
                            }
                            PrintNavalBoard();
                            ownShips = (string[,])tmpOwnShips.Clone();


                            Console.ForegroundColor = ConsoleColor.Red;
                            for (int i = 0; i < collisions.Count; i++)
                            {
                                Console.WriteLine("Collision at the coordinates:{0}", collisions[i]);
                            }
                            Console.ResetColor();
                            Console.WriteLine("Try again position is taken");
                        }

                        collisions.Clear();
                    }
                }
                inBound = false;
                isCollision = true;
                while (isCollision == true && inBound == false)
                {
                    //-----------------------------------------------------SUBMARINE
                    (offsetX, offsetY, direction) = GetOffsetPos("SUBMARINE");

                    Console.WriteLine("Please Choose your SubMarine starting Coordinations(4 positions)");
                    (x, y) = FindCoordinates();
                    inBound = ShipOutOfBounds(x, y, subMarine.Length, direction);
                    Console.Clear();
                    if (inBound == false)
                    {
                        PrintNavalBoard();
                        Console.WriteLine("Out of bounds");
                    }
                    else
                    {
                        int count = 0;
                        for (int i = 0; i < subMarine.Length; i++)
                        {
                            subMarine[i] = getNavalx(x + offsetX * i) + "" + (y + 1 + offsetY * i);
                            for (int j = 0; j < destroyer.Length; j++)
                            {
                                if (subMarine[i] == destroyer[j])
                                {
                                    collisions.Add(battleShip[i]);
                                    count++;
                                    isCollision = true;
                                }
                            }
                        }
                        for (int i = 0; i < battleShip.Length; i++)
                        {
                            for (int j = 0; j < subMarine.Length; j++)
                            {
                                if (subMarine[j] == battleShip[i])
                                {
                                    collisions.Add(battleShip[i]);
                                    count++;
                                }
                            }
                        }

                        if (count == 0)
                        {
                            isCollision = false;
                            SetCoordinators(x, y, "S");
                            SetCoordinators(x + offsetX * 1, y + offsetY * 1, "S");
                            SetCoordinators(x + offsetX * 2, y + offsetY * 2, "S");
                            SetCoordinators(x + offsetX * 3, y + offsetY * 3, "S");
                        }
                        PrintNavalBoard();

                        if (count > 0)
                        {
                            inBound = false;
                            string[,] tmpOwnShips = (string[,])ownShips.Clone();
                            int xTmp, yTmp;
                            SetCoordinators(x, y, "S");
                            SetCoordinators(x + offsetX * 1, y + offsetY * 1, "S");
                            SetCoordinators(x + offsetX * 2, y + offsetY * 2, "S");
                            SetCoordinators(x + offsetX * 3, y + offsetY * 3, "S");
                            Console.Clear();
                            for (int i = 0; i < collisions.Count; i++)
                            {

                                (xTmp, yTmp) = FindCoordinatesNoInput(collisions[i]);
                                SetCoordinators(xTmp, yTmp, "X");
                            }
                            PrintNavalBoard();
                            ownShips = (string[,])tmpOwnShips.Clone();


                            Console.ForegroundColor = ConsoleColor.Red;
                            for (int i = 0; i < collisions.Count; i++)
                            {
                                Console.WriteLine("Collision at the coordinates:{0}", collisions[i]);
                            }
                            Console.ResetColor();
                            Console.WriteLine("Try again position is taken");
                        }
                        collisions.Clear();
                    }

                }

            }


            public (int, int, string) GetOffsetPos(string shipName)
            {
                int offsetX, offsetY;
                string direction;

                while (true)
                {
                    Console.WriteLine("Please choose: Horizontal or Vertical(Press H or V) {0}", shipName);
                    direction = Console.ReadLine().ToUpper();
                    if (direction == "H" || direction == "V")
                    {
                        if (direction == "H")
                        {
                            offsetX = 0;
                            offsetY = 1;
                        }
                        else
                        {
                            offsetX = 1;
                            offsetY = 0;
                        }
                        return (offsetX, offsetY, direction);
                    }
                }
            }


            public void SetCoordinators(int x, int y, string ship)
            {
                this.ownShips[x, y] = ship;
            }

            public void SetMarkedShip(int x, int y, string ship)
            {
                this.markedShips[x, y] = ship;
            }


            public (int, int) FindCoordinates()
            {
                bool isEmpty = true;
                char posx;
                string posy ;
                int posxIndex = -1;
                int posyIndex = -1;
                bool isItNum = false;

                while (isEmpty == true)
                {
                    string startingPoint = Console.ReadLine().ToUpper();


                    if(startingPoint.Length<=1)
                    {
                        isEmpty = true;
                        Console.Clear();
                        this.PrintNavalBoard();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Wrong Input");
                        Console.ResetColor();

                        continue;
                    }
                    else
                    {
                        isEmpty =false;
                        posx = startingPoint[0];
                        posy = startingPoint[1].ToString();

                        posxIndex = getNavalIndex(posx);

                        if(posxIndex==-1)
                        {
                            Console.Clear();
                            this.PrintNavalBoard();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Wrong Input");
                            Console.ResetColor();
                            isEmpty = true;
                            continue;
                        }


                        if (startingPoint.Length > 2)
                        {
                            
                            for (int i = 2; i < startingPoint.Length; i++)
                            {
                                
                                posy = posy + startingPoint[i].ToString();
                                
                            }
                        }
                        isItNum = int.TryParse(posy, out posyIndex);
                        posyIndex = posyIndex - 1;
                        if (isItNum == true)
                        {
                            return (posxIndex, posyIndex);
                        }
                        Console.Clear();
                        this.PrintNavalBoard();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Wrong Input");
                        Console.ResetColor();
                        isEmpty = true;
                    }

                }

                return (posxIndex, posyIndex);
            }


            public (int, int) FindCoordinatesNoInput(string pos)
            {
                char posx;
                string posy;


                posx = pos[0];
                posy = pos[1].ToString();

                if (pos.Length > 2)
                {
                    for (int i = 2; i < pos.Length; i++)
                    {
                        posy = posy + pos[i].ToString();
                    }
                }

                int posxIndex = getNavalIndex(posx);
                int posyIndex = int.Parse(posy) - 1;
                return (posxIndex, posyIndex);

            }


            public bool ShipOutOfBounds(int x, int y, int shipLength, string direction)
            {

                if (direction == "V")
                {
                    if (x + shipLength > 9)
                    {
                        return false;
                    }
                }
                else if (direction == "H")
                {
                    if (y + shipLength > 9)
                    {
                        return false;
                    }
                }
                return true;
            }


            public string getNavalx(int num)
            {
                switch (num)
                {
                    case 0:
                        return "A";
                    case 1:
                        return "B";
                    case 2:
                        return "C";
                    case 3:
                        return "D";
                    case 4:
                        return "E";
                    case 5:
                        return "F";
                    case 6:
                        return "G";
                    case 7:
                        return "H";
                    case 8:
                        return "I";
                    case 9:
                        return "J";
                    default:
                        return "";
                }
            }


            public int getNavalIndex(char c)
            {
                switch (c)
                {
                    case 'A':
                        return 0;
                    case 'B':
                        return 1;
                    case 'C':
                        return 2;
                    case 'D':
                        return 3;
                    case 'E':
                        return 4;
                    case 'F':
                        return 5;
                    case 'G':
                        return 6;
                    case 'H':
                        return 7;
                    case 'I':
                        return 8;
                    case 'J':
                        return 9;
                    default:
                        return -1;

                }
            }


            public void SetNavalBattle(string[,] navalBattle)
            {
                for (int i = 0; i < 10; i++)
                {

                    for (int j = 0; j < 10; j++)
                    {
                        navalBattle[i, j] = "O";

                    }

                }
            }


            public void PrintNavalBattle(string[,] navalBattle)
            {
                Console.WriteLine("    1    2    3    4    5    6    7    8    9    1O");
                Console.WriteLine("    |    |    |    |    |    |    |    |    |    |");
                for (int i = 0; i < 10; i++)
                {
                    Console.Write(getNavalx(i) + "-- ");
                    for (int j = 0; j < 10; j++)
                    {
                        Console.Write(navalBattle[i, j] + "    ");
                    }
                    Console.WriteLine("\n");
                }

            }


            public void PrintNavalBoard()
            {
                Console.ResetColor();
                Console.WriteLine("                                                 {0}                                           ", this.name);
                Console.WriteLine("                <<< MY OWN SHIPS >>>                          <<< MARKED SHIPS I WANT TO ATTACK>>>       ");
                Console.WriteLine("    1    2    3    4    5    6    7    8    9    1O       1    2    3    4    5    6    7    8    9    1O");
                Console.WriteLine("    |    |    |    |    |    |    |    |    |    |        |    |    |    |    |    |    |    |    |    | ");
                for (int i = 0; i < 10; i++)
                {
                    Console.Write(getNavalx(i) + "-- ");
                    for (int j = 0; j < 10; j++)
                    {
                        if (this.ownShips[i, j] == "S")
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                        }
                        if (this.ownShips[i, j] == "B")
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                        }
                        if (this.ownShips[i, j] == "D")
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        }
                        if (this.ownShips[i, j] == "X")
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }


                        Console.Write(this.ownShips[i, j] + "    ");
                        Console.ResetColor();

                    }
                    Console.Write(getNavalx(i) + "-- ");

                    for (int j = 0; j < 10; j++)
                    {
                        if (this.markedShips[i, j] == "X")
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        if (this.markedShips[i, j] == "H")
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }

                        Console.Write(this.markedShips[i, j] + "    ");
                        Console.ResetColor();
                    }

                    Console.WriteLine("\n");
                }

            }


            public int MarkShips(Player enemy)
            {
                int counter = 0;
                int x;
                int y;
                int arrayIndex;
                Console.WriteLine("Please choose a point to attack");
                string attackPoint = Console.ReadLine().ToUpper();
                arrayIndex = Array.IndexOf(enemy.destroyer, attackPoint);
                if (arrayIndex != -1)
                {
                    counter++;
                    (x, y) = FindCoordinatesNoInput(attackPoint);
                    SetMarkedShip(x, y, "H");
                    return counter;
                }
                arrayIndex = Array.IndexOf(enemy.battleShip, attackPoint);
                if (arrayIndex != -1)
                {
                    counter++;
                    (x, y) = FindCoordinatesNoInput(attackPoint);
                    SetMarkedShip(x, y, "H");
                    return counter;
                }
                arrayIndex = Array.IndexOf(enemy.subMarine, attackPoint);
                if (arrayIndex != -1)
                {
                    counter++;
                    (x, y) = FindCoordinatesNoInput(attackPoint);
                    SetMarkedShip(x, y, "H");
                    return counter;
                }



                (x, y) = FindCoordinatesNoInput(attackPoint);
                SetMarkedShip(x, y, "X");

                return counter;

            }




        }





    }





}
