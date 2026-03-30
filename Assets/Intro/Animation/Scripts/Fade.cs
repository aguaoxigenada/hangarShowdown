using UnityEngine;

public class Fade : MonoBehaviour
{
    public Animator anim;

    public static Fade instance;

    void Awake()
    {
        instance = this;
    }

    public void FadeOut()
    {
        anim.Play("FadeOut");
     //   Invoke("Disable", 1f);
    }
/*
    private void Disable()
    {
//        this.enabled = false;
    }
*/
}
