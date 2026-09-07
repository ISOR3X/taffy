// C# port of examples/measure.rs
// Demonstrates nodes with typed context and a single shared measure function.

using Taffy;

abstract record NodeContext;
record TextContext(string Text) : NodeContext;
record ImageContext(float Width, float Height) : NodeContext;

static class MeasureExample
{
    private const float CharWidth = 10f;
    private const float CharHeight = 10f;
    private const string LoremIpsum =
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor " +
        "incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud " +
        "exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure " +
        "dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.";

    private static TaffySize Measure(
        TaffyMeasureMode widthMode, float width,
        TaffyMeasureMode heightMode, float height,
        NodeContext? context)
        => context switch
        {
            TextContext text => MeasureText(widthMode, width, text.Text),
            ImageContext img => MeasureImage(widthMode, width, heightMode, height, img),
            _ => new TaffySize { width = 0, height = 0 },
        };

    private static TaffySize MeasureText(TaffyMeasureMode widthMode, float width, string text)
    {
        float availableWidth = widthMode is TaffyMeasureMode.Exact or TaffyMeasureMode.FitContent
            ? width
            : float.PositiveInfinity;

        int charCount = text.Length;
        int charsPerLine = float.IsInfinity(availableWidth)
            ? charCount
            : Math.Max(1, (int)(availableWidth / CharWidth));
        int lines = (charCount + charsPerLine - 1) / charsPerLine;

        return new TaffySize
        {
            width = Math.Min(charCount * CharWidth, availableWidth),
            height = lines * CharHeight,
        };
    }

    private static TaffySize MeasureImage(
        TaffyMeasureMode widthMode, float width,
        TaffyMeasureMode heightMode, float height,
        ImageContext img)
    {
        bool knownWidth = widthMode == TaffyMeasureMode.Exact;
        bool knownHeight = heightMode == TaffyMeasureMode.Exact;

        float w = knownWidth ? width : img.Width;
        float h = knownHeight ? height : img.Height;

        if (knownWidth && !knownHeight) h = w * (img.Height / img.Width);
        if (knownHeight && !knownWidth) w = h * (img.Width / img.Height);

        return new TaffySize { width = w, height = h };
    }

    public static void Run()
    {
        using var tree = new TaffyTree<NodeContext>();

        var textNode = tree.NewLeafWithContext(new TextContext(LoremIpsum));
        var imageNode = tree.NewLeafWithContext(new ImageContext(400f, 300f));

        // Configure root style via a temporary node, then promote it with children attached.
        var tempRoot = tree.NewNode();
        var rootStyle = tree.GetStyle(tempRoot);
        rootStyle.Display = TaffyDisplay.Flex;
        rootStyle.FlexDirection = TaffyFlexDirection.Column;
        rootStyle.Width = Dimension.Px(200f);
        rootStyle.Height = Dimension.Auto();

        var root = tree.NewWithChildren(rootStyle, [textNode, imageNode]);
        tree.RemoveNode(tempRoot);

        Console.WriteLine("\nCompute layout with infinite viewport:");
        tree.ComputeLayoutWithMeasure(root, float.PositiveInfinity, float.PositiveInfinity, Measure);
        tree.PrintTree(root);

        Console.WriteLine("\nCompute layout with 100x100 viewport:");
        tree.ComputeLayoutWithMeasure(root, 100f, 100f, Measure);
        tree.PrintTree(root);

        var rootLayout = tree.GetLayout(root);
        var textLayout = tree.GetLayout(textNode);
        var imageLayout = tree.GetLayout(imageNode);

        Console.WriteLine($"Root:  x={rootLayout.x}  y={rootLayout.y}  w={rootLayout.width}  h={rootLayout.height}");
        Console.WriteLine($"Text:  x={textLayout.x}   y={textLayout.y}   w={textLayout.width}   h={textLayout.height}");
        Console.WriteLine($"Image: x={imageLayout.x}  y={imageLayout.y}  w={imageLayout.width}  h={imageLayout.height}");
    }
}
