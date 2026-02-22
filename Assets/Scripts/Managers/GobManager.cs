using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobManager : MonoBehaviour
{
    public List<Gob> gobs;

    public GameObject player;

    private void Awake()
    {
        gobs = new List<Gob>();
        player = GameObject.Find("Player");
    }

    public void AddGob(Gob gob)
    {
        if (!gobs.Contains(gob))
        {
            gobs.Add(gob);
        }
    }

    public void RemoveGob(Gob gob)
    {
        if (gobs.Contains(gob))
        {
            gobs.Remove(gob);
        }
    }
}
