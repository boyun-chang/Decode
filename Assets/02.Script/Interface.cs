using UnityEngine;
using System.Collections;

public interface IHint
{
    IEnumerator act();
    void deact();
}