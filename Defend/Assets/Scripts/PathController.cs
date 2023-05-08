using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Controllers {
    public class PathController {
        private readonly List<Vector2Int> _occupiedCells = new();
        private readonly Vector2Int _enemySpawnCell, _enemyGoalCell;

        private const int _boardSize = 10;
        private Transform _debugDotsParent;
        
        PathNode _pathEndNode = null; // the end node from the last search
        private List<PathNode> _nodesToExplore = new();
        private List<PathNode> _exploredNodes = new();
        private List<Vector2Int> _path = new();

        public PathController(Vector2Int enemySpawnCell, Vector2Int enemyGoalCell, Transform debugDotsParent) {
            _enemySpawnCell = enemySpawnCell;
            _enemyGoalCell = enemyGoalCell;
            _debugDotsParent = debugDotsParent;
        }

        public void SetCellsOccupied(IEnumerable<Vector2Int> cells) {
            _occupiedCells.AddRange(cells);
        }
        public void SetCellsFree(IEnumerable<Vector2Int> cells) {
            foreach (var cell in cells.Where(cell => _occupiedCells.Contains(cell))) {
                _occupiedCells.Remove(cell);
            }
        }

        public bool CheckCellsAreFree(IEnumerable<Vector2Int> cellsToCheck) {
            return !cellsToCheck.Any(CheckCellIsOccupied);
        }

        private bool CheckCellIsOccupied(Vector2Int cell) {
            return _occupiedCells.Contains(cell);
        }

        public IEnumerable<Vector2Int> RefreshPath() {
            var path = GetPath(_enemySpawnCell, _enemyGoalCell);
            DrawDebugPath();
            return path;
        }

        public List<Vector2Int> GetPath(Vector2Int startCell, Vector2Int endCell) {
            Search(startCell, endCell);//.GetEnumerator();
            // var done = false;
            // do {
            //     // convert this to manually triggered moves. Or automatic, but move the debug pieces between moves.
            //     done = enumerable.MoveNext();
            //
            //     //DrawDebugPath();
            // } while (!done); 

            // construct the final path from the parents of the node that reached the goal
            _path.Clear();
            var endNode = _pathEndNode;
            if (endNode is null){ Debug.Log("Failed to find end cell :("); return _path; }
            while (endNode.ParentNode is not null) {
                _path.Add(endNode.Cell);
                endNode = endNode.ParentNode;
            }
            return _path;
        }

        /// <summary>
        /// Returns bool for whether it's done or not.
        /// The path can be extracted by iterating through it's parents.
        /// </summary>
        /// <param name="startCell"></param>
        /// <param name="endCell"></param>
        /// <returns></returns>
        private bool Search(Vector2Int startCell, Vector2Int endCell) {
            var shortCircuit = 2000; // limit the searches in case it all goes wrong
            _pathEndNode = null;
            _exploredNodes.Clear();
            _nodesToExplore.Clear();
            _nodesToExplore.Add(new PathNode(startCell, null));

            while (_nodesToExplore.Any()) {
                var promisingNode = _nodesToExplore.Aggregate((n1, n2) => n1.ClosenessMetric < n2.ClosenessMetric ? n1 : n2); // Q
                _nodesToExplore.Remove(promisingNode);

                // look at promisingNode's neighbors
                for (var i = -1; i <= 1; i++) {
                    for (var j = -1; j <= 1; j++) {
                        if (i == 0 && j == 0) continue; // dont re-add the parentCell
                        var neighbor = new PathNode(new Vector2Int(promisingNode.Cell.x + i, promisingNode.Cell.y + j), promisingNode);

                        // if one of the neighbors is the goal, exit
                        if (neighbor.Cell == endCell) {
                            _pathEndNode = neighbor;
                            Debug.Log("Found end cell! :)");
                            break;
                        }

                        if (_occupiedCells.Any(c => c == neighbor.Cell)) continue; // ignore cells in our ignore-list
                        if (_nodesToExplore.Any(n => n.Cell == neighbor.Cell && n.ClosenessMetric < neighbor.ClosenessMetric))
                            continue; // if there's already a better-valued node in the openList with this cell, skip this neighbor
                        if (_exploredNodes.Any(n => n.Cell == neighbor.Cell && n.ClosenessMetric < neighbor.ClosenessMetric))
                            continue; // if there's already a better-valued node in the closedList with this cell, skip this neighbor

                        neighbor.DistanceFromStart = promisingNode.DistanceFromStart + Vector2Int.Distance(neighbor.Cell, promisingNode.Cell);
                        neighbor.DistToGoal = Vector2Int.Distance(neighbor.Cell, endCell); // TODO: check common methods for this
                        neighbor.ClosenessMetric = neighbor.DistanceFromStart + neighbor.DistToGoal;

                        _nodesToExplore.Add(neighbor);
                    }
                }

                if (_pathEndNode is not null) break;

                _exploredNodes.Add(promisingNode);

                shortCircuit--;
                if (shortCircuit <= 0) break;
            }
            return true; // done
        }

        /// <summary>
        /// Draws the results of the algorithm at this point in time.
        /// </summary>
        private void DrawDebugPath() {
            var path = _path;
            
            foreach (Transform dot in _debugDotsParent) {
                dot.gameObject.SetActive(false);
            }

            for (var i = 0; i < path.Count; i++) {
                if (i >= _debugDotsParent.childCount-1) {
                    Debug.Log("Need more dots to draw this path: " + path.Count);
                    return;
                }
                var d = _debugDotsParent.GetChild(i);
                var p = path[i];
                d.gameObject.SetActive(true);
                d.transform.position = new Vector3(p.x, 0, p.y);
            }
        }
    }

    public class PathNode {
        public readonly Vector2Int Cell;
        public readonly PathNode ParentNode;
        public float DistanceFromStart; // G
        public float DistToGoal;        // H
        public float ClosenessMetric;   // F
        public PathNode(Vector2Int cell, PathNode parentNode) {
            Cell = cell;
            ParentNode = parentNode;
        }
    }
}