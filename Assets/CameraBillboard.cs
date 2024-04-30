using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBillboard : MonoBehaviour
{
    [SerializeField] bool BillboardX = true;
    [SerializeField] bool BillboardY = true;
    [SerializeField] bool BillboardZ = true;
    float OffsetToCamera;
    Vector3 localStartPosition;
    
    Transform mainCam;

    void Start()
    {
        mainCam = Camera.main.transform;
        localStartPosition = transform.localPosition;
    }

    void Update()
    {
        transform.LookAt(transform.position + mainCam.rotation * Vector3.forward, mainCam.rotation * Vector3.up);
        if (!BillboardX || !BillboardY || !BillboardZ)
        {
            transform.rotation = Quaternion.Euler(BillboardX ? transform.rotation.eulerAngles.x : 0f,
                BillboardY ? transform.rotation.eulerAngles.y : 0f, BillboardZ ? transform.rotation.eulerAngles.z : 0f);
        }

        transform.localPosition = localStartPosition;
        transform.position = transform.position + transform.rotation * Vector3.forward * OffsetToCamera;
    }
}