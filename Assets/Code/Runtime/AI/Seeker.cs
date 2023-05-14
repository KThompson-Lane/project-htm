using System.Collections;
using UnityEngine;

namespace Code.Runtime.AI
{
    public class Seeker : MonoBehaviour
    {
        public float minPathUpdateTime = .2f;
        public float pathUpdateThreshold = .5f;

        public float speed = 5;
        public float turnDistance = 5f;
        public float turnSpeed = 3;
        private Path _currentPath;

        private int _targetIndex;

        private bool _moving;

        public void MoveToTarget(Transform target)
        {
            if (!_moving)
            {
                StartCoroutine(UpdatePath(target));
                _moving = true;
            }
        }

        public void StopMoving()
        {
            StopAllCoroutines();
            _moving = false;
        }

        void OnPathFound(Vector3[] waypoints, bool success)
        {
            if (success)
            {
                _currentPath = new Path(waypoints, transform.position, turnDistance);
                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
            }
        }

        private IEnumerator UpdatePath(Transform target)
        {
            if (Time.timeSinceLevelLoad < .3f)
            {
                yield return new WaitForSeconds(.3f);
            }
            PathManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));

            float sqrMoveThreshold = pathUpdateThreshold * pathUpdateThreshold;
            Vector3 targetPosPrev = target.position;
        
            while (true)
            {
                yield return new WaitForSeconds(minPathUpdateTime);
                if ((target.position - targetPosPrev).sqrMagnitude > sqrMoveThreshold)
                {
                    PathManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));
                    targetPosPrev = target.position;
                }
            }
        }
    
        private IEnumerator FollowPath()
        {
            bool followingPath = true;
            int pathIndex = 0;
            transform.up= Vector3.Lerp(transform.up, (_currentPath.lookPoints[pathIndex] - transform.position), Time.deltaTime * turnSpeed);
            while (followingPath)
            {
                Vector2 position = transform.position;
                while (_currentPath.turnBoundaries[pathIndex].HasCrossedLine(position))
                {
                    if (pathIndex == _currentPath.finishLineIndex)
                    {
                        followingPath = false;
                        break;
                    }
                    pathIndex++;
                }
                if (followingPath)
                {
                    transform.up= Vector3.Lerp(transform.up, (_currentPath.lookPoints[pathIndex] - transform.position), Time.deltaTime * turnSpeed);
                    transform.Translate(Vector3.up * Time.deltaTime * speed, Space.Self);
                }
                yield return null;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_currentPath != null)
            {
                _currentPath.DrawPath();
            }
        }
    }
}
