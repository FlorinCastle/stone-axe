using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdventurerMaster : MonoBehaviour
{
    [SerializeField] private int _timeBetweenSpawn;
    //[SerializeField] private int _timer;
    [SerializeField] private GameObject _adventurerPrefab;
    [SerializeField] private AdventurerMaterials _adventurerMats;

    [SerializeField] private List<GameObject> _currentAdventurers;
    [SerializeField] private List<WalkingPoint> _walkingPoints;
    [SerializeField] private List<LinePoint> _linePoints;
    [SerializeField] private List<AdventurerData> _adventurerData;
    [SerializeField] private bool advSpawnEnabled;
    [SerializeField] private Slider _spawnProgressSlider;
    /*
    [Header("if any other adventurer types are added, the script itself will need to be edited")]
    [SerializeField] private List<Material> _humanAdvMats;
    [SerializeField] private List<Material> _elfAdvMats;
    [SerializeField] private List<Material> _lizAdvMats;
    */
    private SoundMaster _soundMaster;
    private Coroutine advCoroutine = null;
    private Coroutine timerCoroutine = null;

    private void Awake()
    {
        //_timer = _timeBetweenSpawn;
        if (_adventurerPrefab == null)
            Debug.LogError("Adventurer Prefab is not assigned!");
        _spawnProgressSlider.gameObject.SetActive(false);
        _soundMaster = GameObject.FindGameObjectWithTag("AudioMaster").GetComponent<SoundMaster>();
    }

    public bool startAdventurerSpawn()
    {
        advSpawnEnabled = true;
        _spawnProgressSlider.gameObject.SetActive(true);
        advCoroutine = StartCoroutine(AdventurerSpawn());
        return advSpawnEnabled;
    } 
    public bool disableAdventurerSpawn()
    {
        advSpawnEnabled = false;
        _spawnProgressSlider.gameObject.SetActive(false);
        StopCoroutine(advCoroutine);
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

    public void spawnAdventurer()
    {
        advPlaceholder = Instantiate(_adventurerPrefab, _walkingPoints[0].gameObject.transform, false);
        advPlaceholder.transform.parent = null;
        advPlaceholder.GetComponent<AdventurerAI>().setCurentTarget(_walkingPoints[1].gameObject);
        advPlaceholder.GetComponent<AdventurerAI>().setupAdventurer();
        //advPlaceholder.GetComponent<AdventurerAI>().IsMoving = true;
        _currentAdventurers.Add(advPlaceholder);
        if (this.gameObject.GetComponent<GameMaster>().ShopActive == true)
            _soundMaster.playDoorSound();
    }

    public Color chooseAdvColor(string advType)
    {
        if (advType == "Elf")
        {
            int e = Random.Range(0, _adventurerMats.ElfColors.Count);
            Debug.Log(advType + " " + e);
            return _adventurerMats.ElfColors[e];
            //return Color.red;
        }
        else if (advType == "Human")
        {
            int h = Random.Range(0, _adventurerMats.HumanColors.Count);
            Debug.Log(advType + " " + h);
            return _adventurerMats.HumanColors[h];
            // return Color.blue;
        }
        else if (advType == "Lizardman")
        {
            int l = Random.Range(0, _adventurerMats.LizardColors.Count);
            Debug.Log(advType + " " + l);
            return _adventurerMats.LizardColors[l];
            //return Color.black;
        }
        return Color.magenta;
    }

    IEnumerator AdventurerSpawn()
    {
        //StartCoroutine(AdventurerTimer());
        while (advSpawnEnabled)
        {
            Debug.Log("Started Adventurer Spawn Coroutine");
            timerCoroutine = StartCoroutine(AdventurerTimer());
            //_spawnProgressSlider.gameObject.SetActive(true);
            yield return new WaitForSeconds(_timeBetweenSpawn);
            if (_currentAdventurers.Count < 3)
            {
                spawnAdventurer();
                /*
                advPlaceholder = Instantiate(_adventurerPrefab, _walkingPoints[0].gameObject.transform, false);
                advPlaceholder.transform.parent = null;
                advPlaceholder.GetComponent<AdventurerAI>().setCurentTarget(_walkingPoints[1].gameObject);
                advPlaceholder.GetComponent<AdventurerAI>().setupAdventurer();
                //advPlaceholder.GetComponent<AdventurerAI>().IsMoving = true;
                _currentAdventurers.Add(advPlaceholder);
                */
            }
            StopCoroutine(timerCoroutine);
            _spawnProgressSlider.gameObject.SetActive(false);
            Debug.Log("Finished spawning Coroutine Countdown");
        }
    }
    IEnumerator AdventurerTimer()
    {
        _spawnProgressSlider.gameObject.SetActive(true);
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
