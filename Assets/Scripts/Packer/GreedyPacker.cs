using UnityEngine;

namespace Packer
{
    public class GreedyPacker : IBinaryPacker
    {
        private Rectangle root;
        public GreedyPacker(int w, int h)
        {
            root = new Column(0, 0, w, h);
        }

        public RectInt Insert(int w, int h)
        {
            var rect = root.Insert(w, h);
            if(rect == null)
                return new RectInt(-1, -1, -1, -1);
            return rect.position;
        }

        public void Remove(RectInt pos)
        {
            root.Remove(pos);
        }
    }
}