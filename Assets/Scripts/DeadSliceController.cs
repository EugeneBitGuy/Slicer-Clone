using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadSliceController : MonoBehaviour
{
    private bool _wasThrowed = false;

    public void Throw()
    {
        if(_wasThrowed) return;

        _wasThrowed = true;
        
        Destroy(GetComponent<MeshCollider>());

        gameObject.AddComponent<BoxCollider>().isTrigger = true;
                
        var rb = GetComponent<Rigidbody>();

        rb.isKinematic = false;

        rb.AddForce(Vector3.back * 100f);
        
        Destroy(gameObject, 1f);
    }
    
}
