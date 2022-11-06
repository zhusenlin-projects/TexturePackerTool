using System.Collections.Generic;
using UnityEngine;

namespace Packer
{
    /// <summary>
    /// 高度固定的矩形(行)
    /// </summary>
    public class Row : Rectangle
    {
        private List<Rectangle> columns;

        public Row(int x, int y, int w, int h) : base(x, y, w, h) { }

        private int widthAvailable
        {
            get
            {
                if(used)
                    return 0;

                if (columns == null)
                    return position.width;

                int width=position.width;
                foreach (var column in columns)
                    width -= column.position.width;
                return width;
            }
        }

        public override Rectangle Insert(int w, int h)
        {
            //已分配，无法分配
            if (used)
                return null;

            //高度不足，无法分配
            if (h > position.height)
                return null;

            //尺寸完全匹配，改为占用状态
            if(columns==null && w == position.width && h == position.height)
            {
                used = true;
                return this;
            }

            if (columns == null)
                columns = new List<Rectangle>();

            //尝试从已有的子列中分配
            foreach (var column in columns)
            {
                var rect=column.Insert(w, h);
                if (rect != null)
                    return rect;
            }

            //宽度超过可用宽度，分配失败
            if (w > widthAvailable)
                return null;

            //生成新列
            var newColumn = new Column(position.x + position.width - widthAvailable, position.y, w, position.height);
            columns.Add(newColumn);

            // 从新列中分配
            return newColumn.Insert(w, h);
        }

        public override bool Remove(RectInt rect)
        {
            if (!Contains(rect))
                return false;

            if (used && position.Equals(rect))
            {
                used = false;
                return true;
            }

            if (columns != null)
            {
                foreach (var column in columns)
                {
                    if (column.Remove(rect))
                        return true;
                }
            }

            return false;
        }
    }
}
