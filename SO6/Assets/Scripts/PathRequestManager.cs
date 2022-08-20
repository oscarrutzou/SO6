using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequestManager : MonoBehaviour
{
    
    Queue<PathRequest> pathRequestsQueue = new Queue<PathRequest>();
    PathRequest currenPathRequest;

    static PathRequestManager instance;
    Pathfinding pathfinding;

    bool isProcessingPath;


    private void Awake()
    {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestsQueue.Enqueue(newRequest);
        instance.TryProccesNext();
    }

    void TryProccesNext()
    {
        if (!isProcessingPath && pathRequestsQueue.Count > 0)
        {
            currenPathRequest = pathRequestsQueue.Dequeue(); //Tager den ud af Queue
            isProcessingPath = true;
            pathfinding.StartFindPath(currenPathRequest.pathStart, currenPathRequest.pathEnd);
        }
    }

    public void FinishProcessingPath(Vector3[] path, bool succes)
    {
        currenPathRequest.callback(path, succes);
        isProcessingPath = false;
        TryProccesNext();
    }

    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }

}
