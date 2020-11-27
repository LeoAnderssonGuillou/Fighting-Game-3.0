using System;
using System.Numerics;
using Raylib_cs;

namespace FightingGame3._0_Project
{
    class Program
    {
        class Move
        {
            public string name = "BONK";
            public int atk = 100;
            public int acc = 90;
            public Move(string _name, int _atk, int _acc)
            {
                name = _name;
                atk = _atk;
                acc = _acc;
            }
        }
        class Moveset
        {
            public Move move1 = new Move("", 0, 0);
            public Move move2 = new Move("", 0, 0);
            public Move move3 = new Move("", 0, 0);
            public Move move4 = new Move("", 0, 0);
            public Moveset(Move _move1, Move _move2, Move _move3, Move _move4)
            {
                move1 = _move1;
                move2 = _move2;
                move3 = _move3;
                move4 = _move4;
            }
        }
        static void Main(string[] args)
        {
            Raylib.InitWindow(1000, 800, "Walter Battle 2021");

            bool fighting = true;
            bool won = true;

            int battlePage = 1;
            int cooldown = 0;
            int hideBox = 0;
            int damage = 0;
            int randomMove = 1;

            Move[] moves = new Move[]{
                new Move("BONK", 100, 90),
                new Move("HIT PAN", 150, 80),
                new Move("CHEESEBORG", 90, 100),
                new Move("WAR CRIME", 200, 60),

                new Move("MONKE FLIP", 100, 90),
                new Move("SPINNING GORILLA", 150, 80),
                new Move("REJECT HUMANITY", 90, 100),
                new Move("CHIMP EVENT", 200, 60)
            };

            Moveset walterMoveset = new Moveset(moves[0], moves[1], moves[2], moves[3]);
            Moveset gorillaMoveset = new Moveset(moves[4], moves[5], moves[6], moves[7]);

            string fName = "WALTER";
            int fFullHP = 400;
            int fHP = 400;
            Move fMove = new Move("", 0, 0);
            Moveset fMoveset = walterMoveset;

            string oName = "GORILLA";
            int oFullHP = 400;
            int oHP = 400;
            Move oMove = new Move("", 0, 0);
            Moveset oMoveset = gorillaMoveset;


            Vector2 select = new Vector2(10, 510);
            Random generator = new Random();
            Texture2D walterT = Raylib.LoadTexture("walter.png");

            Color border1 = new Color(214, 214, 214, 255);
            Color border2 = new Color(150, 150, 150, 255);
            Color boxyellow = new Color(255, 255, 156, 255);
            Color boxgreen = new Color(187, 255, 156, 255);
            Color healthgreen = new Color(123, 255, 36, 255);
            Color healthred = new Color(255, 94, 94, 255);


            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(border1);                    //Background
                Raylib.DrawRectangle(10, 10, 980, 480, boxyellow);
                Raylib.DrawRectangle(0, 500, 1000, 300, border2);
                Raylib.DrawRectangle(10, 510, 980, 280, boxgreen);

                Raylib.DrawRectangle(55, 20, fFullHP, 20, healthred);     //Player health bar
                Raylib.DrawRectangle(55, 20, fHP, 20, healthgreen);

                Raylib.DrawRectangle(555, 20, oFullHP, 20, healthred);     //Opponent health bar
                Raylib.DrawRectangle(555, 20, oHP, 20, healthgreen);


                if (fighting == true)
                {
                    switch (battlePage)
                    {
                        case 1:
                            hideBox = Text("WHAT WILL " + fName + " DO?");
                            break;
                        case 2:                                                    //Select move
                            hideBox = 0;
                            Raylib.DrawRectangle(10, 645, 980, 10, border2);
                            Raylib.DrawRectangle(495, 510, 10, 280, border2);
                            select = Select(select, boxyellow);
                            Raylib.DrawText(fMoveset.move1.name, 20, 550, 48, Color.BLACK);
                            Raylib.DrawText(fMoveset.move2.name, 520, 550, 48, Color.BLACK);
                            Raylib.DrawText(fMoveset.move3.name, 20, 700, 48, Color.BLACK);
                            Raylib.DrawText(fMoveset.move4.name, 520, 700, 48, Color.BLACK);
                            break;
                        case 3:                                      //Used move
                            if (select.X == 10 && select.Y == 510)
                            {
                                fMove = fMoveset.move1;
                            }
                            else if (select.X == 505 && select.Y == 510)
                            {
                                fMove = fMoveset.move2;
                            }
                            else if (select.X == 10 && select.Y == 655)
                            {
                                fMove = fMoveset.move3;
                            }
                            else
                            {
                                fMove = fMoveset.move4;
                            }
                            MoveText(fName, fMove.name);
                            break;
                        case 4:                                      //Calculate damage / decide opponents next move
                            damage = Attack(generator, fMove.atk, fMove.acc);
                            oHP = oHP - damage;
                            MoveText(fName, fMove.name);
                            randomMove = generator.Next(1, 5);
                            battlePage++;
                            break;
                        case 5:                                      //Tell damage
                            if (damage > 0)
                            {
                                Text(damage + " DAMAGE!");
                            }
                            else
                            {
                                Text("MISS!");
                            }
                            break;
                        case 6:                                      //Check if won / Opponent used move
                            if (oHP <= 0)
                            {
                                fighting = false;
                                won = true;
                                break;
                            }
                            if (randomMove == 1)
                            {
                                oMove = oMoveset.move1;
                            }
                            else if (randomMove == 2)
                            {
                                oMove = oMoveset.move2;
                            }
                            else if (randomMove == 3)
                            {
                                oMove = oMoveset.move3;
                            }
                            else
                            {
                                oMove = oMoveset.move4;
                            }
                            MoveText(oName, oMove.name);
                            break;
                        case 7:                                       //Opponent calculate damage
                            damage = Attack(generator, oMove.atk, oMove.acc);
                            fHP = fHP - damage;
                            battlePage++;
                            break;
                        case 8:                                       //Opponent tell damage
                            if (damage > 0)
                            {
                                Text(damage + " DAMAGE!");
                            }
                            else
                            {
                                Text("MISS!");
                            }
                            break;
                        case 9:                                       //Check if lost / Next round
                            if (fHP <= 0)
                            {
                                fighting = false;
                                won = false;
                                break;
                            }
                            battlePage = 1;
                            break;
                    }
                }
                else
                {
                    switch (won)
                    {
                        case true:
                            Text(fName + " WINS!");
                            break;
                        case false:
                            Text(oName + " WINS!");
                            break;
                    }
                }


                if (Raylib.IsKeyDown(KeyboardKey.KEY_ENTER) && cooldown == 0)
                {
                    battlePage++;
                    cooldown = 300;
                }
                if (cooldown > 0)
                {
                    cooldown--;
                }
                if (hideBox > 0)
                {
                    hideBox = hideBox - 5;
                }

                //Raylib.DrawRectangle(10 + 485 - hideBox, 510, hideBox, 280, boxgreen);

                Raylib.DrawTexture(walterT, 0, 0, Color.WHITE);
                Raylib.DrawTexture(walterT, 500, 0, Color.WHITE);

                Raylib.EndDrawing();
            }
        }
        static int Text(string text)
        {
            Raylib.DrawText(text, 20, 520, 48, Color.BLACK);
            int box = 485;
            return box;
        }
        static void MoveText(string name, string move)
        {
            Raylib.DrawText(name + " USES " + move + "!", 20, 520, 48, Color.BLACK);
        }
        static int Attack(Random generator, int attack, int accuracy)
        {
            int damage = generator.Next(5, 11) * attack / 10;
            int unlucky = generator.Next(1, 101);
            if (unlucky > accuracy)
            {
                damage = 0;
            }
            return damage;
        }

        static Vector2 Select(Vector2 pos, Color col)
        {
            if (Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT))
            {
                pos.X = 505;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_DOWN))
            {
                pos.Y = 655;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT))
            {
                pos.X = 10;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_UP))
            {
                pos.Y = 510;
            }
            Raylib.DrawRectangle((int)pos.X, (int)pos.Y, 485, 135, col);
            return pos;
        }
    }
}
