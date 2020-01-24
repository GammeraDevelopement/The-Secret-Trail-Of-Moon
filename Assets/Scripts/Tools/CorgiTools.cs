using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Various static methods used throughout my projects
/// </summary>
public static class CorgiTools
{
    /// <summary>
    /// Draws a debug ray and does the actual raycast
    /// </summary>
    /// <returns>The cast.</returns>
    /// <param name="rayOriginPoint">Ray origin point.</param>
    /// <param name="rayDirection">Ray direction.</param>
    /// <param name="rayDistance">Ray distance.</param>
    /// <param name="mask">Mask.</param>
    /// <param name="debug">If set to <c>true</c> debug.</param>
    /// <param name="color">Color.</param>
    public static RaycastHit2D CorgiRayCast2D(Vector2 rayOriginPoint, Vector2 rayDirection, float rayDistance, LayerMask mask, bool debug, Color color)
    {
        Debug.DrawRay(rayOriginPoint, rayDirection * rayDistance, color);
        return Physics2D.Raycast(rayOriginPoint, rayDirection, rayDistance, mask);
    }

    /// <summary>
    /// Outputs the message object to the console, prefixed with the current timestamp
    /// </summary>
    /// <param name="message">Message.</param>
    public static void DebugLogTime(object message)
    {
        Debug.Log(Time.deltaTime + " " + message);
    }

