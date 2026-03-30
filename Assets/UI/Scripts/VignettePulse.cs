using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignettePulse : MonoBehaviour
{
    public Volume volume;
    Vignette vignette;

    public void DoPulse()
    {
        if (volume.profile.TryGet<Vignette>(out vignette))
        {
            vignette.intensity.value = 0.5f;
        }
        
        Invoke(nameof(PulseNormal), 0.5f);
    }

    private void PulseNormal()
    {
        vignette.intensity.value = 0f;
    }

}

    //https://www.youtube.com/watch?v=r7zg3JhLhI8  gracias brou!
