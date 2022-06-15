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
    [SerializeField] private float _adventuturerWaitTimeBeforeMove;

    [SerializeField] private List<GameObject> _currentAdventurers;
    [SerializeField] private List<WalkingPoint> _walkingPoints;
    [SerializeField] private List<LinePoint> _linePoints;
    [SerializeField] private GameObject _headOfLine;
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

    [Header("Chatty")]
    [SerializeField] private GameObject chattyParent;
    [SerializeField] private GameObject speechBubblePrefab;
    [SerializeField] private List<GameObject> _speechBubbles;

    private void Awake()
    {
        //_timer = _timeBetweenSpawn;
        if (_adventurerPrefab == null)
            Debug.LogError("Adventurer Prefab is not assigned!");
        //_spawnProgressSlider.gameObject.SetActive(false);
        _soundMaster = GameObject.FindGameObjectWithTag("AudioMaster").GetComponent<SoundMaster>();
    }

    public bool startAdventurerSpawn()
    {
        advSpawnEnabled = true;
        _spawnProgressSlider.gameObject.SetActive(true);
        advCoroutine = StartCoroutine(AdventurerSpawn());
        //Debug.Log("AdventurerMaster.startAdventurerSpawn(): Started Adventurer Spawn Coroutine");
        return advSpawnEnabled;
    } 
    public bool disableAdventurerSpawn()
    {
        advSpawnEnabled = false;
        _spawnProgressSlider.gameObject.SetActive(false);
        if (advCoroutine != null)
            StopCoroutine(advCoroutine);
        //Debug.Log("AdventurerMaster.disableAdventurerSpawn(): Stopped Adventurer Spawn Coroutine");
        return advSpawnEnabled;
    } 
    public void dismissAdventurers()
    {
        foreach (GameObject adventurer in _currentAdventurers)
            if (adventurer.GetComponent<AdventurerAI>() != null)
                adventurer.GetComponent<AdventurerAI>().IsDismissed = true;
    }
    public void dismissHeadOfLine()
    {
        foreach (GameObject adventurer in _currentAdventurers)
            if (adventurer.GetComponent<AdventurerAI>() != null)
            {
                if (adventurer.GetComponent<AdventurerAI>().CurrentTarget == _headOfLine)
                    adventurer.GetComponent<AdventurerAI>().IsDismissed = true;
            }
    }
    public void removeAdventurer(GameObject adventurer)
    {
        _currentAdventurers.Remove(adventurer);
        GameObject chat = adventurer.GetComponent<AdventurerAI>().MyChatBubble;
        _speechBubbles.Remove(chat);

        Destroy(chat);
        Destroy(adventurer);
    }
    public void removeAllAdventurers()
    {
        StopCoroutine(AdventurerSpawn());
        StopCoroutine(AdventurerTimer());
        foreach (GameObject go in _currentAdventurers)
            Destroy(go);

        _currentAdventurers.Clear();

        foreach(LinePoint linePoint in _linePoints)
        {
            linePoint.IsOccupied = false;
        }
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
        if (gameObject.GetComponent<GameMaster>().ShopActive == true)
            _soundMaster.playDoorSound();

        GameObject chattyBubble = Instantiate(speechBubblePrefab, chattyParent.transform, false);
        chattyBubble.GetComponent<SpeechBubbleTrackObject>().ObjRef = advPlaceholder.GetComponent<AdventurerAI>().ChattyFocus;
        advPlaceholder.GetComponent<AdventurerAI>().MyChatBubble = chattyBubble;
        _speechBubbles.Add(chattyBubble);
    }
    /*
    private GameObject npcPlaceholder;
    private List<GameObject> _NPCs;
    public void spawnNPC(GameObject NPCRef)
    {
        npcPlaceholder = Instantiate(NPCRef, _walkingPoints[0].gameObject.transform, false);
        npcPlaceholder.transform.parent = null;
        // call out to npc component
            // set the target
            // setup the npc

        _NPCs.Add(npcPlaceholder);
        if (this.gameObject.GetComponent<GameMaster>().ShopActive == true)
            _soundMaster.playDoorSound();
    }
    public void dismissNPCs()
    {
        foreach(GameObject npc in _NPCs)
        {
            // check if the npc component exists
                // dismiss the npc
        }
    }
    */
    public Color chooseAdvColor(string advType)
    {
        if (advType == "Elf")
        {
            int e = Random.Range(0, _adventurerMats.ElfColors.Count);
            //Debug.Log(advType + " " + e);
            return _adventurerMats.ElfColors[e];
            //return Color.red;
        }
        else if (advType == "Human")
        {
            int h = Random.Range(0, _adventurerMats.HumanColors.Count);
            //Debug.Log(advType + " " + h);
            return _adventurerMats.HumanColors[h];
            // return Color.blue;
        }
        else if (advType == "Lizardman")
        {
            int l = Random.Range(0, _adventurerMats.LizardColors.Count);
            //Debug.Log(advType + " " + l);
            return _adventurerMats.LizardColors[l];
            //return Color.black;
        }
        return Color.magenta;
    }

    public int getAdvColorRef(string advType, Color32 col)
    {
        if (advType == "Elf")
        {
            if (_adventurerMats.ElfColors.Contains(col))
                return _adventurerMats.ElfColors.IndexOf(col) + 1;
            return 0;
        }
        else if (advType == "Human")
        {
            if (_adventurerMats.HumanColors.Contains(col))
                return _adventurerMats.HumanColors.IndexOf(col) + 1;
            return 0;
        }
        else if (advType == "Lizardman")
        {
            if (_adventurerMats.LizardColors.Contains(col))
                return _adventurerMats.LizardColors.IndexOf(col) + 1;
            return 0;
        }
        return 0;
    }

    IEnumerator AdventurerSpawn()
    {
        //StartCoroutine(AdventurerTimer());
        while (advSpawnEnabled)
        {
            //Debug.LogWarning("Started Adventurer Spawn Coroutine");
            timerCoroutine = StartCoroutine(AdventurerTimer());
            _spawnProgressSlider.gameObject.SetActive(true);
            yield return new WaitForSeconds(_timeBetweenSpawn);
            if (_currentAdventurers.Count < 3)
            {
                spawnAdventurer();
            }
            StopCoroutine(timerCoroutine);
            _spawnProgressSlider.gameObject.SetActive(false);
            //Debug.Log("Finished spawning Coroutine Countdown");
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
    public float AdventurerWaitTime { get => _adventuturerWaitTimeBeforeMove; }
    public int GetMyIndex(GameObject adv)
    {
        if (_currentAdventurers.Contains(adv))
            return _currentAdventurers.IndexOf(adv);
        return -1;
    }
}
