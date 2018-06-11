from math import sqrt
from rateCalcs import *
from linreg_def import linreg
from linreg_def import weightlinreg
from moveShoreline import moveShoreline
from summaryCalcs import get_summary, calcNstar
from summaryCalcs import calcStuff


# 'summary_dict' saves information needed for shorelihne averaging, 
# which will be done after all the rates have been calculated
# I have to preallocate the 'keys' so I can append to them later
global summary_dict
global summary_report


def init_summary_dict():
    global summary_dict
    summary_dict = {'transectId': [], 'group': [],'EPR': [], 'EPRunc': [], 'SCE': [], 'NSM': [], 'TCD': [], 'fillFlag': [] }
    summary_dict.update( {'LRR': [], 'LCI': [], 'WLR': [], 'WCI': [], 'dates': [], 'bias': [], 'biasUncy': []} )


init_summary_dict()


def rateList():
    return [
      {
        "name": "SCE",
        "parent": "SCE",
        "alias": "Shoreline Change Envelope",
        "type": "Double",
        "units": "m",
        "prefix": "DISTANCE",
        "category": "Distance Measurement"
      },
      {
        "name": "NSM",
        "parent": "NSM",
        "alias": "Net Shoreline Movement",
        "type": "Double",
        "units": "m",
        "prefix": "DISTANCE",
        "category": "Distance Measurement"
      },
      {
        "name": "EPR",
        "parent": "EPR",
        "alias": "End Point Rate",
        "type": "Double",
        "units": "m/yr",
        "prefix": "RATE",
        "category": "Point Change"
      },
     {
        "name": "EPRunc",
        "parent": "EPR",
        "alias": "End Point Rate Uncertainty",
        "type": "Double",
      },
      {
        "name": "LRR",
        "parent": "LRR",
        "alias": "Linear Regression Rate",
        "type": "Double",
        "units": "m/yr",
        "prefix": "RATE",
        "category": "Regression Statistics"
      },
      {
        "name": "LR2",
        "parent": "LRR",
        "alias": "R-squared LRR",
        "type": "Double",
      },
      {
        "name": "LSE",
        "parent": "LRR",
        "alias": "Standard Error LRR",
        "type": "Double",
      },
      {
        "name": "LCI{CI}",
        "parent": "LRR",
        "alias": "Confidence Interval LRR",
        "type": "Double",
      },
      {
        "name": "WLR",
        "parent": "WLR",
        "alias": "Weighted Linear Regression",
        "type": "Double",
        "units": "m/yr",
        "prefix": "RATE",
        "category": "Regression Statistics"
      },
      {
        "name": "WR2",
        "parent": "WLR",
        "alias": "R-squared WLR",
        "type": "Double",
      },
      {
        "name": "WSE",
        "parent": "WLR",
        "alias": "Standard Error WLR",
        "type": "Double",
      },
      {
        "name": "WCI{CI}",
        "parent": "WLR",
        "alias": "Confidence Interval WLR",
        "type": "Double",
      }
    ]

# this code gets data for one transect, it applies bias (where appropriate) 
# and finds the rates for this transect 
# afarris@usgs.gov 2017July26
# afarris@usgs.gov 2017Sep19
# afarris@usgs.gov 2017Sep21 added uncertainty and bias_unceratinty to 'rates' for output
# afarris@usgs.gov 2017Sep21 added gobal variables for shoreline averaging
# afarris@usgs.gov 2017Sep25 added code in end to test calculation of summary calcs for summary report
# afarris@usgs.gov 2017Sep26 handles case when no bias data is passed correctly
# afarris@usgs.gov 2017Sep28 added exceptions
# afarris@usgs.gov 2017Sep29 fixed bias bug... bais_distance not returned if bias applied but not bias data for this transect
# afarris@usgs.gov 2017Oct02 fixed problem with bais_distance etc
# afarris@usgs.gov 2017Oct13 added a few lines to allow saving of date info to summary report
# afarris@usgs.gov 2017Nov03 now 'type' can also be WDL, WDL shorelines are NOT bias shifted
# afarris@usgs.gov 2017Nov03 addeed ability to handle WDL as shoteline type
# afarris@usgs.gov 2017Dec15 now always calculate LRR so we will have data if we need it
# afarris@usgs.gov 2017Dec19 if bias_flag = 0, but bias = 0, I now look for recent bias value I can use
# afarris@usgs.gov 2018Feb08, fix another bias problem, the fix above was bootstrapping bias accross big gaps, now have fillFlag


