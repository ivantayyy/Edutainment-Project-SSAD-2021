using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets
{
    public class AssignmentResults
    {
        public List<int> points;
        public int attempts;
        public int maxPoint;

        public AssignmentResults()
        {
            this.points = new List<int>();
        }
    }
}