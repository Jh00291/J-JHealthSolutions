using System.ComponentModel;

namespace J_JHealthSolutions.Model;

/// <summary>
/// Represents the unit of measure for various hospital tests.
/// </summary>
public enum UnitOfMeasure
{
    [Description("mg/dL")]
    MilligramsPerDeciliter,

    [Description("mmHg")]
    MillimetersOfMercury,

    [Description("kg/m²")]
    KilogramsPerSquareMeter,

    [Description("bpm")]
    BeatsPerMinute,

    [Description("L")]
    Liters,

    [Description("mL")]
    Milliliters,

    [Description("μg")]
    Micrograms,

    [Description("ng/mL")]
    NanogramsPerMilliliter,

    [Description("cells/μL")]
    CellsPerMicroliter,

    [Description("IU")]
    InternationalUnits,

    [Description("%")]
    Percentage,

    [Description("g/dL")]
    GramsPerDeciliter,

    [Description("Units/L")]
    UnitsPerLiter,

    [Description("mol/L")]
    MolesPerLiter,

    [Description("mEq/L")]
    MilliequivalentsPerLiter,

}