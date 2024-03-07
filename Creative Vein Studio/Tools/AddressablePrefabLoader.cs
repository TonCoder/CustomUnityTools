using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if AddressableAssets
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
#endif
using UnityEngine.Serialization;

namespace _MAIN_APP.Scripts
{
#if AddressableAssets
    public class AddressablePrefabLoader : MonoBehaviour
    {
        [SerializeField, Space(10)] List<AddressableAssetEntry> loadedReferences = new List<AddressableAssetEntry>();

        private void Start()
        {
            Addressables.InitializeAsync().Completed += InitComplete;
        }

        private void OnDestroy()
        {
            loadedReferences?.ForEach(x => x.reference.ReleaseAsset());
            Addressables.InitializeAsync().Completed -= InitComplete;
        }

        private void InitComplete(AsyncOperationHandle<IResourceLocator> obj)
        {
            // Perform actions since its completed loading
            Debug.Log("Ready to get addressable objects");
        }

        public void Load()
        {
            Addressables.LoadAssetAsync<GameObject>(loadedReferences).Completed += asyncOp =>
            {
                if (asyncOp.Status == AsyncOperationStatus.Succeeded)
                {
                    Instantiate(asyncOp.Result, this.transform);
                }
            };
        }

        public void Load(AssetReference val, out GameObject outVal)
        {
            // if the reference val exist in the reference list then return it
            if (loadedReferences.Any(x => x.reference == val))
            {
                outVal =
                    loadedReferences.FirstOrDefault(x => x.reference == val)?.reference.Asset as GameObject;
                return;
            }

            // else add it to the list and then load it and return it
            loadedReferences.Add(new AddressableAssetEntry() { iD = val.Asset.GetInstanceID(), reference = val });
            outVal = loadedReferences.Last().reference.LoadAssetAsync<GameObject>().WaitForCompletion();
        }

        public void ReleaseAsset(AssetReference val)
        {
            var asset = loadedReferences.FirstOrDefault(x => x.reference == val);
            if (asset != null)
            {
                asset.reference.ReleaseAsset();
                loadedReferences.Remove(asset);
            }
        }

        public void ReleaseAsset(GameObject val)
        {
            var asset =
                loadedReferences.FirstOrDefault(x => x.iD == val.GetInstanceID());
            if (asset != null)
            {
                asset.reference.ReleaseAsset();
                loadedReferences.Remove(asset);
            }
        }
        
        public void ReleaseAssets()
        {
            loadedReferences?.ForEach(x => x.reference.ReleaseAsset());
        }

    }

    [Serializable]
    public class AddressableAssetEntry
    {
        public int iD;
        public AssetReference reference;
    }
#endif
}