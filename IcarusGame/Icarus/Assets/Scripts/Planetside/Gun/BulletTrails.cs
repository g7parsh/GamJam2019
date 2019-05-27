using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrails : MonoBehaviour
{
    [SerializeField]
    private GameObject TrailLine = null;
    [SerializeField]
    private float Lifetime = 1.0f;
    [SerializeField]
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnShot(Vector3 End)
    {
        if (TrailLine == null)
        {
            Debug.Log("No line renderer assigned for bullet trail");
            return;
        }
        Vector3 shotDir = End - transform.position;
        Vector3 endPoint = transform.position + shotDir;
        Vector3[] points = { transform.position,
                Vector3.Lerp(transform.position, endPoint, 0.5f),
                endPoint };
        GameObject newTrail = Instantiate(TrailLine);
        LineRenderer lineRender = newTrail.GetComponent<LineRenderer>();
        lineRender.transform.parent = this.transform;
        lineRender.SetPositions(points);
        lineRender.enabled = true;
        StartCoroutine("FadeOut", lineRender);
    }

    private IEnumerator FadeOut(LineRenderer line)
    {
        float t = 0.0f;
        while(t < Lifetime)
        {
            t += Time.deltaTime;
            Color start = line.startColor;
            Color end = line.endColor;
            start.a = Mathf.Min(start.a, 1 - t / Lifetime);
            end.a = Mathf.Min(end.a, 1 - t / Lifetime);
            line.startColor = start;
            line.endColor = end;
            yield return null;
        }
        Destroy(line.gameObject);
    }
}
