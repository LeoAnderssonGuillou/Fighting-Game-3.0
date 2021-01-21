using System;
using System.Numerics;
using Raylib_cs;

namespace FightingGame3._0_Project
{
    class Program
    {
        class Move      //Class for moves
        {
            public string name = "BONK";
            public int atk = 100;
            public int acc = 90;
            public Sound sfx;
            public Move(string _name, int _atk, int _acc, Sound _sfx)
            {
                name = _name;
                atk = _atk;
                acc = _acc;
                sfx = _sfx;
            }
        }

        class Fighter       //Class for fighters
        {
            public string name = "FIGHTER";
            public int fullHP = 400;
            public Texture2D image;
            public Texture2D thumbnail;
            public Move move1;
            public Move move2;
            public Move move3;
            public Move move4;
            public Fighter(string _name, int _fullHP, Texture2D _image, Texture2D _thumbnail, Move _move1, Move _move2, Move _move3, Move _move4)
            {
                name = _name;
                fullHP = _fullHP;
                image = _image;
                thumbnail = _thumbnail;
                move1 = _move1;
                move2 = _move2;
                move3 = _move3;
                move4 = _move4;
            }
        }

        static void Main(string[] args)
        {
            Raylib.InitWindow(1000, 800, "Walter Battle 2021");     //Initialize Raylib
            Raylib.SetTargetFPS(60);
            Raylib.InitAudioDevice();

            int gameState = 1;

            int[,] fighterGrid = new int[5, 4];     //Create fighter roster and assign values
            for (int x = 0; x < 5; x++)
            {
                fighterGrid[x, 0] = x;
                fighterGrid[x, 1] = x + 5;
                fighterGrid[x, 2] = x + 10;
                fighterGrid[x, 3] = x + 15;
            }
            Vector2 selectPos = new Vector2(0, 0);
            Vector2 lockedSelect = new Vector2(0, 0);
            bool hasSelected = false;

            bool fighting = true;
            bool won = true;

            bool fDamaged = false;      //Keeps track of if a fighter is damaged or dead
            int fBlinkCooldown = 0;
            bool fHide = false;
            int fTimesBlinked = 0;
            int fDying = 0;
            int oDying = 0;

            bool oDamaged = false;      //Handles damage and death animation
            int oBlinkCooldown = 0;
            bool oHide = false;
            int oTimesBlinked = 0;

            int cooldown = 0;           //The cooldown int is used throughout to make sure a real-world key press only causes 1 input
            int coolNormal = 20;
            int coolShort = 10;
            int coolLong = 60;

            int page = 0;               //Miscellaneous variables
            int hideBox = 0;
            int damage = 0;
            int randomMove = 1;
            int randomOpp = 0;
            Random generator = new Random();
            Vector2 select = new Vector2(10, 510);
            int gameMode = 0;

            Texture2D blankThumb = Raylib.LoadTexture("media/blank.png");       //Fighter textures
            Texture2D walterT = Raylib.LoadTexture("media/walter.png");
            Texture2D walterThumb = Raylib.LoadTexture("media/walter-thumb.png");
            Texture2D gorillaT = Raylib.LoadTexture("media/gorilla.png");
            Texture2D gorillaThumb = Raylib.LoadTexture("media/gorilla-thumb.png");
            Texture2D floppaT = Raylib.LoadTexture("media/floppa.png");
            Texture2D floppaThumb = Raylib.LoadTexture("media/floppa-thumb.png");
            Texture2D linusT = Raylib.LoadTexture("media/linus.png");
            Texture2D linusThumb = Raylib.LoadTexture("media/linus-thumb.png");
            Texture2D obamaT = Raylib.LoadTexture("media/obama.png");
            Texture2D obamaThumb = Raylib.LoadTexture("media/obama-thumb.png");
            Texture2D juanT = Raylib.LoadTexture("media/juan.png");
            Texture2D juanThumb = Raylib.LoadTexture("media/juan-thumb.png");
            Texture2D johnporkT = Raylib.LoadTexture("media/johnpork.png");
            Texture2D johnporkThumb = Raylib.LoadTexture("media/johnpork-thumb.png");
            Texture2D grievousT = Raylib.LoadTexture("media/grievous.png");
            Texture2D grievousThumb = Raylib.LoadTexture("media/grievous-thumb.png");
            Texture2D dolphinT = Raylib.LoadTexture("media/dolphin.png");
            Texture2D dolphinThumb = Raylib.LoadTexture("media/dolphin-thumb.png");
            Texture2D israelT = Raylib.LoadTexture("media/israel.png");
            Texture2D israelThumb = Raylib.LoadTexture("media/israel-thumb.png");
            Texture2D yodaT = Raylib.LoadTexture("media/yoda.png");
            Texture2D yodaThumb = Raylib.LoadTexture("media/yoda-thumb.png");

            Sound hit = Raylib.LoadSound("media/hit.ogg");                      //Move sound effects
            Sound heavy = Raylib.LoadSound("media/heavy.ogg");
            Sound heal = Raylib.LoadSound("media/heal.ogg");
            Sound bonk = Raylib.LoadSound("media/bonk.ogg");
            Sound bell = Raylib.LoadSound("media/bell.ogg");
            Sound boom = Raylib.LoadSound("media/boom.ogg");
            Sound chimpevent = Raylib.LoadSound("media/chimpevent.ogg");
            Sound reject = Raylib.LoadSound("media/reject.ogg");
            Sound roast = Raylib.LoadSound("media/roast.ogg");
            Sound getdown = Raylib.LoadSound("media/getdown.ogg");
            Sound cough = Raylib.LoadSound("media/cough.ogg");
            Sound deathsound = Raylib.LoadSound("media/deathsound.ogg");
            Sound dolph = Raylib.LoadSound("media/dolph.ogg");
            Sound sus = Raylib.LoadSound("media/sus.ogg");

            Rectangle fSize = new Rectangle(0, 0, 500, 500);                //Rectangles used for drawing textures
            Rectangle thumbSize = new Rectangle(0, 0, 160, 150);
            Vector2 fLocation = new Vector2(0, 0);
            Rectangle oSize = new Rectangle(0, 0, -500, 500);
            Vector2 oLocation = new Vector2(500, 0);

            Move fMove = new Move("", 0, 0, bonk);                  //Create default fighter, opponent and moves - These are changed based on selected fighters and uses in the battle switch.
            Move oMove = new Move("", 0, 0, bonk);
            Fighter fFighter = new Fighter("", 0, walterT, walterT, fMove, fMove, fMove, fMove);
            Fighter oFighter = new Fighter("", 0, floppaT, floppaT, oMove, oMove, oMove, oMove);
            int fHP = 0;
            int oHP = 0;
            Color fColor = Color.WHITE;
            Color oColor = Color.WHITE;

            Move[] moves = new Move[]{                  //Array of moves
                new Move("BONK", 100, 90, bonk),
                new Move("HIT PAN", 150, 80, bell),
                new Move("CHEESEBORG", 100, 100, heal),
                new Move("WAR CRIME", 200, 60, heavy),

                new Move("MONKE FLIP", 95, 90, hit),
                new Move("SPINNING GORILLA", 145, 80, hit),
                new Move("REJECT HUMANITY", 110, 100, reject),
                new Move("CHIMP EVENT", 250, 50, chimpevent),

                new Move("HISS", 75, 90, hit),
                new Move("ROAST", 125, 80, roast),
                new Move("MELON", 100, 100, heal),
                new Move("MEGA CHONK", 175, 60, heavy),

                new Move("TECH TIP", 100, 100, hit),
                new Move("STARE", 150, 85, boom),
                new Move("RTX ON", 130, 100, reject),
                new Move("DROP", 200, 70, hit),

                new Move("OBAMIUM", 95, 95, bell),
                new Move("LAST NAME", 150, 80, hit),
                new Move("OBAMACARE", 100, 100, heal),
                new Move("THE WORD", 250, 50, getdown),

                new Move("BACKFLIP CRUSH", 115, 90, hit),
                new Move("QUAKE DANCE", 160, 70, boom),
                new Move("'MAN'", 80, 100, heal),
                new Move("DESTROY UNIVERSE", 1000, 10, heavy),

                new Move("BEACH BITE", 125, 80, hit),
                new Move("KINDA SUS NGL", 160, 70, sus),
                new Move("VIBE", 90, 100, reject),
                new Move("EXECUTE", 200, 60, roast),

                new Move("COUGH", 110, 100, cough),
                new Move("GO UP STAIRS", 170, 80, boom),
                new Move("AUTISM RECHARGE", 120, 100, cough),
                new Move("6 MILLION", 250, 60, heavy),

                new Move("TOP SPEED", 110, 100, dolph),
                new Move("TAKE LOAN", 100, 100, heal),
                new Move("DOLPHIN MAN", 160, 75, dolph),

                new Move("DEATH SOUND", 120, 90, deathsound),
                new Move("KETAMINE", 110, 100, heal),
                new Move("CBT", 400, 50, deathsound),
            };

            Fighter[] fighters = new Fighter[20]{        //Array of fighters
                new Fighter("RANDOM", 400, walterT, walterThumb, moves[0], moves[0], moves[2], moves[0]),
                new Fighter("WALTER", 400, walterT, walterThumb, moves[0], moves[1], moves[2], moves[3]),
                new Fighter("BIG FLOPPA", 600, floppaT, floppaThumb, moves[8], moves[9], moves[10], moves[11]),
                new Fighter("LINUS", 350, linusT, linusThumb, moves[12], moves[13], moves[14], moves[15]),
                new Fighter("GORILLA", 400, gorillaT, gorillaThumb, moves[4], moves[5], moves[6], moves[7]),
                new Fighter("OBAMA", 400, obamaT, obamaThumb, moves[16], moves[17], moves[18], moves[19]),
                new Fighter("JUAN", 450, juanT, juanThumb, moves[20], moves[21], moves[22], moves[23]),
                new Fighter("JOHN PORK", 450, johnporkT, johnporkThumb, moves[24], moves[25], moves[26], moves[27]),
                new Fighter("GRIEVOUS", 300, grievousT, grievousThumb, moves[28], moves[29], moves[30], moves[31]),
                new Fighter("DOLPHIN", 400, dolphinT, dolphinThumb, moves[32], moves[0], moves[33], moves[34]),
                new Fighter("YODA", 300, yodaT, yodaThumb, moves[35], moves[19], moves[36], moves[37]),

                new Fighter("LINUS", 400, linusT, blankThumb, moves[12], moves[13], moves[14], moves[15]),  //There needs to be 20 fighters for the game to function.
                new Fighter("LINUS", 400, linusT, blankThumb, moves[12], moves[13], moves[14], moves[15]),  //These placeholders can easily be modified to add new characters.
                new Fighter("LINUS", 400, linusT, blankThumb, moves[12], moves[13], moves[14], moves[15]),
                new Fighter("LINUS", 400, linusT, blankThumb, moves[12], moves[13], moves[14], moves[15]),
                new Fighter("LINUS", 400, linusT, blankThumb, moves[12], moves[13], moves[14], moves[15]),
                new Fighter("LINUS", 400, linusT, blankThumb, moves[12], moves[13], moves[14], moves[15]),
                new Fighter("LINUS", 400, linusT, blankThumb, moves[12], moves[13], moves[14], moves[15]),
                new Fighter("LINUS", 400, linusT, blankThumb, moves[12], moves[13], moves[14], moves[15]),
                new Fighter("LINUS", 400, linusT, blankThumb, moves[12], moves[13], moves[14], moves[15]),
            };
            int playableFighters = 11;  //Current number of finished fighters

            Color bordergrey1 = new Color(214, 214, 214, 255);      //Colors
            Color bordergrey2 = new Color(150, 150, 150, 255);
            Color boxyellow = new Color(255, 255, 156, 255);
            Color boxgreen = new Color(187, 255, 156, 255);
            Color healthgreen = new Color(123, 255, 36, 255);
            Color healthred = new Color(255, 94, 94, 255);
            Color faded = new Color(255, 255, 255, 60);
            Color transparent = new Color(255, 255, 255, 0);
            Color selectcolor = boxgreen;
            Color selectcolor2 = new Color(255, 156, 159, 255);


            while (!Raylib.WindowShouldClose())         //The main loop that runs each frame
            {
                Raylib.BeginDrawing();
                if (gameState == 1)                 //Menu screen
                {
                    Raylib.ClearBackground(bordergrey1);
                    Raylib.DrawRectangle(10, 10, 980, 780, boxyellow);
                    CenteredText("WALTER BATTLE", 1000, 90, 95, 0);
                    CenteredText("2021", 1000, 90, 200, 0);
                    Raylib.DrawRectangle(300, 380, 400, 120, bordergrey1);
                    Raylib.DrawRectangle(310, 390, 380, 100, boxgreen);
                    Raylib.DrawRectangle(300, 570, 400, 120, bordergrey1);
                    Raylib.DrawRectangle(310, 580, 380, 100, boxgreen);

                    if (Raylib.IsKeyDown(KeyboardKey.KEY_UP) && cooldown == 0)
                    {
                        gameMode = 0;
                        cooldown = coolShort;
                    }
                    else if (Raylib.IsKeyDown(KeyboardKey.KEY_DOWN) && cooldown == 0)
                    {
                        gameMode = 1;
                        cooldown = coolShort;
                    }
                    switch (gameMode)
                    {
                        case 0:
                            Raylib.DrawRectangle(310, 390, 380, 100, healthgreen);
                            break;
                        case 1:
                            Raylib.DrawRectangle(310, 580, 380, 100, healthgreen);
                            break;
                    }
                    CenteredText("CAMPAIGN", 400, 54, 413, 300);
                    CenteredText("BATLLE", 400, 54, 603, 300);

                    page = 0;           //Some variables need to be reset when sent back to the main menu after a battle
                    fDying = 0;
                    oDying = 0;
                    selectPos = new Vector2(0, 0);
                    lockedSelect = new Vector2(0, 0);
                    hasSelected = false;
                    fighting = true;
                    fColor = Color.WHITE;
                    oColor = Color.WHITE;
                    selectcolor = boxgreen;

                    if (Raylib.IsKeyDown(KeyboardKey.KEY_ENTER) && cooldown == 0 && gameMode == 1)
                    {
                        gameState = 2;
                        cooldown = coolNormal;
                    }
                }
                if (gameState == 2)             //Fighter select
                {
                    Raylib.ClearBackground(bordergrey1);
                    Raylib.DrawRectangle(10, 10, 980, 780, boxyellow);
                    Raylib.DrawRectangle(97, 147, 806, 606, bordergrey2);
                    Raylib.DrawRectangle(103, 153, 794, 594, Color.WHITE);
                    for (int i = 0; i < 4; i++)                                         //Draw grid
                    {
                        Raylib.DrawRectangle(257 + 160 * i, 150, 6, 600, bordergrey2);
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        Raylib.DrawRectangle(100, 297 + 150 * i, 800, 6, bordergrey2);
                    }

                    if (cooldown <= 0)        //Change selected cell based on input
                    {
                        if (Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT) && selectPos.X < 4) { selectPos.X++; cooldown = coolShort; }
                        if (Raylib.IsKeyDown(KeyboardKey.KEY_DOWN) && selectPos.Y < 3) { selectPos.Y++; cooldown = coolShort; }
                        if (Raylib.IsKeyDown(KeyboardKey.KEY_LEFT) && selectPos.X > 0) { selectPos.X--; cooldown = coolShort; }
                        if (Raylib.IsKeyDown(KeyboardKey.KEY_UP) && selectPos.Y > 0) { selectPos.Y--; cooldown = coolShort; }
                    }

                    //Makes only the finished fighters selectable
                    if (fighterGrid[(int)selectPos.X, (int)selectPos.Y] > playableFighters - 1)
                    {
                        selectPos.Y--;
                    }

                    Raylib.DrawRectangle(97 + 160 * (int)selectPos.X, 147 + 150 * (int)selectPos.Y, 166, 156, selectcolor);     //Selected box frame
                    Raylib.DrawRectangle(103 + 160 * (int)selectPos.X, 153 + 150 * (int)selectPos.Y, 154, 144, Color.WHITE);    //Selected box background

                    //This part of gameState 2 is dictated by if the player has selected their fighter or not.
                    switch (hasSelected)
                    {
                        case false:     //If they haven't, and press enter, a fighter is selected and its attributes applied.
                            CenteredText("SELECT YOUR FIGHTER", 1000, 70, 54, 0);
                            if (Raylib.IsKeyDown(KeyboardKey.KEY_ENTER) && cooldown == 0)
                            {
                                if (selectPos.X == 0 && selectPos.Y == 0)       //If RANDOM FIGHTER is selected
                                {
                                    fFighter = fighters[generator.Next(1, 11)];
                                }
                                else
                                {
                                    fFighter = fighters[fighterGrid[(int)selectPos.X, (int)selectPos.Y]];
                                }
                                fHP = fFighter.fullHP;      //Apply attributes of selected fighter
                                cooldown = coolNormal;
                                lockedSelect = selectPos;
                                selectPos = new Vector2(0, 0);
                                selectcolor = selectcolor2;
                                hasSelected = true;


                                if (gameMode == 0)      //Game progression proceeds depending on if BATTLE or CAMPAIGN mode is active.
                                {
                                    gameState = 3;
                                }
                            }
                            break;
                        case true:    //If the player has chosen a figther, they may now chose the opponent. If they do (by pressing enter), the game moves on to gameState 3.  
                            CenteredText("SELECT YOUR OPPONENT", 1000, 70, 54, 0);
                            Raylib.DrawRectangle(103 + 160 * (int)lockedSelect.X, 153 + 150 * (int)lockedSelect.Y, 154, 144, boxgreen);

                            if (Raylib.IsKeyDown(KeyboardKey.KEY_ENTER) && cooldown == 0)
                            {
                                if (selectPos.X == 0 && selectPos.Y == 0)       //If RANDOM OPPONENT is selected
                                {
                                    randomOpp = generator.Next(1, 10);      //Makes sure randomized opponent is not the same as player's fighter
                                    if (randomOpp >= fighterGrid[(int)lockedSelect.X, (int)lockedSelect.Y]) { randomOpp++; }
                                    oFighter = fighters[randomOpp];

                                }
                                else
                                {
                                    oFighter = fighters[fighterGrid[(int)selectPos.X, (int)selectPos.Y]];
                                }
                                oHP = oFighter.fullHP;
                                cooldown = coolNormal;
                                gameState = 3;
                            }
                            break;
                    }

                    for (int x = 0; x < 5; x++)         //Draw fighter thumbnails
                    {
                        Raylib.DrawTextureRec(fighters[x].thumbnail, thumbSize, new Vector2(100 + 160 * x, 150), Color.WHITE);
                        Raylib.DrawTextureRec(fighters[x + 5].thumbnail, thumbSize, new Vector2(100 + 160 * x, 300), Color.WHITE);
                        Raylib.DrawTextureRec(fighters[x + 10].thumbnail, thumbSize, new Vector2(100 + 160 * x, 450), Color.WHITE);
                        Raylib.DrawTextureRec(fighters[x + 15].thumbnail, thumbSize, new Vector2(100 + 160 * x, 600), Color.WHITE);
                    }
                    Raylib.DrawRectangle(103, 153, 154, 144, Color.WHITE);
                    CenteredText("?", 160, 85, 185, 100);
                }

                if (gameState == 3)                     //Announces "[FIGHTER] VS [OPPONENT]"
                {
                    Raylib.ClearBackground(bordergrey1);
                    Raylib.DrawRectangle(10, 10, 980, 780, boxyellow);

                    //Here, the page number can't be controlled by the player. It is instead controlled by time, using the cooldown variable.
                    switch (page)
                    {
                        case 0:
                            Raylib.DrawTextureRec(fFighter.image, fSize, fLocation, fColor);
                            CenteredText(fFighter.name, 500, 52, 500, 0);
                            Raylib.PlaySound(boom);
                            cooldown = 0;
                            break;
                        case 1:
                            Raylib.DrawTextureRec(fFighter.image, fSize, fLocation, fColor);
                            CenteredText(fFighter.name, 500, 52, 500, 0);
                            break;
                        case 2:
                            Raylib.DrawTextureRec(fFighter.image, fSize, fLocation, fColor);
                            CenteredText(fFighter.name, 500, 52, 500, 0);
                            CenteredText("VS", 1000, 52, 250, 0);
                            break;
                        case 3:
                            Raylib.DrawTextureRec(fFighter.image, fSize, fLocation, fColor);
                            CenteredText(fFighter.name, 500, 52, 500, 0);
                            CenteredText("VS", 1000, 52, 250, 0);
                            Raylib.DrawTextureRec(oFighter.image, oSize, oLocation, oColor);
                            CenteredText(oFighter.name, 500, 52, 500, 500);
                            break;
                        case 4:
                            gameState = 4;
                            page = 1;
                            cooldown = 20;
                            hideBox = 980;
                            break;
                    }

                    if (cooldown == 0)
                    {
                        page++;
                        if (page != 4)
                        {
                            Raylib.PlaySound(boom);
                            cooldown = 60;
                            if (page == 3)
                            {
                                cooldown = 180;
                            }
                        }
                    }
                }


                if (gameState == 4)         //The actual battle
                {
                    Raylib.ClearBackground(bordergrey1);                    //Background
                    Raylib.DrawRectangle(10, 10, 980, 480, boxyellow);
                    Raylib.DrawRectangle(0, 500, 1000, 300, bordergrey2);
                    Raylib.DrawRectangle(10, 510, 980, 280, boxgreen);

                    Raylib.DrawRectangle(55, 20, 400, 20, healthred);     //Player health bar
                    Raylib.DrawRectangle(55, 20, fHP * 400 / fFighter.fullHP, 20, healthgreen);

                    Raylib.DrawRectangle(555, 20, 400, 20, healthred);     //Opponent health bar
                    Raylib.DrawRectangle(555, 20, oHP * 400 / oFighter.fullHP, 20, healthgreen);

                    Raylib.DrawTextureRec(fFighter.image, fSize, fLocation, fColor);   //Draw figthers
                    Raylib.DrawTextureRec(oFighter.image, oSize, oLocation, oColor);

                    if (fighting == true)       //As long as fighting = true, the game will not move on from letting the fighters take turns attacking.
                    {
                        //Using this switch, the page number dictates what part of the round is happening.
                        switch (page)
                        {
                            case 1:
                                Text("WHAT WILL " + fFighter.name + " DO?");
                                break;
                            case 2:                                                    //Select move
                                hideBox = 0;
                                Raylib.DrawRectangle(10, 645, 980, 10, bordergrey2);
                                Raylib.DrawRectangle(495, 510, 10, 280, bordergrey2);
                                select = Select(select, boxyellow);
                                CenteredText(fFighter.move1.name, 500, 48, 550, 0);
                                CenteredText(fFighter.move2.name, 500, 48, 550, 500);
                                CenteredText(fFighter.move3.name, 500, 48, 700, 0);

                                if (fFighter.move4.name == "DESTROY UNIVERSE")
                                {
                                    CenteredText("DESTROY", 500, 48, 676, 500);
                                    CenteredText("UNIVERSE", 500, 48, 724, 500);
                                }
                                else
                                {
                                    CenteredText(fFighter.move4.name, 500, 48, 700, 500);
                                }
                                break;
                            case 3:                                      //Tell what move is being used
                                if (select.X == 10 && select.Y == 510)
                                {
                                    fMove = fFighter.move1;
                                }
                                else if (select.X == 505 && select.Y == 510)
                                {
                                    fMove = fFighter.move2;
                                }
                                else if (select.X == 10 && select.Y == 655)
                                {
                                    fMove = fFighter.move3;
                                }
                                else
                                {
                                    fMove = fFighter.move4;
                                }
                                MoveText(fFighter.name, fMove.name);
                                break;
                            case 4:                                      //Calculate result, randomize opponents next move
                                if (fMove == fFighter.move3)             //and play sfx. Happnes exactly once per round.
                                {
                                    fHP = fHP + fMove.atk;
                                    damage = 1;
                                    if (fHP > fFighter.fullHP)
                                    {
                                        fHP = fFighter.fullHP;
                                    }
                                }
                                else
                                {
                                    damage = Attack(generator, fMove.atk, fMove.acc);
                                    oHP = oHP - damage;
                                }
                                MoveText(fFighter.name, fMove.name);
                                randomMove = generator.Next(1, 8);
                                if (damage > 0)
                                {
                                    Raylib.PlaySound(fMove.sfx);
                                    if (fMove != fFighter.move3)
                                    {
                                        oDamaged = true;
                                    }
                                }
                                page++;
                                break;
                            case 5:                                      //Tell result
                                if (fMove == fFighter.move3 && fHP == fFighter.fullHP)
                                {
                                    Text("HP FULLY RESTORED!");
                                }
                                else if (fMove == fFighter.move3)
                                {
                                    Text(fMove.atk + " HP RESTORED!");
                                }
                                else if (damage > 0)
                                {
                                    Text(damage + " DAMAGE!");
                                }
                                else
                                {
                                    Text("MISS!");
                                }
                                break;
                            case 6:                                      //Check if won / Tell opponent uses move
                                if (oHP <= 0)
                                {
                                    fighting = false;
                                    won = true;
                                    oDying = 255;
                                    cooldown = coolLong;
                                    page = 1;
                                    break;
                                }
                                if (randomMove == 1 | randomMove == 2)
                                {
                                    oMove = oFighter.move1;
                                }
                                else if (randomMove == 3 | randomMove == 4)
                                {
                                    oMove = oFighter.move2;
                                }
                                else if (randomMove == 5 | randomMove == 6)
                                {
                                    oMove = oFighter.move4;
                                }
                                else
                                {
                                    oMove = oFighter.move3;
                                }
                                MoveText(oFighter.name, oMove.name);
                                break;
                            case 7:                                       //Calculate result of opponents move and play sfx.
                                if (oMove == oFighter.move3)              //Happnes exactly once per round.
                                {
                                    oHP = oHP + oMove.atk;
                                    damage = 1;
                                    if (oHP > oFighter.fullHP)
                                    {
                                        oHP = oFighter.fullHP;
                                    }
                                }
                                else
                                {
                                    damage = Attack(generator, oMove.atk, oMove.acc);
                                    fHP = fHP - damage;
                                }
                                MoveText(oFighter.name, oMove.name);
                                if (damage > 0)
                                {
                                    Raylib.PlaySound(oMove.sfx);
                                    if (oMove != oFighter.move3)
                                    {
                                        fDamaged = true;
                                    }
                                }
                                page++;
                                break;
                            case 8:                                       //Tell result of opponents move
                                if (oMove == oFighter.move3 && oHP == oFighter.fullHP)
                                {
                                    Text("HP FULLY RESTORED!");
                                }
                                else if (oMove == oFighter.move3)
                                {
                                    Text(oMove.atk + " HP RESTORED!");
                                }
                                else if (damage > 0)
                                {
                                    Text(damage + " DAMAGE!");
                                }
                                else
                                {
                                    Text("MISS!");
                                }
                                break;
                            case 9:                                       //Check if lost / Next round (by setting page to 1)
                                if (fHP <= 0)
                                {
                                    fighting = false;
                                    won = false;
                                    fDying = 255;
                                    cooldown = coolLong;
                                    page = 1;
                                    break;
                                }
                                page = 1;
                                break;
                        }
                    }
                    //The following code runs in gameState 4 when the player has won or lost.
                    else
                    {                               //Tell who won
                        switch (won)
                        {
                            case true:
                                Text(fFighter.name + " WINS!");
                                break;
                            case false:
                                Text(oFighter.name + " WINS!");
                                break;
                        }
                        if (page > 1)
                        {
                            gameState = 1;
                        }
                    }

                    //Handles the death/fade animation of a fighter when it dies
                    if (fDying > 0)
                    {
                        fDying -= 12;
                        if (fDying < 0) { fDying = 0; }
                        fColor = new Color(255, 255, 255, fDying);
                    }
                    if (oDying > 0)
                    {
                        oDying -= 12;
                        if (oDying < 0) { oDying = 0; }
                        oColor = new Color(255, 255, 255, oDying);
                    }

                    //Handles blinking/damage animation of player's fighter
                    if (fDamaged == true)
                    {
                        if (fBlinkCooldown <= 0)
                        {
                            fHide = !fHide;
                            fBlinkCooldown = 3;
                            fTimesBlinked++;
                        }
                        if (fHide == true) { fColor = faded; }
                        else { fColor = Color.WHITE; }
                        fBlinkCooldown--;
                        if (fTimesBlinked == 6)
                        {
                            fTimesBlinked = 0;
                            fDamaged = false;
                        }
                    }
                    //Handles blinking/damage animation of opponent
                    if (oDamaged == true)
                    {
                        if (oBlinkCooldown <= 0)
                        {
                            oHide = !oHide;
                            oBlinkCooldown = 3;
                            oTimesBlinked++;
                        }
                        if (oHide == true) { oColor = faded; }
                        else { oColor = Color.WHITE; }
                        oBlinkCooldown--;
                        if (oTimesBlinked == 6)
                        {
                            oTimesBlinked = 0;
                            oDamaged = false;
                        }
                    }

                    if (Raylib.IsKeyDown(KeyboardKey.KEY_ENTER) && cooldown == 0)   //When enter is pressed:
                    {                                                               //-Go to next page
                        page++;                                                     //-Make sure it doesn't instantly happen again
                        cooldown = coolNormal;                                      //-Create illusion of text being written out character by character using hideBox
                        hideBox = 980;
                    }
                    if (hideBox > 0)
                    {
                        hideBox = hideBox - 45;
                    }
                    Raylib.DrawRectangle(10 + 980 - hideBox, 510, hideBox, 280, boxgreen);
                }

                if (cooldown > 0)   //Here's where the cooldown int gets closer to 0 every frame
                {
                    cooldown--;
                }
                Raylib.EndDrawing();
            }
        }

