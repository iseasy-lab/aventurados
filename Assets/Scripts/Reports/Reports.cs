using UnityEngine;
using System;
using UnityEngine.UI;

namespace Reports
{
    public class Reports
    {
        public static int StepCounter { get; set; }
        
        public void AddStep()
        {
            StepCounter++;
        }
    }
}