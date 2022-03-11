using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Master : MonoBehaviour
{
    [SerializeField] private List<GameObject> _npcList;
    [SerializeField] private List<WalkingPoint> _walkingPoints;
    [SerializeField] private List<NPCPoint> _npcPoints;

    private SoundMaster _soundMaster;

    private void Awake()
    {
        _soundMaster = GameObject.FindGameObjectWithTag("AudioMaster").GetComponent<SoundMaster>();
    }

    private GameObject npcPlaceholder;
    [SerializeField,HideInInspector] private List<GameObject> _NPCs;
    public void spawnNPC(GameObject NPCRef)
    {
        npcPlaceholder = Instantiate(NPCRef, _walkingPoints[0].gameObject.transform, false);
        npcPlaceholder.transform.parent = null;
        // call out to npc component
        // set the target
        npcPlaceholder.GetComponent<NPC_AI>().setCurentTarget(_walkingPoints[1].gameObject);
        // setup the npc
        //npcPlaceholder.GetComponent<NPC_AI>().setupNPC(NPCRef);

        _NPCs.Add(npcPlaceholder);
        if (this.gameObject.GetComponent<GameMaster>().ShopActive == true)
            _soundMaster.playDoorSound();
    }
    public void dismissNPCs()
    {
        foreach(GameObject npc in _npcList)
            if (npc.GetComponent<NPC_AI>() != null)
                npc.GetComponent<NPC_AI>().IsDismissed = true;
    }
}
