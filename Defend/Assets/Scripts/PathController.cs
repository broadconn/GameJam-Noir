using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Controllers {
    public class PathController {
        private readonly List<Vector2Int> _occupiedCells = new();
        private Vector2Int _enemySpawnCell, _enemyGoalCell;
        private readonly Transform _debugPathDotsParent;
        private readonly Transform _debugToExploreDotsParent;
        private readonly Transform _debugCheckedDotsParent;
        
        private PathNode _pathEndNode = null; // the end node from the last search
        private readonly List<PathNode> _nodesToExplore = new();
        private readonly List<PathNode> _exploredNodes = new();

        public PathController(Vector2Int enemySpawnCell, Vector2Int enemyGoalCell, Transform debugDotsParent) {
            _enemySpawnCell = enemySpawnCell;
            _enemyGoalCell = enemyGoalCell;
            
            _debugPathDotsParent = debugDotsParent.Find("Path");
            _debugToExploreDotsParent = debugDotsParent.Find("Explore");
            _debugCheckedDotsParent = debugDotsParent.Find("Checked");
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

        /// <summary>
        /// Returns the list of cells making up the path.
        /// </summary>
        /// <param name="startCell"></param>
        /// <param name="endCell"></param>
        /// <returns></returns>W
        public List<Vector2Int> FullSearch() {
            InitNewSearch(_enemySpawnCell, _enemyGoalCell);

            var maxSteps = 2000;
            while(_pathEndNode == null || maxSteps >= 0) {
                StepSearch();
                maxSteps--;
            }

            return GetV2Path();
        }

        public void InitNewSearch(Vector2Int startCell, Vector2Int endCell) {
            _enemySpawnCell = startCell;
            _enemyGoalCell = endCell;
            InitNewSearch();
        }
        public void InitNewSearch() {
            _pathEndNode = null;
            _exploredNodes.Clear();
            _nodesToExplore.Clear();
            _nodesToExplore.Add(new PathNode(null, _enemySpawnCell));
        }
        
        public void StepSearch() {
            if (_pathEndNode is not null
                || !_nodesToExplore.Any()) {
                return;
            }
            
            // get the most promising node from the explore list
            var promisingNode = _nodesToExplore.Aggregate((n1, n2) => n1.ClosenessMetric < n2.ClosenessMetric ? n1 : n2); // Q
            _nodesToExplore.Remove(promisingNode);

            // look at promisingNode's neighbors
            for (var i = -1; i <= 1; i++) {
                for (var j = -1; j <= 1; j++) {
                    if (i == 0 && j == 0) continue; // dont re-add the parentCell
                    var neighbor = new PathNode(promisingNode, new Vector2Int(promisingNode.Cell.x + i, promisingNode.Cell.y + j));

                    // if one of the neighbors is the goal, exit
                    if (neighbor.Cell == _enemyGoalCell) {
                        _pathEndNode = neighbor;
                        return;
                    }

                    if (_occupiedCells.Any(c => c == neighbor.Cell)) 
                        continue; // ignore cells in our ignore-list
                    if (_nodesToExplore.Any(n => n.Cell == neighbor.Cell /*&& n.ClosenessMetric < neighbor.ClosenessMetric*/))
                        continue; // if there's already a better-valued node in the openList with this cell, skip this neighbor
                    if (_exploredNodes.Any(n => n.Cell == neighbor.Cell /*&& n.ClosenessMetric < neighbor.ClosenessMetric*/))
                        continue; // if there's already a better-valued node in the closedList with this cell, skip this neighbor
                    
                    if (i != 0 && j != 0) // if cell is diagonal from neighbor, ignore it if either of the 0-axis neighbors are occupied.
                        if (_occupiedCells.Any(c => c == new Vector2Int(promisingNode.Cell.x + i, promisingNode.Cell.y) && _occupiedCells.Any(cl => cl == new Vector2Int(promisingNode.Cell.x, promisingNode.Cell.y + j))))
                            continue;

                    neighbor.DistanceFromStart = promisingNode.DistanceFromStart + Vector2Int.Distance(neighbor.Cell, promisingNode.Cell);
                    neighbor.DistToGoal = Vector2Int.Distance(neighbor.Cell, _enemyGoalCell); // TODO: check common methods for this (Heuristics: Manhattan, Diagonal, or Euclidean)
                    neighbor.ClosenessMetric = neighbor.DistanceFromStart + neighbor.DistToGoal;

                    _nodesToExplore.Add(neighbor);
                }
            }

            _exploredNodes.Add(promisingNode);  
        }

        private List<Vector2Int> GetV2Path() {
            List<Vector2Int> path = new();
            var endNode = _pathEndNode;
            if (endNode is null){ return path; }
            while (endNode.ParentNode is not null) {
                path.Add(endNode.Cell);
                endNode = endNode.ParentNode;
            }
            return path;
        }

        public void DrawAllDebug() {
            DrawDebugPath();
            DrawDotsWithNodeList(_debugCheckedDotsParent, _exploredNodes);
            DrawDotsWithNodeList(_debugToExploreDotsParent, _nodesToExplore);
        } 
        
        public void DrawDebugPath() {
            DrawDotsWithV2List(_debugPathDotsParent, GetV2Path());
        }

        private static void DrawDotsWithV2List(Transform dotsParent, List<Vector2Int> cells) {
            foreach (Transform dot in dotsParent) {
                dot.gameObject.SetActive(false);
            }

            for (var i = 0; i < cells.Count; i++) {
                if (i >= dotsParent.childCount-1) {
                    return;
                }
                var c = cells[i];
                var d = dotsParent.GetChild(i);
                d.gameObject.SetActive(true);
                d.transform.position = new Vector3(c.x, 0, c.y);
            }
        }

        private static void DrawDotsWithNodeList(Transform dotsParent, List<PathNode> cells) {
            foreach (Transform dot in dotsParent) {
                dot.gameObject.SetActive(false);
            }

            for (var i = 0; i < cells.Count; i++) {
                if (i >= dotsParent.childCount-1) {
                    return;
                }
                
                var c = cells[i];
                var d = dotsParent.GetChild(i);
                d.gameObject.SetActive(true);
                d.transform.position = new Vector3(c.Cell.x, 0, c.Cell.y);
                    
                var debugCube = d.GetComponent<DebugCube>();
                debugCube.FMetric = c.ClosenessMetric;
                debugCube.DistFromStart = c.DistanceFromStart;
                debugCube.EstDistFromEnd = c.DistToGoal;
            }
        }
    }

    public class PathNode {
        public readonly Vector2Int Cell;
        public readonly PathNode ParentNode;
        public float DistanceFromStart; // G
        public float DistToGoal;        // H
        public float ClosenessMetric;   // F
        public PathNode(PathNode parentNode, Vector2Int cell) {
            Cell = cell;
            ParentNode = parentNode;
        }
    }
}