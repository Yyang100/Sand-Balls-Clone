// dnSpy decompiler from Assembly-CSharp.dll
using System;
using UnityEngine;

/// <summary>
/// Create circles // holes
/// </summary>
public class Stamp : MonoBehaviour
{

    public float radius = 1f;

    public void CutStamp(RegionGenerator myRegion)
    {
        myRegion.Cut(transform.position, this.radius);
    }
    

     

}
