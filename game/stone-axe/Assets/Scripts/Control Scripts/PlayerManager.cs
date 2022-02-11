using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _playerRef;
    [SerializeField] private GameObject _playerHeadPrefabRef;
    [SerializeField] private List<GameObject> _playerHeads;
    [SerializeField] private List<AdventurerData> _playerSpecies;
    //[Header("Player Creation")]
    //[SerializeField] private TMP_Dropdown _playerSpeciesDropdown;
    [Header("Shop Points")]
    [SerializeField] private GameObject _counterPoint;
    [SerializeField] private GameObject _craftPoint;
    [SerializeField] private GameObject _disassemblePoint;
    [Header("Market Points")]
    [SerializeField] private GameObject _stallPoint;
    [SerializeField] private GameObject _questBoardPoint;

    [SerializeField] private int playerColor = -1;
    [SerializeField] private string playerSpecies;

    private bool playerExists;

    private void Awake()
    {
        if (this.gameObject.GetComponent<UIControl>().ShopUIActive == true && playerExists == false)
            spawnPlayer();
    }

    public PlayerSave savePlayer()
    {
        PlayerSave saveObject = new PlayerSave
        {
            playerHead = _playerHeadPrefabRef.name,
            playerSpecies = playerSpecies,
            playerColor = playerColor,
        };
        return saveObject;
    }
    public void loadPlayerData(PlayerSave playerSave)
    {
        foreach (GameObject pHead in _playerHeads)
            if (pHead.name == playerSave.playerSpecies)
                _playerHeadPrefabRef = pHead;

        playerColor = playerSave.playerColor;
        if (playerSave.playerSpecies == "Elf")
            _playerColor = this.gameObject.GetComponent<AdventurerMaterials>().ElfColors[playerColor];
        else if (playerSave.playerSpecies == "Human")
            _playerColor = this.gameObject.GetComponent<AdventurerMaterials>().HumanColors[playerColor];
        else if (playerSave.playerSpecies == "Lizardman")
            _playerColor = this.gameObject.GetComponent<AdventurerMaterials>().LizardColors[playerColor];
    }


    private Renderer[] _renderers;
    private MaterialPropertyBlock _propBlock;
    private Color32 _playerColor;
    public void spawnPlayer()
    {
        _playerRef = Instantiate(_playerPrefab);
        if (_playerHeadPrefabRef != null)
            _playerRef.GetComponent<PlayerScript>().setupHead(_playerHeadPrefabRef);
        else
        {
            //Debug.LogWarning("no player head");
            int headIndex = Random.Range(0, _playerHeads.Count);
            _playerRef.GetComponent<PlayerScript>().setupHead(_playerHeads[headIndex]);
            _playerHeadPrefabRef = _playerHeads[headIndex];
        }
        _renderers = _playerRef.gameObject.GetComponentsInChildren<Renderer>();
        _propBlock = new MaterialPropertyBlock();
        foreach(Renderer ren in _renderers)
        {
            ren.GetPropertyBlock(_propBlock);
            _propBlock.SetColor("_Color", _playerColor);
            ren.SetPropertyBlock(_propBlock);
        }

        playerExists = true;
        warpToCounter();
    }
    public void setPlayerHead (string value)
    {
        playerSpecies = value;
        if (value == "Elf")
            _playerHeadPrefabRef = _playerHeads[0];
        else if (value == "Human")
            _playerHeadPrefabRef = _playerHeads[1];
        else if (value == "Lizardman")
            _playerHeadPrefabRef = _playerHeads[2];
        else
            _playerHeadPrefabRef = null;
    }
    public void setPlayerColor (int colorIndex, Color32 colorInput)
    {
        playerColor = colorIndex;
        _playerColor = colorInput;
    }

    public void warpToCounter()
    {
        if (this.gameObject.GetComponent<GameMaster>().ShopActive == true)
        {
            _playerRef.transform.position = _counterPoint.transform.position;
            _playerRef.transform.rotation = _counterPoint.transform.rotation;
        }
    }
    public void warpToCraft()
    {
        if (this.gameObject.GetComponent<GameMaster>().ShopActive == true)
        {
            _playerRef.transform.position = _craftPoint.transform.position;
            _playerRef.transform.rotation = _craftPoint.transform.rotation;
        }
    }
    public void warpToDisassemble()
    {
        if (this.gameObject.GetComponent<GameMaster>().ShopActive == true)
        {
            _playerRef.transform.position = _disassemblePoint.transform.position;
            _playerRef.transform.rotation = _disassemblePoint.transform.rotation;
        }
    }

    public void warpToStall()
    {
        if (this.gameObject.GetComponent<GameMaster>().MarketActive == true)
        {
            _playerRef.transform.position = _stallPoint.transform.position;
            _playerRef.transform.rotation = _stallPoint.transform.rotation;
        }
    }
    public void warpToQuestBoard()
    {
        if (this.gameObject.GetComponent<GameMaster>().MarketActive == true)
        {
            _playerRef.transform.position = _questBoardPoint.transform.position;
            _playerRef.transform.rotation = _questBoardPoint.transform.rotation;
        }
    }

    public List<AdventurerData> PlayerSpecies { get => _playerSpecies; }
    public bool PlayerExists { get => playerExists; }
    public int PlayerColor { get => playerColor; set => playerColor = value; }
    public void removePlayer()
    {
        Destroy(_playerRef);
        playerExists = false;
    }

}
[System.Serializable]
public class PlayerSave
{
    //public GameObject playerHead;
    public string playerHead;
    public string playerSpecies;
    public int playerColor;
}