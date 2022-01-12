using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _playerRef;
    [SerializeField] private GameObject _playerHeadPrefabRef;
    [SerializeField] private List<GameObject> _playerHeads;
    [Header("Shop Points")]
    [SerializeField] private GameObject _counterPoint;
    [SerializeField] private GameObject _craftPoint;
    [SerializeField] private GameObject _disassemblePoint;
    [Header("Market Points")]
    [SerializeField] private GameObject _stallPoint;
    [SerializeField] private GameObject _questBoardPoint;

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
        };
        return saveObject;
    }
    public void loadPlayerData(PlayerSave playerSave)
    {
        foreach (GameObject pHead in _playerHeads)
            if (pHead.name == playerSave.playerHead)
                _playerHeadPrefabRef = pHead;
    }


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
        playerExists = true;
        warpToCounter();
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

    public bool PlayerExists { get => playerExists; }
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
}