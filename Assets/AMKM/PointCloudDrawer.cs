﻿using UnityEngine;
using System.Collections;
using Windows.Kinect;

[RequireComponent(typeof(Camera))]
public class PointCloudDrawer : MonoBehaviour
{
    public Material lineMaterial;
    private CameraSpacePoint[] _pointCloud;
    private int _depthWidth, _depthHeight, _downSample;
    private bool _drawPointCloud = false;

    void Start()
    {
        KinectCalib.instance.OnPointCloudUpdate += onPointCloudUpdate;
    }

    public void SetDrawPointCloud(bool draw)
    {
        _drawPointCloud = draw;
    }

    public void OnPostRender()
    {
        if(_drawPointCloud)
            DrawPointCloud();
    }

    // To show the lines in the editor
    void OnDrawGizmos()
    {
        if(Application.isEditor)
        {
            DrawPointCloud();

        }
    }

    void onPointCloudUpdate(CameraSpacePoint[] pointCloud, int depthMapWidth, int depthMapHeight, int downsample)
    {
        _pointCloud = pointCloud.Clone() as CameraSpacePoint[];
        this._depthWidth = depthMapWidth;
        this._depthHeight = depthMapHeight;
        this._downSample = downsample;

        CameraSpacePoint debugPoint = _pointCloud[depthMapWidth / 2];
        Vector3 debugVector = new Vector3(debugPoint.X, debugPoint.Y, debugPoint.Z);
    }

    void DrawPointCloud()
    {
        if (_pointCloud != null && _pointCloud.Length != 0)
        {
            int curLine = 0;
            for (int ty = 0; ty < _depthHeight; ty += _downSample)
            {
                for (int tx = 0; tx < _depthWidth; tx += _downSample)
                {
                    int index = ty * _depthWidth + tx;
                    CameraSpacePoint point = _pointCloud[index];

                    GL.Begin(GL.LINES);
                    GL.Color(new Color(lineMaterial.color.r, lineMaterial.color.g, lineMaterial.color.b, lineMaterial.color.a));
                    lineMaterial.SetPass(0);
                    Vector3 start = new Vector3(point.X, point.Y, point.Z);
                    Vector3 end = new Vector3(point.X, point.Y, point.Z + 0.005f);
                    GL.Vertex3(start.x, start.y, start.z);
                    GL.Vertex3(end.x, end.y, end.z);
                    GL.End();
                }
                curLine++;
            }
        }
    }
}
