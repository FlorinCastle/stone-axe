using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _playerRef;
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

    public void spawnPlayer()
    {
        _playerRef = Instantiate(_playerPrefab);
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
