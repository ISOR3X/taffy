// C# port of examples/measure.rs
// Demonstrates nodes with custom measure functions (text wrapping and fixed-size images).

using Taffy;

static class MeasureExample
{
    private const float CharWidth = 10f;
    private const float CharHeight = 10f;
    private const string LoremIpsum =
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor " +
        "incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud " +
        "exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure " +
        "dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.";

    private const float ImageNaturalWidth = 400f;
    private const float ImageNaturalHeight = 300f;

    public static void Run()
    {
        using var tree = new TaffyTree();

        // --- Text node ---
        // Wraps characters into lines based on available width, using fixed char dimensions.
        var textNode = tree.NewNode();
        tree.SetMeasureFunction(textNode, (widthMode, width, heightMode, height) =>
        {
            float availableWidth = widthMode == TaffyMeasureMode.Exact ? width
                                 : widthMode == TaffyMeasureMode.FitContent ? width
                                 : float.PositiveInfinity;

            int charCount = LoremIpsum.Length;
            int charsPerLine = float.IsInfinity(availableWidth)
                ? charCount
                : Math.Max(1, (int)(availableWidth / CharWidth));
            int lines = (charCount + charsPerLine - 1) / charsPerLine;

            return new TaffySize
            {
                width = Math.Min(charCount * CharWidth, availableWidth),
                height = lines * CharHeight,
            };
        });

        // --- Image node ---
        // Returns natural dimensions, preserving aspect ratio when only one axis is constrained.
        var imageNode = tree.NewNode();
        tree.SetMeasureFunction(imageNode, (widthMode, width, heightMode, height) =>
        {
            bool knownWidth = widthMode == TaffyMeasureMode.Exact;
            bool knownHeight = heightMode == TaffyMeasureMode.Exact;

            float w = knownWidth ? width : ImageNaturalWidth;
            float h = knownHeight ? height : ImageNaturalHeight;

            if (knownWidth && !knownHeight)
                h = w * (ImageNaturalHeight / ImageNaturalWidth);
            else if (knownHeight && !knownWidth)
                w = h * (ImageNaturalWidth / ImageNaturalHeight);

            return new TaffySize { width = w, height = h };
        });

        // --- Root container (flex column, fixed width, auto height) ---
        // NewWithChildren clones the style from an existing node's style pointer, so we
        // configure a temporary node, then promote it to the root with its children attached.
        var tempRoot = tree.NewNode();
        var rootStyle = tree.GetStyle(tempRoot);
        rootStyle.Display = TaffyDisplay.Flex;
        rootStyle.FlexDirection = TaffyFlexDirection.Column;
        rootStyle.Width = Dimension.Px(200f);
        rootStyle.Height = Dimension.Auto();

        var root = tree.NewWithChildren(rootStyle, [textNode, imageNode]);
        tree.RemoveNode(tempRoot);

        // Compute layout and print
        Console.WriteLine("\nCompute layout with infinite viewport:");
        tree.ComputeLayout(root);
        tree.PrintTree(root);

        Console.WriteLine("\nCompute layout with 100x100 viewport:");
        tree.ComputeLayout(root, 100f, 100f);
        tree.PrintTree(root);

        var rootLayout = tree.GetLayout(root);
        var textLayout = tree.GetLayout(textNode);
        var imageLayout = tree.GetLayout(imageNode);

        Console.WriteLine($"Root:  x={rootLayout.x}  y={rootLayout.y}  w={rootLayout.width}  h={rootLayout.height}");
        Console.WriteLine($"Text:  x={textLayout.x}   y={textLayout.y}   w={textLayout.width}   h={textLayout.height}");
        Console.WriteLine($"Image: x={imageLayout.x}  y={imageLayout.y}  w={imageLayout.width}  h={imageLayout.height}");
    }
}
