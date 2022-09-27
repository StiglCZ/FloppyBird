using System;
using System.ComponentModel.Design.Serialization;
using GameEngine;
using Sys = System;
namespace Program{

    public class Program {
        public static Engine engine = new Engine();
        public static bool jumpActive = false;
        public static int fps = 1;
        public static bool afterJump = false;
        public static int winX, deadY, deadY2;
        public static bool dead = false;
        public static bool debugmode = false;
        public static int distance = 2;
        public static int space = 3;
        public static void Start() {
            winX = engine.getResolution()[0];
            deadY = engine.getResolution()[1];
            deadY2 = -1;
            deadY--;
            engine.changeColor(ConsoleColor.White, ConsoleColor.DarkGreen);
            engine.setFramerate(fps);
            engine.changeDefaultcolor(ConsoleColor.White, ConsoleColor.Black);
        }

        public static void isdead() {

            engine.Stop();
            Console.WriteLine("You losed, press any key to exit...");
            Console.ReadKey();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void BrickUpdate() {
            if (engine.GameObjects[engine.UpdateScriptRunning].X == engine.GameObjects[0].X &&
            engine.GameObjects[engine.UpdateScriptRunning].Y == engine.GameObjects[0].Y) {

                dead = true;
                engine.changeDefaultcolor(ConsoleColor.White, ConsoleColor.Red);
                engine.changeColor(ConsoleColor.White, ConsoleColor.Red);
            }
          

        }
        public static void Update1() {
            

            engine.setFramerate(fps);

            if (dead) {
                isdead();
            }
            if (jumpActive) {
                if (engine.GameObjects[0].Y == 0) {
                    dead = true;
                }
                else {
                    engine.GameObjects[0].Y--;
                    engine.GameObjects[0].X++;
                    afterJump = true;
                    jumpActive = false;
                }

            } else if (afterJump) {
                engine.GameObjects[0].X++;
                afterJump = false;
            }
            else {
                Move();
            }
            engine.GameObjects[engine.GameObjects.Count - 1].X = engine.GameObjects[0].X;
            engine.GameObjects[engine.GameObjects.Count - 1].Y = engine.GameObjects[0].Y;
            if (engine.GameObjects[0].X == winX) {
                ReStartMap();
            }
            if (engine.GameObjects[0].Y == deadY) {
                engine.changeColor(ConsoleColor.White, ConsoleColor.Red);
                engine.changeDefaultcolor(ConsoleColor.White, ConsoleColor.Red);
                dead = true;
            }

        }
        public static void Move() {
            engine.GameObjects[0].Y++;
            engine.GameObjects[0].X++;
        }
        public static void KeyPressed(char key) {

            if (dead) {

                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Environment.Exit(0);
            }
            else {
                jumpActive = true;
            }
        }
        public static void task() {
            int secs = 0;
            while (true) {
                string x = "";
                if (debugmode) {
                    x += " | Obj: " + engine.GameObjects.Count + " | Dead: " + dead + " | RX:" + engine.getResolution()[0] + " | RY:" + engine.getResolution()[1] + "| X: " + engine.GameObjects[0].X + "| Y: " + engine.GameObjects[0].Y;
                }
                Thread.Sleep(1000);
                engine.addTextBefore("Time:" + secs.ToString() + "s | Level:" + level + " | Fps: " + fps + x);
                secs++;
            }

        }
        public static int level = 0;
        public static void ReStartMap() {
            level++;
            switch (level) {
                case 0:
                    engine.changeColor(ConsoleColor.White, ConsoleColor.Green);
                    engine.changeDefaultcolor(ConsoleColor.White, ConsoleColor.Green); break;
                case 1:
                    space--;
                    engine.changeColor(ConsoleColor.White, ConsoleColor.Blue); fps++;
                    engine.changeDefaultcolor(ConsoleColor.White, ConsoleColor.Blue); break;
                case 2:
                    engine.changeColor(ConsoleColor.Black, ConsoleColor.Yellow); fps++;
                    engine.changeDefaultcolor(ConsoleColor.Black, ConsoleColor.Yellow); break;
                case 3:
                    engine.Stop();
                    Console.Clear(); Console.WriteLine("You won!"); Console.ReadKey();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Environment.Exit(0);
                    break;
            }
            engine.GameObjects.Clear();
            engine.CreateGameObject("Bird", "Floppy bird", 0, 2, '#');
            Script script1 = new Script();
            script1.Start = new Script.Start1(Start);
            script1.Update = new Script.Update1(Update1);
            engine.GameObjects[0].AddScript(script1);

            Script script2 = new Script();
            script2.Start = new Script.Start1(Start);
            script2.Update = new Script.Update1(BrickUpdate);

            //for (int i =0; i < 100; i++){
            Random random = new Random();
            //engine.CreateGameObject("Brick", "Kills bird", random.Next(8,99), random.Next(0,9), 'O');

            for (int j = 5; j < engine.getResolution()[0] - 4; j += distance) {
                int randomgen = random.Next(4, 10);
                int randomgen2 = random.Next(1, 5);
                randomgen = randomgen2 + space;
                for (int i2 = randomgen; i2 < 9; i2++) {
                    engine.CreateGameObject("Brick", "Kills bird", j, i2 + 2, '█');
                    engine.GameObjects[^1].AddScript(script2);
                }
                for (int i2 = randomgen2; i2 > 0; i2--) {
                    engine.CreateGameObject("Brick", "Kills bird", j, i2, '█');
                    engine.GameObjects[^1].AddScript(script2);
                }

            }
        }

        public static void Main(string[] args) {

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--fps" || args[i] == "-f" || args[i] == "--framerate" || args[i] == "fps"){
                    fps = int.Parse(args[i + 1]);
                }
                else if (args[i] == "--dbg" || args[i] == "-d" || args[i] == "--debug" || args[i] == "dbg"){
                    debugmode = true;
                }
                else if (args[i] == "--distance" || args[i] == "-p"){
                    distance = int.Parse(args[i + 1]);
                }
                else if (args[i] == "--spaces" || args[i] == "-i"){
                    space = int.Parse(args[i + 1]);
                }
                else if(args[i] == "--help" || args[i] == "-h"){
                    Console.WriteLine("-i spaces between pipes");
                    Console.WriteLine("-d debug");
                    Console.WriteLine("-f framerate");
                    Console.WriteLine("-p smoothest of pipes");
                    Environment.Exit(0);
                }
            }

        


