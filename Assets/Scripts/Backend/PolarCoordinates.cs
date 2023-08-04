using UnityEngine;

/// <summary>
/// Represents a point in 3D space using polar coordinates.
/// </summary>
public class PolarCoordinates
{
    /// <summary>
    /// Gets the radius of the polar coordinates in Meter.
    /// </summary>
    public float Radius { get; }

    /// <summary>
    /// Value of the horizontal polar angle (normalized between -0.5 and +0.5 equivalent to -180 to +180 degrees)
    /// </summary>
    public float Hor { get; }

    /// <summary>
    /// Value of the vertical polar angle (normalized between -0.5 and +0.5 equivalent to -90 to +90 degrees)
    /// </summary>
    public float Ver { get; }

    public PolarCoordinates(float radius, float hor, float ver)
    {
        Radius = radius;
        Hor = hor;
        Ver = ver;
    }

    /// <summary>
    /// Converts the item's spherical coordinates to Cartesian coordinates.
    /// </summary>
    /// <returns>The item's position in Cartesian coordinates.</returns>
    public Vector3 ToCartesianCoordinates( float radius = 5f)
    {
        // Convert normalized Radius to meters
        float rad = Radius * radius;
        // Convert normalized horizontal and vertical angles (+/-0.5) to radians
        // Center of image = Unity +Z forward
        float hor = -Hor * 2 * Mathf.PI;
        float ver = (-Ver + 0.5f) * Mathf.PI;
        // Debug.Log($"rad: {Rad}/{rad} hor: {Hor}/{hor}:  ver: {Ver}/{ver}");

        // Calculate Cartesian coordinates
        // https://studyflix.de/mathematik/kugelkoordinaten-1519
        float x = rad * Mathf.Sin(ver) * Mathf.Cos(hor);
        float y = rad * Mathf.Sin(ver) * Mathf.Sin(hor);
        float z = rad * Mathf.Cos(ver);
        // Debug.Log("x => Unity Z: " + x + " y => Unity -X: " + y + " z => Unity Y" + z);

        // Convert to left-handed coordinate system
        return new Vector3(-y, z, x);
    }

    /// <summary>
    /// Converts a vector in Cartesian coordinates to polar coordinates.
    /// </summary>
    /// <param name="cartesian">The vector in Cartesian coordinates to convert.</param>
    /// <returns>The vector in polar coordinates.</returns>
    public static PolarCoordinates FromCartesianCoordinates(Vector3 cartesian)
    {
        float radius = Mathf.Sqrt(
            Mathf.Pow(cartesian.x, 2) + Mathf.Pow(cartesian.y, 2) + Mathf.Pow(cartesian.z, 2)
        );
        float hor = Mathf.Atan2(cartesian.y, cartesian.x);
        float ver = Mathf.Acos(cartesian.z / radius);

        return new PolarCoordinates(radius, hor, ver);
    }
}
