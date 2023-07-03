using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootLineController : MonoBehaviour
{
    public Gradient missedCol;
    public Gradient Col;

    public Color start;
    public Color end;
    public void SetupLine(Vector3 startPos, Vector3 endPos, bool isMissed)
    {
        GetComponent<LineRenderer>().SetPosition(0, startPos);
        GetComponent<LineRenderer>().SetPosition(1, endPos);
        GetComponent<LineRenderer>().colorGradient = Col;
        if (isMissed)
        {
            GetComponent<LineRenderer>().colorGradient = missedCol;
        }
        StartCoroutine(DesatroySelf());
    }

    private float fadeStart = 0;
    private float fadeTime = 200;

    private void Update()
    {
        if (fadeStart < fadeTime)
        {
            fadeStart += Time.deltaTime * fadeTime;

            GetComponent<LineRenderer>().startColor = Color.Lerp(start, end, 0);
        }
    }

    IEnumerator DesatroySelf()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
