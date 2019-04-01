using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class VhsShader : MonoBehaviour
{
    public Material shaderMaterial;
    [Range(0, 5)]
    public int downSampling = 0;
    public bool applyVsh = true;
    // Start is called before the first frame update
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        int width = source.width >> downSampling;
        int height = source.height >> downSampling;
        RenderTexture rt = RenderTexture.GetTemporary(width, height);
        if(applyVsh)
            Graphics.Blit(source, rt, shaderMaterial);
        else
            Graphics.Blit(source, rt);
        Graphics.Blit(rt, destination);
        RenderTexture.ReleaseTemporary(rt);
    }
}
