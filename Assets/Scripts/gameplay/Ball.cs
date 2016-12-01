using UnityEngine;
using System.Collections.Generic;

namespace gameplay
{
    public class Ball : MonoBehaviour
    {
        public LinkedListNode<Ball> thisNode;
        public BallsController controller;

        public bool moving = true;

        float currentFraction; // a float value between 0 and 1

        public float SqrDistanceFrom(Ball other)
        {
            return (other.transform.position - this.transform.position).sqrMagnitude;
        }

        public Ball GetNext()
        {
            return thisNode.Next == null ? null : thisNode.Next.Value;
        }

        public Ball GetPrev()
        {
            return thisNode.Previous == null ? null : thisNode.Previous.Value;
        }

        void FixedUpdate() {
            currentFraction += Time.deltaTime * controller.speed;
            transform.position = controller.GetCoordWithFraction(currentFraction);
        }
    }
}
