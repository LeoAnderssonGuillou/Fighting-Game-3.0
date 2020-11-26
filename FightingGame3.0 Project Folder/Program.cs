using System;
using System.Numerics;
using Raylib_cs;

namespace FightingGame3._0_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            Raylib.InitWindow(1000, 800, "Grafiktest");

            bool fighting = true;
            bool won = true;

            int battlePage = 1;
            int cooldown = 0;

            string fName = "WALTER";
            int fFullHP = 400;
            int fHP = 400;
            int fAttack = 100;
            string fMove1 = "BONK";

            string oName = "GORILLA";
            int oFullHP = 400;
            int oHP = 400;
            int oAttack = 100;
            string oMove1 = "MONKE FLIP";


            Vector2 select = new Vector2(10, 510);
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
                            Text("WHAT WILL " + fName + " DO?");
                            break;
                        case 2:                                                    //Select move
                            Raylib.DrawRectangle(10, 645, 980, 10, border2);
                            Raylib.DrawRectangle(495, 510, 10, 280, border2);
                            select = Select(select, boxyellow);
                            Raylib.DrawText("BONK", 20, 550, 48, Color.BLACK);
                            Raylib.DrawText("WAR CRIME", 520, 550, 48, Color.BLACK);
                            break;
                        case 3:                                      //Do damage
                            MoveText(fName, fMove1);
                            oHP = oHP - fAttack;
                            battlePage++;
                            break;
                        case 4:                                      //Used move
                            MoveText(fName, fMove1);
                            break;
                        case 5:                                      //Check if won / Opponent does damage
                            if (oHP <= 0)
                            {
                                fighting = false;
                                won = true;
                                break;
                            }
                            MoveText(oName, oMove1);
                            fHP = fHP - oAttack;
                            battlePage++;
                            break;
                        case 6:                                       //Opponent used move
                            MoveText(oName, oMove1);
                            break;
                        case 7:                                       //Check if lost / Next round
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


                Raylib.DrawTexture(walterT, 0, 0, Color.WHITE);
                Raylib.DrawTexture(walterT, 500, 0, Color.WHITE);

                Raylib.EndDrawing();
            }
        }
        static void Text(string text)
        {
            Raylib.DrawText(text, 20, 520, 48, Color.BLACK);
        }
        static void MoveText(string name, string move)
        {
            Raylib.DrawText(name + " USES " + move, 20, 520, 48, Color.BLACK);
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
