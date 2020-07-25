using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShot : MonoBehaviour
{
    #region Variables
    // * //
    #region ROPE
    [Header("Rope")]
    public float segmentLength = 0.25f;
    public int segmentCount = 35;
    public float segLineWidth = 0.1f;
    public float gravityScale = 1;
    private LineRenderer lineRenderer;
    private List<RopeSegment> ropeSegments = new List<RopeSegment>();
    #endregion

    #region BRIDGE
    [Header("Bridge")]
    public bool isBridge = true;
    public bool useCollider = true;
    public Transform StartPoint;
    public Transform EndPoint;
    #endregion

    #region SLING SHOT
    [Header("Sling Shot")]
    public bool useMouse = false;
    public Vector2 yMouseClamping;
    bool moveToMouse = false;
    int indexMousePos;
    Vector3 mousePositionWorld;
    #endregion

    #region COLLISON
    [Header("Collision")]
    public int currentColPoint; //TODO: Public for debugging only!
    private EdgeCollider2D edgeCollider;
    #endregion

    #region SHAPE
    [Header("Shape"), Tooltip("CURRENTLY NOT IN USE!!")]
    public Deviation[] deviations;
    #endregion

    public float elasticityMultiplier = .5f;
    #endregion


    #region Defaults
    void Start()
    {
        #region Component Initialization
        this.lineRenderer = this.GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        #endregion

        #region Bridge Shape Setup
        Vector3 ropeStartPoint = StartPoint.position;
        for (int i = 0; i < segmentCount; i++)
        {
            this.ropeSegments.Add(new RopeSegment(ropeStartPoint));
            ropeStartPoint.y -= segmentLength;
        }
        #endregion

        #region Collider Setup
        if (useCollider && edgeCollider != null)
        {
            edgeCollider.enabled = true;
            edgeCollider.edgeRadius = segLineWidth / 2;
        }
        else
            edgeCollider.enabled = false;
        #endregion
    }

    #region Updates
    void Update()
    {
        this.DrawRope();

        #region Mouse Testing Setup
        if (useMouse)
        {
            #region Input Handling
            if (Input.GetMouseButtonDown(0))
            {
                this.moveToMouse = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                this.moveToMouse = false;
            }
            #endregion

            #region Calculations
            Vector3 screenMousePos = Input.mousePosition;
            float xStart = StartPoint.position.x;
            float xEnd = EndPoint.position.x;
            this.mousePositionWorld = Camera.main.ScreenToWorldPoint(new Vector3(screenMousePos.x, screenMousePos.y, 10));
            float currX = this.mousePositionWorld.x; //followObject.transform.position.x
            #endregion

            #region Mouse Index Ratio
            float ratio = (currX - xStart) / (xEnd - xStart);
            if (ratio > 0)
            {
                this.indexMousePos = (int)(this.segmentCount * ratio);
            }
            #endregion
        }
        #endregion
    }

    private void FixedUpdate()
    {
        this.Simulate();
    }
    #endregion
    #endregion

    private void OnCollisionEnter2D(Collision2D other)
    {
        int closestIndex = 805;
        float closestDist = float.MaxValue;

        for (int i = 0; i < edgeCollider.points.Length; i++)
        {
            Vector2 edgePoint = edgeCollider.points[i];

            float thisDist = Vector2.Distance(edgePoint, other.GetContact(0).point);

            if (thisDist < closestDist)
            {
                closestDist = thisDist;
                closestIndex = i;
            }

            currentColPoint = closestIndex;
        }

        #region TESTING //!REMOVE LATER
        // print(closestIndex);
        // GameObject go = new GameObject();
        // go.transform.position = edgeCollider.points[closestIndex];
        // go.name = "Collision w/ Point " + closestIndex;
        #endregion
    }

    private void Simulate()
    {
        // SIMULATION
        Vector2 forceGravity = new Vector2(0f, gravityScale * -1);

        for (int i = 1; i < this.segmentCount; i++)
        {
            RopeSegment firstSegment = this.ropeSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity;
            firstSegment.posNow += forceGravity * Time.fixedDeltaTime;
            this.ropeSegments[i] = firstSegment;
        }

        //CONSTRAINTS
        for (int i = 0; i < 50; i++)
        {
            this.ApplyConstraint();
        }
    }

    private void ApplyConstraint()
    {
        //
        for (int i = 0; i < this.segmentCount - 1; i++)
        {
            RopeSegment firstSeg = this.ropeSegments[i];
            RopeSegment secondSeg = this.ropeSegments[i + 1];

            float dist = (firstSeg.posNow - secondSeg.posNow).magnitude;
            float error = Mathf.Abs(dist - this.segmentLength);
            Vector2 changeDir = Vector2.zero;

            if (dist > segmentLength)
            {
                changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;
            }
            else if (dist < segmentLength)
            {
                changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
            }

            Vector2 changeAmount = changeDir * error;
            if (i != 0)
            {
                firstSeg.posNow -= changeAmount * .5f;
                this.ropeSegments[i] = firstSeg;
                secondSeg.posNow += changeAmount * .5f;
                this.ropeSegments[i + 1] = secondSeg;
            }
            else
            {
                secondSeg.posNow += changeAmount;
                this.ropeSegments[i + 1] = secondSeg;
            }
            //*
            Vector2[] colliderPoints = new Vector2[this.segmentCount];
            colliderPoints[i] = this.ropeSegments[i].posNow;

            if (this.moveToMouse && (indexMousePos > 1 && indexMousePos < this.segmentCount - 2) && i == indexMousePos && (yMouseClamping.x < mousePositionWorld.y && mousePositionWorld.y < yMouseClamping.y))
            {
                RopeSegment thisSegment = this.ropeSegments[i];
                RopeSegment nextSegment = this.ropeSegments[i + 1];
                thisSegment.posNow = new Vector2(this.mousePositionWorld.x, this.mousePositionWorld.y);
                nextSegment.posNow = new Vector2(this.mousePositionWorld.x, this.mousePositionWorld.y);
                // thisSegment.posNow = new Vector2 (this.followObject.transform.position.x, this.followObject.transform.position.y);
                // nextSegment.posNow = new Vector2 (this.followObject.transform.position.x, this.followObject.transform.position.y);
                this.ropeSegments[i] = thisSegment;
                this.ropeSegments[i + 1] = nextSegment;
            }

            if (edgeCollider.points.Length == 0)
                print("**NULL**");
            else if (useCollider)
                edgeCollider.points[i] = colliderPoints[i];
        }

        #region Brigde Functionality (Start && End Points)
        if (isBridge)
        {
            #region Constraint First Segment to Start Point
            RopeSegment startSegment = this.ropeSegments[0];
            startSegment.posNow = this.StartPoint.position;
            this.ropeSegments[0] = startSegment;
            #endregion

            #region Constrant Last Segment to End Point
            RopeSegment endSegment = this.ropeSegments[this.ropeSegments.Count - 1];
            endSegment.posNow = this.EndPoint.position;
            this.ropeSegments[this.ropeSegments.Count - 1] = endSegment;
            #endregion
        }
        #endregion

        //? Currently not working (deviations):
        // if (deviations != null && deviations.Length > 0)
        // {
        //     for (int i = 0; i < deviations.Length; i++)
        //     {
        //         RopeSegment tempSeg = ropeSegments[deviations[i].ropeIndex];
        //         tempSeg.posNow = deviations[i].transform.transform.position;
        //         ropeSegments[deviations[i].ropeIndex] = tempSeg;
        //     }
        // }
    }

    private void DrawRope()
    {
        float lineWidth = this.segLineWidth;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Vector3[] ropePositions = new Vector3[this.segmentCount];

        for (int i = 0; i < this.segmentCount; i++)
        {
            ropePositions[i] = this.ropeSegments[i].posNow;
        }

        lineRenderer.positionCount = ropePositions.Length;
        lineRenderer.SetPositions(ropePositions);

        if (useCollider)
            CreateColliders();
    }

    private void CreateColliders()
    {
        Vector2[] colliderPoints = new Vector2[this.segmentCount];

        for (int i = 0; i < this.segmentCount; i++)
        {
            colliderPoints[i] = this.ropeSegments[i].posNow;
        }

        edgeCollider.points = colliderPoints;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y + yMouseClamping.x), .25f);
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y + yMouseClamping.y), .25f);

        Gizmos.DrawWireSphere(StartPoint.transform.position, .25f);
        Gizmos.DrawWireSphere(EndPoint.transform.position, .25f);
    }


    public struct RopeSegment
    {
        public Vector2 posNow;
        public Vector2 posOld;

        public RopeSegment(Vector2 pos)
        {
            this.posNow = pos;
            this.posOld = pos;
        }
    }

    [System.Serializable]
    public struct Deviation
    {
        public string name;
        public Transform transform;
        public int ropeIndex;
    }
}