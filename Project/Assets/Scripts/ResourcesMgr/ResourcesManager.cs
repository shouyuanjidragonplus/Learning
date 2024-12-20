using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using System.IO;
using Object = UnityEngine.Object;

namespace Framework.Asset
{
    public partial class ResourcesManager : Manager<ResourcesManager>
    {
        public T LoadResource<T>(string name, bool forceBundle = false, bool addToCache = true,string assetDeepPath = null) where T : Object
        {

            return null;
        }
        public void ReleaseRes(string path, bool free = false)
        {
           
        }
    }
}
