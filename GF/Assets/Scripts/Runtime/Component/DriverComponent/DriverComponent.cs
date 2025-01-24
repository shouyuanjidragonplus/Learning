using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

/// <summary>
/// 编辑器模式下的一些测试
/// </summary>
[DisallowMultipleComponent]
[AddComponentMenu("Game Framework/Driver")]
public class DriverComponent : GameFrameworkComponent
{
    public bool LoadAB = false;
}