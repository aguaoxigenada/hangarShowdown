using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encuentro : MonoBehaviour
{
    [SerializeField] Transform limits;
    Oleada[] oleadas;
    int currentOleada = -1; // era -1
    public bool wantToUseLimits;
    void Awake()
    {
        oleadas = GetComponentsInChildren<Oleada>();
    }

    void Start()
    {
       // oleadas[currentOleada].ActivateAllEnemies();
        // foreach (Oleada o in oleadas)
        // {
        //     o.DeactivateAllEnemies();
        // }
        // if (wantToUseLimits)
        //     limits.gameObject.SetActive(false);
        currentOleada = 0;
    }

    void Update()
    {
        if ((currentOleada >= 0) && currentOleada < oleadas.Length)
        {
           //Debug.Log(oleadas[currentOleada].AreAllEnemiesDead());

            if (oleadas[currentOleada].AreAllEnemiesDead())
            {
                Debug.Log("Entranding");
                currentOleada++;
                if (currentOleada < oleadas.Length)
                {
                    oleadas[currentOleada].ActivateAllEnemies();
                }
                else
                {
                   // Debug.Log("Por Aca");
                   GameManager.instance.CheckIfWon(true);
                    // Destruir los limites cuando se acabe la oleada
                    if (wantToUseLimits)
                        limits.gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other) // No lo voy a usar para la entrega
    {
        if (other.CompareTag("Player") && currentOleada < 0)
        {
            currentOleada = 0;
            oleadas[currentOleada].ActivateAllEnemies();
            // Mostrar los limites
            if (wantToUseLimits)
                limits.gameObject.SetActive(true);
        }
    }
}
