using ESRI.ArcGIS.Geodatabase;

public class CSharpHelper
{
    public static void IGeometryDefEdit_set_GridSize_2(IGeometryDefEdit igde, int Index, double A_2)
    {
        // This is not directly accessible from VB.NET in VS2015 so we implement in C#
        igde.set_GridSize(Index, A_2);
    }

    public static void IFieldsEdit_set_Field_2(IFieldsEdit ife, int Index, IField A_2)
    {
        // This is not directly accessible from VB.NET in VS2015 so we implement in C#
        ife.set_Field(Index, A_2);
    }
}