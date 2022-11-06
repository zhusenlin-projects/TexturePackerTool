using Packer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class CurrentSubTextureLog : MonoBehaviour
{
    public TextureSampler sampler;
    public Text log;
    public Text subTextureFilePath;
    private const string format = "name:{0}\n<color=#00ff00ff>tilling:{1}\noffset:{2}</color>\n\nposx:{3}\nposy:{4}\nwidth:{5}\nheight:{6}\n\natlas:{7}\n";

    public void UpdateLog(ResInfo info,string atlasPath)
    {
        Vector4 st=sampler.GetComponent<RawImage>().material.GetVector(sampler.texSTParamName);
        log.text=string.Format(format, info.filename, $"{st.x}, {st.y}", $"{st.z}, {st.w}",info.transformInfo.posx, info.transformInfo.posy, info.transformInfo.width, info.transformInfo.height, atlasPath);
        subTextureFilePath.text = $"¡ø {info.filepath}";
    }
}
