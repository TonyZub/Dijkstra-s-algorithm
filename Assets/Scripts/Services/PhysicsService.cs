using UnityEngine;
using System.Collections.Generic;


namespace TestAlgorithm
{
    public sealed class PhysicsService
    {
        #region Constants

        private const float OBJECT_COLLIDER_RADIUS = 0.5f;
        private const float NORMAL_VECTOR_POSITION_RANDOMISER_LIMIT = 10f;
        private const float NORMAL_VECTOR_POSITION_RANDOMISER_STEP = 0.2f;
        private const int MAXIMAL_POSITION_RANDOM_ITERATIONS = 100;

        #endregion


        #region Methods

        public bool CreateRayCast(Ray rayFrom, out RaycastHit hit, float distance)
        {
            return Physics.Raycast(rayFrom, out hit, distance);
        }

        public Vector2 ValidateStartPosition(Vector2 startPosition)
        {
            startPosition.x = startPosition.x > Data.ProgrammData.ScreenEdgeRight ?
                   Data.ProgrammData.ScreenEdgeRight : startPosition.x < Data.ProgrammData.ScreenEdgeLeft ?
                        Data.ProgrammData.ScreenEdgeLeft : startPosition.x;
            startPosition.y = startPosition.y > Data.ProgrammData.ScreenEdgeUp ?
                Data.ProgrammData.ScreenEdgeUp : startPosition.y < Data.ProgrammData.ScreenEdgeDown ?
                    Data.ProgrammData.ScreenEdgeDown : startPosition.y;
            return startPosition;
        }

        public Vector2 RandomizePosition()
        {
            return new Vector2(Random.Range(Data.ProgrammData.ScreenEdgeLeft, Data.ProgrammData.ScreenEdgeRight),
                Random.Range(Data.ProgrammData.ScreenEdgeDown, Data.ProgrammData.ScreenEdgeUp));
        }

        public Vector2 RandomizePositionBetween(Vector2 firstPosition, Vector3 secondPosition, float step)
        {
            Vector2 middlePosition = new Vector2((firstPosition.x + secondPosition.x) / 2, 
                (firstPosition.y + secondPosition.y) / 2);
            float randomHorizontalMove = Random.Range(-step, step);
            float randomVerticalMove = Random.Range(-step, step);
            Vector2 randomPosition = new Vector2(middlePosition.x + randomHorizontalMove,
                middlePosition.y + randomVerticalMove);
            return ValidateStartPosition(randomPosition);
        }

        public bool IsPositionEmpty(Vector2 position)
        {
            return !Physics.CheckSphere(new Vector3(position.x, position.y, 0), OBJECT_COLLIDER_RADIUS);
        }

        public bool IsPositionEmpty(Vector2 position, List<NodeData> nodesData, List<EdgeData> edgeDatas)
        {
            bool isEmpty = true;
            for (int i = 0; i < nodesData.Count; i++)
            {
                if (Vector2.Distance(nodesData[i].Position, position) < OBJECT_COLLIDER_RADIUS)
                {
                    isEmpty = false;
                    break;
                }
            }
            for (int i = 0; i < edgeDatas.Count; i++)
            {
                if (Vector2.Distance(edgeDatas[i].Position, position) < OBJECT_COLLIDER_RADIUS)
                {
                    isEmpty = false;
                    break;
                }
            }

            return isEmpty;
        }

        public Vector2 GetRandomEmptyPosition()
        {
            Vector2 position = RandomizePosition();
            int iteration = 0;
            while (!IsPositionEmpty(position))
            {
                position = RandomizePosition();
                if(iteration > MAXIMAL_POSITION_RANDOM_ITERATIONS)
                {
                    throw new System.Exception("Too low space to spawn new object");
                }
                iteration++;
            }
            return position;
        }

        public Vector2 GetRandomEmptyPosition(List<NodeData> nodesData, List<EdgeData> edgeDatas)
        {
            Vector2 position = RandomizePosition();
            int iteration = 0;
            while (!IsPositionEmpty(position, nodesData, edgeDatas))
            {
                position = RandomizePosition();
                if (iteration > MAXIMAL_POSITION_RANDOM_ITERATIONS)
                {
                    throw new System.Exception("Too low space to spawn new object");
                }
                iteration++;
            }
            return position;
        }

        public Vector2 GetRandomEmptyPositionBetween(Vector2 firstPosition, Vector2 secondPosition)
        {
            float step = 0;
            Vector2 position = RandomizePositionBetween(firstPosition, secondPosition, step);
            while (!IsPositionEmpty(position))
            {
                position = RandomizePositionBetween(firstPosition, secondPosition, step);
                step += NORMAL_VECTOR_POSITION_RANDOMISER_STEP;
                if(step > NORMAL_VECTOR_POSITION_RANDOMISER_LIMIT)
                {
                    throw new System.Exception("Too low space to spawn new edge");
                }
            }
            return position;
        }

        public Vector2 GetRandomEmptyPositionBetween(Vector2 firstPosition, Vector2 secondPosition, 
            List<NodeData> nodesData, List<EdgeData> edgeDatas)
        {
            float step = 0;
            Vector2 position = RandomizePositionBetween(firstPosition, secondPosition, step);
            while (!IsPositionEmpty(position, nodesData, edgeDatas))
            {
                position = RandomizePositionBetween(firstPosition, secondPosition, step);
                step += NORMAL_VECTOR_POSITION_RANDOMISER_STEP;
                if (step > NORMAL_VECTOR_POSITION_RANDOMISER_LIMIT)
                {
                    throw new System.Exception("Too low space to spawn new edge");
                }
            }
            return position;
        }

        public bool ValidateNodeIndex(int nodeIndex, int maximalNodeindex)
        {
            return nodeIndex >= 0 && nodeIndex < maximalNodeindex;
        }

        #endregion
    }
}

