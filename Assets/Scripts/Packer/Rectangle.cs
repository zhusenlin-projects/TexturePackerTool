using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Packer
{
    public abstract class Rectangle
    {
        public RectInt position;
        public bool    used;

        public Rectangle(int x,int y,int w,int h)
        {
            position = new RectInt(x,y,w,h);
            used = false;
        }
        
        //当前矩形是否包含
        public bool Contains(RectInt rect) => (position.x <= rect.x && position.y <= rect.y && position.xMax >= rect.xMax && position.yMax >= rect.yMax);

        public abstract Rectangle Insert(int w, int h);
        public abstract bool Remove(RectInt rect);

    }
}
