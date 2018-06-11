# afaris@usgs.gov 2018jan07 minor changes for SCE and NSM
# afaris@usgs.gov 2018jan08 minor change -- re-ordrerd SCE output
def getSummaryCalcsLookup():
    return [
        {"name": "numTrans", "description": "total number of transects"},
        {"name": "ave", "description": "average rate"},
        {"name": "aveUncy", "description": "average of the confidence intervals associated with rates"},
        {"name": "Nstar", "description": "reduced n (number of independent transects)"},
        {"name": "uncyOfAve", "description": "uncertainty of the average rate using reduced n"},
        {"name": "ave +/- uncyOfAve", "description": "average rate with reduced n uncertainty"},
        {"name": "", "description": ""},    #use for gap
        
        {"name": "numEro", "description": "number of erosional transects"},
        {"name": "percEro", "description": "percent of all transects that are erosional"},
        {"name": "percEroSig", "description": "percent of all transects that have statistically significant erosion"},
        {"name": "min", "description": "maximum value erosion"},
        {"name": "minId", "description": "maximum value erosion transect ID"},
        {"name": "aveEro", "description": "average of all erosional rates"},
        {"name": "", "description": ""},    #use for gap

        {"name": "numAcc", "description": "number of accretional transects"},
        {"name": "percAcc", "description": "percent of all transects that are accretional"},
        {"name": "percAccSig", "description": "percent of all transects that have statistically significant accretion"},
        {"name": "max", "description": "maximum value accretion"},
        {"name": "maxId", "description": "maximum value accretion transect ID"},
        {"name": "aveAcc", "description": "average of all accretional rates"},
        # the next group is for SCE
        {"name": "aveDist", "description": "average distance"},         # this one also for NSM
        {"name": "maxDist", "description": "maximum distance"},
        {"name": "maxIdDist", "description": "maximum distance transect ID"},
        {"name": "minDist", "description": "minimum distance"},
        {"name": "minIdDist", "description": "minimum distance transect ID"},
        # the rest are for NSM
        {"name": "numEroNSM", "description": "number of transects with negative distance"}, #
        {"name": "percEroNSM", "description": "percent of all transects that have a negative distance"},
        {"name": "minNSM", "description": "maximum negative distance"},
        {"name": "minIdNSM", "description": "maximum negative distance transect ID"},
        {"name": "aveEroNSM", "description": "average of all negative distances"},
        {"name": "", "description": ""},    #use for gap
        {"name": "numAccNSM", "description": "number of transects with positive distance"},
        {"name": "percAccNSM", "description": "percent of all transects that have a positive distance"},
        {"name": "maxNSM", "description": "maximum positive distance"},
        {"name": "maxIdNSM", "description": "maximum positive distance transect ID"},
        {"name": "aveAccNSM", "description": "average of all positive distances"},



    ]


#average rates for each rate;= 			aveRate 
#maximum value accretion = 			maxRate  
#maximum value accretion (with transect ID)   = maxRateId    
#maximum value erosion = 			minRate
#maximum value erosion (with transect ID);     = minRateId
#percent erosional transects 		       = percentEro
#percent accretional transects      		= percentAcc
#total number of transects;         =  numTrans
#regionally averaged rate uncertainty         = uncyOfAve


#These next two require quite a bit of verbage to be clear about what they are:
#percent erosional transects whose erosion is considered signicantly diferent from 0        = percentEroSig
#percent accretional transects   whose accresion is considered signicantly diferent from 0        = percentAccSig

#You could call them maybe percent_signifcant_erosion ???



#I added these next two b/c I think they would be useful and Emily OK'd their inclusion
#average of shoreline uncertainty        = aveUncy
# reduced n        = nstar
