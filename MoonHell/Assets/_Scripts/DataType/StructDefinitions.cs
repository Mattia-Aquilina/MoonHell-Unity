using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This is obviously an example and I have no idea what kind of game you're making.
/// You can use a similar manager for controlling your menu states or dynamic-cinematics, etc
/// </summary>
[Serializable]
public enum GameState
{
    Menu = 0,
    Preparation = 1,
    PreHunting = 2,
    Jump = 3,
    Hunting = 4,
    AssemblingResources =5,
    StartUp = 6
}
/// <summary>
/// Continete le statisiche di base comuni a tutte unità. All'interno di essa troviamo:
/// hp [punti vita], ms [velocità di moviemento], di [danni inflitti], dr [danni ricevuti];
/// </summary>
[Serializable]
public struct BaseStats
{
    public int MaxHP;
    public int hp;
    public float ms;
    public float di;
    public float dr;

    /// <summary>
    /// Funzioneb che, ricevuto come parametro delle statistiche, le somma alle statistiche attuali.
    /// </summary>
    /// <param name="s">statistiche da sommare</param>
    public void AddStats(BaseStats s)
    { 
        MaxHP += s.MaxHP;
        ms += s.ms; 
        //di += s.di;
        //dr += s.dr;
    }
    /// <summary>
    /// Funzione che inizializza i valori delle statistiche a quelli attuali. Gli HP non verranno inizializzati!
    /// </summary>
    public void init()
    {
        MaxHP = 0;
        ms = 0f;
        di = dr = 1f;
    }
    /// <summary>
    /// Funzione che calcola come le statistiche attuali dovrebbero aumentare se gli venissero sommate le statistiche s
    /// passate come parametro se trattate come empower.
    /// </summary>
    /// <param name="s">
    /// Statistiche da aggiungere a quelle attuali. Notare che se i valori in s sono minori di 1, l'incremento verrà calcolato in percentuale, menetre un valore maggiore di 1 sarà considerato un incremento fisso. L'unica eccezione sono la ats, che viene incrementata sempre in percentuale, e la cdr, che viene incrementata sempre in maniera statica.
    /// </param>
    /// <returns>
    /// Ritorna la strut contente le statistiche da sommare per ottenere l'incremento specificato come parametro
    /// </returns>
    public BaseStats GetStatsIncrement(BaseStats s)
    {
        BaseStats increment = s;
        increment.init();

        if (s.MaxHP > 1)
            //static increment
            increment.MaxHP = s.MaxHP;
        else
            increment.MaxHP = MaxHP * s.MaxHP;

        if (s.ms > 1)
            //static increment
            increment.ms = s.ms;
        else
            increment.ms = ms * s.ms;

        return increment;
    }
}

/// <summary>
/// Enum che contiene tutti i diversi tipi di personaggi giocabili
/// Sempre se nel gioco esisteranno più di un personaggio;
/// </summary>
[Serializable]
public enum HeroType
{
    sample1 = 0,
    sample2 = 1
}

/// <summary>
/// Contiene tutte le statistiche base di un personaggio giocabile. All'interno di essa troviamo:
/// proiezione, materializzazione, manipolazione, manifestazione, ats [ velocità di attacco ], cdr [ cooldown reduction ];
/// </summary>
[Serializable]
public struct HeroStats
{
    public float proiezione;
    public float materializzazione;
    public float manipolazione;
    public float manifestazione;
    public float ats;
    public float cdr;

    /// <summary>
    /// Funzioneb che, ricevuto come parametro delle statistiche, le somma alle statistiche attuali.
    /// </summary>
    /// <param name="s">statistiche da sommare</param>
    public void AddStats(HeroStats s)
    {
        proiezione += s.proiezione;
        materializzazione += s.materializzazione;
        manipolazione += s.manipolazione;
        manifestazione += s.manifestazione;
        ats += s.ats;
        cdr += s.cdr;
    }

