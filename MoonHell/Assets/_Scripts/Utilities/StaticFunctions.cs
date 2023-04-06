using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe static che conterrà tuttti vari metodi utili
/// </summary>
public static class StaticFunctions 
{
    /// <summary>
    /// Funzione che calcola gli exp richiesti per il prossimo level up di una scuola di spirito
    /// </summary>
    /// <param name="level">Livello precedente a quello a cui si vuole calcolare gli exp richiesti</param>
    /// <returns></returns>
    public static int getNextExp(int level)
    {
        //calcolo temporaneo
        return (int)(GameManager.kConstLevels*level*Mathf.Pow(2,level) + GameManager.hConstLevels*level*GameManager.baseExp);
    }
}
