using System;
using System.IO;
using UnityEngine;

namespace Packer
{
    /// <summary>
    /// 图集子贴图
    /// </summary>
    public class SubTexture
    {
        /// <summary>
        /// 所属图集对象
        /// </summary>
        public TextureAtlas textureAtlas { get; private set; }

        /// <summary>
        /// 子贴图坐标数据
        /// </summary>
        public RectInt position { get; private set; }

        public System.Drawing.Image img { get; private set; }

        public SubTexture(string imgPath)
        {
            if (!File.Exists(imgPath))
                Debug.Log($"warning:<{imgPath}>not found");
            if (img != null)
                Destroy();

            FileStream fs = new FileStream(imgPath, FileMode.Open, FileAccess.Read);
            img = System.Drawing.Image.FromStream(fs);
        }

        public void Destroy()
        {
            if(img != null)
                img.Dispose();
            img = null;
        }


        /// <summary>
        /// 子图集的打包
        /// </summary>
        public void PackTexture(Action onSuccess = null)
        {
            if (img == null)
                return;

            int width= img.Width;
            int height= img.Height;

            // 子贴图已存在，但与新贴图尺寸不一致，需要销毁旧贴图
            if (textureAtlas!=null && (position.width != width || position.height != height))
            {
                textureAtlas?.ReleaseSubTexture(this);
                textureAtlas = null;
            }

            if(textureAtlas!=null)
            {
                //更新贴图内容
                textureAtlas.UpdateSubTexture(this, img, ()=>
                {
                    onSuccess?.Invoke();
                });
            }
            else
            {
                //生成新的子贴图
                TextureAtlasManager.instance.NewSubTexture(this, img, width, height, (atlas, pos) =>
                {
                    textureAtlas = atlas;
                    position = pos;
                    onSuccess?.Invoke();
                });
            }
        }

    }
}