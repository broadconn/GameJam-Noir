using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Controllers {
    public class PathController {
        private readonly List<Vector2> _occupiedCells = new();
        
        public PathController() {
            
        }

        public void SetCellsOccupied(List<Vector2> cells) {
            foreach (var cell in cells) {
                _occupiedCells.Add(cell);
            }
        }
        public void SetCellsFree(IEnumerable<Vector2> cells) {
            foreach (var cell in cells.Where(cell => _occupiedCells.Contains(cell))) {
                _occupiedCells.Remove(cell);
            }
        }

        public bool CheckCellsAreFree(List<Vector2> cellsToCheck) {
            return !cellsToCheck.Any(cell => _occupiedCells.Contains(cell));
        }
    }
}