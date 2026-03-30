using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMaker : MonoBehaviour
{
    [SerializeField] float noiseRadius = 1f;
    [SerializeField] bool silent = false;
    [SerializeField] bool onlyWhenMoving = false;
    [SerializeField] float frequency = 5f;

    public interface INoiseListener { void OnHeard(NoiseMaker noiseMaker); }

    void Start()
    {
        oldPosition = transform.position;
    }

    public void MakeNoise()
    {
        InternalMakeNoise();
    }

    Vector3 oldPosition;
    float timeSinceLastNoise;
    void Update()
    {

        if (!silent)
        {
            timeSinceLastNoise += Time.deltaTime;
            if (timeSinceLastNoise > 1f / frequency)
            {
                timeSinceLastNoise -= (1f / frequency);
                bool makesSoundAllTheTime = !onlyWhenMoving; // raro el onlyWhenMoving, no es raro...je
                if (makesSoundAllTheTime || (oldPosition != transform.position))
                    InternalMakeNoise();

                oldPosition = transform.position;
            }
        }
    }
    void InternalMakeNoise() // activa externamente a la inferfaz INoiseListenerINoiseListener que esta en enemigo
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, noiseRadius);
        foreach (Collider c in colliders)
        {
            INoiseListener listener = c.GetComponent<INoiseListener>();  // buenisimo, como tiene la interfaz le avisa que haga algodon
            listener?.OnHeard(this);  // manda el objeto que tiene el listener
        }
    }

}
