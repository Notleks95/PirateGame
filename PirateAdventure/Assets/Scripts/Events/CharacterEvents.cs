using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class CharacterEvents
{
    //character damage and value
    public static UnityAction<GameObject, int> characterDamaged;

    //character heal and value
    public static UnityAction<GameObject, int> characterHealed;

    //character gold pickup
    public static UnityAction<GameObject, int> goldCollected;

}
