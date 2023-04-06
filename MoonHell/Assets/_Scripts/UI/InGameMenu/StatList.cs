using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatList : MonoBehaviour
{
    [SerializeField] private GameObject PRO;
    [SerializeField] private GameObject MAT;
    [SerializeField] private GameObject MNP;
    [SerializeField] private GameObject MNF;
    [SerializeField] private GameObject ATS;
    [SerializeField] private GameObject CDR;
    [SerializeField] private GameObject HP;
    [SerializeField] private GameObject EXP;
    // Update is called once per frame
    void OnEnable()
    {
        var heroStats = GameManager.Instance.HeroInstance.HeroStats;
        var Stats = GameManager.Instance.HeroInstance.Stats;

        PRO.GetComponent<TextMeshProUGUI>().text = "PRO : " + heroStats.proiezione;
        MAT.GetComponent<TextMeshProUGUI>().text = "MAT : " + heroStats.materializzazione;
        MNP.GetComponent<TextMeshProUGUI>().text = "MNP : " + heroStats.manipolazione;
        MNF.GetComponent<TextMeshProUGUI>().text = "MNF : " + heroStats.manifestazione;

        ATS.GetComponent<TextMeshProUGUI>().text = "ATS : " + heroStats.ats;
        CDR.GetComponent<TextMeshProUGUI>().text = "CDR : " + heroStats.cdr;

        HP.GetComponent<TextMeshProUGUI>().text = "HP : " + Stats.hp;
        EXP.GetComponent<TextMeshProUGUI>().text = "EXP : "; //da implementare
    }
}
