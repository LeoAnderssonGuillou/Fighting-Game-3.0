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

            int battlePage = 1;
            int cooldown = 0;

            Vector2 select = new Vector2(10, 510);

            Texture2D walterT = Raylib.LoadTexture("walter.png");
            Color border1 = new Color(214, 214, 214, 255);
            Color border2 = new Color(150, 150, 150, 255);
            Color boxyellow = new Color(255, 255, 156, 255);
            Color boxgreen = new Color(187, 255, 156, 255);


            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(border1);
                Raylib.DrawRectangle(10, 10, 980, 480, boxyellow);
                Raylib.DrawRectangle(0, 500, 1000, 300, border2);
                Raylib.DrawRectangle(10, 510, 980, 280, boxgreen);


                switch (battlePage)
                {
                    case 1:
                        Text("WHAT WILL WALTER DO?");
                        break;
                    case 2:
                        Raylib.DrawRectangle(10, 645, 980, 10, border2);
                        Raylib.DrawRectangle(495, 510, 10, 280, border2);
                        select = Select(select, boxyellow);
                        break;
                    case 3:


                        break;
                }



                if (Raylib.IsKeyDown(KeyboardKey.KEY_ENTER) && cooldown == 0)
                {
                    battlePage++;
                    cooldown = 480;
                }
                if (cooldown > 0)
                {
                    cooldown--;
                }


                Raylib.DrawTexture(walterT, 0, 0, Color.WHITE);




                Raylib.EndDrawing();
            }
        }
        static void Text(string text)
        {
            Raylib.DrawText(text, 20, 520, 48, Color.BLACK);
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
