using UnityEngine;

namespace Code.Runtime.AI
{
    public class Path
    {
        public readonly Vector3[] lookPoints;
        public readonly Line[] turnBoundaries;
        public readonly int finishLineIndex;

        public Path(Vector3[] waypoints, Vector3 startPos, float turnDst)
        {
            lookPoints = waypoints;
            turnBoundaries = new Line[lookPoints.Length];
            finishLineIndex = turnBoundaries.Length - 1;
        
            Vector2 previousPoint = startPos;
            for (int i = 0; i < lookPoints.Length; i++)
            {
                Vector2 currentPoint = lookPoints[i];
                Vector2 toCurrentPoint = (currentPoint - previousPoint).normalized;
                var turnBoundary = (i == finishLineIndex) ? currentPoint : currentPoint - toCurrentPoint * turnDst;
                turnBoundaries[i] = new Line(turnBoundary, previousPoint - toCurrentPoint * turnDst);
                previousPoint = turnBoundary;
            }
        }
    
        public void DrawPath()
        {
            Gizmos.color = Color.black;
            foreach (var p in lookPoints)
            {
                Gizmos.DrawCube(p, new Vector3(0.4f,0.4f,0.4f));
            }
            Gizmos.color = Color.white;

            foreach (var line in turnBoundaries)
            {
                line.DrawLine(3f);
            }
        }
    }
}
