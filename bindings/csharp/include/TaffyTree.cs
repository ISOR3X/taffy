using System;
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
    /// </summary>
    public readonly struct TaffyNode(TaffyNodeId id)
    {
        internal TaffyNodeId Id { get; } = id;
    }

    /// <summary>
    /// Borrowed mutable reference to a node's style. Valid only while the owning
    /// <see cref="TaffyTree"/> is alive and no structural tree mutation has occurred.
    /// Do not store beyond the current scope.
    /// </summary>
    public readonly unsafe struct TaffyStyleRef(TaffyStyle* ptr)
    {
        private readonly TaffyStyle* _ptr = ptr;

        // Display / Position / Overflow
        public TaffyDisplay Display
        {
            get => NativeMethods.TaffyStyle_GetDisplay(_ptr);
            set => NativeMethods.TaffyStyle_SetDisplay(_ptr, value).ThrowIfError();
        }
        public TaffyPosition Position
        {
            get => NativeMethods.TaffyStyle_GetPosition(_ptr);
            set => NativeMethods.TaffyStyle_SetPosition(_ptr, value).ThrowIfError();
        }
        public TaffyOverflow OverflowX
        {
            get => NativeMethods.TaffyStyle_GetOverflowX(_ptr);
            set => NativeMethods.TaffyStyle_SetOverflowX(_ptr, value).ThrowIfError();
        }
        public TaffyOverflow OverflowY
        {
            get => NativeMethods.TaffyStyle_GetOverflowY(_ptr);
            set => NativeMethods.TaffyStyle_SetOverflowY(_ptr, value).ThrowIfError();
        }

        // Alignment
        public TaffyAlignContent? AlignContent
        {
            get { var v = NativeMethods.TaffyStyle_GetAlignContent(_ptr); return v == 0 ? null : (TaffyAlignContent)v; }
            set => NativeMethods.TaffyStyle_SetAlignContent(_ptr, value ?? TaffyAlignContent.Normal).ThrowIfError();
        }
        public TaffyAlignItems? AlignItems
        {
            get { var v = NativeMethods.TaffyStyle_GetAlignItems(_ptr); return v == 0 ? null : (TaffyAlignItems)v; }
            set => NativeMethods.TaffyStyle_SetAlignItems(_ptr, value ?? TaffyAlignItems.Normal).ThrowIfError();
        }
        public TaffyAlignItems? AlignSelf
        {
            get { var v = NativeMethods.TaffyStyle_GetAlignSelf(_ptr); return v == 0 ? null : (TaffyAlignItems)v; }
            set => NativeMethods.TaffyStyle_SetAlignSelf(_ptr, value ?? TaffyAlignItems.Normal).ThrowIfError();
        }
        public TaffyAlignContent? JustifyContent
        {
            get { var v = NativeMethods.TaffyStyle_GetJustifyContent(_ptr); return v == 0 ? null : (TaffyAlignContent)v; }
            set => NativeMethods.TaffyStyle_SetJustifyContent(_ptr, value ?? TaffyAlignContent.Normal).ThrowIfError();
        }
        public TaffyAlignItems? JustifyItems
        {
            get { var v = NativeMethods.TaffyStyle_GetJustifyItems(_ptr); return v == 0 ? null : (TaffyAlignItems)v; }
            set => NativeMethods.TaffyStyle_SetJustifyItems(_ptr, value ?? TaffyAlignItems.Normal).ThrowIfError();
        }
        public TaffyAlignItems? JustifySelf
        {
            get { var v = NativeMethods.TaffyStyle_GetJustifySelf(_ptr); return v == 0 ? null : (TaffyAlignItems)v; }
            set => NativeMethods.TaffyStyle_SetJustifySelf(_ptr, value ?? TaffyAlignItems.Normal).ThrowIfError();
        }

        // Flex
        public TaffyFlexDirection FlexDirection
        {
            get => NativeMethods.TaffyStyle_GetFlexDirection(_ptr);
            set => NativeMethods.TaffyStyle_SetFlexDirection(_ptr, value).ThrowIfError();
        }
        public TaffyFlexWrap FlexWrap
        {
            get => NativeMethods.TaffyStyle_GetFlexWrap(_ptr);
            set => NativeMethods.TaffyStyle_SetFlexWrap(_ptr, value).ThrowIfError();
        }
        public TaffyDimension FlexBasis
        {
            get => NativeMethods.TaffyStyle_GetFlexBasis(_ptr);
            set => NativeMethods.TaffyStyle_SetFlexBasis(_ptr, value.value, value.unit).ThrowIfError();
        }
        public float FlexGrow
        {
            get => NativeMethods.TaffyStyle_GetFlexGrow(_ptr);
            set => NativeMethods.TaffyStyle_SetFlexGrow(_ptr, value).ThrowIfError();
        }
        public float FlexShrink
        {
            get => NativeMethods.TaffyStyle_GetFlexShrink(_ptr);
            set => NativeMethods.TaffyStyle_SetFlexShrink(_ptr, value).ThrowIfError();
        }

        // Grid
        public TaffyGridAutoFlow GridAutoFlow
        {
            get => NativeMethods.TaffyStyle_GetGridAutoFlow(_ptr);
            set => NativeMethods.TaffyStyle_SetGridAutoFlow(_ptr, value).ThrowIfError();
        }
        public TaffyGridPlacement GridColumn
        {
            get => NativeMethods.TaffyStyle_GetGridColumn(_ptr);
            set => NativeMethods.TaffyStyle_SetGridColumn(_ptr, value).ThrowIfError();
        }
        public TaffyGridPlacement GridRow
        {
            get => NativeMethods.TaffyStyle_GetGridRow(_ptr);
            set => NativeMethods.TaffyStyle_SetGridRow(_ptr, value).ThrowIfError();
        }

        // Size
        public TaffyDimension Width
        {
            get => NativeMethods.TaffyStyle_GetWidth(_ptr);
            set => NativeMethods.TaffyStyle_SetWidth(_ptr, value.value, value.unit).ThrowIfError();
        }
        public TaffyDimension Height
        {
            get => NativeMethods.TaffyStyle_GetHeight(_ptr);
            set => NativeMethods.TaffyStyle_SetHeight(_ptr, value.value, value.unit).ThrowIfError();
        }
        public TaffyDimension MinWidth
        {
            get => NativeMethods.TaffyStyle_GetMinWidth(_ptr);
            set => NativeMethods.TaffyStyle_SetMinWidth(_ptr, value.value, value.unit).ThrowIfError();
        }
        public TaffyDimension MinHeight
        {
            get => NativeMethods.TaffyStyle_GetMinHeight(_ptr);
            set => NativeMethods.TaffyStyle_SetMinHeight(_ptr, value.value, value.unit).ThrowIfError();
        }
        public TaffyDimension MaxWidth
        {
            get => NativeMethods.TaffyStyle_GetMaxWidth(_ptr);
            set => NativeMethods.TaffyStyle_SetMaxWidth(_ptr, value.value, value.unit).ThrowIfError();
        }
        public TaffyDimension MaxHeight
        {
            get => NativeMethods.TaffyStyle_GetMaxHeight(_ptr);
            set => NativeMethods.TaffyStyle_SetMaxHeight(_ptr, value.value, value.unit).ThrowIfError();
        }

        // Inset
        public TaffyDimension InsetTop
        {
            get => NativeMethods.TaffyStyle_GetInsetTop(_ptr);
            set => NativeMethods.TaffyStyle_SetInsetTop(_ptr, value.value, value.unit).ThrowIfError();
        }
        public TaffyDimension InsetBottom
        {
            get => NativeMethods.TaffyStyle_GetInsetBottom(_ptr);
            set => NativeMethods.TaffyStyle_SetInsetBottom(_ptr, value.value, value.unit).ThrowIfError();
        }
        public TaffyDimension InsetLeft
        {
            get => NativeMethods.TaffyStyle_GetInsetLeft(_ptr);
            set => NativeMethods.TaffyStyle_SetInsetLeft(_ptr, value.value, value.unit).ThrowIfError();
        }
        public TaffyDimension InsetRight
        {
            get => NativeMethods.TaffyStyle_GetInsetRight(_ptr);
            set => NativeMethods.TaffyStyle_SetInsetRight(_ptr, value.value, value.unit).ThrowIfError();
        }

        // Margin
        public TaffyDimension MarginTop
        {
            get => NativeMethods.TaffyStyle_GetMarginTop(_ptr);
            set => NativeMethods.TaffyStyle_SetMarginTop(_ptr, value.value, value.unit).ThrowIfError();
        }
        public TaffyDimension MarginBottom
        {
            get => NativeMethods.TaffyStyle_GetMarginBottom(_ptr);
            set => NativeMethods.TaffyStyle_SetMarginBottom(_ptr, value.value, value.unit).ThrowIfError();
        }
        public TaffyDimension MarginLeft
        {
            get => NativeMethods.TaffyStyle_GetMarginLeft(_ptr);
            set => NativeMethods.TaffyStyle_SetMarginLeft(_ptr, value.value, value.unit).ThrowIfError();
        }
        public TaffyDimension MarginRight
        {
            get => NativeMethods.TaffyStyle_GetMarginRight(_ptr);
            set => NativeMethods.TaffyStyle_SetMarginRight(_ptr, value.value, value.unit).ThrowIfError();
        }
        public void SetMargin(TaffyEdge edge, TaffyDimension value) =>
            NativeMethods.TaffyStyle_SetMargin(_ptr, edge, value).ThrowIfError();

        // Padding
        public TaffyDimension PaddingTop
        {
            get => NativeMethods.TaffyStyle_GetPaddingTop(_ptr);
            set => NativeMethods.TaffyStyle_SetPaddingTop(_ptr, value.value, value.unit).ThrowIfError();
        }
        public TaffyDimension PaddingBottom
        {
            get => NativeMethods.TaffyStyle_GetPaddingBottom(_ptr);
            set => NativeMethods.TaffyStyle_SetPaddingBottom(_ptr, value.value, value.unit).ThrowIfError();
        }
        public TaffyDimension PaddingLeft
        {
            get => NativeMethods.TaffyStyle_GetPaddingLeft(_ptr);
            set => NativeMethods.TaffyStyle_SetPaddingLeft(_ptr, value.value, value.unit).ThrowIfError();
        }
        public TaffyDimension PaddingRight
        {
            get => NativeMethods.TaffyStyle_GetPaddingRight(_ptr);
            set => NativeMethods.TaffyStyle_SetPaddingRight(_ptr, value.value, value.unit).ThrowIfError();
        }

        // Border
        public TaffyDimension BorderTop
        {
            get => NativeMethods.TaffyStyle_GetBorderTop(_ptr);
            set => NativeMethods.TaffyStyle_SetBorderTop(_ptr, value.value, value.unit).ThrowIfError();
        }
        public TaffyDimension BorderBottom
        {
            get => NativeMethods.TaffyStyle_GetBorderBottom(_ptr);
            set => NativeMethods.TaffyStyle_SetBorderBottom(_ptr, value.value, value.unit).ThrowIfError();
        }
        public TaffyDimension BorderLeft
        {
            get => NativeMethods.TaffyStyle_GetBorderLeft(_ptr);
            set => NativeMethods.TaffyStyle_SetBorderLeft(_ptr, value.value, value.unit).ThrowIfError();
        }
        public TaffyDimension BorderRight
        {
            get => NativeMethods.TaffyStyle_GetBorderRight(_ptr);
            set => NativeMethods.TaffyStyle_SetBorderRight(_ptr, value.value, value.unit).ThrowIfError();
        }

        // Gap
        public TaffyDimension ColumnGap
        {
            get => NativeMethods.TaffyStyle_GetColumnGap(_ptr);
            set => NativeMethods.TaffyStyle_SetColumnGap(_ptr, value.value, value.unit).ThrowIfError();
        }
        public TaffyDimension RowGap
        {
            get => NativeMethods.TaffyStyle_GetRowGap(_ptr);
            set => NativeMethods.TaffyStyle_SetRowGap(_ptr, value.value, value.unit).ThrowIfError();
        }

        // Grid template columns
        public int GridTemplateColumnsCount => (int)NativeMethods.TaffyStyle_GetGridTemplateColumnsCount(_ptr);

        public TaffyTrackSizingFunction GetGridTemplateColumnsAt(int index) =>
            NativeMethods.TaffyStyle_GetGridTemplateColumnsAt(_ptr, (System.UIntPtr)index);

        public void SetGridTemplateColumns(TaffyTrackSizingFunction[] tracks)
        {
            fixed (TaffyTrackSizingFunction* ptr = tracks)
                NativeMethods.TaffyStyle_SetGridTemplateColumns(_ptr, ptr, (System.UIntPtr)tracks.Length).ThrowIfError();
        }

        // Grid template rows
        public int GridTemplateRowsCount => (int)NativeMethods.TaffyStyle_GetGridTemplateRowsCount(_ptr);

        public TaffyTrackSizingFunction GetGridTemplateRowsAt(int index) =>
            NativeMethods.TaffyStyle_GetGridTemplateRowsAt(_ptr, (System.UIntPtr)index);

        public void SetGridTemplateRows(TaffyTrackSizingFunction[] tracks)
        {
            fixed (TaffyTrackSizingFunction* ptr = tracks)
                NativeMethods.TaffyStyle_SetGridTemplateRows(_ptr, ptr, (System.UIntPtr)tracks.Length).ThrowIfError();
        }

        // Grid auto columns
        public int GridAutoColumnsCount => (int)NativeMethods.TaffyStyle_GetGridAutoColumnsCount(_ptr);

        public TaffyTrackSizingFunction GetGridAutoColumnsAt(int index) =>
            NativeMethods.TaffyStyle_GetGridAutoColumnsAt(_ptr, (System.UIntPtr)index);

        public void SetGridAutoColumns(TaffyTrackSizingFunction[] tracks)
        {
            fixed (TaffyTrackSizingFunction* ptr = tracks)
                NativeMethods.TaffyStyle_SetGridAutoColumns(_ptr, ptr, (System.UIntPtr)tracks.Length).ThrowIfError();
        }

        // Grid auto rows
        public int GridAutoRowsCount => (int)NativeMethods.TaffyStyle_GetGridAutoRowsCount(_ptr);

        public TaffyTrackSizingFunction GetGridAutoRowsAt(int index) =>
            NativeMethods.TaffyStyle_GetGridAutoRowsAt(_ptr, (System.UIntPtr)index);

        public void SetGridAutoRows(TaffyTrackSizingFunction[] tracks)
        {
            fixed (TaffyTrackSizingFunction* ptr = tracks)
                NativeMethods.TaffyStyle_SetGridAutoRows(_ptr, ptr, (System.UIntPtr)tracks.Length).ThrowIfError();
        }

        // Misc
        public float? AspectRatio
        {
            get { var v = NativeMethods.TaffyStyle_GetAspectRatio(_ptr); return float.IsNaN(v) ? null : v; }
            set => NativeMethods.TaffyStyle_SetAspectRatio(_ptr, value ?? float.NaN).ThrowIfError();
        }
        public float ScrollbarWidth
        {
            get => NativeMethods.TaffyStyle_GetScrollbarWidth(_ptr);
            set => NativeMethods.TaffyStyle_SetScrollbarWidth(_ptr, value).ThrowIfError();
        }
    }

    /// <summary>
    /// Managed wrapper around a Taffy layout tree. Dispose to free native memory.
    /// </summary>
    public sealed unsafe class TaffyTree : IDisposable
    {
        private TaffyNativeTree* _ptr;

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
            }
        }

        private TaffyNativeTree* Ptr => _ptr != null ? _ptr : throw new ObjectDisposedException(nameof(TaffyTree));

        public TaffyNode NewNode()
        {
            var result = NativeMethods.TaffyTree_NewNode(Ptr);
            result.return_code.ThrowIfError();
            return new TaffyNode(result.value);
        }

        public void RemoveNode(TaffyNode node) =>
            NativeMethods.TaffyTree_RemoveNode(Ptr, node.Id).ThrowIfError();

        public void AppendChild(TaffyNode parent, TaffyNode child) =>
            NativeMethods.TaffyTree_AppendChild(Ptr, parent.Id, child.Id).ThrowIfError();

        public TaffyStyleRef GetStyle(TaffyNode node)
        {
            var result = NativeMethods.TaffyTree_GetStyleMut(Ptr, node.Id);
            result.return_code.ThrowIfError();
            return new TaffyStyleRef(result.value);
        }

        public void ComputeLayout(TaffyNode root, float availableWidth = float.PositiveInfinity, float availableHeight = float.PositiveInfinity) =>
            NativeMethods.TaffyTree_ComputeLayout(Ptr, root.Id, availableWidth, availableHeight).ThrowIfError();

        public void PrintTree(TaffyNode root) =>
            NativeMethods.TaffyTree_PrintTree(Ptr, root.Id).ThrowIfError();

        public TaffyLayout GetLayout(TaffyNode node)
        {
            var result = NativeMethods.TaffyTree_GetLayout(Ptr, node.Id);
            result.return_code.ThrowIfError();
            return result.value;
        }

        public void SetMeasureFunction(
            TaffyNode node,
            NativeMethods.TaffyTree_SetNodeContext_measure_function_delegate measureFn,
            void* context = null) =>
            NativeMethods.TaffyTree_SetNodeContext(Ptr, node.Id, measureFn, context).ThrowIfError();

        public int ChildCount(TaffyNode parent) =>
            (int)NativeMethods.TaffyTree_ChildCount(Ptr, parent.Id);

        public TaffyNode ChildAt(TaffyNode parent, int index)
        {
            var result = NativeMethods.TaffyTree_ChildAt(Ptr, parent.Id, (System.UIntPtr)index);
            result.return_code.ThrowIfError();
            return new TaffyNode(result.value);
        }
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
        public static TaffyTrackSizingFunction Fr(float value) =>
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
}
