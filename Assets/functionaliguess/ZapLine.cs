using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZapLine : MonoBehaviour
{
    // Start is called before the first frame update
    public LineRenderer line;

    void Start()
    {
        StartCoroutine(fadeOut(line, 0.75f));
        Destroy(gameObject, 0.75f);
    }

    // Update is called once per frame

    IEnumerator fadeOut(LineRenderer line, float duration)
    {
        float counter = 0;
        Color spriteColor = line.startColor;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, counter / duration);

            spriteColor = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            line.startColor = spriteColor;
            line.endColor = spriteColor;

            yield return null;
        }
    }
}