        //Writes out text in the textbox
        static void Text(string text)
        {
            Raylib.DrawText(text, 20, 520, 48, Color.BLACK);
        }
        //Writes out what move is being used in the textbox
        static void MoveText(string name, string move)
        {
            Raylib.DrawText(name + " USES " + move + "!", 20, 520, 48, Color.BLACK);
        }

        //Allows text to easely be drawn centered within a given box (x-axis only)
        static void CenteredText(string text, int fullWidth, int fontSize, int yPos, int xStart)
        {
            Raylib.DrawText(text, xStart + (fullWidth - Raylib.MeasureText(text, fontSize)) / 2, yPos, fontSize, Color.BLACK);
        }

        //Generates damage/result based on properties of active move
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

        //Handles the player's selecting a move
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

        //Changes fullHP and attack power of a fighter using a given float (i.e. 1.25f => 25% power increase)
        static Fighter ChangePower(Fighter fighter, float power)
        {
            fighter.fullHP = (int)(fighter.fullHP * power);
            fighter.move1.atk = (int)(fighter.move1.atk * power);
            fighter.move2.atk = (int)(fighter.move2.atk * power);
            fighter.move3.atk = (int)(fighter.move3.atk * power);
            fighter.move4.atk = (int)(fighter.move4.atk * power);
            return fighter;
        }
    }
}
