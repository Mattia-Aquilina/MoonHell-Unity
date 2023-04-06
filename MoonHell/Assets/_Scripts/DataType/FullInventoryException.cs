using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullInventoryException : Exception
{
    public FullInventoryException() { }

    public FullInventoryException(string message) : base(message) { }

    public FullInventoryException(string message, Exception inner)
       : base(message, inner)
    {
    }
}
