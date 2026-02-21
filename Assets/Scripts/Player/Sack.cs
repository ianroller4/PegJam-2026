using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sack : MonoBehaviour
{
    [SerializeField] private List<Gob> daggerGobs;
    [SerializeField] private List<Gob> magicGobs;
    [SerializeField] private List<Gob> tankGobs;
    [SerializeField] private List<Gob> bomberGobs;

    public enum ActiveGob
    {
        DAGGER,
        MAGIC,
        TANK,
        BOMBER
    }

    public ActiveGob activeGob;

    // Start is called before the first frame update
    void Start()
    {
        activeGob = ActiveGob.DAGGER;

        daggerGobs = new List<Gob>();
        magicGobs = new List<Gob>();
        tankGobs = new List<Gob>();
        bomberGobs = new List<Gob>();
    }

    public void AddGob(Gob gob)
    {
        gob.gameObject.SetActive(false);
        if (gob.GetComponent<DaggerGob>() != null)
        {
            if (!daggerGobs.Contains(gob))
            {
                daggerGobs.Add(gob);
            }
        }
        else if (gob.GetComponent<MagicGob>() != null)
        {
            if (!magicGobs.Contains(gob))
            {
                magicGobs.Add(gob);
            }
        }
        else if (gob.GetComponent<TankGob>() != null)
        {
            if (!tankGobs.Contains(gob))
            {
                tankGobs.Add(gob);
            }
        }
        else if (gob.GetComponent<BomberGob>() != null)
        {
            if (!bomberGobs.Contains(gob))
            {
                bomberGobs.Add(gob);
            }
        }
    }

    public void RemoveGob(Gob gob)
    {
        if (gob.GetComponent<DaggerGob>() != null)
        {
            if (daggerGobs.Contains(gob))
            {
                daggerGobs.Remove(gob);
            }
        }
        else if (gob.GetComponent<MagicGob>() != null)
        {
            if (magicGobs.Contains(gob))
            {
                magicGobs.Remove(gob);
            }
        }
        else if (gob.GetComponent<TankGob>() != null)
        {
            if (tankGobs.Contains(gob))
            {
                tankGobs.Remove(gob);
            }
        }
        else if (gob.GetComponent<BomberGob>() != null)
        {
            if (bomberGobs.Contains(gob))
            {
                bomberGobs.Remove(gob);
            }
        }
    }

    public Gob GetGob()
    {
        Gob gob = null;
        switch (activeGob)
        {
            case ActiveGob.DAGGER:
                if (daggerGobs.Count > 0)
                {
                    gob = daggerGobs[0];
                }
                break;
            case ActiveGob.MAGIC:
                if (magicGobs.Count > 0)
                {
                    gob = magicGobs[0];
                }
                break;
            case ActiveGob.TANK:
                if (tankGobs.Count > 0)
                {
                    gob = tankGobs[0];
                }
                break;
            case ActiveGob.BOMBER:
                if (bomberGobs.Count > 0)
                {
                    gob = bomberGobs[0];
                }
                break;
        }

        if (gob != null)
        {
            gob.gameObject.SetActive(true);
            gob.gameObject.transform.position = transform.position + Vector3.up;
            RemoveGob(gob);
        }
        return gob;
    }

    public void SwitchGob(bool right)
    {
        if (right)
        {
            switch (activeGob)
            {
                case ActiveGob.DAGGER:
                    activeGob = ActiveGob.MAGIC;
                    break;
                case ActiveGob.MAGIC:
                    activeGob = ActiveGob.TANK;
                    break;
                case ActiveGob.TANK:
                    activeGob = ActiveGob.BOMBER;
                    break;
                case ActiveGob.BOMBER:
                    activeGob = ActiveGob.DAGGER;
                    break;
            }
        }
        else
        {
            switch (activeGob)
            {
                case ActiveGob.DAGGER:
                    activeGob = ActiveGob.BOMBER;
                    break;
                case ActiveGob.MAGIC:
                    activeGob = ActiveGob.DAGGER;
                    break;
                case ActiveGob.TANK:
                    activeGob = ActiveGob.MAGIC;
                    break;
                case ActiveGob.BOMBER:
                    activeGob = ActiveGob.TANK;
                    break;
            }
        }
    }
}
