using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VectorGraphics;
using UnityEngine;


public static class SpriteGenerator
{
    /// <summary>
    /// Scene (Scene)
    /// Scene.Root - (SceneNode)
    /// Scene.Root.Children - (List<SceneNode>)
    /// Scene.Root.Children[i] - (SceneNode)
    /// Scene.Root.Children[i].Shapes - (List<Shape>)
    /// Scene.Root.Children[i].Shapes[j] - (Shape)
    /// Scene.Root.Children[i].Shapes[j].Contours - (BezierContour[])
    /// Scene.Root.Children[i].Shapes[j].Contours[k] - (BezierContour)
    /// Scene.Root.Children[i].Shapes[j].Contours[k].Segments - (BezierPathSegment[])
    /// Scene.Root.Children[i].Shapes[j].Contours[k].Segments[l] - (BezierPathSegment)
    /// </summary>
    private static void CreateTestSvg()
    {
        Scene scene = new Scene();
        List<SceneNode> nodes = new List<SceneNode>();

        List<BezierContour> paths = new List<BezierContour>();
        List<BezierPathSegment> shapeSegments = new List<BezierPathSegment>();

        BezierContour path = new BezierContour() { Segments = shapeSegments.ToArray() };
        paths.Add(path);

        List<Shape> shapes = new List<Shape>();
        Shape shape = new Shape() { Contours = paths.ToArray() };
        shapes.Add(shape);

        SceneNode node = new SceneNode() { Shapes = shapes };

        scene.Root.Children.Add(node);
    }

    private static Sprite SvgToSprite(Scene scene, VectorUtils.TessellationOptions tesOptions)
    {
        // Dynamically import the SVG data, and tessellate the resulting vector scene.
        List<VectorUtils.Geometry> geoms = VectorUtils.TessellateScene(scene, tesOptions);

        // Build a sprite with the tessellated geometry.
        var sprite = VectorUtils.BuildSprite(geoms, 10.0f, VectorUtils.Alignment.Center, Vector2.zero, 128, true);
        return sprite;
    }
}

