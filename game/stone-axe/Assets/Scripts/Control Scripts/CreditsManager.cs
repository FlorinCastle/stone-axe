using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    [Header("Production Credit")]
    [TextArea()]
    [SerializeField] private string _productionCreditBody;
    [Header("Music Credit")]
    [TextArea()]
    [SerializeField] private string _musicCreditBody;
    [Header("Art Credit")]
    [TextArea()]
    [SerializeField] private string _artCreditBody;
    [Header("Other Credit")]
    [TextArea()]
    [SerializeField] private string _otherCreditBody;

    public string ProductionCredit { get => _productionCreditBody; }
    public string MusicCredit { get => _musicCreditBody; }
    public string ArtCredit { get => _artCreditBody; }
    public string OtherCredit { get => _otherCreditBody; }

}
