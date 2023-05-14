using UnityEngine;

namespace Code.Runtime.AI
{
    public struct Line
    {
        private const float VerticalLineGradient = 1e5f;

        private readonly float _gradient;
        private readonly Vector2 _pointOnLine1;
        private readonly Vector2 _pointOnLine2;

        private readonly bool _approachSide;

        public Line(Vector2 pointOnLine, Vector2 pointPerpendicularToLine) {
            float dx = pointOnLine.x - pointPerpendicularToLine.x;
            float dy = pointOnLine.y - pointPerpendicularToLine.y;

            if (dy == 0) {
                _gradient = VerticalLineGradient;
            } else {
                _gradient = -dx / dy;
            }
            
            _pointOnLine1 = pointOnLine;
            _pointOnLine2 = pointOnLine + new Vector2 (1, _gradient);

            _approachSide = false;
            _approachSide = GetSide (pointPerpendicularToLine);
        }

        private bool GetSide(Vector2 p) {
            return (p.x - _pointOnLine1.x) * (_pointOnLine2.y - _pointOnLine1.y) > (p.y - _pointOnLine1.y) * (_pointOnLine2.x - _pointOnLine1.x);
        }

        public bool HasCrossedLine(Vector2 p) {
            return GetSide (p) != _approachSide;
        }

        public void DrawLine(float length)
        {
            var lineDirection = new Vector3(1, _gradient,0).normalized;
            var lineCenter = new Vector3(_pointOnLine1.x, _pointOnLine1.y,0 );
            Gizmos.DrawLine(lineCenter - lineDirection * length/ 2f, lineCenter + lineDirection * length/2f);
        
        }
    }
}