//! Values types for C FFI

use taffy::prelude as core;

use super::{TaffyFFIDefault, TaffyReturnCode};

#[derive(Debug, Clone, Copy, PartialEq, Eq)]
#[repr(C)]
pub enum TaffyEdge {
    /// The top edge of the box
    Top,
    /// The bottom edge of the box
    Bottom,
    /// The left edge of the box
    Left,
    /// The right edge of the box
    Right,
    /// Both the top and bottom edges of the box
    Vertical,
    /// Both the left and right edges of the box
    Horizontal,
    /// All four edges of the box
    All,
}

#[derive(Debug, Clone, Copy, PartialEq, Eq)]
#[repr(C)]
pub enum TaffyUnit {
    /// A none value (used to unset optional fields)
    None,
    /// Fixed Length (pixel) value
    Length,
    /// Percentage value
    Percent,
    /// Min-content size
    MinContent,
    /// Max-content size
    MaxContent,
    /// fit-content() function with a pixel limit
    FitContentPx,
    /// fit-content() function with a percentage limit
    FitContentPercent,
    /// Automatic values
    Auto,
    /// fr unit
    Fr,
}

#[derive(Debug, Clone, Copy, PartialEq, Eq)]
#[repr(C)]
pub enum TaffyMeasureMode {
    /// A none value (used to unset optional fields)
    Exact,
    /// Fixed Length (pixel) value
    FitContent,
    /// Percentage value
    MinContent,
    /// Min-content size
    MaxContent,
}

#[derive(Debug, Clone, Copy)]
#[repr(C)]
pub struct TaffySize {
    pub width: f32,
    pub height: f32,
}
impl From<TaffySize> for core::Size<f32> {
    #[inline(always)]
    fn from(value: TaffySize) -> Self {
        core::Size { width: value.width, height: value.height }
    }
}

#[repr(C)]
pub struct TaffyLayout {
    pub x: f32,
    pub y: f32,
    pub width: f32,
    pub height: f32,
}
impl TaffyFFIDefault for TaffyLayout {
    fn default() -> Self {
        TaffyLayout { x: 0.0, y: 0.0, width: 0.0, height: 0.0 }
    }
}

#[derive(Debug, Clone, Copy, PartialEq)]
#[repr(C)]
pub struct TaffyDimension {
    /// The value. If the unit is variant that doesn't require a value (e.g. Auto) then the value is ignored.
    pub value: f32,
    pub unit: TaffyUnit,
}
impl TaffyFFIDefault for TaffyDimension {
    fn default() -> Self {
        Self { unit: TaffyUnit::None, value: 0.0 }
    }
}

impl TaffyDimension {
    #[inline(always)]
    pub fn from_raw(unit: TaffyUnit, value: f32) -> Self {
        Self { unit, value }
    }
}

impl From<core::LengthPercentage> for TaffyDimension {
    fn from(value: core::LengthPercentage) -> Self {
        let raw = value.into_raw();
        match raw.tag() {
            core::CompactLength::LENGTH_TAG => Self { unit: TaffyUnit::Length, value: raw.value() },
            core::CompactLength::PERCENT_TAG => Self { unit: TaffyUnit::Percent, value: raw.value() },
            _ => Self { unit: TaffyUnit::None, value: 0.0 },
        }
    }
}

impl TryFrom<TaffyDimension> for core::LengthPercentage {
    type Error = TaffyReturnCode;

    fn try_from(value: TaffyDimension) -> Result<Self, Self::Error> {
        match value.unit {
            TaffyUnit::Length => Ok(core::LengthPercentage::length(value.value)),
            TaffyUnit::Percent => Ok(core::LengthPercentage::percent(value.value)),
            TaffyUnit::None => Err(TaffyReturnCode::InvalidNone),
            TaffyUnit::Auto => Err(TaffyReturnCode::InvalidAuto),
            TaffyUnit::MinContent => Err(TaffyReturnCode::InvalidMinContent),
            TaffyUnit::MaxContent => Err(TaffyReturnCode::InvalidMaxContent),
            TaffyUnit::FitContentPx => Err(TaffyReturnCode::InvalidFitContentPx),
            TaffyUnit::FitContentPercent => Err(TaffyReturnCode::InvalidFitContentPercent),
            TaffyUnit::Fr => Err(TaffyReturnCode::InvalidFr),
        }
    }
}

impl From<core::LengthPercentageAuto> for TaffyDimension {
    fn from(value: core::LengthPercentageAuto) -> Self {
        let raw = value.into_raw();
        match raw.tag() {
            core::CompactLength::LENGTH_TAG => Self { unit: TaffyUnit::Length, value: raw.value() },
            core::CompactLength::PERCENT_TAG => Self { unit: TaffyUnit::Percent, value: raw.value() },
            core::CompactLength::AUTO_TAG => Self { unit: TaffyUnit::Auto, value: 0.0 },
            _ => Self { unit: TaffyUnit::None, value: 0.0 },
        }
    }
}

impl TryFrom<TaffyDimension> for core::LengthPercentageAuto {
    type Error = TaffyReturnCode;

