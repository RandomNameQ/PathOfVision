using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathOfVision
{
    public class ScreenArea
    {
        public int height;
        public int width;
        public int startX; // Начальная координата X
        public int startY; // Начальная координата Y

        public ScreenArea(int width, int height, int startX, int startY)
        {
            this.width = width;
            this.height = height;
            this.startX = startX;
            this.startY = startY;
        }
    }
}
