using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using UnityEngine;

namespace Packer
{
    /// <summary>
    /// 图集
    /// </summary>
    public class TextureAtlas
    {
        private IBinaryPacker packer;

        //图集的位图
        public System.Drawing.Bitmap bitmap { get; private set;}
        public System.Drawing.Graphics drawHandle { get; private set; }

        public TextureAtlas(int size, System.Drawing.Imaging.PixelFormat format)
        {
            size = Mathf.Min(size, 2048);

            //初始化图集
            bitmap = new System.Drawing.Bitmap(size, size, format);
            drawHandle=System.Drawing.Graphics.FromImage(bitmap);

            //贪婪打包器
            packer = new GreedyPacker(size,size);
        }

        public void Destroy()
        {
            bitmap?.Dispose();
            drawHandle?.Dispose();
        }

        /// <summary>
        /// 生成新的子贴图
        /// </summary>
        public void NewSubTexture(SubTexture agent, System.Drawing.Image subImg, int width, int height, Action<TextureAtlas, RectInt> onSuccess)
        {
            if (agent == null || subImg == null)
            {
                Debug.LogWarning("NewSubTexture参数错误!");
                return;
            }

            //分配空间
            var rect = packer.Insert(width, height);
            if (rect.x < 0)
                return;

            DrawIntoAtlas(subImg, rect);
            onSuccess?.Invoke(this, rect);
        }

        /// <summary>
        /// 更新子贴图内容
        /// </summary>
        public void UpdateSubTexture(SubTexture agent, System.Drawing.Image subImg, Action onSuccess)
        {
            if (agent == null || agent.textureAtlas != this)
            {
                Debug.LogWarning("UpdateSubTexture参数错误!");
                return;
            }
            DrawIntoAtlas(subImg, agent.position);
            onSuccess?.Invoke();
        }

        /// <summary>
        /// 销毁子贴图
        /// </summary>
        public void ReleaseSubTexture(SubTexture agent)
        {
            packer.Remove(agent.position);
        }


        /// <summary>
        /// 将子图绘制入图集
        /// </summary>
        private void DrawIntoAtlas(System.Drawing.Image img,RectInt rect)
        {
            int width = img.Width;
            int height = img.Height;

            drawHandle.DrawImage(
                img,
                new System.Drawing.Rectangle(rect.x, rect.y, rect.width, rect.height),
                0,
                0,
                width,
                height,
                GraphicsUnit.Pixel
                );

        }

        /// <summary>
        /// 保存图集至磁盘
        /// </summary>
        public void Save(string path)
        {
            string fold = Path.GetDirectoryName(path);
            if (!Directory.Exists(fold))
                Directory.CreateDirectory(fold);
            bitmap.Save(path, ImageFormat.Png);
        }
    }
}
