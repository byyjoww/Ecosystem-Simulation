using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Elysium.Minimap
{
    public class FollowTarget : MonoBehaviour
    {
        [SerializeField] RefreshGroup refreshGroup = RefreshGroup.START;
        [SerializeField] private Transform target;
        [SerializeField] private bool position = true;
        [SerializeField] private bool rotation = false;

        void Start()
        {
            if (refreshGroup != RefreshGroup.START) { return; }

            if (position) { UpdatePosition(); }
            if (rotation) { UpdateRotation(); }
        }

        private void Update()
        {
            if (refreshGroup != RefreshGroup.UPDATE) { return; }

            if (position) { UpdatePosition(); }
            if (rotation) { UpdateRotation(); }
        }

        private void FixedUpdate()
        {
            if (refreshGroup != RefreshGroup.FIXED_UPDATE) { return; }

            if (position) { UpdatePosition(); }
            if (rotation) { UpdateRotation(); }
        }

        void LateUpdate()
        {
            if (refreshGroup != RefreshGroup.LATE_UPDATE) { return; }

            if (position) { UpdatePosition(); }
            if (rotation) { UpdateRotation(); }
        }

        void UpdatePosition()
        {
            var newPosition = target.position;
            newPosition.y = transform.position.y;
            transform.position = newPosition;
        }

        void UpdateRotation()
        {
            transform.rotation = Quaternion.Euler(90f, target.eulerAngles.y, 0f);
        }
    }
}