def runCalcs():
    # sample data from 20131023_104317.GA_transects.in.xml, tansect 14
    params = {
#        "dates": [1924.67, 1933.83, 1973.75,1978.16, 1979.08, 1999.77 ],
        "dates": [1933, 1924, 1973.75,1978.16, 1979.08, 1999.77 ],
        "shore": [-1279.72, -761.09, -389.5, -386.32, -386.31, -290.73 ],
        "uncy": [10.8, 10.8, 5.1, 5.1, 5.1, 6.9 ],
        "CI": 0.90,
        "rates2calc": ['EPR','NSM','LRR','WLR'],
        "group": 13,
        "type": ["HWL","HWL","HWL","HWL","HWL","MHW"],
        "bias": 14.48,
        "biasUncy": 8.12,
        "shore_x": 457977,
        "shore_y": 3940206,
        "origin_x": 458363,
        "origin_y": 3940206,
        "distance_to_shore_from_origin": 386
    }

    return calc(params)


def calc(params):
    global summary_dict

    dates = params.get("dates")
    shore = params.get("shore")
    uncy = params.get("uncy")
    CI = params.get("CI")
    rates2calc = params.get("rates2calc")
    transectId = params.get("transectId")
    TCD = params.get("TCD")

    # the following are optional, 
    type = params.get("type")
    bias = params.get("bias")
    biasUncy = params.get("biasUncy")
    group = params.get("group")

    shore_x = params.get("shore_x")
    shore_y = params.get("shore_y")
    origin_x = params.get("origin_x")
    origin_y = params.get("origin_y")
    d2r = params.get("distance_to_shore_from_origin")

    # check if bias has been genearlly applied, even if this transect has no bias value:
    bias_flag = params.has_key('bias')
    # check that inputs make sense
    if not transectId:
        raise Exception('IPY: transect ID missing')
    if not dates or not shore or not uncy or not CI:
        # throw exception, input data not passed in
        raise Exception('IPY: A least one of the following is empty: dates, shore, uncy, CI, group on transect : '+ str(transectId))
    if len(dates) != len(shore) or len(dates) != len(uncy):
        # throw exception
        raise Exception('IPY: all these variables should be the same length: dates, shore, uncy. They are not for transect: ' + str(transectId))
    if bias:
        if not biasUncy or not type:
            # throw exception, no bias uncy
            raise Exception('IPY: bias was passed in, but either bias_uncy or type were not for transect: ' + str(transectId))
        if not shore_x or not shore_y or not d2r:
            # throw exception, no bias uncy
            raise Exception('IPY: bias was passed in, but either shore_x, shore_y or distance were not for transect: ' + str(transectId))
        if not origin_x or not origin_y:
            # throw exception, no bias uncy
            raise Exception('IPY: bias was passed in, but either origin_x or origin_y were not for transect: ' + str(transectId))
        if len(shore_x) != len(shore_y) or len(shore_x) != len(d2r):
            # throw exception
            raise Exception('IPY: all these variables should be the same length: shore_x, shore_y, distance but are not for transect: ' + str(transectId))

    # SCE = Shoreline Change Envelope
    # NSM = Net Shoreline Movement
    # EPR = End Point Rate
    # NB = no bias

    shoreNoBias = list(shore)
    shoreBias = list(shore)
    uncyNoBias = list(uncy)
    uncyWBias = list(uncy)
    # the follow keeps track of bias values that were 'borrowed' from another transect so they are borrowed again
    fillFlag = 0

        # assume bias could either be not passed in, or a zero passed in, either way, I set it to 0
    if not bias:
        bias = 0
    if bias_flag == 1 and bias == 0:
        # if bias is generally applied, but no bias value exists for this transect, then look
        # for an old value that can be used if it is from a transect that is close enough to this transect
        oldBias = summary_dict.get("bias")
        oldBiasUncy = summary_dict.get("biasUncy")
        oldTCD = summary_dict.get("TCD")
        oldFillFlag = summary_dict.get("fillFlag")
        if oldBias and oldBiasUncy and oldTCD:
            # first get all good biases
            biasGood = []
            biasUncyGood = []
            distGood = []
            for b,t,u,f in zip(oldBias,oldTCD,oldBiasUncy,oldFillFlag):
                if b > 0 and f == 0:
                    # only keep non-zero bias and values that weren't taken from another transect(flag=0)
                    biasGood.append(b)
                    biasUncyGood.append(u)
                    distGood.append(abs(t - TCD))
            # if there are any good bias values,  find closest bias using 'total cumulative distance'
            if biasGood:
                # set initial values to the first value just to start
                nearestBias = biasGood[0]
                nearestBiasUncy = biasUncyGood[0]
                nearestDist = distGood[0]
                for b,u,d in zip(biasGood,biasUncyGood,distGood):
                    if d < nearestDist:
                        # reset the values every time a closer value is found
                        # (this would be a lot easier if I could use Matlab's "[b,i] = min(x)")
                        nearestBias = b
                        nearestBiasUncy = u
                        nearestDist = d
                # see if closest bias value is close enough
                if nearestDist < 2000:
                    # it is close enoough
                    bias = nearestBias
                    biasUncy = nearestBiasUncy
                    # set flag to 1 so this bias data isn't used for another transect
                    fillFlag = 1    

    if bias != 0:
        # if we have a bias value, step thru each shoreline on this transect applying bias where necesary
        for ii in range(len(type)):     
            if type[ii] == "HWL":
                # apply bias
                shoreBias[ii] = shoreBias[ii] + bias
                # add bias uncy to shore uncy in quadrature
                uncyWBias[ii] = sqrt(uncy[ii]*uncy[ii] + biasUncy*biasUncy)
            elif type[ii] == "MHW" or type[ii] == "WDL":
                # data does not need to be shifted
                # but I think I need a line of code or python gets upset
                foo = 1
            else:
                # throw exception, type has to be MHW or HWL
                raise Exception('IPY: type has to be HWL, WDL or MHW, but is not for transect: ' + str(transect))
    else:
        a=1
        
    # some transects are missing data, get rid of zeros
    # this is left over from the last version of DSAS, it probably isn't necesary
    # but I am leaving it in just in case.
    removeZeros(dates,shoreBias,uncyWBias)
    removeZeros(dates,shoreNoBias,uncyNoBias)
    # find the indicies for first and last survey
    indexFirst, indexLast = getIndices(dates,shoreBias)

    # create 'dictionary' to pass to all the rate calculation functions
    data = {'shoreNoBias': shoreNoBias, 'shoreBias':shoreBias, 'dates':dates, 'uncyNoBias': uncyNoBias, 'uncyWBias': uncyWBias, 'type': type, 'bias':bias, 'indexFirst':indexFirst, 'indexLast':indexLast,'CI': CI, 'shore_x': shore_x, 'shore_y': shore_y, 'origin_x': origin_x, 'origin_y': origin_y, 'd2r': d2r, 'bias_flag': bias_flag}
   
    # now calculate the rates
    # preallocate 'rates' which hold all the calculated rates
    rates = {}

    # get number of shorelines
    dates = data.get('dates')
    ShrCount = len(dates)

    # code is set up so that if a bias value is passed in, it will calc both bias and
    # no bias rates, ie: EPR and NB_EPR
    # However, if no bias value is passed in, it will only calculate the no bias rate (of course)
    # but will call it the standard name, ie EPR.  NB_EPR is not returned

    summary_dict['bias'].append(bias)
    summary_dict['biasUncy'].append(biasUncy)
    summary_dict['TCD'].append(TCD)
    summary_dict['fillFlag'].append(fillFlag)

    if ShrCount > 1:
        summary_dict['dates'].extend(dates)
        summary_dict['group'].append(group)
        summary_dict['transectId'].append(transectId)
        if dates[indexLast] == dates[indexFirst]:
            # throw exception, trouble with dates
            raise Exception('IPY: There is something wrong with the dates for transectId: ' + str(transect))

        # always calcukate LRR because we might need the information
        rates.update(linreg(data))
        summary_dict['LRR'].append( rates.get("LRR") )
        summary_dict['LCI'].append( rates.get("LCI") )

        for c in rates2calc:
            if c == "EPR":
                # calc end point rate
                rates.update (endPointRate(data))
                summary_dict[ 'EPR' ].append( rates.get("EPR") )
                summary_dict['EPRunc'].append( rates.get("EPRunc") )
            elif c == 'NSM':
                # find the net shoreline movement
                rates.update (netShoreMove(data))
                summary_dict['NSM'].append( rates.get("NSM") )
            elif c == 'SCE':
                # find difference between most landward and most seaward shorelines
                rates.update (shoreChangeEnvelope(data))
                summary_dict['SCE'].append( rates.get("SCE")  )
            #elif c == 'LRR':
                 # calculate linear regression
                 # rates.update(linreg(data))
                 # summary_dict['LRR'].append( rates.get("LRR") )
                 # summary_dict['LCI'].append( rates.get("LCI") )
            elif c == 'WLR':
                 # calculate weighted linear regression
                 rates.update(weightlinreg(data))
                 summary_dict['WLR'].append( rates.get("WLR") )
                 summary_dict['WCI'].append( rates.get("WCI") )

    if bias_flag:
        if bias != 0 :
            # bias is being applied AND bias exists for this transect
            rates.update(moveShoreline(data))
            rates.update({'bias_uncertainty': uncyWBias })
        else:
            # no good bias data.
            # put in None for the output from moveShoreline
            rates.update({'bias_x': [None]*len(type), 'bias_y': [None]*len(type), 'bias': None, 'bias_distance': [None]*len(type)})
            rates.update({'bias_uncertainty': [None]*len(type) })
    
    rates.update({'uncertainty': uncyNoBias} )

    return (rates)

def getSummary():
    global summary_dict
    global summary_report 
    report = get_summary(summary_dict)
    init_summary_dict()
    return report


