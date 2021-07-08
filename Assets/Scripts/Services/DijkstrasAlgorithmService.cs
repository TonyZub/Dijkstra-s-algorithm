using System.Collections.Generic;
using UnityEngine;


namespace TestAlgorithm
{
    public sealed class DijkstrasAlgorithmService
    {
        #region Fields

        private readonly ContextModel _context;

        #endregion


        #region Constructor

        public DijkstrasAlgorithmService(ContextModel context)
        {
            _context = context;
        }

        #endregion


        #region Methods

        public int[][] CreateWeightMatrix()
        {
            int matrixSize = _context.NodeModels.Count;
            int[][] weightMatrix = new int[matrixSize][];
            for (int i = 0; i < matrixSize; i++)
            {
                weightMatrix[i] = new int[matrixSize];
                for (int j = 0; j < matrixSize; j++)
                {
                    if (i == j)
                    {
                        weightMatrix[i][j] = 0;
                    }
                    else if (j < i)
                    {
                        weightMatrix[i][j] = weightMatrix[j][i];
                    }
                    else
                    {
                        int minIndex = Mathf.Min(i, j);
                        int maxIndex = Mathf.Max(i, j);
                        EdgeData foundData = _context.EdgeDatas.Find(x => x.NodeStartIndex == minIndex &&
                            x.NodeEndIndex == maxIndex);
                        weightMatrix[i][j] = foundData == null ? int.MaxValue : foundData.Weight;
                    }
                }
            }
            return weightMatrix;
        }

        public List<EdgeModel> GetEdgesInShortestPath(NodeModel firstNode, NodeModel secondNode)
        {
            int firstIndex = Mathf.Min(firstNode.Index, secondNode.Index);
            int secondIndex = Mathf.Max(firstNode.Index, secondNode.Index);
            List<EdgeModel> edgesList = new List<EdgeModel>();

            if (GetShortNodesPath(firstIndex, secondIndex, out List<ShortNodeData> nodesDataPool))
            {
                edgesList = GetEdgesPathFromShortNodeDatas(nodesDataPool, firstIndex, secondIndex);
            }

            return edgesList;
        }

        public bool GetShortNodesPath(int firstIndex, int secondIndex, out List<ShortNodeData> nodesDataPool)
        {
            bool hasPath = false;
            int maximalIterations = _context.EdgeModels.Count * 2;
            List<EdgeModel> newPathEdges = new List<EdgeModel>();
            List<EdgeData> visitedEdges = new List<EdgeData>();
            List<ShortNodeData> nodesPool = new List<ShortNodeData>();
            List<ShortNodeData> nodesQueue = new List<ShortNodeData>();
            List<int> visitedNodeIndexes = new List<int>();

            for (int i = 0; i < _context.NodeModels.Count; i++)
            {
                nodesPool.Add(new ShortNodeData(_context.NodeModels[i].Index,
                    _context.NodeModels[i].Index, int.MaxValue));
            }
            nodesPool[firstIndex] = new ShortNodeData(firstIndex, firstIndex, 0);
            nodesQueue.Add(nodesPool[firstIndex]);

            int iteration = 0;
            while (true)
            {
                ShortNodeData currentNode = nodesQueue[0];
                int currenNodeIndex = currentNode.Index;
                List<EdgeData> connectedEdges = _context.EdgeDatas.
                    FindAll(x => x.NodeStartIndex == currentNode.Index || x.NodeEndIndex == currentNode.Index);
                if (connectedEdges.Count == 0) break;

                for (int edge = 0; edge < connectedEdges.Count; edge++)
                {
                    if (!visitedEdges.Contains(connectedEdges[edge]))
                    {
                        int connectedNodeIndex = connectedEdges[edge].NodeStartIndex == currenNodeIndex ?
                            connectedEdges[edge].NodeEndIndex : connectedEdges[edge].NodeStartIndex;
                        if (nodesPool[connectedNodeIndex].Weight == int.MaxValue || nodesPool[connectedNodeIndex].Weight >
                            connectedEdges[edge].Weight + nodesPool[currenNodeIndex].Weight)
                        {
                            nodesPool[connectedNodeIndex] = new ShortNodeData(connectedNodeIndex, currenNodeIndex,
                                connectedEdges[edge].Weight + nodesPool[currenNodeIndex].Weight);
                        }
                        visitedEdges.Add(connectedEdges[edge]);
                        if (!visitedNodeIndexes.Contains(nodesPool[connectedNodeIndex].Index) &&
                            !nodesQueue.Contains(nodesPool[connectedNodeIndex]))
                        {
                            nodesQueue.Add(nodesPool[connectedNodeIndex]);
                        }
                    }
                }

                if (currenNodeIndex == secondIndex)
                {
                    hasPath = true;
                    break;
                }

                visitedNodeIndexes.Add(nodesPool[currenNodeIndex].Index);
                nodesQueue.Remove(nodesQueue[0]);

                if (iteration > maximalIterations || nodesQueue.Count == 0)
                {
                    hasPath = false;
                    break;
                }
                else
                {
                    nodesQueue.Sort((a, b) => a.Weight.CompareTo(b.Weight));
                }
            }
            nodesDataPool = nodesPool;
            return hasPath;
        }

        public List<EdgeModel> GetEdgesPathFromShortNodeDatas(List<ShortNodeData> nodeDatas,
            int startNodeIndex, int endNodeIndex)
        {
            List<EdgeModel> edgesList = new List<EdgeModel>();
            int iterations = 0;
            List<ShortNodeData> nodePathDatas = new List<ShortNodeData>();
            ShortNodeData lastNodeData = nodeDatas.Find(x => x.Index == endNodeIndex);
            while (true)
            {
                if (lastNodeData.Index == startNodeIndex)
                {
                    break;
                }
                else if (iterations > nodeDatas.Count)
                {
                    throw new System.Exception("Too much iterations in while cycle");
                }

                int nextIndex = (nodeDatas.Find(x => x.Index == endNodeIndex)).Index;
                nodePathDatas.Add(lastNodeData);
                lastNodeData = nodeDatas.Find(x => x.Index == lastNodeData.FromNodeIndex);
                iterations++;
            }

            for (int i = 0; i < nodePathDatas.Count; i++)
            {
                int minimalIndex = Mathf.Min(nodePathDatas[i].Index, nodePathDatas[i].FromNodeIndex);
                int maximalIndex = Mathf.Max(nodePathDatas[i].Index, nodePathDatas[i].FromNodeIndex);
                EdgeModel foundEdge = _context.EdgeModels.Find(x => x.StartNodeModelIndex == minimalIndex &&
                    x.EndNodeModelIndex == maximalIndex);
                if (foundEdge != null) edgesList.Add(foundEdge);
            }
            return edgesList;
        }

        #endregion
    }
}

