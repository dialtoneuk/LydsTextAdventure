using System;
using System.Collections.Generic;
using System.Text;

namespace LydsTextAdventure
{
    public class Rectangle
    {

        public int Height
        {
            get { return this._height; }
        }
        private int _height;
        public int Width {
            get { return this._width; }
        } 
        private int _width;

        public Rectangle(int w, int h)
        {

            this._height = h;
            this._width = w;
        } 
    }
}
