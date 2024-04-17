using Script.Misc;
using UnityEngine;

namespace UI
{
    public class CanvasCameraController : Singleton<CanvasCameraController>
    {
        [SerializeField] private Canvas canvasCamera;
        public Canvas CanvasCamera => canvasCamera;
    }
}
