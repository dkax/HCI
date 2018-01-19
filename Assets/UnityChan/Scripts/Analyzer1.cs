using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leap;

class Analyzer1 : AbstractAnalyzer {
    private bool lastResult = false;
    override public bool Analyze(Frame frame) {
        bool result = false;
        foreach (Hand hand in frame.Hands) {
            if (hand.GrabStrength >= 0.95) {
                    result = true;
            }
        }
        if (result != lastResult)
            lastResult = result;
        else
            result = false;
        return result;
    }
}
