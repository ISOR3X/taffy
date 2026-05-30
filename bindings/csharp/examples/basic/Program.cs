// C# port of bindings/c/examples/basic.c
// Demonstrates the TaffySharp high-level API.
//
// To run this example:
//   1. Build ctaffy:  cargo build --release -p ctaffy
//   2. Copy the native library next to this executable:
//        Windows:  copy target\release\ctaffy.dll  bindings\csharp\examples\basic\
//        Linux:    cp  target/release/libctaffy.so bindings/csharp/examples/basic/
//        macOS:    cp  target/release/libctaffy.dylib bindings/csharp/examples/basic/
//   3. dotnet run --project bindings/csharp/examples/basic

using Taffy;
using var tree = new TaffyTree();

// Create child node and set styles
var child = tree.NewNode();
var childStyle = tree.GetStyle(child);
childStyle.Width  = Dimension.Percent(0.5f);
childStyle.Height = Dimension.Auto();

// Create parent node and set styles
var parent = tree.NewNode();
var parentStyle = tree.GetStyle(parent);
parentStyle.Width  = Dimension.Px(100f);
parentStyle.Height = Dimension.Px(100f);
parentStyle.JustifyContent = TaffyAlignContent.Center;

// Build the tree
tree.AppendChild(parent, child);

// Compute layout with a 100×100 viewport
Console.WriteLine("\nCompute layout with 100x100 viewport:");
tree.ComputeLayout(parent, 100f, 100f);
tree.PrintTree(parent);

// Compute layout with an infinite viewport
Console.WriteLine("\nCompute layout with infinite viewport:");
tree.ComputeLayout(parent, float.PositiveInfinity, float.PositiveInfinity);
tree.PrintTree(parent);

// Inspect result
var childLayout  = tree.GetLayout(child);
var parentLayout = tree.GetLayout(parent);
Console.WriteLine($"\nParent layout: x={parentLayout.x} y={parentLayout.y} w={parentLayout.width} h={parentLayout.height}");
Console.WriteLine($"Child  layout: x={childLayout.x}  y={childLayout.y}  w={childLayout.width}  h={childLayout.height}");
