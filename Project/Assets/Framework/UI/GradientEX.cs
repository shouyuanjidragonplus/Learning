using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
public enum GradientType
{
    TowColor = 0,
    ThreeColor,
}

/// <summary>
/// 渐变字体
/// </summary>
public class GradientEX : BaseMeshEffect
{
    [SerializeField] private GradientType m_GradientType = GradientType.TowColor;
    [SerializeField] private Color32 m_TopColor = Color.white;
    [SerializeField] private Color32 m_MiddleColor = Color.white;
    [SerializeField] private Color32 m_BottomColor = Color.black;
    [Range(0f, 1f)] [SerializeField] private float m_ColorOffset = 0.5f;
    [SerializeField] private Camera m_UICamera;
    private List<UIVertex> iVertices = new List<UIVertex>();

    public override void ModifyMesh(VertexHelper vh)
    {
        this.iVertices.Clear();

        if (!IsActive())
        {
            return;
        }

        var count = vh.currentVertCount;
        if (count == 0)
            return;

        /*
         *  TL--------TR
         *  |          |
         *  |          |
         *  CL--------CR
         *  |          |
         *  |          |
         *  BL--------BR
         * **/


        for (int i = 0; i < count; i++)
        {
            UIVertex vertex = UIVertex.simpleVert;
            vh.PopulateUIVertex(ref vertex, i);
            this.iVertices.Add(vertex);
        }
        vh.Clear();

        for (int i = 0; i < this.iVertices.Count; i += 4)
        {

            UIVertex TL = this.GeneralUIVertex(this.iVertices[i + 0]);
            UIVertex TR = this.GeneralUIVertex(this.iVertices[i + 1]);
            UIVertex BR = this.GeneralUIVertex(this.iVertices[i + 2]);
            UIVertex BL = this.GeneralUIVertex(this.iVertices[i + 3]);

            TL.color = this.m_TopColor;
            TR.color = this.m_TopColor;
            BL.color = this.m_BottomColor;
            BR.color = this.m_BottomColor;


            vh.AddVert(TL);
            vh.AddVert(TR);

            if (this.m_GradientType == GradientType.ThreeColor)
            {
                UIVertex CR = this.GeneralUIVertex(this.iVertices[i + 2]);
                UIVertex CL = this.GeneralUIVertex(this.iVertices[i + 3]);

                CR.position = (TR.position + BR.position) * this.m_ColorOffset;
                CL.position = (TL.position + BL.position) * this.m_ColorOffset;
                CR.uv0 = (TR.uv0 + BR.uv0) * this.m_ColorOffset;
                CL.uv0 = (TL.uv0 + BL.uv0) * this.m_ColorOffset;
                CR.color = this.m_MiddleColor;
                CL.color = this.m_MiddleColor;

                vh.AddVert(CR);
                vh.AddVert(CL);
            }

            vh.AddVert(BR);
            vh.AddVert(BL);
        }

        int step = 4;
        if (this.m_GradientType == GradientType.ThreeColor)
        {
            step = 6;
        }

        for (int i = 0; i < vh.currentVertCount; i += step)
        {
            if (this.m_GradientType == GradientType.ThreeColor)
            {
                vh.AddTriangle(i + 0, i + 1, i + 2);
                vh.AddTriangle(i + 2, i + 3, i + 0);
                vh.AddTriangle(i + 3, i + 2, i + 4);
                vh.AddTriangle(i + 4, i + 5, i + 3);
            }
            else
            {
                vh.AddTriangle(i + 0, i + 1, i + 2);
                vh.AddTriangle(i + 2, i + 3, i + 0);
            }
        }

    }

    private UIVertex GeneralUIVertex(UIVertex vertex)
    {
        UIVertex result = UIVertex.simpleVert;
        result.normal = new Vector3(vertex.normal.x, vertex.normal.y, vertex.normal.z);
        result.position = new Vector3(vertex.position.x, vertex.position.y, vertex.position.z);
        result.tangent = new Vector4(vertex.tangent.x, vertex.tangent.y, vertex.tangent.z, vertex.tangent.w);
        result.uv0 = new Vector2(vertex.uv0.x, vertex.uv0.y);
        result.uv1 = new Vector2(vertex.uv1.x, vertex.uv1.y);
        return result;
    }
}