        engine.setResolution(100, 11);
            engine.debug = true;
            engine.defaultFramerate = fps;
            engine.CreateGameObject("Bird", "Floppy bird", 0, 5, '#');

            Script script1 = new Script();
            script1.Start = new Script.Start1(Start);
            script1.Update = new Script.Update1(Update1);
            engine.GameObjects[0].AddScript(script1);
            
            Script script2 = new Script();
            script2.Start = new Script.Start1(Start);
            script2.Update = new Script.Update1(BrickUpdate);
            
            //for (int i =0; i < 100; i++){
                Random random = new Random();
                //engine.CreateGameObject("Brick", "Kills bird", random.Next(8,99), random.Next(0,9), 'O');

                for(int j = 8; j < engine.getResolution()[0] -2; j+= distance) {
                    int randomgen = random.Next(5, 10);
                    int randomgen2 = random.Next(1, 5);
                    randomgen = randomgen2 + space;
                    for (int i2 = randomgen; i2 < 9; i2++){
                        engine.CreateGameObject("Brick", "Kills bird", j, i2 +2, '█');
                        engine.GameObjects[^1 ].AddScript(script2);
                    }
                    for (int i2 = randomgen2; i2 > 0; i2--){
                    engine.CreateGameObject("Brick", "Kills bird", j, i2 , '█');
                    engine.GameObjects[^1].AddScript(script2);
                    }



            }
            engine.CreateGameObject("copybird", "Renders bird", 0, 5, '#');


            //engine.GameObjects[i +1].AddScript(script2);
            //}
            BackgroundTask bgTask = new BackgroundTask();
            bgTask.backGroundTask = task;
            bgTask.start();
            engine.Init();
            
            while (true){

                char key = Console.ReadKey().KeyChar;
                KeyPressed(key);
            }
        }
    }
}