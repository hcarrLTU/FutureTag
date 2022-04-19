using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public bool shaking;
    //public Ball ballScript = Ball.GetComponent<Ball>();
    // Start is called before the first frame update
    void Start()
    {
        shaking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (shaking == true)
        {
            StartCoroutine(Shake(0.01f, 0.1f));
            shaking = false;
        }
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        //Debug.Log("Camera shaking");
        Vector3 originalPosition = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            //float y = Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        transform.position = originalPosition;
    }
}
