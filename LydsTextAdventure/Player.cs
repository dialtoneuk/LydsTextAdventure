using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    public class Player : Entity
    {

        public new Texture texture = new Texture('P');

        public override Texture GetTexture()
        {
            return this.texture;
        }

        public override List<Command> RegisterCommands()
        {

            return new List<Command>(){
                new Command("down", () => {
                    this.position.y++;
                }, "s"),
                new Command("up", () => {
                    this.position.y--;
                }, "w"),
                new Command("left", () => {
                    this.position.x--;
                }, "a"),
                new Command("right", () => {
                    this.position.x++;
                }, "d"),
                new Command("position", () =>
                {
                    Program.DebugLog(this.position.ToString());
                }, "p")
            };
        }
    }
}