    /// <summary>
    /// Fades the specified image to the target opacity and duration.
    /// </summary>
    /// <param name="target">Target.</param>
    /// <param name="opacity">Opacity.</param>
    /// <param name="duration">Duration.</param>
    public static IEnumerator FadeImage(Image target, float duration, Color color)
    {
        if (target == null)
            yield break;

        float alpha = target.color.a;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / duration)
        {
            if (target == null)
                yield break;
            Color newColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha, color.a, t));
            target.color = newColor;
            yield return null;
        }
    }
    /// <summary>
    /// Fades the specified image to the target opacity and duration.
    /// </summary>
    /// <param name="target">Target.</param>
    /// <param name="opacity">Opacity.</param>
    /// <param name="duration">Duration.</param>
    public static IEnumerator FadeText(Text target, float duration, Color color)
    {
        if (target == null)
            yield break;

        float alpha = target.color.a;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / duration)
        {
            if (target == null)
                yield break;
            Color newColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha, color.a, t));
            target.color = newColor;
            yield return null;
        }
    }

    /// <summary>
    /// Fades the specified image to the target opacity and duration.
    /// </summary>
    /// <param name="target">Target.</param>
    /// <param name="opacity">Opacity.</param>
    /// <param name="duration">Duration.</param>
    public static IEnumerator FadeSprite(SpriteRenderer target, float duration, Color color)
    {
        if (target == null)
            yield break;

        float alpha = target.color.a;

        float t = 0f;
        while (t < 1.0f)
        {
            if (target == null)
                yield break;

            Color newColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha, color.a, t));
            target.color = newColor;

            t += Time.deltaTime / duration;

            yield return null;

        }
        Color finalColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha, color.a, t));
        target.color = finalColor;
    }

    /// <summary>
    /// Fades the specified material to the target opacity and duration.
    /// </summary>
    /// <param name="target">Target.</param>
    /// <param name="opacity">Opacity.</param>
    /// <param name="duration">Duration.</param>
    public static IEnumerator FadeMaterial(Material target, float duration, Color color)
    {
        if (target == null)
            yield break;

        float alpha = target.color.a;

        float t = 0f;
        while (t < 1.0f)
        {
            if (target == null)
                yield break;

            Color newColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha, color.a, t));
            target.color = newColor;

            t += Time.deltaTime / duration;

            yield return null;

        }
        Color finalColor = new Color(color.r, color.g, color.b, Mathf.SmoothStep(alpha, color.a, t));
        target.color = finalColor;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target"></param>
    /// <param name="duration"></param>
    /// <param name="targetAlpha"></param>
    /// <returns></returns>
    public static IEnumerator FadeCanvasGroup(CanvasGroup target, float duration, float targetAlpha)
    {
        if (target == null)
            yield break;

        float currentAlpha = target.alpha;

        float t = 0f;
        while (t < 1.0f)
        {
            if (target == null)
                yield break;

            float newAlpha = Mathf.SmoothStep(currentAlpha, targetAlpha, t);
            target.alpha = newAlpha;

            t += Time.deltaTime / duration;

            yield return null;

        }
        target.alpha = targetAlpha;
    }

    /// <summary>
    /// Moves an object from point A to point B in a given time
    /// </summary>
    /// <param name="movingObject">Moving object.</param>
    /// <param name="pointA">Point a.</param>
    /// <param name="pointB">Point b.</param>
    /// <param name="time">Time.</param>
    public static IEnumerator MoveFromTo(GameObject movingObject, Vector3 pointA, Vector3 pointB, float time)
    {
        float t = 0f;
        float closeEnough = 0.01f;
        while (Vector3.Distance(movingObject.transform.position, pointB) > closeEnough)
        {
            t += Time.deltaTime / time;
            movingObject.transform.position = Vector3.Lerp(pointA, pointB, t);

            yield return 0;
        }

        //Asocio la posicion final
        movingObject.transform.position = pointB;
    }

    /// <summary>
    /// Moves an object from point A to point B in a given time
    /// </summary>
    /// <param name="movingObject">Moving object.</param>
    /// <param name="pointA">Point a.</param>
    /// <param name="pointB">Point b.</param>
    /// <param name="time">Time.</param>
    public static IEnumerator MoveLocalFromTo(GameObject movingObject, Vector3 pointA, Vector3 pointB, float time)
    {
        float t = 0f;
        float closeEnough = 0.01f;
        while (Vector3.Distance(movingObject.transform.localPosition, pointB) > closeEnough)
        {
            t += Time.deltaTime / time;
            movingObject.transform.localPosition = Vector3.Lerp(pointA, pointB, t);

            yield return 0;
        }

        //Asocio la posicion final
        movingObject.transform.localPosition = pointB;
    }

    /// <summary>
    /// Moves an object from point A to point B in a given time
    /// </summary>
    /// <param name="movingObject">Moving object.</param>
    /// <param name="pointA">Point a.</param>
    /// <param name="pointB">Point b.</param>
    /// <param name="time">Time.</param>
    public static IEnumerator MoveFromToTarget(GameObject movingObject, Transform target, float time)
    {
        float t = 0f;
        float closeEnough = 0.01f;
        time = time * 10;

        while (Vector3.Distance(movingObject.transform.position, target.position) > closeEnough)
        {
            t += Time.deltaTime / time;
            movingObject.transform.position = Vector3.Lerp(movingObject.transform.position, target.position, t);

            yield return 0;
        }

        //Asocio la posicion final
        movingObject.transform.position = target.position;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="particle"></param>
    /// <param name="startSize"></param>
    /// <param name="endSize"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public static IEnumerator AnimateFromTo(ParticleSystem particle, float startSize, float endSize, float time)
    {
        float t = 0f;
#pragma warning disable 0618
        while (particle.startSize < endSize)
        {
            t += Time.deltaTime / time;
            particle.startSize += t;
            yield return 0;
        }
#pragma warning restore 0618
    }

    /// <summary>
    /// Scales an object from point A to point B in a given time
    /// </summary>
    /// <param name="scalingObject">Moving object.</param>
    /// <param name="pointA">Point a.</param>
    /// <param name="pointB">Point b.</param>
    /// <param name="time">Time.</param>
    public static IEnumerator ScaleFromTo(GameObject scalingObject, float pointA, float pointB, float time)
    {
        float t = 0f;
        while (scalingObject.transform.localScale.x != pointB)
        {
            t += Time.deltaTime / time;

            float start_X = pointA, start_Y = (pointA > 0) ? pointA : -pointA;
            float end_X = pointB, end_Y = (pointB > 0) ? pointB : -pointB;

            scalingObject.transform.localScale = Vector3.Lerp(
                new Vector3(start_X, start_Y, 1),
                new Vector3(end_X, end_Y, 1),
                t);
            yield return 0;
        }
    }

    /// <summary>
    /// Rotating an object from point A to point B in a given time
    /// </summary>
    /// <param name="rotatingObject">Moving object.</param>
    /// <param name="pointA">Point a.</param>
    /// <param name="pointB">Point b.</param>
    /// <param name="time">Time.</param>
    public static IEnumerator RotateFromTo(GameObject rotatingObject, Vector3 pointA, Vector3 pointB, float time)
    {
        float t = time;
        while (t > 0.0f)
        {
            t -= Time.deltaTime;
            rotatingObject.transform.rotation = Quaternion.Lerp(Quaternion.Euler(pointA), Quaternion.Euler(pointB), 1 - (t / time));
            yield return 0;
        }
        rotatingObject.transform.rotation = Quaternion.Euler(pointB);
    }

    /// <summary>
    /// Rotating an object from point A to point B in a given time
    /// </summary>
    /// <param name="rotatingObject">Moving object.</param>
    /// <param name="pointA">Point a.</param>
    /// <param name="pointB">Point b.</param>
    /// <param name="time">Time.</param>
    public static IEnumerator RotateLocalFromTo(GameObject rotatingObject, Vector3 pointA, Vector3 pointB, float time)
    {
        float t = time;
        while (t > 0.0f)
        {
            t -= Time.deltaTime;
            rotatingObject.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(pointA), Quaternion.Euler(pointB), 1 - (t / time));
            yield return 0;
        }
        rotatingObject.transform.localRotation = Quaternion.Euler(pointB);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rotatingObject"></param>
    /// <param name="angle"></param>
    /// <param name="speed"></param>
    /// <returns></returns>
    public static IEnumerator RotateLocalAngle(GameObject rotatingObject, Vector3 angle, float time)
    {
        float t = time;
        Quaternion target = rotatingObject.transform.localRotation * Quaternion.Euler(angle);

        while (t > 0.0f)
        {
            t -= Time.deltaTime;
            rotatingObject.transform.localRotation = Quaternion.Lerp(rotatingObject.transform.localRotation, target, 1 - (t / time));
            yield return 0;
        }        
        rotatingObject.transform.localRotation = target;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static Vector2 GetInputAxisNormalized(Vector2 input)
    {
        float x = 0, y = 0;
        if (Mathf.Abs(input.x) > 0.1f)
            x = 1 * Mathf.Sign(input.x);
        else
            x = 0;

        if (Mathf.Abs(input.y) > 0.1f)
            y = 1 * Mathf.Sign(input.y);
        else
            y = 0;

        return new Vector2(x, y);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static Vector2 GetInputAxisNormalizedCross(Vector2 input)
    {
        float x = 0, y = 0;
        if (Mathf.Abs(input.x) > 0.1f && Mathf.Abs(input.y) < 0.1f)
            x = 1 * Mathf.Sign(input.x);
        else
            x = 0;

        if (Mathf.Abs(input.y) > 0.1f && Mathf.Abs(input.x) < 0.1f)
            y = 1 * Mathf.Sign(input.y);
        else
            y = 0;

        return new Vector2(x, y);
    }

    /// <summary>
    /// Metodo para reemplazar la descripcion de un tutorial por el boton correspondiente
    /// </summary>
    /// <param name="description"></param>
    /// <returns></returns>
    public static string ReplaceButtonTutorial(string description)
    {
        description = description.Replace("btn_cross", "<quad class=\"btn_cross\"/>");
        description = description.Replace("btn_triangle", "<quad class=\"btn_triangle\"/>");
        description = description.Replace("btn_circle", "<quad class=\"btn_circle\"/>");
        description = description.Replace("btn_square", "<quad class=\"btn_square\"/>");
        description = description.Replace("btn_r1", "<quad class=\"btn_r1\"/>");
        description = description.Replace("btn_r2", "<quad class=\"btn_r2\"/>");
        description = description.Replace("btn_l1", "<quad class=\"btn_l1\"/>");
        description = description.Replace("btn_l2", "<quad class=\"btn_l2\"/>");
        description = description.Replace("btn_joystick_left", "<quad class=\"btn_joystick_left\"/>");
        description = description.Replace("btn_joystick_right", "<quad class=\"btn_joystick_right\"/>");
        description = description.Replace("btn_up", "<quad class=\"btn_up\"/>");
        description = description.Replace("btn_down", "<quad class=\"btn_down\"/>");
        description = description.Replace("btn_left", "<quad class=\"btn_left\"/>");
        description = description.Replace("btn_right", "<quad class=\"btn_right\"/>");
        description = description.Replace("btn_touchpad", "<quad class=\"btn_touchpad\"/>");

        return description;
    }
}