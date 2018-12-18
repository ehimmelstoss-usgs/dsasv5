from math import sqrt
from linreg_def import linreg

# this defined many of the rate calculations  
# afarris@usgs.gov 2017July26
# afarris@usgs.gov 2017Sep26 handles case when no bias data is passed correctly
# afarris@usgs.gov 2017Sep28 added exceptions
# afarris@usgs.gov 2017Oct02 fixed very minor bug in one exception
# afarris@usgs.gov 2017Dec15 now if all shoreliens are HWL, and bias_flag=1 then EPR=NB_EPR,SCE=NB_SCE and NSM=NB_NSM
# afarris@usgs.gov 2018feb12 now if all shoreliens are MHW, bias =0 and bias_flag=1 then EPR=NB_EPR,SCE=NB_SCE and NSM=NB_NSM
# afarris@usgs.gov 2018feb13 fixed typo in EPR

# if no bias data is available, it has been set to 0

def removeZeros(dates,shore,uncer):
    # this bit of code is left over from previous version of DSAS, probably isn't 
    # necessary, but I don't think it will do any harm to leave it in.
    # If some transects had 4 surveys while other transects had 3 surveys,
    # then the transects with 3 surveys will have some zeros.  I need to
    # get rid of these elements. I search for a 0 in the date.

    if 0 in dates:
        f = dates.index(0)
        del dates[f]
        del shore[f]
        del uncer[f]
    return(dates,shore,uncer)


def getIndices(dates,shore):
    # find first year
    dateFirst = min(dates)
    # find last year
    dateLast = max(dates)

    try:
        # get indicies for shoreline that correspond to the first and last dates
        indexFirst = dates.index(dateFirst)
        indexLast = dates.index(dateLast)
    except:
        # throw exception, trouble with dates
        raise Exception('IPY: There is something wrong with the dates ' )

    return(indexFirst,indexLast)

def netShoreMove(data):
    indexFirst = data.get('indexFirst')
    indexLast = data.get('indexLast')
    shoreNoBias = data.get('shoreNoBias')
    shoreBias = data.get('shoreBias')
    bias = data.get('bias')
    bias_flag = data.get('bias_flag')
    type = data.get('type')
    # calculate net shoreline movement
    if bias_flag:
        if bias != 0:
            # bias value was passed in, do both bias and no bias calculation
            netMoveNB = shoreNoBias[indexLast] - shoreNoBias[indexFirst]
            netMoveB = shoreBias[indexLast] - shoreBias[indexFirst]
            rates = {'NB_NSM': netMoveNB, 'NSM': netMoveB}
        else:
            # bias is being applied, but no bias value for this transect
            netMoveNB = shoreNoBias[indexLast] - shoreNoBias[indexFirst]
            rates = {'NB_NSM': netMoveNB, 'NSM': None}
            # need to check...if all shorelines are HWL or all MHW, then bias is not needed and 
            # rate can be used for both
            flag = 0
            for t in type:
                if t != 'HWL':
                    flag = flag + 1
            if flag == 0 or flag == len(type):
                    # all shoreline are HWL or all are (MHW or WDL)
                    rates.update ( {'NSM': netMoveNB})

    else:
        # no bias data, just do no bias calc
        netMoveB = shoreNoBias[indexLast] - shoreNoBias[indexFirst]
        rates = {'NSM': netMoveB}

    return rates



