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

            Texture2D walterT = Raylib.LoadTexture("walter.png");

            Color border1 = new Color(214, 214, 214, 255);
            Color border2 = new Color(150, 150, 150, 255);
            Color boxyellow = new Color(255, 255, 156, 255);
            Color boxgreen = new Color(187, 255, 156, 255);

            Font pixel = Raylib.LoadFont(@"manaspace.ttf");
            Vector2 textLoc = new Vector2(20f, 520f);


            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();

                Raylib.ClearBackground(border1);
                Raylib.DrawRectangle(10, 10, 980, 480, boxyellow);
                Raylib.DrawRectangle(0, 500, 1000, 300, border2);
                Raylib.DrawRectangle(10, 510, 980, 280, boxgreen);
                Raylib.DrawTextEx(pixel, "WHAT WILL WALTER DO?", textLoc, 52, 0, Color.BLACK);







                Raylib.DrawTexture(walterT, 0, 0, Color.WHITE);




                Raylib.EndDrawing();
            }
        }
    }
}