    /// <summary>
    /// Funzione che calcola come le statistiche attuali dovrebbero aumentare se gli venissero sommate le statistiche s
    /// passate come parametro se trattate come empower.
    /// </summary>
    /// <param name="s">
    /// Statistiche da aggiungere a quelle attuali. Notare che se i valori in s sono minori di 1, l'incremento verrà calcolato in percentuale, menetre un valore maggiore di 1 sarà considerato un incremento fisso. L'unica eccezione sono la ats, che viene incrementata sempre in percentuale, e la cdr, che viene incrementata sempre in maniera statica.
    /// </param>
    /// <returns>
    /// Ritorna la strut contente le statistiche da sommare per ottenere l'incremento specificato come parametro
    /// </returns>
    public HeroStats GetStatsIncrement(HeroStats s)
    {
        HeroStats increment = s;
        increment.init();

        if (s.proiezione > 1)
            //statical increment
            increment.proiezione = s.proiezione;
        else
            increment.proiezione = proiezione * s.proiezione;

        if (s.materializzazione > 1)
            //statical increment
            increment.materializzazione = s.materializzazione;
        else
            increment.materializzazione = materializzazione * s.materializzazione;

        if (s.manipolazione > 1)
            //statical increment
            increment.manipolazione = s.manipolazione;
        else
            increment.manipolazione = manipolazione * s.manipolazione;

        if (s.manifestazione > 1)
            //statical increment
            increment.manifestazione = s.manifestazione;
        else
            increment.manifestazione = s.manifestazione * s.manifestazione;

        //l'attack speed incrementa sempre in percentuale
        increment.ats = ats * s.ats;
        //il cdr aumenta sempre in maniera static
        increment.cdr = s.cdr;

        return increment;
    }

    /// <summary>
    /// Funzione che inizializza i valori delle statistiche a quelli attuali
    /// </summary>
    public void init()
    {
        proiezione = materializzazione = manipolazione = manifestazione = ats = cdr = 0;
    }
}

public enum StatsNames {
    Pro = 0,
    Mat = 1,
    Mnp = 2,
    Mnf = 3,
    Ats = 4,
    Cdr = 5,
    Hp = 6,
    Exp = 7
}

/// <summary>
/// Contiene le stats di ogni nemico (base) presente nel gioco. All'interno di essa troviamo: spirtiDrop [ numero di spiriti rilasciati all'uccisione ],
/// exp [ esperienza rilasciata all'uccisione ], def [ tasso riduzione danni, non sempre presente e in caso nulla ]
/// </summary>
[Serializable]
public struct EnemyStats
{
    public int spiritDrop; //risorse droppate alla morte
    public int exp; //esperienza ricevuta alla morte
    public int def; //tasso di protezione dai danni
    public int damage; //danno base del nemico, in caso di nemici più complessi con diversi attacchi si implenenta una nuova struttura
}

/// <summary>
/// Contiene le statistiche necessarie al calcolo dell’intensità di tutte le spell. Contiene tutti gli scaling per ogni parametro del personaggio (intesi come percentuali);
/// </summary>
[Serializable]
public struct SpellEffectStats
{
    //Espresso sia come numero, in tal caso si tratta di danno base assoluto, 
    //che come percentuale. In tal caso assume un valore < 1
    //puo essere 0, ex. spell di supporto
    public float baseDmg;
    public float defPen; //tasso di difesa ignorata 
    public float cd;
    //Tassi Di scaling

    public float ScalingProiezione;
    public float ScalingMaterializzazione;
    public float ScalingManipolazione;
    public float ScalingManifestazione;
    public float ScalingAts;
    public float ScalingCdr;
}

/// <summary>
/// Contiene le statistiche necessarie al calcolo dell’intensità di tutte le spell. Contiene tutti gli scaling per ogni parametro del personaggio (intesi come percentuali);
/// </summary>
[Serializable]
public struct SpellSizesStats
{
    public float range;
    public float size;
    public float duration;
    public float radius;
}

[Serializable]
public enum SpiritSchool
{
    fury = 0,
    shadows = 1,
    precision =2,
    repulsion = 3,
    clearness = 4,
    will = 5,
    summon = 6,
    symphony = 7,
    absorption = 8,
    enanchement = 9
}

[Serializable]
public enum PlanetType
{
    Magma = 0,
    Green = 1,
}

///======================================///
///Lista delle statistiche custom dei nemici
///======================================///

[Serializable]
public enum EnemySkillStats { 
    //Parametri per chi cura
    HealAmmount = 0,
    NumberOfHealing = 1,
    ChannelTime = 2
}

[Serializable]
public class ChiaveValore {
    public EnemySkillStats key;
    public float val;
}

///======================================///
///===============LISTA ITEM=============///
///======================================///

public enum ItemType { 
    BfSword = 0,
    Dagger = 1,
    WoodenShield = 2,
    WoodenBow = 3
}

public enum EnemyType { 
    ToothMonster = 0,
    BallBot = 1,
    OrbMage = 2
}


