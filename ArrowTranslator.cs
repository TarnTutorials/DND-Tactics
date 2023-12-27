using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ArrowTranslator
{
    public enum ArrowDirection
    {
        None = 0,
        Vertical = 1,
        Horizontal = 2,
        TopLeft = 3,
        TopRight = 4,
        BottomLeft = 5,
        BottomRight = 6,
        UpFinish = 7,
        DownFinish = 8,
        LeftFinish = 9,
        RightFinish = 10   
    }

    public ArrowDirection TranslateDirection(OverlayTile previous, OverlayTile current, OverlayTile next)
    {
        bool isFinal = next == null;
        bool isStart = previous == null;

        Vector2Int pastDirection = previous != null ? current.Position2D - previous.Position2D : new Vector2Int(0, 0);
        Vector2Int futureDirection = next != null ? next.Position2D - current.Position2D : new Vector2Int(0, 0);
        Vector2Int direction = pastDirection != futureDirection ? pastDirection + futureDirection : futureDirection;

        if(direction == new Vector2Int(0, 1) && !isFinal && !isStart)
        {
            return ArrowDirection.Vertical;
        }
        if(direction == new Vector2Int(0, -1) && !isFinal && !isStart)
        {
            return ArrowDirection.Vertical;
        }
        if(direction == new Vector2Int(1, 0) && !isFinal && !isStart)
        {
            return ArrowDirection.Horizontal;
        }
        if(direction == new Vector2Int(-1, 0) && !isFinal && !isStart)
        {
            return ArrowDirection.Horizontal;
        }

        if(direction == new Vector2Int(1, 1) && !isFinal && !isStart)
        {
            if(pastDirection.y < futureDirection.y)
            {
                return ArrowDirection.TopRight;
            }
            else
            {
                return ArrowDirection.BottomLeft;
            }
        }
        if(direction == new Vector2Int(1, -1) && !isFinal && !isStart)
        {
            if(pastDirection.y < futureDirection.y)
            {
                return ArrowDirection.TopLeft;
            }
            else
            {
                return ArrowDirection.BottomRight;
            }
            
        }
        if(direction == new Vector2Int(-1, 1) && !isFinal && !isStart)
        {
            if(pastDirection.y < futureDirection.y)
            {
                return ArrowDirection.TopLeft;
            }
            else
            {
                return ArrowDirection.BottomRight;
            }
        }
        if(direction == new Vector2Int(-1, -1) && !isFinal && !isStart)
        {
            if(pastDirection.y < futureDirection.y)
            {
                return ArrowDirection.TopRight;
            }
            else
            {
                return ArrowDirection.BottomLeft;
            }
        }

        if(direction == new Vector2Int(0, 1) && isFinal && !isStart)
        {
            return ArrowDirection.UpFinish;
        }
        if(direction == new Vector2Int(0, -1) && isFinal && !isStart)
        {
            return ArrowDirection.DownFinish;
        }
        if(direction == new Vector2Int(1, 0) && isFinal && !isStart)
        {
            return ArrowDirection.RightFinish;
        }
        if(direction == new Vector2Int(-1, 0) && isFinal && !isStart)
        {
            return ArrowDirection.LeftFinish;
        }
        else
        {
            return ArrowDirection.None;
        }
    }
}
