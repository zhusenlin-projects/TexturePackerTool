using Packer;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using UnityEngine;

public class TempScript : MonoBehaviour
{
    public TextureSampler ts;

    private string m_TextureFolder= "E:\\UnityProjects\\TexturePacker\\TexturePackage\\";
    private string m_OutputFilePath = "E:\\UnityProjects\\TexturePacker\\Outputs\\res.png";


    private void Start()
    {
        RunCoreDemo();
    }

    private void RunCoreDemo()
    {
        List<string> imgPathList = new List<string>();


        DirectoryInfo dir = new DirectoryInfo(m_TextureFolder);
        FileInfo[] inf = dir.GetFiles();
        foreach(var info in inf)
        {
            if (info.Extension.Equals(".png"))
                imgPathList.Add(info.FullName);
        }

        foreach (string imgPath in imgPathList)
        {
            SubTexture subTex=new SubTexture(imgPath);
            subTex.PackTexture();
            subTex.Destroy();
        }

        TextureAtlasManager.instance.textureAtlas.Save(m_OutputFilePath);
        ts?.UpdateTexture(LoadLocalFileToTexture(m_OutputFilePath),null);
        /*
        int newWidth  = 0;
        int newHeight = 0;


        foreach(var path in imgPathList)
        {
            if (!File.Exists(path))
                Debug.Log($"warning:<{path}>not found");
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            System.Drawing.Image image = Image.FromStream(fs);
            int width = image.Width;
            int height = image.Height;
            newWidth += width;
            if(height>newHeight)
                newHeight = height;
            image.Dispose();
        }
        Debug.Log($"{newWidth},{newHeight}");
        //初始化一张图片
        System.Drawing.Bitmap result=new System.Drawing.Bitmap(newWidth, newHeight,System.Drawing.Imaging.PixelFormat.Format32bppArgb);

        System.Drawing.Graphics newGraphic= System.Drawing.Graphics.FromImage(result);

        int left = 0;
        foreach (var path in imgPathList)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            System.Drawing.Image image = Image.FromStream(fs);
            int width = image.Width;
            int height = image.Height;

            //draw
            newGraphic.DrawImage(
                image,
                new Rectangle(left,0,width,height),
                0,
                0,
                width,
                height,
                GraphicsUnit.Pixel);
            image.Dispose();

            left += width;
        }

        string fold = Path.GetDirectoryName(m_OutputFilePath);
        if(!Directory.Exists(fold))
            Directory.CreateDirectory(fold);

        result.Save(m_OutputFilePath,ImageFormat.Png);
        result.Dispose();
        newGraphic.Dispose();

        ts?.UpdateTexture(LoadLocalFileToTexture(m_OutputFilePath));


        if (File.Exists(m_OutputFilePath))
            Debug.Log("Output Successfully...");
        else
            Debug.Log("Output Failed... Can Not Find Pic...");
        */
    }


    public Texture2D LoadLocalFileToTexture(string path)
    {
        if(!File.Exists(path))
        {
            Debug.LogError($"<{path}> Not Exits...");
            return null;
        }
        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        System.Drawing.Image image = Image.FromStream(fs);
        var buffer = new byte[fs.Length];
        fs.Position = 0;
        fs.Read(buffer, 0, buffer.Length);
        Texture2D texture = new Texture2D(image.Width, image.Height);
        texture.LoadImage(buffer);
        return texture;
    }
}
