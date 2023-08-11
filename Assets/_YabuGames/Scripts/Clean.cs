using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _YabuGames.Scripts
{
    public class Clean : MonoBehaviour
    {

        [SerializeField] private Texture2D dirtMaskBase;
        [SerializeField] private Texture2D brush;
        [SerializeField] private Material _material;
        [SerializeField] private Vector3 cleaningErrorOffset;

        private Texture2D _templateDirtMask;
        

        private void Start()
        {
            CreateTexture();
        }

        public void CleanTheDirt(Transform washer,float washingForce)
        {
            if (Physics.Raycast(washer.position+cleaningErrorOffset, transform.TransformDirection(washer.forward), out RaycastHit hit))
            {
                Debug.Log(hit.transform.name);
                
                cleaningErrorOffset.x = washingForce / 5;
                var textureCoord = hit.textureCoord;

                var pixelX = (int)(textureCoord.x * _templateDirtMask.width);
                var pixelY = (int)(textureCoord.y * _templateDirtMask.height);

                for (var x = 0; x < brush.width * washingForce; x++) 
                {
                    for (var y = 0; y < brush.height/2; y++)
                    {
                        Color pixelDirt = brush.GetPixel(x, y);
                        Color pixelDirtMask = _templateDirtMask.GetPixel(pixelX + x, pixelY + y);

                        _templateDirtMask.SetPixel(pixelX + x,
                            pixelY + y,
                            new Color(0, pixelDirtMask.g * pixelDirt.g, 0));
                    }
                }
                
                _templateDirtMask.Apply();
            }
        }

        private void CreateTexture()
        {
            _templateDirtMask = new Texture2D(dirtMaskBase.width, dirtMaskBase.height);
            _templateDirtMask.SetPixels(dirtMaskBase.GetPixels());
            _templateDirtMask.Apply();

            _material.SetTexture("_DirtMask", _templateDirtMask);
        }
    }
}
