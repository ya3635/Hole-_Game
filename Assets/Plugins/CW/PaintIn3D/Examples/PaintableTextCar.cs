using PaintCore;
using PaintIn3D;
using System.Collections.Generic;
using UnityEngine;

public class PaintableTextCar : MonoBehaviour
{
    public Texture Texture;
    public Texture SecondTexture;

    public List<CwPaintableMesh> cwPaintableMeshes;
    public List<CwPaintableMeshTexture> cwPaintableMeshTextures;

    public Transform TileTrandform;

    public float hardness = 5;
    public CwBlendMode CwBlendMode;
    public CwGroup Group;
    public Color Color = Color.white;
    public float Opacity = 1f;
    public Texture Tile;
    public Transform TileTransform;
    public float TileOpacity = 1f;
    public float TileTransition = 4f;
    public Vector3 FinalSize;
    public float Scale = 1f;
    public float Radius;
    public float Angle = 0;
    public LayerMask Layers;

    public Matrix4x4 matrix => TileTransform != null ? TileTransform.localToWorldMatrix : Matrix4x4.identity;

    private void Awake()
    {
        //var finalSize = FinalSize * Radius;

        //CwCommandSphere.Instance.SetState(false, 0);
        //CwCommandSphere.Instance.SetShape(Quaternion.identity, finalSize, Angle);
        //CwCommandSphere.Instance.SetMaterial(CwBlendMode, hardness, Color, Opacity, Tile, matrix, TileOpacity, TileTransition);

        //CwCommandSphere.Instance.ClearMask();
        //CwCommandSphere.Instance.DepthMask = null;

        //for (int i = 0; i < cwPaintableMeshes.Count; i++)
        //{

        //    CwCommandSphere.Instance.SetLocation(cwPaintableMeshes[i].transform.position);

        //    var worldRadius = Mathf.Sqrt(Vector3.Dot(finalSize, finalSize));

        //    CwPaintableManager.SubmitAll(CwCommandSphere.Instance, cwPaintableMeshes[i].transform.position, worldRadius, Layers, Group, cwPaintableMeshes[i], cwPaintableMeshTextures[i]);

        //    CwPaintableManager.Submit(CwCommandSphere.Instance, cwPaintableMeshes[i], cwPaintableMeshTextures[i]);

        //    cwPaintableMeshTextures[i].ExecuteCommands(false, false);

        //    Debug.Log("Paint Sphre Awake");
        //}
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            CwCommandReplace.Instance.SetMaterial(Texture, Color.white);
            for (int i = 0; i < cwPaintableMeshes.Count; i++)
            {
                CwPaintableManager.Submit(CwCommandReplace.Instance, cwPaintableMeshes[i], cwPaintableMeshTextures[i]);
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            CwCommandReplace.Instance.SetMaterial(SecondTexture, Color.white);


            for (int i = 0; i < cwPaintableMeshes.Count; i++)
            {
                CwPaintableManager.Submit(CwCommandReplace.Instance, cwPaintableMeshes[i], cwPaintableMeshTextures[i]);
            }
        }


        if (Input.GetKeyDown(KeyCode.D))
        {

            CwCommandSphere.Instance.SetMaterial(CwBlendMode, hardness, Color, Opacity, Tile, Matrix4x4.identity, Matrix4x4.identity, TileOpacity, TileTransition);

            for (int i = 0; i < cwPaintableMeshes.Count; i++)
            {
                CwCommandSphere.Instance.SetState(false, 0);

                CwCommandSphere.Instance.SetLocation(cwPaintableMeshes[i].transform.position);

                var finalSize = FinalSize * Radius;

                var worldRadius = Mathf.Sqrt(Vector3.Dot(finalSize, finalSize));

                CwCommandSphere.Instance.SetShape(Quaternion.Euler(Vector3.zero), finalSize, Angle);
                CwCommandSphere.Instance.ClearMask();
                CwCommandSphere.Instance.DepthMask = null;
                //CwPaintableManager.SubmitAll(CwCommandSphere.Instance, cwPaintableMeshes[i].transform.position, worldRadius, Layers, Group, cwPaintableMeshes[i], cwPaintableMeshTextures[i]);

                CwPaintableManager.Submit(CwCommandSphere.Instance, cwPaintableMeshes[i], cwPaintableMeshTextures[i]);



                Debug.Log("Paint Sphre");
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            for (int i = 0; i < cwPaintableMeshes.Count; i++)
            {
                var counter = cwPaintableMeshes[i].GetComponent<CwChangeCounter>();

                counter.PaintableTexture = cwPaintableMeshTextures[i];
                counter.MaskMesh = cwPaintableMeshes[i].GetComponent<MeshFilter>().sharedMesh;

                counter.Texture = cwPaintableMeshTextures[i].Current;
                counter.Color = Color.white;
            }
        }
    }
}
