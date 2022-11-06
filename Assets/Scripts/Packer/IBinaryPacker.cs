using UnityEngine;

namespace Packer
{
    /// <summary>
    /// 二维打包器接口
    /// </summary>
    public interface IBinaryPacker
    {
        RectInt Insert(int w, int h);
        void Remove(RectInt pos);
    }
}
