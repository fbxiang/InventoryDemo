using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UniInventory.Registry;
using System.Linq;

namespace gameplay
{
    public enum BallColor
    {
        MAGENTA=0xff00ff, YELLOW=0xffff00, CYAN=0x00ffff, GRAY=0xaaaaaa, 
    }

    public class BallsController : MonoBehaviour
    {
        static T GetRandomEnum<T>()
        {
            System.Array A = System.Enum.GetValues(typeof(T));
            T V = (T)A.GetValue(UnityEngine.Random.Range(0, A.Length));
            return V;
        }

        LinkedList<Ball> balls;
        public float speed = 0.04f;

        public TextAsset curveFile;
        public Vector3[] spacePoints;
        public int[] nextTimePoint;
        
        void Awake()
        {
            var lines = curveFile.text.Split('\n');
            var info = lines[0].Trim().Split(' ');
            if (info.Length != 2 || !info[0].Equals("s"))
            {
                Debug.LogError("Invalid file format");
            }
            int numberOfSteps = int.Parse(info[1]);
            spacePoints = new Vector3[numberOfSteps];
            nextTimePoint = new int[numberOfSteps];

            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                if (line.Length == 0 || line[0] == '#') continue;  // empty line or comment
                if (line[0] != 'p' && line[0] != 'n')
                {
                    Debug.LogError("Invalid file format " + line);
                }
                info = line.Split(' ');

                switch (info[0])
                {
                    case "p":  // a point
                        spacePoints[int.Parse(info[1])] = new Vector3(float.Parse(info[2]), float.Parse(info[3]), float.Parse(info[4]));
                        break;
                    case "n":  // a next relation
                        nextTimePoint[int.Parse(info[1])] = int.Parse(info[2]);
                        break;
                    default:
                        Debug.LogError("Invalid file format: " + line);
                        break;
                }
            }            
        }

        void Start()
        {
            balls = new LinkedList<Ball>();
        }

        float totalTime = 0;

        public Vector3 StartPoint
        {
            get
            {
                return spacePoints[0];
            }
        }

        public Vector3 GetCoordWithFraction(float fraction)
        {
            float index = fraction * spacePoints.Length;
            int pIndex = (int)index;
            return Vector3.Lerp(spacePoints[pIndex], spacePoints[pIndex+1], index - pIndex);
        }

        public Ball LastBall
        {
            get
            {
                return balls.Last == null ? null : balls.Last.Value;
            }
        }

        public bool canAppend()
        {
            return LastBall == null || (LastBall.transform.position - StartPoint).sqrMagnitude >= 1;
        }

        public void appendNewBall(BallColor color)
        {
            var ballObj = Instantiate(Objects.Ball);

            int HaxVal = (int)color;
            byte R = (byte)((HaxVal >> 16) & 0xFF);
            byte G = (byte)((HaxVal >> 8) & 0xFF);
            byte B = (byte)((HaxVal) & 0xFF);

            ballObj.GetComponent<Renderer>().material.SetColor("_Color", new Color32(R, G, B, 255));
            Ball ball = ballObj.GetComponent<Ball>();
            LinkedListNode<Ball> ballNode = new LinkedListNode<Ball>(ball);
            balls.AddLast(ballNode);
            ball.controller = this;
            ball.thisNode = ballNode;
            ball.transform.position = StartPoint;
            ballObj.SetActive(true);
        }

        void Update()
        {
            print("update " + Time.deltaTime);
            if (canAppend())
            {
                if (LastBall != null)
                    print((LastBall.transform.position - StartPoint).magnitude);
                appendNewBall(GetRandomEnum<BallColor>());
            }
        }
    }
}

