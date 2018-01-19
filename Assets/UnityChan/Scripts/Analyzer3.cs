using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leap;

class Analyzer3 : AbstractAnalyzer {
    public bool isDown = false;
    public int indexNow = 0;
    public Vector[,] leftPosition;//左手
    public Vector[,] rightPosition;//右手
    public double[] rightDisResult = new double[5];
    private readonly Vector vectorZero = new Vector(0, 0, 0);
    public Analyzer3() {
        leftPosition = new Vector[2, 5];
        rightPosition = new Vector[2, 5];
        for (int i = 0; i < 2; i++) {
            for (int j = 0; j < 5; j++) {
                leftPosition[i, j] = new Vector(0, 0, 0);
                rightPosition[i, j] = new Vector(0, 0, 0);
            }
        }
    }
    double CalDis(Vector v, Vector e) {
        double temp0 = System.Math.Pow((double)v.x - (double)e.x, 2.0);
        double temp1 = System.Math.Pow((double)v.y - (double)e.y, 2.0);
        double temp2 = System.Math.Pow((double)v.z - (double)e.z, 2.0);
        double res = System.Math.Sqrt(temp0 + temp1 + temp2);
        return res;
    }
    void SaveData(Hand hand) {
        if (hand != null) {
            if (hand.IsRight) {//右手
                foreach (Finger finger in hand.Fingers) {///有可能有手指没有检测到·
                    Finger.FingerType fingerType = finger.Type;
                    switch (fingerType) {
                        case Finger.FingerType.TYPE_INDEX:
                            rightPosition[indexNow, 0] = finger.TipPosition;
                            break;
                        case Finger.FingerType.TYPE_MIDDLE:
                            rightPosition[indexNow, 1] = finger.TipPosition;
                            break;
                        case Finger.FingerType.TYPE_PINKY:
                            rightPosition[indexNow, 2] = finger.TipPosition;
                            break;
                        case Finger.FingerType.TYPE_RING:
                            rightPosition[indexNow, 3] = finger.TipPosition;
                            break;
                        case Finger.FingerType.TYPE_THUMB:
                            rightPosition[indexNow, 4] = finger.TipPosition;
                            break;
                        default:
                            break;
                    }
                }
            } else {
                foreach (Finger finger in hand.Fingers) {///有可能有手指没有检测到·
                    Finger.FingerType fingerType = finger.Type;
                    switch (fingerType) {
                        case Finger.FingerType.TYPE_INDEX:
                            leftPosition[indexNow, 0] = finger.TipPosition;
                            break;
                        case Finger.FingerType.TYPE_MIDDLE:
                            leftPosition[indexNow, 1] = finger.TipPosition;
                            break;
                        case Finger.FingerType.TYPE_PINKY:
                            leftPosition[indexNow, 2] = finger.TipPosition;
                            break;
                        case Finger.FingerType.TYPE_RING:
                            leftPosition[indexNow, 3] = finger.TipPosition;
                            break;
                        case Finger.FingerType.TYPE_THUMB:
                            leftPosition[indexNow, 4] = finger.TipPosition;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
    override public bool Analyze(Frame frame) {
        int count = frame.Hands.Count;
        if (count == 0) {//没有手
            for (int i = 0; i < 5; i++) {
                leftPosition[indexNow, i] = vectorZero;
                rightPosition[indexNow, i] = vectorZero;
            }
        } else if (count == 1) {
            Hand hand = frame.Hands[0];
            bool isRight = hand.IsRight;
            if (isRight) {//右手
                SaveData(hand);
                for (int i = 0; i < 5; i++) {//左手没有检测到，置0
                    leftPosition[indexNow, i] = vectorZero;
                }
            } else {//左手
                SaveData(hand);
                for (int i = 0; i < 5; i++) {//左手没有检测到，置0
                    rightPosition[indexNow, i] = vectorZero;
                }
            }
        } else {//左右手都有
            SaveData(frame.Hands[0]);
            SaveData(frame.Hands[1]);
        }
        // Debug.Log (RightPosition [Index_Now,0]);

        //手信息存储完毕
        //根据右手两帧之间的不同来判断是不是点击手势
        //Index帧前一帧为(Index+1)%2
        for (int i = 0; i < 5; i++) {
            rightDisResult[i] = CalDis(rightPosition[indexNow, i], rightPosition[(indexNow + 1) % 2, i]);
            //Debug.Log (RightDisResult [i]);
        }
        int tt = 0;//另外4根手指中移动距离<2.5的个数
        for (int i = 1; i < 5; i++) {
            if (rightDisResult[i] < 2.5) {
                tt++;
            }
        }
        bool hasDrop = false;
        if (rightPosition[indexNow, 1].y - rightPosition[indexNow, 0].y > 15)
            hasDrop = true;
        bool isTrue = false;
        if (hasDrop && rightDisResult[0] > 3.0 && tt >= 3 && !isDown && rightPosition[indexNow, 0].y < rightPosition[(indexNow + 1) % 2, 0].y) {
            isTrue = true;
            Vector hit = rightPosition[indexNow, 0];
            int hitX = 1, hitZ = 1;
            if (hit.x < -30) hitX = 0;
            if (hit.x > 30) hitX = 2;
            if (hit.z < -30) hitZ = 0;
            if (hit.z > 30) hitZ = 2;
            int hitValue = hitX * 3 + hitZ;
            isDown = true;

        }
        if (rightPosition[indexNow, 0].y > rightPosition[(indexNow + 1) % 2, 0].y + 1.0) {
            isDown = false;
        }
        indexNow = (indexNow + 1) % 2;
        return isTrue;
    }
}
