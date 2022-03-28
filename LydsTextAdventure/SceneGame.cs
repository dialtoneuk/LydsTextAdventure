﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LydsTextAdventure
{
    class SceneGame : Scene
    {

        protected WorldChunks world;
        protected Player player;
        protected InputManager inputManager = new InputManager();
        protected Camera camera;

        public SceneGame(string name, List<Command> commands = null) : base(name, commands) { }

        protected override List<Command> LoadCommands()
        {

            List<Command> commands = new List<Command>();

            commands.AddRange(inputManager.GetInteractionCommands());
            commands.AddRange(inputManager.GetMovementCommands());

#if DEBUG
            Program.DebugLog("added cheat commands");
            commands.AddRange(inputManager.GetCheatCommands());
#endif

            return commands;
        }

        public override void Before()
        {

            this.player = new Player();
            this.player.isSolid = false;

            //link input manager
            inputManager.SetPlayer(this.player);

            this.world = new WorldChunks(4, 4);
            this.world.GenerateWorld();

            for (int i = 0; i < 10; i++)
            {

                EntityMoving m = new EntityMoving();
                m.position.y = 10 * i;
                m.position.x = 10 + i;
                m.SetSpeed(i);
            }

            this.camera = new Camera(this.player, Camera.Perspective.CENTER_ON_OWNER);
            this.camera.SetMainCamera(true);
            this.camera.SetSize((Buffer.WindowWidth - 48), 41);
            this.camera.SetName("Main Camera");
            this.camera.position.x = 0;
            this.camera.position.y = 0;

            WindowPlayerStatistics stats = new WindowPlayerStatistics();
            stats.SetPlayer(this.player);
            stats.SetPosition((Buffer.WindowWidth - 48), 0);

            WindowInventory inventory = new WindowInventory();
            inventory.SetPlayer(this.player);
            inventory.SetPosition((Buffer.WindowWidth - 48), 9);
            inventory.SetSize(40, 52);

            WindowConsole console = new WindowConsole();
            console.SetSize((Buffer.WindowWidth - 48) / 2, 20);
            console.SetPosition(0, 41);

            base.Before();
        }

        public override void Update()
        {

            //create any new chunks around the player they haven't seen yet
            this.world.CreateChunksAroundPlayer(this.player, 4);

            //render world and entities using this camera
            this.camera.UpdateBuffer();

            //then do the base updates
            base.Update();
        }


        public override void Start()
        {

            InputController.IsTextInput = false;
            InputController.IsAwaitingInput = true;

            //set the players spawn position
            this.player.position.SetPosition(this.world.GetInitialSpawnPoint());

            //update spawn chunks
            this.world.UpdateChunks();

            base.Start();
        }
    }
}
