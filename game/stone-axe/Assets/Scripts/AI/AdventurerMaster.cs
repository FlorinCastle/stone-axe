using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Slider _spawnProgressSlider;

    private void Awake()
    {
        //_timer = _timeBetweenSpawn;
        if (_adventurerPrefab == null)
            Debug.LogError("Adventurer Prefab is not assigned!");
        _spawnProgressSlider.gameObject.SetActive(false);
    }

    public bool startAdventurerSpawn()
    {
        advSpawnEnabled = true;
        _spawnProgressSlider.gameObject.SetActive(true);
        StartCoroutine(AdventurerSpawn());
        return advSpawnEnabled;
    } 
    public bool disableAdventurerSpawn()
    {
        advSpawnEnabled = false;
        _spawnProgressSlider.gameObject.SetActive(false);
        StopCoroutine(AdventurerSpawn());
        return advSpawnEnabled;
    } 
    public void dismissAdventurers()
    {
        foreach (GameObject adventurer in _currentAdventurers)
            if (adventurer.GetComponent<AdventurerAI>() != null)
                adventurer.GetComponent<AdventurerAI>().IsDismissed = true;
    }
    public void removeAdventurer(GameObject adventurer)
    {
        _currentAdventurers.Remove(adventurer);
        Destroy(adventurer);
    }
    public void removeAllAdventurers()
    {
        StopCoroutine(AdventurerSpawn());
        StopCoroutine(AdventurerTimer());
        foreach (GameObject go in _currentAdventurers)
            Destroy(go);

        _currentAdventurers.Clear();
    }

    private GameObject advPlaceholder;
    IEnumerator AdventurerSpawn()
    {
        //StartCoroutine(AdventurerTimer());
        while (advSpawnEnabled)
        {
            Debug.Log("Started Adventurer Spawn Coroutine");
            StartCoroutine(AdventurerTimer());
            yield return new WaitForSeconds(_timeBetweenSpawn);
            if (_currentAdventurers.Count < 3)
            {
                advPlaceholder = Instantiate(_adventurerPrefab, _walkingPoints[0].gameObject.transform, false);
                advPlaceholder.transform.parent = null;
                advPlaceholder.GetComponent<AdventurerAI>().setCurentTarget(_walkingPoints[1].gameObject);
                advPlaceholder.GetComponent<AdventurerAI>().setupAdventurer();
                //advPlaceholder.GetComponent<AdventurerAI>().IsMoving = true;
                _currentAdventurers.Add(advPlaceholder);
            }
            StopCoroutine(AdventurerTimer());
            Debug.Log("Finished spawning Coroutine Countdown");
        }
    }
    IEnumerator AdventurerTimer()
    {
        float duration = _timeBetweenSpawn;
        float normalizedTime = 0f;
        while (normalizedTime <= 1f)
        {
            _spawnProgressSlider.value = normalizedTime;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
    }

    public List<AdventurerData> GetAdvRaceList { get => _adventurerData; }
}
