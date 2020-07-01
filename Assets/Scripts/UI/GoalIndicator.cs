using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoalIndicator : MonoBehaviour
{
    public Transform target;
    public TextMeshProUGUI distText;
    public float hideDist;

    private void Update()
    {
        var dir = target.position - transform.position;

        if (dir.magnitude < hideDist)
        {
            SetChildrenActive(false);
        }
        else
        {
            SetChildrenActive(true);

            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


            distText.text = Vector2.Distance(transform.position, target.position).ToString("F1");
            distText.transform.rotation = Quaternion.AngleAxis(angle - angle, Vector3.forward);
        }
    }

    void SetChildrenActive(bool value)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(value);
        }
    }
}
