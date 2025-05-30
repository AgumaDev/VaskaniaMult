using UnityEngine;
using DG.Tweening;

public class LightFlickerMainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float timer;
    public float originalTimer;

    void Start()
    {
        timer = originalTimer;
    }

    // Update is called once per frame
    void Update()
    {
        timer = timer - Time.deltaTime;
        if (timer < 0)
        {
            GetComponent<Light>().DOIntensity(Random.Range(2000, 4000), 0.3f);
            timer = originalTimer;
        }
    }
}
