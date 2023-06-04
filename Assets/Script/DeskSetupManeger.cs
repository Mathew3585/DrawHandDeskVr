using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskSetupManeger : MonoBehaviour
{

    public GameObject visual;
    public Transform pivot;
    public Transform creationHand;

    public float defaultWeidht = .3f;
    public float defaultHeight = .01f;
    public float HeightOffset ;

    public GameObject[] ObjectToSpawnAfter;

    public OVRPassthroughLayer updateShapePassthrought;
    public OVRPassthroughLayer afterShapePassthrought;


    private Vector3 startPosition;
    private bool isUpdatatingShape;

    private Renderer visualRender;

    // Start is called before the first frame update
    void Start()
    {
        updateShapePassthrought.hidden = false;
        afterShapePassthrought.hidden = true;
        visual.SetActive(false);
        foreach (var item in ObjectToSpawnAfter)
        {
            item.SetActive(false);
        }

        visualRender = visual.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            visual.SetActive(true);
            startPosition = creationHand.position;
            isUpdatatingShape = true;

            afterShapePassthrought.RemoveSurfaceGeometry(visual);
            visualRender.enabled = true;
        }
        else if (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
        {
            isUpdatatingShape = false;

            foreach (var item in ObjectToSpawnAfter)
            {
                item.SetActive(true);
            }


            updateShapePassthrought.hidden = true;
            afterShapePassthrought.hidden = false;
            afterShapePassthrought.AddSurfaceGeometry(visual);
            visualRender.enabled = false;


        }

        if (isUpdatatingShape)
        {
            UpdateShape();
        }
    }

    public void UpdateShape()
    {
        float distance  = Vector3.ProjectOnPlane(creationHand.position - startPosition, Vector3.up).magnitude;

        visual.transform.localScale = new Vector3(distance, defaultHeight, defaultWeidht);

        pivot.right = Vector3.ProjectOnPlane(creationHand.position - startPosition, Vector3.up);

        pivot.position = startPosition + pivot.rotation * new Vector3(visual.transform.localScale.x / 2 , HeightOffset, visual.transform.localScale.z / 2);
    }
}
