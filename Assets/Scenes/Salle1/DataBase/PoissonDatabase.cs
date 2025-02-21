using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Poisson
{
    public string nom;
    public GameObject prefab;
}

[CreateAssetMenu(fileName = "PoissonDatabase", menuName = "Poissons/Database", order = 1)]
public class PoissonDatabase : ScriptableObject
{
    public List<Poisson> poissons;
}
