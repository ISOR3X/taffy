using System.Runtime.CompilerServices;

namespace Taffy
{
    public sealed class TaffyException(TaffyReturnCode code)
        : Exception($"Taffy operation failed: {code}")
    {
        public TaffyReturnCode ReturnCode { get; } = code;
    }

    internal static class TaffyReturnCodeExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfError(this TaffyReturnCode code)
        {
            if (code != TaffyReturnCode.Ok)
                throw new TaffyException(code);
        }

        // Overload for the raw uint that comes back from DllImport on some paths
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfError(this uint code)
        {
            if (code != (uint)TaffyReturnCode.Ok)
                throw new TaffyException((TaffyReturnCode)code);
        }
    }

    /// <summary>
    /// Lightweight node identifier. Nodes are owned by a <see cref="TaffyTree"/>.
    /// Record struct so equality members are generated automatically.
    /// </summary>
    public readonly record struct TaffyNode(TaffyNodeId Id)
    {
        internal TaffyNodeId Id { get; } = Id;
    }

    /// <summary>
    /// Borrowed mutable reference to a node's style. Valid only while the owning
    /// <see cref="TaffyTree"/> is alive and no structural tree mutation has occurred.
    /// Do not store beyond the current scope.
    /// </summary>
    public readonly unsafe struct TaffyStyleRef(TaffyStyle* ptr)
    {
        internal TaffyStyle* Ptr => ptr;

        #region DISPLAY

        public TaffyDisplay Display
        {
            get => NativeMethods.TaffyStyle_GetDisplay(ptr);
            set => NativeMethods.TaffyStyle_SetDisplay(ptr, value).ThrowIfError();
        }

        public TaffyPosition Position
        {
            get => NativeMethods.TaffyStyle_GetPosition(ptr);
            set => NativeMethods.TaffyStyle_SetPosition(ptr, value).ThrowIfError();
        }

        public TaffyOverflow OverflowX
        {
            get => NativeMethods.TaffyStyle_GetOverflowX(ptr);
            set => NativeMethods.TaffyStyle_SetOverflowX(ptr, value).ThrowIfError();
        }

        public TaffyOverflow OverflowY
        {
            get => NativeMethods.TaffyStyle_GetOverflowY(ptr);
            set => NativeMethods.TaffyStyle_SetOverflowY(ptr, value).ThrowIfError();
        }

        #endregion

        #region ALIGNMENT

        public TaffyAlignContent? AlignContent
        {
            get
            {
                var v = NativeMethods.TaffyStyle_GetAlignContent(ptr);
                return v == 0 ? null : (TaffyAlignContent)v;
            }
            set => NativeMethods.TaffyStyle_SetAlignContent(ptr, value ?? TaffyAlignContent.Normal).ThrowIfError();
        }

        public TaffyAlignItems? AlignItems
        {
            get
            {
                var v = NativeMethods.TaffyStyle_GetAlignItems(ptr);
                return v == 0 ? null : (TaffyAlignItems)v;
            }
            set => NativeMethods.TaffyStyle_SetAlignItems(ptr, value ?? TaffyAlignItems.Normal).ThrowIfError();
        }

        public TaffyAlignItems? AlignSelf
        {
            get
            {
                var v = NativeMethods.TaffyStyle_GetAlignSelf(ptr);
                return v == 0 ? null : (TaffyAlignItems)v;
            }
            set => NativeMethods.TaffyStyle_SetAlignSelf(ptr, value ?? TaffyAlignItems.Normal).ThrowIfError();
        }

        public TaffyAlignContent? JustifyContent
        {
            get
            {
                var v = NativeMethods.TaffyStyle_GetJustifyContent(ptr);
                return v == 0 ? null : (TaffyAlignContent)v;
            }
            set => NativeMethods.TaffyStyle_SetJustifyContent(ptr, value ?? TaffyAlignContent.Normal).ThrowIfError();
        }

        public TaffyAlignItems? JustifyItems
        {
            get
            {
                var v = NativeMethods.TaffyStyle_GetJustifyItems(ptr);
                return v == 0 ? null : (TaffyAlignItems)v;
            }
            set => NativeMethods.TaffyStyle_SetJustifyItems(ptr, value ?? TaffyAlignItems.Normal).ThrowIfError();
        }

        public TaffyAlignItems? JustifySelf
        {
            get
            {
                var v = NativeMethods.TaffyStyle_GetJustifySelf(ptr);
                return v == 0 ? null : (TaffyAlignItems)v;
            }
            set => NativeMethods.TaffyStyle_SetJustifySelf(ptr, value ?? TaffyAlignItems.Normal).ThrowIfError();
        }

        #endregion

        #region FLEX

        public TaffyFlexDirection FlexDirection
        {
            get => NativeMethods.TaffyStyle_GetFlexDirection(ptr);
            set => NativeMethods.TaffyStyle_SetFlexDirection(ptr, value).ThrowIfError();
        }

        public TaffyFlexWrap FlexWrap
        {
            get => NativeMethods.TaffyStyle_GetFlexWrap(ptr);
            set => NativeMethods.TaffyStyle_SetFlexWrap(ptr, value).ThrowIfError();
        }

        public TaffyDimension FlexBasis
        {
            get => NativeMethods.TaffyStyle_GetFlexBasis(ptr);
            set => NativeMethods.TaffyStyle_SetFlexBasis(ptr, value.value, value.unit).ThrowIfError();
        }

        public float FlexGrow
        {
            get => NativeMethods.TaffyStyle_GetFlexGrow(ptr);
            set => NativeMethods.TaffyStyle_SetFlexGrow(ptr, value).ThrowIfError();
        }

        public float FlexShrink
        {
            get => NativeMethods.TaffyStyle_GetFlexShrink(ptr);
            set => NativeMethods.TaffyStyle_SetFlexShrink(ptr, value).ThrowIfError();
        }

        #endregion

        #region GRID

        public TaffyGridAutoFlow GridAutoFlow
        {
            get => NativeMethods.TaffyStyle_GetGridAutoFlow(ptr);
            set => NativeMethods.TaffyStyle_SetGridAutoFlow(ptr, value).ThrowIfError();
        }

        public TaffyGridPlacement GridColumn
        {
            get => NativeMethods.TaffyStyle_GetGridColumn(ptr);
            set => NativeMethods.TaffyStyle_SetGridColumn(ptr, value).ThrowIfError();
        }

        public TaffyGridPlacement GridRow
        {
            get => NativeMethods.TaffyStyle_GetGridRow(ptr);
            set => NativeMethods.TaffyStyle_SetGridRow(ptr, value).ThrowIfError();
        }

        #endregion

        #region SIZE

        public TaffyDimension Width
        {
            get => NativeMethods.TaffyStyle_GetWidth(ptr);
            set => NativeMethods.TaffyStyle_SetWidth(ptr, value.value, value.unit).ThrowIfError();
        }

        public TaffyDimension Height
        {
            get => NativeMethods.TaffyStyle_GetHeight(ptr);
            set => NativeMethods.TaffyStyle_SetHeight(ptr, value.value, value.unit).ThrowIfError();
        }

        public TaffyDimension MinWidth
        {
            get => NativeMethods.TaffyStyle_GetMinWidth(ptr);
            set => NativeMethods.TaffyStyle_SetMinWidth(ptr, value.value, value.unit).ThrowIfError();
        }

        public TaffyDimension MinHeight
        {
            get => NativeMethods.TaffyStyle_GetMinHeight(ptr);
            set => NativeMethods.TaffyStyle_SetMinHeight(ptr, value.value, value.unit).ThrowIfError();
        }

        public TaffyDimension MaxWidth
        {
            get => NativeMethods.TaffyStyle_GetMaxWidth(ptr);
            set => NativeMethods.TaffyStyle_SetMaxWidth(ptr, value.value, value.unit).ThrowIfError();
        }

        public TaffyDimension MaxHeight
        {
            get => NativeMethods.TaffyStyle_GetMaxHeight(ptr);
            set => NativeMethods.TaffyStyle_SetMaxHeight(ptr, value.value, value.unit).ThrowIfError();
        }

        #endregion

        #region INSET

        public TaffyDimension InsetTop
        {
            get => NativeMethods.TaffyStyle_GetInsetTop(ptr);
            set => NativeMethods.TaffyStyle_SetInsetTop(ptr, value.value, value.unit).ThrowIfError();
        }

        public TaffyDimension InsetBottom
        {
            get => NativeMethods.TaffyStyle_GetInsetBottom(ptr);
            set => NativeMethods.TaffyStyle_SetInsetBottom(ptr, value.value, value.unit).ThrowIfError();
        }

        public TaffyDimension InsetLeft
        {
            get => NativeMethods.TaffyStyle_GetInsetLeft(ptr);
            set => NativeMethods.TaffyStyle_SetInsetLeft(ptr, value.value, value.unit).ThrowIfError();
        }

        public TaffyDimension InsetRight
        {
            get => NativeMethods.TaffyStyle_GetInsetRight(ptr);
            set => NativeMethods.TaffyStyle_SetInsetRight(ptr, value.value, value.unit).ThrowIfError();
        }

        // High-level API
        public TaffyEdges Inset
        {
            get => new(InsetTop, InsetRight, InsetBottom, InsetLeft);
            set
            {
                InsetTop = value.Top;
                InsetRight = value.Right;
                InsetBottom = value.Bottom;
                InsetLeft = value.Left;
            }
        }

        #endregion

        #region MARGIN

        // Low-level API
        public TaffyDimension MarginTop
        {
            get => NativeMethods.TaffyStyle_GetMarginTop(ptr);
            set => NativeMethods.TaffyStyle_SetMarginTop(ptr, value.value, value.unit).ThrowIfError();
        }

        public TaffyDimension MarginBottom
        {
            get => NativeMethods.TaffyStyle_GetMarginBottom(ptr);
            set => NativeMethods.TaffyStyle_SetMarginBottom(ptr, value.value, value.unit).ThrowIfError();
        }

        public TaffyDimension MarginLeft
        {
            get => NativeMethods.TaffyStyle_GetMarginLeft(ptr);
            set => NativeMethods.TaffyStyle_SetMarginLeft(ptr, value.value, value.unit).ThrowIfError();
        }

        public TaffyDimension MarginRight
        {
            get => NativeMethods.TaffyStyle_GetMarginRight(ptr);
            set => NativeMethods.TaffyStyle_SetMarginRight(ptr, value.value, value.unit).ThrowIfError();
        }

        // High-level API
        public TaffyEdges Margin
        {
            get => new(MarginTop, MarginRight, MarginBottom, MarginLeft);
            set
            {
                MarginTop = value.Top;
                MarginRight = value.Right;
                MarginBottom = value.Bottom;
                MarginLeft = value.Left;
            }
        }

        #endregion

        #region PADDING

        // Low-level API
        public TaffyDimension PaddingTop
        {
            get => NativeMethods.TaffyStyle_GetPaddingTop(ptr);
            set => NativeMethods.TaffyStyle_SetPaddingTop(ptr, value.value, value.unit).ThrowIfError();
        }

        public TaffyDimension PaddingBottom
        {
            get => NativeMethods.TaffyStyle_GetPaddingBottom(ptr);
            set => NativeMethods.TaffyStyle_SetPaddingBottom(ptr, value.value, value.unit).ThrowIfError();
        }

        public TaffyDimension PaddingLeft
        {
            get => NativeMethods.TaffyStyle_GetPaddingLeft(ptr);
            set => NativeMethods.TaffyStyle_SetPaddingLeft(ptr, value.value, value.unit).ThrowIfError();
        }

        public TaffyDimension PaddingRight
        {
            get => NativeMethods.TaffyStyle_GetPaddingRight(ptr);
            set => NativeMethods.TaffyStyle_SetPaddingRight(ptr, value.value, value.unit).ThrowIfError();
        }

        // High-level API
        public TaffyEdges Padding
        {
            get => new(PaddingTop, PaddingRight, PaddingBottom, PaddingLeft);
            set
            {
                PaddingTop = value.Top;
                PaddingRight = value.Right;
                PaddingBottom = value.Bottom;
                PaddingLeft = value.Left;
            }
        }

        #endregion

        #region BORDER

        public TaffyDimension BorderTop
        {
            get => NativeMethods.TaffyStyle_GetBorderTop(ptr);
            set => NativeMethods.TaffyStyle_SetBorderTop(ptr, value.value, value.unit).ThrowIfError();
        }

        public TaffyDimension BorderBottom
        {
            get => NativeMethods.TaffyStyle_GetBorderBottom(ptr);
            set => NativeMethods.TaffyStyle_SetBorderBottom(ptr, value.value, value.unit).ThrowIfError();
        }

        public TaffyDimension BorderLeft
        {
            get => NativeMethods.TaffyStyle_GetBorderLeft(ptr);
            set => NativeMethods.TaffyStyle_SetBorderLeft(ptr, value.value, value.unit).ThrowIfError();
        }

        public TaffyDimension BorderRight
        {
            get => NativeMethods.TaffyStyle_GetBorderRight(ptr);
            set => NativeMethods.TaffyStyle_SetBorderRight(ptr, value.value, value.unit).ThrowIfError();
        }

        // High-level API
        public TaffyEdges Border
        {
            get => new(BorderTop, BorderRight, BorderBottom, BorderLeft);
            set
            {
                BorderTop = value.Top;
                BorderRight = value.Right;
                BorderBottom = value.Bottom;
                BorderLeft = value.Left;
            }
        }

        #endregion

        #region GAP

        public TaffyDimension ColumnGap
        {
            get => NativeMethods.TaffyStyle_GetColumnGap(ptr);
            set => NativeMethods.TaffyStyle_SetColumnGap(ptr, value.value, value.unit).ThrowIfError();
        }

        public TaffyDimension RowGap
        {
            get => NativeMethods.TaffyStyle_GetRowGap(ptr);
            set => NativeMethods.TaffyStyle_SetRowGap(ptr, value.value, value.unit).ThrowIfError();
        }

        // High-level API
        public TaffyAxes Gap
        {
            get => new(ColumnGap, RowGap);
            set
            {
                ColumnGap = value.Width;
                RowGap = value.Height;
            }
        }

        #endregion

        #region GRID

        // Grid template columns
        public int GridTemplateColumnsCount => (int)NativeMethods.TaffyStyle_GetGridTemplateColumnsCount(ptr);

        public TaffyTrackSizingFunction GetGridTemplateColumnsAt(int index) =>
            NativeMethods.TaffyStyle_GetGridTemplateColumnsAt(ptr, (UIntPtr)index);

        public void SetGridTemplateColumns(TaffyTrackSizingFunction[] tracks)
        {
            fixed (TaffyTrackSizingFunction* ptr1 = tracks)
                NativeMethods.TaffyStyle_SetGridTemplateColumns(ptr, ptr1, (UIntPtr)tracks.Length).ThrowIfError();
        }

        // Grid template rows
        public int GridTemplateRowsCount => (int)NativeMethods.TaffyStyle_GetGridTemplateRowsCount(ptr);

        public TaffyTrackSizingFunction GetGridTemplateRowsAt(int index) =>
            NativeMethods.TaffyStyle_GetGridTemplateRowsAt(ptr, (UIntPtr)index);

        public void SetGridTemplateRows(TaffyTrackSizingFunction[] tracks)
        {
            fixed (TaffyTrackSizingFunction* ptr1 = tracks)
                NativeMethods.TaffyStyle_SetGridTemplateRows(ptr, ptr1, (UIntPtr)tracks.Length).ThrowIfError();
        }

        // Grid auto columns
        public int GridAutoColumnsCount => (int)NativeMethods.TaffyStyle_GetGridAutoColumnsCount(ptr);

        public TaffyTrackSizingFunction GetGridAutoColumnsAt(int index) =>
            NativeMethods.TaffyStyle_GetGridAutoColumnsAt(ptr, (UIntPtr)index);

        public void SetGridAutoColumns(TaffyTrackSizingFunction[] tracks)
        {
            fixed (TaffyTrackSizingFunction* ptr1 = tracks)
                NativeMethods.TaffyStyle_SetGridAutoColumns(ptr, ptr1, (UIntPtr)tracks.Length).ThrowIfError();
        }

        // Grid auto rows
        public int GridAutoRowsCount => (int)NativeMethods.TaffyStyle_GetGridAutoRowsCount(ptr);

        public TaffyTrackSizingFunction GetGridAutoRowsAt(int index) =>
            NativeMethods.TaffyStyle_GetGridAutoRowsAt(ptr, (UIntPtr)index);

        public void SetGridAutoRows(TaffyTrackSizingFunction[] tracks)
        {
            fixed (TaffyTrackSizingFunction* ptr1 = tracks)
                NativeMethods.TaffyStyle_SetGridAutoRows(ptr, ptr1, (UIntPtr)tracks.Length).ThrowIfError();
        }

        #endregion

        #region MISC

        public float? AspectRatio
        {
            get
            {
                var v = NativeMethods.TaffyStyle_GetAspectRatio(ptr);
                return float.IsNaN(v) ? null : v;
            }
            set => NativeMethods.TaffyStyle_SetAspectRatio(ptr, value ?? float.NaN).ThrowIfError();
        }

        public float ScrollbarWidth
        {
            get => NativeMethods.TaffyStyle_GetScrollbarWidth(ptr);
            set => NativeMethods.TaffyStyle_SetScrollbarWidth(ptr, value).ThrowIfError();
        }

        #endregion
    }

    /// <summary>
    /// Managed wrapper around a Taffy layout tree. Dispose to free native memory.
    /// <typeparam name="TContext">Per-node context type used during layout measurement.</typeparam>
    /// </summary>
    public unsafe class TaffyTree<TContext> : IDisposable where TContext : class
    {
        private TaffyNativeTree* _ptr;
        private readonly Dictionary<ulong, TContext> _nodeContexts = [];

        public TaffyTree()
        {
            _ptr = NativeMethods.TaffyTree_New();
            if (_ptr == null)
                throw new OutOfMemoryException("TaffyTree_New returned null");
        }

        public void Dispose()
        {
            if (_ptr != null)
            {
                NativeMethods.TaffyTree_Free(_ptr);
                _ptr = null;
                _nodeContexts.Clear();
            }
        }

        private TaffyNativeTree* Ptr =>
            _ptr != null ? _ptr : throw new ObjectDisposedException(nameof(TaffyTree<>));

        public TaffyNode NewNode()
        {
            var result = NativeMethods.TaffyTree_NewNode(Ptr);
            result.return_code.ThrowIfError();
            return new TaffyNode(result.value);
        }

        /// <summary>
        /// Creates a leaf node with associated context data used during <see cref="ComputeLayoutWithMeasure"/>.
        /// Does not call the rust `new_leaf_with_context` as C# manages its own context.
        /// </summary>
        public TaffyNode NewLeafWithContext(TContext context)
        {
            var result = NativeMethods.TaffyTree_NewNode(Ptr);
            result.return_code.ThrowIfError();
            var node = new TaffyNode(result.value);
            _nodeContexts[node.Id.Item1] = context;
            return node;
        }

        /// <summary>
        /// Updates the context for an existing node and marks it dirty so layout is recomputed.
        /// </summary>
        public void SetNodeContext(TaffyNode node, TContext context)
        {
            _nodeContexts[node.Id.Item1] = context;
            NativeMethods.TaffyTree_SetNodeContext(Ptr, node.Id, null).ThrowIfError();
        }

        public TContext? GetNodeContext(TaffyNode node) =>
            _nodeContexts.TryGetValue(node.Id.Item1, out var ctx) ? ctx : default;

        public void RemoveNode(TaffyNode node)
        {
            _nodeContexts.Remove(node.Id.Item1);
            NativeMethods.TaffyTree_RemoveNode(Ptr, node.Id).ThrowIfError();
        }

        public TaffyNode NewWithChildren(TaffyStyleRef style, TaffyNode[] children)
        {
            var ids = new TaffyNodeId[children.Length];
            for (int i = 0; i < children.Length; i++)
                ids[i] = children[i].Id;
            fixed (TaffyNodeId* pIds = ids)
            {
                var result = NativeMethods.TaffyTree_NewWithChildren(Ptr, style.Ptr, pIds, (UIntPtr)ids.Length);
                result.return_code.ThrowIfError();
                return new TaffyNode(result.value);
            }
        }

        public void AppendChild(TaffyNode parent, TaffyNode child) =>
            NativeMethods.TaffyTree_AppendChild(Ptr, parent.Id, child.Id).ThrowIfError();

        public void RemoveChild(TaffyNode parent, TaffyNode child) =>
            NativeMethods.TaffyTree_RemoveChild(Ptr, parent.Id, child.Id).ThrowIfError();

        public TaffyStyleRef GetStyle(TaffyNode node)
        {
            var result = NativeMethods.TaffyTree_GetStyleMut(Ptr, node.Id);
            result.return_code.ThrowIfError();
            return new TaffyStyleRef(result.value);
        }

        /// <summary>
        /// Copies the style from <paramref name="style"/> into the node and marks it dirty for relayout.
        /// Call this after mutating a <see cref="TaffyStyleRef"/> obtained from <see cref="GetStyle"/>.
        /// </summary>
        public void SetStyle(TaffyNode node, TaffyStyleRef style)
        {
            NativeMethods.TaffyTree_SetStyle(Ptr, node.Id, style.Ptr).ThrowIfError();
        }

        public void ComputeLayout(TaffyNode root, float availableWidth = float.PositiveInfinity,
            float availableHeight = float.PositiveInfinity) =>
            NativeMethods.TaffyTree_ComputeLayout(Ptr, root.Id, availableWidth, availableHeight).ThrowIfError();

        /// <summary>
        /// Compute layout, calling <paramref name="measureFn"/> for each leaf node that needs measurement.
        /// The context previously stored via <see cref="NewLeafWithContext"/> is passed as the last argument.
        /// </summary>
        public void ComputeLayoutWithMeasure(
            TaffyNode root,
            float availableWidth,
            float availableHeight,
            Func<TaffyMeasureMode, float, TaffyMeasureMode, float, TContext?, TaffySize> measureFn)
        {
            NativeMethods.TaffyTree_ComputeLayoutWithMeasure_measure_function_delegate nativeDelegate =
                (wm, w, hm, h, nodeId, _) =>
                {
                    TContext? ctx = _nodeContexts.TryGetValue(nodeId.Item1, out var found) ? found : null;
                    return measureFn(wm, w, hm, h, ctx);
                };
            NativeMethods
                .TaffyTree_ComputeLayoutWithMeasure(Ptr, root.Id, availableWidth, availableHeight, nativeDelegate)
                .ThrowIfError();
            GC.KeepAlive(nativeDelegate);
        }

        public void PrintTree(TaffyNode root) =>
            NativeMethods.TaffyTree_PrintTree(Ptr, root.Id).ThrowIfError();

        public TaffyLayout GetLayout(TaffyNode node)
        {
            var result = NativeMethods.TaffyTree_GetLayout(Ptr, node.Id);
            result.return_code.ThrowIfError();
            return result.value;
        }

        public TaffyNode? GetParent(TaffyNode node)
        {
            var result = NativeMethods.TaffyTree_GetParent(Ptr, node.Id);
            return result.has_value ? new TaffyNode(result.value) : null;
        }

        public int ChildCount(TaffyNode parent) =>
            (int)NativeMethods.TaffyTree_ChildCount(Ptr, parent.Id);

        public TaffyNode ChildAt(TaffyNode parent, int index)
        {
            var result = NativeMethods.TaffyTree_ChildAt(Ptr, parent.Id, (UIntPtr)index);
            result.return_code.ThrowIfError();
            return new TaffyNode(result.value);
        }
    }

    /// <summary>
    /// An owned, heap-allocated style that lives independently of any node.
    /// Mirrors Rust's <c>Style</c> struct — create once, apply to many nodes via
    /// <see cref="TaffyTree{TContext}.SetStyle(TaffyNode, TaffyStyleOwned)"/> or implicit cast to <see cref="TaffyStyleRef"/>.
    /// Dispose when done to free native memory.
    /// </summary>
    public unsafe sealed class TaffyStyleOwned : IDisposable
    {
        private TaffyStyle* _ptr;

        public TaffyStyleOwned(Action<TaffyStyleRef>? configure = null)
        {
            _ptr = NativeMethods.TaffyStyle_New();
            if (_ptr == null)
                throw new OutOfMemoryException("TaffyStyle_New returned null");
            configure?.Invoke(new TaffyStyleRef(_ptr));
        }

        public void Dispose()
        {
            if (_ptr != null)
            {
                NativeMethods.TaffyStyle_Free(_ptr);
                _ptr = null;
            }
        }

        internal TaffyStyle* Ptr =>
            _ptr != null ? _ptr : throw new ObjectDisposedException(nameof(TaffyStyleOwned));

        public static implicit operator TaffyStyleRef(TaffyStyleOwned s) => new(s.Ptr);
    }

    /// <summary>
    /// Convenience factory for <see cref="TaffyDimension"/> values.
    /// </summary>
    public static class Dimension
    {
        public static TaffyDimension Px(float value) => new() { value = value, unit = TaffyUnit.Length };
        public static TaffyDimension Percent(float value) => new() { value = value, unit = TaffyUnit.Percent };
        public static TaffyDimension Auto() => new() { value = 0, unit = TaffyUnit.Auto };
        public static TaffyDimension MinContent() => new() { value = 0, unit = TaffyUnit.MinContent };
        public static TaffyDimension MaxContent() => new() { value = 0, unit = TaffyUnit.MaxContent };
        public static TaffyDimension Fr(float value) => new() { value = value, unit = TaffyUnit.Fr };
    }

    /// <summary>
    /// Convenience factory for <see cref="TaffyTrackSizingFunction"/> values matching Taffy's style helpers.
    /// </summary>
    public static class TrackSizingFunction
    {
        static TaffyDimension Auto => Dimension.Auto();

        /// <summary>Fixed pixel track.</summary>
        public static TaffyTrackSizingFunction Px(float value) =>
            new() { min = Dimension.Px(value), max = Dimension.Px(value) };

        /// <summary>Percentage track.</summary>
        public static TaffyTrackSizingFunction Percent(float value) =>
            new() { min = Dimension.Percent(value), max = Dimension.Percent(value) };

        /// <summary>Flexible fr track (auto min, fr max).</summary>
        public static TaffyTrackSizingFunction Fr(float value = 1f) =>
            new() { min = Auto, max = Dimension.Fr(value) };

        /// <summary>auto track.</summary>
        public static TaffyTrackSizingFunction AutoTrack() =>
            new() { min = Auto, max = Auto };

        /// <summary>min-content track.</summary>
        public static TaffyTrackSizingFunction MinContent() =>
            new() { min = Dimension.MinContent(), max = Dimension.MinContent() };

        /// <summary>max-content track.</summary>
        public static TaffyTrackSizingFunction MaxContent() =>
            new() { min = Dimension.MaxContent(), max = Dimension.MaxContent() };

        /// <summary>minmax(min, max) track.</summary>
        public static TaffyTrackSizingFunction MinMax(TaffyDimension min, TaffyDimension max) =>
            new() { min = min, max = max };
    }

    /// <summary>
    /// Convenience factory for setting rust-style `Rect<LengthPercentageAuto>` properties easily.
    /// </summary>
    public struct TaffyEdges
    {
        public TaffyDimension Top, Right, Bottom, Left;

        public TaffyEdges(TaffyDimension top, TaffyDimension right, TaffyDimension bottom, TaffyDimension left)
        {
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }

        public TaffyEdges(TaffyDimension all) : this(all, all, all, all)
        {
        }

        public TaffyEdges(TaffyEdge edge, TaffyDimension value)
        {
            var zero = new TaffyDimension { value = 0, unit = TaffyUnit.Length };
            Top = Right = Bottom = Left = zero;
            switch (edge)
            {
                case TaffyEdge.Top: Top = value; break;
                case TaffyEdge.Bottom: Bottom = value; break;
                case TaffyEdge.Left: Left = value; break;
                case TaffyEdge.Right: Right = value; break;
                case TaffyEdge.Vertical: Top = Bottom = value; break;
                case TaffyEdge.Horizontal: Left = Right = value; break;
                case TaffyEdge.All: Top = Right = Bottom = Left = value; break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(edge), edge, null);
            }
        }
    }

    /// <summary>
    /// Convenience factory for setting rust-style `Size<LengthPercentageAuto>` properties easily.
    /// </summary>
    public struct TaffyAxes(TaffyDimension width, TaffyDimension height)
    {
        public TaffyDimension Width = width, Height = height;

        public TaffyAxes(TaffyDimension all) : this(all, all)
        {
        }
    }
}
