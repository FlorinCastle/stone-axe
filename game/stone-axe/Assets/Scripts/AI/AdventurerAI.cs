using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventurerAI : MonoBehaviour
{
    // REMINDER: forward is along the z-Axis of the object
    private AdventurerMaster _advMaster;
    private AdventurerData _advRaceRef;

    [SerializeField] private GameObject _currentPoint;
    [Header("Body Refs")]
    [SerializeField] private GameObject _headMark;

    private void Awake()
    {
        _advMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<AdventurerMaster>();
    }

    public void setupAdventurer()
    {
        chooseRace();
        setupAdventurerModel();
    }

    private void chooseRace()
    {
        List<AdventurerData> adventRef = _advMaster.GetAdvRaceList;
        int i = Random.Range(0, adventRef.Count);
        _advRaceRef = adventRef[i];
    }

    private void setupAdventurerModel()
    {
        if (_advRaceRef != null)
        {

        }
        else
            Debug.LogWarning("Adventurer Data for " + this.gameObject.name + " is not assigned! Use chooseRace() first then use setupAdventurerModel()");
    }
}