    fn try_from(value: TaffyDimension) -> Result<Self, Self::Error> {
        match value.unit {
            TaffyUnit::Auto => Ok(core::LengthPercentageAuto::auto()),
            TaffyUnit::Length => Ok(core::LengthPercentageAuto::length(value.value)),
            TaffyUnit::Percent => Ok(core::LengthPercentageAuto::percent(value.value)),
            TaffyUnit::None => Err(TaffyReturnCode::InvalidNone),
            TaffyUnit::MinContent => Err(TaffyReturnCode::InvalidMinContent),
            TaffyUnit::MaxContent => Err(TaffyReturnCode::InvalidMaxContent),
            TaffyUnit::FitContentPx => Err(TaffyReturnCode::InvalidFitContentPx),
            TaffyUnit::FitContentPercent => Err(TaffyReturnCode::InvalidFitContentPercent),
            TaffyUnit::Fr => Err(TaffyReturnCode::InvalidFr),
        }
    }
}

impl From<core::Dimension> for TaffyDimension {
    fn from(value: core::Dimension) -> Self {
        let raw = value.into_raw();
        match raw.tag() {
            core::CompactLength::LENGTH_TAG => Self { unit: TaffyUnit::Length, value: raw.value() },
            core::CompactLength::PERCENT_TAG => Self { unit: TaffyUnit::Percent, value: raw.value() },
            core::CompactLength::AUTO_TAG => Self { unit: TaffyUnit::Auto, value: 0.0 },
            _ => Self { unit: TaffyUnit::None, value: 0.0 },
        }
    }
}

impl TryFrom<TaffyDimension> for core::Dimension {
    type Error = TaffyReturnCode;

    fn try_from(value: TaffyDimension) -> Result<Self, Self::Error> {
        match value.unit {
            TaffyUnit::Auto => Ok(core::Dimension::auto()),
            TaffyUnit::Length => Ok(core::Dimension::length(value.value)),
            TaffyUnit::Percent => Ok(core::Dimension::percent(value.value)),
            TaffyUnit::None => Err(TaffyReturnCode::InvalidNone),
            TaffyUnit::MinContent => Err(TaffyReturnCode::InvalidMinContent),
            TaffyUnit::MaxContent => Err(TaffyReturnCode::InvalidMaxContent),
            TaffyUnit::FitContentPx => Err(TaffyReturnCode::InvalidFitContentPx),
            TaffyUnit::FitContentPercent => Err(TaffyReturnCode::InvalidFitContentPercent),
            TaffyUnit::Fr => Err(TaffyReturnCode::InvalidFr),
        }
    }
}

/// Track sizing function for CSS Grid layout.
///
/// Corresponds to `TrackSizingFunction = MinMax<MinTrackSizingFunction, MaxTrackSizingFunction>` in Taffy.
/// The `min` field is the minimum sizing function and `max` is the maximum sizing function.
/// Both are encoded as `TaffyDimension`; note that `Fr` and `FitContent*` are invalid for `min`.
#[derive(Debug, Clone, Copy, PartialEq)]
#[repr(C)]
pub struct TaffyTrackSizingFunction {
    pub min: TaffyDimension,
    pub max: TaffyDimension,
}

impl Default for TaffyTrackSizingFunction {
    fn default() -> Self {
        Self {
            min: TaffyDimension { unit: TaffyUnit::Auto, value: 0.0 },
            max: TaffyDimension { unit: TaffyUnit::Auto, value: 0.0 },
        }
    }
}

impl From<core::MinTrackSizingFunction> for TaffyDimension {
    fn from(value: core::MinTrackSizingFunction) -> Self {
        let raw = value.into_raw();
        match raw.tag() {
            core::CompactLength::LENGTH_TAG => Self { unit: TaffyUnit::Length, value: raw.value() },
            core::CompactLength::PERCENT_TAG => Self { unit: TaffyUnit::Percent, value: raw.value() },
            core::CompactLength::AUTO_TAG => Self { unit: TaffyUnit::Auto, value: 0.0 },
            core::CompactLength::MIN_CONTENT_TAG => Self { unit: TaffyUnit::MinContent, value: 0.0 },
            core::CompactLength::MAX_CONTENT_TAG => Self { unit: TaffyUnit::MaxContent, value: 0.0 },
            _ => Self { unit: TaffyUnit::None, value: 0.0 },
        }
    }
}

impl TryFrom<TaffyDimension> for core::MinTrackSizingFunction {
    type Error = TaffyReturnCode;
    fn try_from(value: TaffyDimension) -> Result<Self, Self::Error> {
        match value.unit {
            TaffyUnit::Auto => Ok(core::MinTrackSizingFunction::auto()),
            TaffyUnit::Length => Ok(core::MinTrackSizingFunction::length(value.value)),
            TaffyUnit::Percent => Ok(core::MinTrackSizingFunction::percent(value.value)),
            TaffyUnit::MinContent => Ok(core::MinTrackSizingFunction::min_content()),
            TaffyUnit::MaxContent => Ok(core::MinTrackSizingFunction::max_content()),
            TaffyUnit::None => Err(TaffyReturnCode::InvalidNone),
            TaffyUnit::FitContentPx => Err(TaffyReturnCode::InvalidFitContentPx),
            TaffyUnit::FitContentPercent => Err(TaffyReturnCode::InvalidFitContentPercent),
            TaffyUnit::Fr => Err(TaffyReturnCode::InvalidFr),
        }
    }
}

