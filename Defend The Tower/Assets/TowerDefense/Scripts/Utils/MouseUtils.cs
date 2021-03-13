using UnityEngine;

namespace TowerDefense.Scripts.Utils
{
    public static class MouseUtils
    {
        private static readonly LayerMask FloorMask = LayerMask.GetMask("Floor");

        public static Vector3 GetMouseWorldPosition()
        {
            if (Camera.main is null) return Vector3.negativeInfinity;
            
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, FloorMask))
                return raycastHit.point;

            return Vector3.negativeInfinity;
        }
    }
}