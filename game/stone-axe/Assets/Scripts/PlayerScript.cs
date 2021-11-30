using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private GameObject _headMark;

    public void setupHead(GameObject playerHead)
    {
        Instantiate(playerHead, _headMark.transform);
    }
}
