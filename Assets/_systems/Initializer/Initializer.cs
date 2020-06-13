using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    [SerializeField, RequireInterface(typeof(IInitializable))]
    protected List<Object> initializebles;
}
