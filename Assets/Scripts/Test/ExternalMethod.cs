using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalMethod : MonoBehaviour {

    public InternalMethod internalMethod;

    private void Awake()
    {
        internalMethod.teacherKind = InternalMethod.Teacher.Druid;
    }
}
