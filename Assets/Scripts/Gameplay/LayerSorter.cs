using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSorter : MonoBehaviour
{
    public class LayerData
    {
        public SpriteRenderer Renderer;
        public Transform Transform;
        public int StartLayer;
    }

    [SerializeField] private BoxCollider2D mPlayerCollider;
    [SerializeField] private Transform mPlayerTransform;
    [SerializeField] private SpriteRenderer[] mPlayerRenderers;

    private List<LayerData> mLocalLayerData = new List<LayerData>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == mPlayerCollider) return;

        SpriteRenderer renderer = collision.gameObject.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            LayerData data = new LayerData();
            data.Renderer = renderer;
            data.Transform = collision.transform;
            data.StartLayer = renderer.sortingOrder;

            if (!mLocalLayerData.Contains(data))
            {
                mLocalLayerData.Add(data);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SpriteRenderer renderer = collision.gameObject.GetComponent<SpriteRenderer>();
        for (int i = 0; i < mLocalLayerData.Count; i++)
        {
            if (mLocalLayerData[i].Renderer == renderer)
            {
                renderer.sortingOrder = mLocalLayerData[i].StartLayer;
                mLocalLayerData.RemoveAt(i);
            }
        }
    }

    private void LateUpdate()
    {
        if (mLocalLayerData.Count > 0)
        {
            Sort();
        }
    }

    private void Sort()
    {
        float playerY = mPlayerTransform.position.y - 0.3f;

        for (int i = 0; i < mLocalLayerData.Count; i++)
        {
            float objectY = mLocalLayerData[i].Transform.position.y;
            if (playerY > objectY)
            {
                // behind object
                for (int j = 0; j < mPlayerRenderers.Length; j++)
                {
                    mPlayerRenderers[j].sortingOrder = 1;
                }
            }
            else
            {
                // in front of object
                for (int j = 0; j < mPlayerRenderers.Length; j++)
                {
                    mPlayerRenderers[j].sortingOrder = 5;
                }
            }
        }
    }
}
