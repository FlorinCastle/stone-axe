using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubbleTrackObject : MonoBehaviour
{
    [SerializeField] private GameObject chatty;
    private GameObject obj;

    Camera mCamera;
    private RectTransform rt;
    Vector2 pos;

    private bool beingHandled;

    private Coroutine _chat;

    private void Start()
    {
        mCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rt = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        if (obj)
        {
            pos = RectTransformUtility.WorldToScreenPoint(mCamera, obj.transform.position);
            rt.position = pos;
        }
        else
            Debug.LogError(gameObject.name + ": No Object Attached to (TrackObject)");
    }

    public void chat()
    {
        if (!beingHandled)
            _chat = StartCoroutine(Message());
    }

    private IEnumerator Message()
    {
        beingHandled = true;
        chatty.SetActive(true);

        yield return new WaitForSeconds(3f);

        chatty.SetActive(false);

        beingHandled = false;
    }


    public GameObject ObjRef { set => obj = value; }
}
