﻿using UnityEngine;
using System.Collections;

namespace UniInventory.Entity
{
    public class EntityLiving : MonoBehaviour
    {
        public float life;
        public Vector3 Position
        {
            get
            {
                return transform.position;
            }
        }

        public Vector3 LookVector
        {
            get
            {
                return transform.forward;
            }
        }
    }
}

