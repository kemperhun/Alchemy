using UnityEngine;

public static class HexMetrics
{

    public const float outerRadius = 1;

    public const float innerRadius = outerRadius * 0.8660254037844f;

    public static Vector3[] corners = {
        new Vector3(0f, 0f, outerRadius),
        new Vector3(innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(0f, 0f, -outerRadius),
        new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(0f, 0f, outerRadius)
    };
    public static float HexHeight = -1;

    public static Vector3 ConvertToHex(Vector3 targetPos)
    {
		targetPos.x = (targetPos.x + targetPos.z * 0.5f - targetPos.z / 2) * (HexMetrics.innerRadius * 2f);
		targetPos.z = targetPos.z * (HexMetrics.outerRadius * 1.5f);
		
        //targetPos.x = (int)targetPos.x;
        //targetPos.z = (int)targetPos.z;
        //targetPos.x = (targetPos.x) * Mathf.Sqrt(3) * (outerRadius) + targetPos.z * (innerRadius);
        //targetPos.z = targetPos.z * (outerRadius) * 1.5f;
        return targetPos;
    }
}