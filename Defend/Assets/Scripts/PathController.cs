using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Controllers {
    public class PathController {
        private readonly List<Vector2> _occupiedCells = new();

        public void SetCellsOccupied(IEnumerable<Vector2> cells) {
            _occupiedCells.AddRange(cells);
        }
        public void SetCellsFree(IEnumerable<Vector2> cells) {
            foreach (var cell in cells.Where(cell => _occupiedCells.Contains(cell))) {
                _occupiedCells.Remove(cell);
            }
        }

        public bool CheckCellsAreFree(IEnumerable<Vector2> cellsToCheck) {
            return !cellsToCheck.Any(cell => _occupiedCells.Contains(cell));
        }
    }
}