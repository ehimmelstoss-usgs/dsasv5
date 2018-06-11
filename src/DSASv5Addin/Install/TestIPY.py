import sys
import clr

clr.AddReference('ESRI.ArcGIS.System')
clr.AddReference('ESRI.ArcGIS.Carto')
clr.AddReference('ESRI.ArcGIS.ArcMapUI')
clr.AddReference('ESRI.ArcGIS.Framework')

from ESRI.ArcGIS import esriSystem
from ESRI.ArcGIS import Carto
from ESRI.ArcGIS import ArcMapUI
from ESRI.ArcGIS import Framework

#import xalglib



# The class you want to access externally.
class DoCalculations():

    #def doStudentTDistribution(self, arg1, arg2):
    #    #return str(sys.path)
    #    return xalglib.studenttdistribution(arg1, arg2)


    def doit(self, doc):
        fMap = ArcMapUI.IMxDocument.FocusMap.GetValue(doc)        
        lyrs = []
        lyrs_enum = Carto.IMap.get_Layers(fMap)
        for i in range(Carto.IMap.LayerCount.GetValue(fMap)):
            lyr = Carto.IEnumLayer.Next(lyrs_enum)
            lyrs.append(Carto.ILayer.Name.GetValue(lyr))
        return str(lyrs)

