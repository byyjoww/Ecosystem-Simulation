using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectTransformExtensions
{
    public static float Left(this RectTransform e)
    {
        return e.position.x - e.rect.width * e.pivot.x;
    }

    public static float Right(this RectTransform e)
    {
        return e.position.x + e.rect.width * (1f - e.pivot.x);
    }

    public static float Top(this RectTransform e)
    {
        return e.position.y + e.rect.height * (1f - e.pivot.y);
    }

    public static float Bottom(this RectTransform e)
    {
        return e.position.y - e.rect.height * e.pivot.y;
    }
}