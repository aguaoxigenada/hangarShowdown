using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Navigation : MonoBehaviour
{
    // Start is called before the first frame update
    public void ChargeScene(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }

    public void ActivePanel(GameObject activateCanvas)
    {
        activateCanvas.SetActive(true);
    }

    public void DesactivePanel(GameObject desactivateCanvas)
    {
        desactivateCanvas.SetActive(false);
    }
}