impl From<core::MaxTrackSizingFunction> for TaffyDimension {
    fn from(value: core::MaxTrackSizingFunction) -> Self {
        let raw = value.into_raw();
        match raw.tag() {
            core::CompactLength::LENGTH_TAG => Self { unit: TaffyUnit::Length, value: raw.value() },
            core::CompactLength::PERCENT_TAG => Self { unit: TaffyUnit::Percent, value: raw.value() },
            core::CompactLength::AUTO_TAG => Self { unit: TaffyUnit::Auto, value: 0.0 },
            core::CompactLength::MIN_CONTENT_TAG => Self { unit: TaffyUnit::MinContent, value: 0.0 },
            core::CompactLength::MAX_CONTENT_TAG => Self { unit: TaffyUnit::MaxContent, value: 0.0 },
            core::CompactLength::FIT_CONTENT_PX_TAG => Self { unit: TaffyUnit::FitContentPx, value: raw.value() },
            core::CompactLength::FIT_CONTENT_PERCENT_TAG => {
                Self { unit: TaffyUnit::FitContentPercent, value: raw.value() }
            }
            core::CompactLength::FR_TAG => Self { unit: TaffyUnit::Fr, value: raw.value() },
            _ => Self { unit: TaffyUnit::None, value: 0.0 },
        }
    }
}

impl TryFrom<TaffyDimension> for core::MaxTrackSizingFunction {
    type Error = TaffyReturnCode;
    fn try_from(value: TaffyDimension) -> Result<Self, Self::Error> {
        match value.unit {
            TaffyUnit::Auto => Ok(core::MaxTrackSizingFunction::auto()),
            TaffyUnit::Length => Ok(core::MaxTrackSizingFunction::length(value.value)),
            TaffyUnit::Percent => Ok(core::MaxTrackSizingFunction::percent(value.value)),
            TaffyUnit::MinContent => Ok(core::MaxTrackSizingFunction::min_content()),
            TaffyUnit::MaxContent => Ok(core::MaxTrackSizingFunction::max_content()),
            TaffyUnit::FitContentPx => Ok(core::MaxTrackSizingFunction::fit_content_px(value.value)),
            TaffyUnit::FitContentPercent => Ok(core::MaxTrackSizingFunction::fit_content_percent(value.value)),
            TaffyUnit::Fr => Ok(core::MaxTrackSizingFunction::fr(value.value)),
            TaffyUnit::None => Err(TaffyReturnCode::InvalidNone),
        }
    }
}

impl From<core::TrackSizingFunction> for TaffyTrackSizingFunction {
    fn from(value: core::TrackSizingFunction) -> Self {
        Self { min: value.min.into(), max: value.max.into() }
    }
}

impl TryFrom<TaffyTrackSizingFunction> for core::TrackSizingFunction {
    type Error = TaffyReturnCode;
    fn try_from(value: TaffyTrackSizingFunction) -> Result<Self, Self::Error> {
        Ok(core::TrackSizingFunction { min: value.min.try_into()?, max: value.max.try_into()? })
    }
}

/// For all fields, zero represents not set
#[derive(Debug, Clone, Copy, PartialEq)]
#[repr(C)]
pub struct TaffyGridPlacement {
    pub start: i16,
    pub end: i16,
    pub span: u16,
}

impl TaffyFFIDefault for TaffyGridPlacement {
    fn default() -> Self {
        Self { start: 0, end: 0, span: 0 }
    }
}

impl From<TaffyGridPlacement> for core::Line<core::GridPlacement> {
    fn from(p: TaffyGridPlacement) -> Self {
        let start = match (p.start, p.span) {
            (0, 0) => core::GridPlacement::Auto,
            (0, s) => core::GridPlacement::Span(s),
            (l, _) => core::GridPlacement::Line(l.into()),
        };
        let end = if p.end != 0 { core::GridPlacement::Line(p.end.into()) } else { core::GridPlacement::Auto };
        core::Line { start, end }
    }
}

impl From<core::Line<core::GridPlacement>> for TaffyGridPlacement {
    fn from(placement: core::Line<core::GridPlacement>) -> Self {
        let (start, start_span) = match placement.start {
            core::GridPlacement::Line(l) => (l.as_i16(), 0u16),
            core::GridPlacement::Span(s) => (0i16, s),
            _ => (0i16, 0u16),
        };
        let (end, end_span) = match placement.end {
            core::GridPlacement::Line(l) => (l.as_i16(), 0u16),
            core::GridPlacement::Span(s) => (0i16, s),
            _ => (0i16, 0u16),
        };
        let span = if start_span != 0 { start_span } else { end_span };
        Self { start, end, span }
    }
}
