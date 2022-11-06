

using System.Collections.Generic;
using UnityEngine;

namespace Packer
{
    public class Column : Rectangle
    {
        private List<Rectangle> rows;

        public Column(int x, int y, int w, int h) : base(x, y, w, h) { }

        private int heightAvailable
        {
            get
            {
                if (used)
                    return 0;

                if (rows == null)
                    return position.height;

                int height = position.height;
                foreach (var row in rows)
                    height -= row.position.height;

                return height;
            }
        }

        public override Rectangle Insert(int w, int h)
        {
            //已被占用，无法分配
            if (used)
                return null;

            //宽度不足，无法分配
            if (w > position.width)
                return null;

            // 尺寸完全匹配，改为占用状态
            if (rows == null && position.width == w && position.height == h)
            {
                used = true;
                return this;
            }

            if (rows == null)
                rows = new List<Rectangle>();

            // 尝试从已有子行中分配
            foreach (var row in rows)
            {
                var rect = row.Insert(w, h);
                if (rect != null)
                    return rect;
            }

            // 高度超过可用高度，分配失败
            if (h > heightAvailable)
                return null;

            //生成新行
            var newRow = new Row(position.x, position.y + position.height - heightAvailable, position.width, h);
            rows.Add(newRow);
            rows.Sort((lhs, rhs) => { return lhs.position.height.CompareTo(rhs.position.height); });

            //从新行中分配
            return newRow.Insert(w, h);
        }

        public override bool Remove(RectInt rect)
        {
            //不包含此rect
            if (!Contains(rect))
                return false;

            //整张占图
            if(used && position.Equals(rect))
            {
                used = false;
                return true;
            }

            if(rows!=null)
            {
                foreach(var row in rows)
                {
                    if (row.Remove(rect))
                        return true;
                }
            }
            return false;
        }
    }
}
