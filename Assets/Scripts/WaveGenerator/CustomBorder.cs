using NUnit.Framework;
using UnityEngine;

public class CustomBorder : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    private int prevLength;

    [SerializeField] private bool CalcPoint;
    private bool didCalc;

    private Vector3 point;
    public Vector3 GetPoint()
    {
        //initial point min
        int rand = Random.Range(0, points.Length);
        Vector3 a = points[rand].position;

        //Debug.Log($"a {points[rand].name}");

        int choice= Random.Range(1, 3);

        //Second point Max
        Vector3 b;

        if (choice == 1)
        {
            rand++;

            if (rand >= points.Length)
                rand = 0;
        }
        else
        {
            rand--;

            if (rand < 0)
                rand = points.Length - 1;
        }

        b = points[rand].position;

        //Debug.Log($"b {points[rand].name}");

        float lerp = Random.Range(0.0f, 1.0f);

        Vector3 pos = Vector3.Lerp(a, b, lerp);
        return pos;
    }

    private void OnDrawGizmos()
    {
        if (points == null)
            return;

        if (points.Length == 0)
            return;

        if (points[0] == null)
            return;

        Vector3 prev = points[0].position;
        for (int i = 0; i < points.Length; i++)
        {
            Gizmos.color = Color.orange;
            Gizmos.DrawWireSphere(points[i].position, .3f);
            Gizmos.color = Color.green;

            if (i == 0)
                continue;

            Gizmos.DrawLine(points[i].position,prev);
            
            if (i == points.Length-1)
            {
                Gizmos.DrawLine(points[i].position, points[0].position);
                continue;
            }
            
            prev = points[i].position;
        }

        if (!didCalc)
        {
            if (CalcPoint)
            {
                point = GetPoint();
                didCalc = true;
            }
        }
        else
        {
            if (!CalcPoint)
                didCalc = false;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(point, 0.2f);
    }
}
