using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// La classe base da cui erediteranno tutti gli status effects, Contiene un sono metodo abstract che deve essere
/// ridefinito denominato effect()
/// </summary>
public abstract class StatusBase 
{
    /// <summary>
    /// Nome del debuff
    /// </summary>
    public const string nome = "";
    public float duration { get; private set; }

    /// <summary>
    /// Il metodo effect conterrà la logica necessaria per applicare un determinato
    /// Debuf.
    /// </summary>
    /// <param name="unit">L'unità su cui si vuole applicae il debuff</param>
    public abstract void Effect(UnitBase unit);

    public StatusBase(float duration)
    {
        this.duration = duration;
    }
    
}
