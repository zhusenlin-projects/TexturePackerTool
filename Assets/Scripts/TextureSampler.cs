using Packer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(RawImage))]
public class TextureSampler : MonoBehaviour
{
    public Shader shader;
    public string texParamName = "_MainTex";
    public string texSTParamName = "_MainTex_ST";
    public int maxWidth = 265;
    public int maxHeight = 265;

    private RawImage rawImage = null;
    private Material material = null;
    private Material Mat
    {
        get
        {
            if (material == null)
            {
                if (shader == null)
                    Debug.LogError("Texture Sampler Shader Is Null...");
                material = new Material(shader);
            }
            return material;
        }
    }

    private void Awake()
    {
        rawImage = GetComponent<RawImage>();
        rawImage.material = Mat;
    }

    /// <summary>
    /// 更新贴图
    /// </summary>
    /// <param name="tex">贴图</param>
    public void UpdateTexture(Texture tex, TransformInfo subTexRect)
    {
        if(tex == null)
        {
            Debug.Log("Update Texture Is Null");
            return;
        }
        rawImage.texture = tex;
        UpdateTexture(subTexRect);
    }

    //更新贴图
    public void UpdateTexture(TransformInfo subTexRect)
    {
        int targetWidth = subTexRect.width < maxWidth ? subTexRect.width : maxWidth;
        int targetHeight = subTexRect.height < maxHeight ? subTexRect.width : maxHeight;
        rawImage.GetComponent<RectTransform>().sizeDelta = new Vector2(targetWidth, targetHeight);

        //caculate uv
        float tillingx = subTexRect.width / 2048f;
        float tillingy = subTexRect.height / 2048f;
        float offsetx = subTexRect.posx / 2048f;
        float offsety = subTexRect.posy / 2048f;

        Mat.SetVector(texSTParamName, new Vector4(tillingx, tillingy, offsetx, offsety));
    }


}