def shoreChangeEnvelope(data):
    # calculate shoreline change envelope
    # diffreence between most seaward and most landward shoreline
    bias = data.get('bias')
    bias_flag = data.get('bias_flag')
    type = data.get('type')

    if bias_flag:
        # bias being applied
        if bias != 0:
            # a bias value was included, do both bias and no-bias calculation
            # do "no bias" first
            shoreNoBias = data.get('shoreNoBias')
            shoreMax = max(shoreNoBias)
            shoreMin = min(shoreNoBias)
            envelopeNB = shoreMax - shoreMin
            # now do bias
            shoreBias = data.get('shoreBias')
            shoreMax = max(shoreBias)
            shoreMin = min(shoreBias)
            envelope = shoreMax - shoreMin
            rates = {'NB_SCE': envelopeNB, 'SCE': envelope}
        else:
            # bias is being applied, but no bias data for this transect
            # do "no bias" 
            shoreNoBias = data.get('shoreNoBias')
            shoreMax = max(shoreNoBias)
            shoreMin = min(shoreNoBias)
            envelopeNB = shoreMax - shoreMin
            rates = {'NB_SCE': envelopeNB, 'SCE': None}
            # need to check...if all shorelines are HWL, then bias is not needed and 
            # rate can be used for both
            flag = 0
            for t in type:
                if t != 'HWL':
                    flag = flag + 1
            if flag == 0 or flag == len(type):
                # all shoreline are HWL or all are (MHW or WDL)
                rates.update ( {'SCE': envelopeNB})

    else:
        # bias not being applied, just do no bias calculation
        shoreNoBias = data.get('shoreNoBias')
        shoreMax = max(shoreNoBias)
        shoreMin = min(shoreNoBias)
        envelope = shoreMax - shoreMin
        rates = {'SCE': envelope}
    return rates



def endPointRate(data):
    # calculate net shoreline movement
    # do "no bias" first
    indexFirst = data.get('indexFirst')
    indexLast = data.get('indexLast')
    dates = data.get('dates')
    bias = data.get('bias')
    bias_flag = data.get('bias_flag')
    # check to see if a bias was applied
    if bias_flag:
        # bias being applied
        if bias != 0:
            # a bias value was passed in and applied, calc rates both ways 
            # first do no bias calc 
            shore = data.get('shoreNoBias')
            uncy = data.get('uncyNoBias')
            EPR = (shore[indexLast] - shore[indexFirst])/(dates[indexLast] - dates[indexFirst])
            numerator =  (uncy[indexLast] * uncy[indexLast]) + (uncy[indexFirst] * uncy[indexFirst])
            EPRunc = sqrt(numerator) / (dates[indexLast] - dates[indexFirst])
            rates = {'NB_EPR': EPR, 'NB_EPRunc': EPRunc}
        
            # now calc again using the bias shifted values
            shore = data.get('shoreBias')
            uncy = data.get('uncyWBias')
            EPR = (shore[indexLast] - shore[indexFirst])/(dates[indexLast] - dates[indexFirst])
            numerator =  (uncy[indexLast] * uncy[indexLast]) + (uncy[indexFirst] * uncy[indexFirst])
            EPRunc = sqrt(numerator) / (dates[indexLast] - dates[indexFirst])
            rates.update ( {'EPR': EPR, 'EPRunc': EPRunc})
        else:
            # bias being applied, but no bias data for this transect
            shore = data.get('shoreNoBias')
            uncy = data.get('uncyNoBias')
            type = data.get('type')
            EPR = (shore[indexLast] - shore[indexFirst])/(dates[indexLast] - dates[indexFirst])
            numerator =  (uncy[indexLast] * uncy[indexLast]) + (uncy[indexFirst] * uncy[indexFirst])
            EPRunc = sqrt(numerator) / (dates[indexLast] - dates[indexFirst])
            rates = {'NB_EPR': EPR, 'NB_EPRunc': EPRunc}
            rates.update ( {'EPR': None, 'EPRunc': None})
            # need to check...if all shorelines are HWL, then bias is not needed and 
            # rates can be used for both
            flag = 0
            for t in type:
                if t != 'HWL':
                    flag = flag + 1
            if flag == 0 or flag == len(type):
                # all shoreline are HWL or all are (MHW or WDL)
                rates.update ( {'EPR': EPR, 'EPRunc': EPRunc})

                    

    else:
        # no bias, just do the 'non-bias' calculation
        shore = data.get('shoreNoBias')
        uncy = data.get('uncyNoBias')
        EPR = (shore[indexLast] - shore[indexFirst])/(dates[indexLast] - dates[indexFirst])
        numerator =  (uncy[indexLast] * uncy[indexLast]) + (uncy[indexFirst] * uncy[indexFirst])
        EPRunc = sqrt(numerator) / (dates[indexLast] - dates[indexFirst])
        rates = {'EPR': EPR, 'EPRunc': EPRunc}

    return rates
