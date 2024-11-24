using UnityEngine;
using UnityEngine.Video;
public class SimpleVideo : MonoBehaviour
{

    VideoPlayer vp;
    Renderer rend;
    Texture2D cam;
    public RenderTexture rt;

    void Start()
    {
        vp = GetComponent<VideoPlayer>();
        vp.sendFrameReadyEvents = true;
        vp.frameReady += FrameReady;
        rend = GetComponent<Renderer>();
    }

    void FrameReady(VideoPlayer vp, long fn)
    {
        Debug.Log(fn);
    }
}
