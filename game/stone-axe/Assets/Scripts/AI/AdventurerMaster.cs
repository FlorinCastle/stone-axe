using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventurerMaster : MonoBehaviour
{
    [SerializeField] private int _timeBetweenSpawn;
    //[SerializeField] private int _timer;
    [SerializeField] private GameObject _adventurerPrefab;

    [SerializeField] private List<GameObject> _currentAdventurers;
    [SerializeField] private List<WalkingPoint> _walkingPoints;
    [SerializeField] private List<LinePoint> _linePoints;
    [SerializeField] private List<AdventurerData> _adventurerData;
    [SerializeField] private bool advSpawnEnabled;


    private void Awake()
    {
        //_timer = _timeBetweenSpawn;
        if (_adventurerPrefab == null)
            Debug.LogError("Adventurer Prefab is not assigned!");
    }

    public bool startAdventurerSpawn()
    {
        advSpawnEnabled = true;
        StartCoroutine(AdventurerSpawn());
        return advSpawnEnabled;
    }

    public bool disableAdventurerSpawn()
    {
        advSpawnEnabled = false;
        StopCoroutine(AdventurerSpawn());
        return advSpawnEnabled;
    }

    private GameObject advPlaceholder;
    IEnumerator AdventurerSpawn()
    {
        while (advSpawnEnabled)
        {
            Debug.Log("Started Adventurer Spawn Coroutine");
            advPlaceholder = Instantiate(_adventurerPrefab);
            advPlaceholder.GetComponent<AdventurerAI>().setupAdventurer();
            yield return new WaitForSeconds(_timeBetweenSpawn);
            Debug.Log("Finished Coroutine");
        }
    }

    public List<AdventurerData> GetAdvRaceList { get => _adventurerData; }
}
