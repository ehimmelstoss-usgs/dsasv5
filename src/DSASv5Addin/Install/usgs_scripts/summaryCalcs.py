# afarris@usgs.gov 2017sep26 calculates stats for summary report
# afarris@usgs.gov 2017sep27 addressed issue of division by zero
# afarris@usgs.gov 2017Sep28 added exceptions
# afarris@usgs.gov 2017Sep29 tried to fix nstar problem
# afarris@usgs.gov 2017Oct02 tried to fix nstar problem again
# afarris@usgs.gov 2017Oct03 FINALLY fixed nstar problem and SCE bug
# afarris@usgs.gov 2017Oct06 made sure regional unceratinty is positive
# afarris@usgs.gov 2017Nov03 changed the way summary_report is stuctured add added new outputs
# afarris@usgs.gov 2017Nov06 minor changes
# afarris@usgs.gov 2017Dec08 filled gaps for calculation of nstar
# afarris@usgs.gov 2017Dec11 minor fix in calcualtion of nstarSum
# afarris@usgs.gov 2017Dec14 fix calcualtion of uncertainty of regional rate
# afarris@usgs.gov 2018Jan02 further refinements to calculation of nstar
# afarris@usgs.gov 2018Jan07 minor changes to SCE and NSM
# afarris@usgs.gov 2018Feb10 now check that lists passed to calcStuff are not empty 
# afarris@usgs.gov 2018jun01 made some changes suggested by code review 

from math import sqrt
def get_summary(summary_dict):
    # summary_report will hold output
    global summary_report

    # here is where I calculate all the stuff Emily needs.
    group = summary_dict.get('group')
    transectId = summary_dict.get('transectId')
    # I don't know which rates were calculated, so I try to extract them all
    EPR = summary_dict.get("EPR")
    EPRunc = summary_dict.get("EPRunc")
    NSM = summary_dict.get("NSM")
    SCE = summary_dict.get("SCE")
    LRR = summary_dict.get("LRR")
    LCI = summary_dict.get("LCI")
    WLR = summary_dict.get("WLR")
    WCI = summary_dict.get("WCI")
    dates = summary_dict.get("dates")

    if len(group) != len(transectId): 
        # throw exception
        raise Exception('IPY: Group and transectId need to be the same length ' )
    if (not EPR) and (not NSM) and (not LRR) and (not SCE) and (not WLR):
        # throw exception, no rates were calculated
        raise Exception('IPY: No rates were found ' )

    # 'set' works like Matlab's unique
    groupNumbers = set(group)
    summary_report = {'numGroups': len(groupNumbers)}

    uniqueDates = set(dates)
    summary_report.update( {'listOfDates': uniqueDates})
    #summary_report.update( {'firstDate': min(uniqueDates)})
    #summary_report.update( {'lastDate': max(uniqueDates)})

    summary_report['rates'] = {}

    if LRR:
        if len(LRR) != len(transectId) or len(LCI) != len(transectId):
            # throw exception
            raise Exception('IPY: LRR, LCI and transectId need to be the same length, they are not ')
        summary_report['rates']['LRR'] = {}
        summary_report['rates']['LRR']['A'] = {}
        # get summary stats for all transects
        # pass datat to "calcStuff", it will calculate everything and save results in 
        # global dictionary "summary_report". It returns nsatr, but we don't need it now
        foo = calcStuff(LRR,LCI,transectId,'LRR','A')
        # now get summary stats for each group
        nstarSum = 0
        for gCnt in groupNumbers:
            summary_report['rates']['LRR'][str(gCnt)] = {}
            # stepping thru each group
            # preallocate
            rateG = []
            uncyG =[]
            trIdG = []
            for g,r,u,t in zip(group,LRR,LCI,transectId): 
                # stepping thru each transect
                if g == gCnt:
                    # this transect is in this group, keep it
                    rateG.append(r)
                    uncyG.append(u)
                    trIdG.append(t)

            # now we have all the transects for this group, calc summary stats and save them
            n = calcStuff(rateG,uncyG,trIdG,'LRR',str(gCnt))
            if n:
                nstarSum = nstarSum + n
        # we've stepped thru all the groups, and added all the nstars
        # recalc uncy using this nstar,
        # first get old average
        temp = summary_report['rates']['LRR']['A']['aveUncy']
        if nstarSum != 0:
            summary_report['rates']['LRR']['A']['uncyOfAve'] = abs(temp/sqrt(nstarSum))
        else:
            summary_report['rates']['LRR']['A']['uncyOfAve'] = None
        summary_report['rates']['LRR']['A']['Nstar'] = nstarSum

    if EPR:
        if  len(EPR) != len(transectId) or len(EPRunc) != len(transectId):
            # throw exception
            raise Exception('IPY: EPR, EPunc and transectId need to be the same length, they are not ' )
        summary_report['rates']['EPR'] = {}
        summary_report['rates']['EPR']['A'] = {}
        # send data out to program that will calculate everything
        foo = calcStuff(EPR,EPRunc,transectId,'EPR','A')
        # now get summary stats for each group
        nstarSum = 0
        for gCnt in groupNumbers:
            summary_report['rates']['EPR'][str(gCnt)] = {}
            # stepping thru each group
            # preallocate
            rateG = []
            uncyG =[]
            trIdG = []
            for g,r,u,t in zip(group,EPR,EPRunc,transectId): 
                # stepping thru each transect
                if g == gCnt:
                    # this transect is in this group, keep it
                    rateG.append(r)
                    uncyG.append(u)
                    trIdG.append(t)

            # now we hhave all the transects for this group, calc summary stats and save them
            n = calcStuff(rateG,uncyG,trIdG,'EPR',str(gCnt))
            if n:
                nstarSum = nstarSum + n
        # recalc uncy using this nstar,
        # first get old average
        temp = summary_report['rates']['EPR']['A']['aveUncy']
        if nstarSum != 0:
            summary_report['rates']['EPR']['A']['uncyOfAve'] = abs(temp/sqrt(nstarSum))
        else:
            summary_report['rates']['EPR']['A']['uncyOfAve'] = None
        summary_report['rates']['EPR']['A']['Nstar'] = nstarSum

    if NSM:
        if len(NSM) != len(transectId):
            # throw exception
            raise Exception('IPY: NSM and transectId need to be the same length, they are not ')
        summary_report['rates']['NSM'] = {}
        summary_report['rates']['NSM']['A'] = {}
        # send data out to program that will calculate everything
        # no uncertainty, make a fake list
        uncy = [0]*len(transectId)
        foo = calcStuff(NSM,uncy,transectId,'NSM','A')
        # now get summary stats for each group
        for gCnt in groupNumbers:
            summary_report['rates']['NSM'][str(gCnt)] = {}
            # stepping thru each group
            # preallocate
            rateG = []
            uncyG =[]
            trIdG = []
            for g,r,u,t in zip(group,NSM,uncy,transectId): 
                # stepping thru each transect
                if g == gCnt:
                    # this transect is in this group, keep it
                    rateG.append(r)
                    uncyG.append(u)
                    trIdG.append(t)

            # now we hhave all the transect for this group, calc summary stats and save them
            foo = calcStuff(rateG,uncyG,trIdG,'NSM',str(gCnt))
            # no unccy for this method, so no nstar

    if SCE:
        if len(SCE) != len(transectId):
            # throw exception
            raise Exception('IPY: SCE and transectId need to be the same length, they are not ' )
        summary_report['rates']['SCE'] = {}
        summary_report['rates']['SCE']['A'] = {}
        # send data out to program that will calculate everything
        # no uncertainty, make a fake list
        uncy = [0]*len(transectId)
        foo = calcStuff(SCE,uncy,transectId,'SCE','A')
        # now get summary stats for each group
        for gCnt in groupNumbers:
            summary_report['rates']['SCE'][str(gCnt)] = {}
            # stepping thru each group
            # preallocate
            rateG = []
            uncyG =[]
            trIdG = []
            for g,r,u,t in zip(group,SCE,uncy,transectId): 
                # stepping thru each transect
                if g == gCnt:
                    # this transect is in this group, keep it
                    rateG.append(r)
                    uncyG.append(u)
                    trIdG.append(t)
                        
            # now we have all the transects for this group, calc summary stats and save them
            foo = calcStuff(rateG,uncyG,trIdG,'SCE',str(gCnt))
            # no unccy for this method, so no nstar

    if WLR:
        if len(WLR) != len(transectId) or len(WCI) != len(transectId):
            # throw exception
            raise Exception('IPY: WLR, WCI and transectId need to be the same length , they are not' )
        summary_report['rates']['WLR'] = {}
        summary_report['rates']['WLR']['A'] = {}
        # send data out to program that will calculate everything
        foo = calcStuff(WLR,WCI,transectId,'WLR','A')
        nstarSum = 0
        # now get summary stats for each group
        for gCnt in groupNumbers:
            summary_report['rates']['WLR'][str(gCnt)] = {}
            # stepping thru each group
            # preallocate
            rateG = []
            uncyG =[]
            trIdG = []
            for g,r,u,t in zip(group,WLR,WCI,transectId): 
                # stepping thru each transect
                if g == gCnt:
                    # this transect is in this group, keep it
                    rateG.append(r)
                    uncyG.append(u)
                    trIdG.append(t)

            # now we hhave all the data for this group, calc summary stats and save them
            n = calcStuff(rateG,uncyG,trIdG,'WLR',str(gCnt))
            if n: 
                nstarSum = nstarSum + n
        # recalc uncy using this nstar,
        # first get old average
        temp = summary_report['rates']['WLR']['A']['aveUncy']
        if nstarSum != 0:
            summary_report['rates']['WLR']['A']['uncyOfAve'] = abs(temp/sqrt(nstarSum))
        else:
            summary_report['rates']['WLR']['A']['uncyOfAve'] = None
        summary_report['rates']['WLR']['A']['Nstar'] = nstarSum
        
    foo = summary_report
    return summary_report

def calcStuff(rateIn,uncyIn,trIDIn,rateName,groupName):
    from math import sqrt
    global summary_report
    # get rid of Nones first
    rate = []
    uncy = []
    trID = []
    for r,u,t in zip(rateIn,uncyIn,trIDIn): 
        if r:
            # rate is NOT a None, keep it
            rate.append(r)
            uncy.append(u)
            trID.append(t)
    if rate and uncy and trID:
        # the vecotrs are not empty, do calcs

        # make a version of uncy with filled gaps
        # This is not as good as a linear interpolation, but I don't have time to code that up.
        # This method at least is conservative, leading to an nstar that may be smaller than
        # it should be, but I'd rather it be too small than too large.
        uncyNoGap = []
        lastGood = []
        for u in uncyIn: 
            if u:
                # rate is NOT a None, keep it
                uncyNoGap.append(u)
                lastGood = u
            else:
                # there is a gap, fill gap with last good number 'lastGood'
                if lastGood:
                    uncyNoGap.append(lastGood)

        numTrans = len(trID)
        if numTrans != 0:
            sumRate = sum(rate)
            aveRate = (sumRate) / (numTrans)
            if sum(uncy) == 0:
                aveUncy = None
            else:
                aveUncy = sum(uncy) / (numTrans)
            if max(rate) > 0:
                maxRate = max(rate)
                # next line gets the transectId at the spot of maximum erosion
                maxRateId = trID[rate.index(maxRate)]
            else:
                # this variable should be empty if there was no accretion
                maxRate = None
                maxRateId = None

            if min(rate) < 0:
                minRate = min(rate)
                minRateId = trID[rate.index(minRate)]
            elif rateName == 'SCE':
                # SCE is always positive, but we still want the minimum number
                minRate = min(rate)
                minRateId = trID[rate.index(minRate)]
            else:
                # this variable should be empty if there was no erosion
                minRate = None
                minRateId = None
            # the following count the number of erosional transects
            numEro = sum(1 for f in rate if f < 0)
            numAcc = sum(1 for f in rate if f > 0)
            percentEro = float(numEro) / float(numTrans)
            percentAcc = float(numAcc) / float(numTrans)

            # Emily also wants to know the percent of transects with erosion that is considrered
            # significantly different from zero.  In other words, is rate + CI still < 0?
            y=0
            for r,u in zip(rate,uncy):
                if (r + u) < 0:
                    y= y + 1
            percentEroSig = float(y) / float(numTrans)
            y=0
            for r,u in zip(rate,uncy):
                if (r - u) > 0:
                    y= y + 1
            percentAccSig = float(y) / float(numTrans)

        
            # Now calculate the avearage erosion
            y = 0
            for r in rate:
                if (r) < 0:
                    y= y + r
            if numEro > 0:
                aveEro = float(y) / float(numEro)
            else:
                aveEro = None
            # Now calculate the avearage accretion
            y=0
            for r in rate:
                if (r) > 0:
                    y= y + r
            if numAcc > 0:
                aveAcc = float(y) / float(numAcc)
            else:
                aveAcc = None


            #this will be reset
            nstar = 1

            # calculate nstar, reduced n.  But not if dummy values of 0 were passed in
            if sum(uncy) > 0:
                nstar = calcNstar(uncy)
                #nstar = calcNstar(uncyNoGap)
                if nstar != 0:
                    uncyOfAve = abs(aveUncy/sqrt(nstar))
                else:
                    uncyOfAve = None
            else:
                nstar = None
                uncyOfAve = None
    else:
        # at least one of the lists is empty, set everything to null
        nstar = None
        aveRate = None
        maxRate = None
        maxRateId = None
        minRate = None
        minRateId = None
        percentEro = None
        percentAcc = None
        percentEroSig = None
        percentAccSig = None
        numTrans = None
        uncyOfAve = None
        aveUncy = None
        aveEro = None
        aveAcc = None
        numEro = None
        numAcc = None


    if rateName == 'LRR' or rateName == 'WLR' or rateName == 'EPR':
            summary_report['rates'][rateName][groupName]['ave'] = aveRate 
            summary_report['rates'][rateName][groupName]['max'] = maxRate 
            summary_report['rates'][rateName][groupName]['maxId'] = maxRateId
            summary_report['rates'][rateName][groupName]['min'] = minRate
            summary_report['rates'][rateName][groupName]['minId'] = minRateId
            summary_report['rates'][rateName][groupName]['percEro'] = percentEro
            summary_report['rates'][rateName][groupName]['percAcc'] = percentAcc
            summary_report['rates'][rateName][groupName]['percEroSig'] = percentEroSig
            summary_report['rates'][rateName][groupName]['percAccSig'] = percentAccSig
            summary_report['rates'][rateName][groupName]['numTrans'] =  numTrans
            summary_report['rates'][rateName][groupName]['uncyOfAve'] = uncyOfAve
            summary_report['rates'][rateName][groupName]['aveUncy'] = aveUncy
            summary_report['rates'][rateName][groupName]['aveEro'] = aveEro
            summary_report['rates'][rateName][groupName]['aveAcc'] = aveAcc
            summary_report['rates'][rateName][groupName]['numEro'] = numEro
            summary_report['rates'][rateName][groupName]['numAcc'] = numAcc
            summary_report['rates'][rateName][groupName]['Nstar'] = nstar
    elif rateName == 'SCE':
            summary_report['rates'][rateName][groupName]['numTrans'] =  numTrans
            summary_report['rates'][rateName][groupName]['aveDist'] = aveRate 
            summary_report['rates'][rateName][groupName]['maxDist'] = maxRate 
            summary_report['rates'][rateName][groupName]['maxIdDist'] = maxRateId
            summary_report['rates'][rateName][groupName]['minDist'] = minRate
            summary_report['rates'][rateName][groupName]['minIdDist'] = minRateId
    elif rateName == 'NSM':
            summary_report['rates'][rateName][groupName]['aveDist'] = aveRate 
            summary_report['rates'][rateName][groupName]['maxNSM'] = maxRate 
            summary_report['rates'][rateName][groupName]['maxIdNSM'] = maxRateId
            summary_report['rates'][rateName][groupName]['minNSM'] = minRate
            summary_report['rates'][rateName][groupName]['minIdNSM'] = minRateId
            summary_report['rates'][rateName][groupName]['percEroNSM'] = percentEro
            summary_report['rates'][rateName][groupName]['percAccNSM'] = percentAcc
            summary_report['rates'][rateName][groupName]['numTrans'] =  numTrans
            summary_report['rates'][rateName][groupName]['aveEroNSM'] = aveEro
            summary_report['rates'][rateName][groupName]['aveAccNSM'] = aveAcc
            summary_report['rates'][rateName][groupName]['numEroNSM'] = numEro
            summary_report['rates'][rateName][groupName]['numAccNSM'] = numAcc

    return(nstar)

def calcNstar(data):

    from xalglib import corrr1d
    from xalglib import create_real_vector
    from xalglib import pearsoncorr2

    mean = lambda nums: sum(nums, 0.0) / len(nums)

    # This code calculates the reduced n (nstar) that is necessary to calculate the uncertainty of a 
    # the regionaly averaged shoreline change rate. The idea is that in order to calculate the uncertainty
    # you need to know 'n' or the number of indepdent samples. This is tricky, neighboring transects
    # are clearly not independent, transects many km apart are independent, at what distance aprat are two
    # transects independt?  This method uses the uncertainty in the calculated rate. It uses a cross
    # correlation to determine the alongshore variablilty of uncertianty and from this a reduced n.
    # This method was developed by Jeff List.  He wrote Matlab codes to do the calculation.
    # His method is discussed in the USGS Open File Report # 2010-1118
    # Amy Farris (afarris@usgs.gov) converted his codes into IronPython in August of 2017.

    # This code takes as input the uncertainty of the rate for all the transects in a region: "data"
    # It returns the nstar.
    # Note that the region should be identified with care. It should be a more or less continuous section
    # of coast with goegraphic/geomorphic continuity.

    # Jeff wrote his own cross correlation, I follow his exmple here.
    # "r_corrcoef" is the cross correlation.  I do only positive lags b/c that is what Jeff used
    # I call the function 'pearsoncorr2' below, it calcualtes the correlation coefficient.
    npts = len(data)
    r_corrcoef = []
    lag_corrcoef = []
    neg_corrcoef = []

    for i in range(npts):
        lag_corrcoef.append( i -1)
        numPtsCorr = len(data[lag_corrcoef[i]+1:npts])
        corr_coef = pearsoncorr2(data[lag_corrcoef[i]+1:npts],data[1:npts-lag_corrcoef[i]],numPtsCorr-1)
        r_corrcoef.append(corr_coef)
        if corr_coef < 0:
            neg_corrcoef.append(lag_corrcoef[i])
    # end Jeff's cross correlation

    # Jeff used the first zero croossing of the cross correlation as his cutoff.  
    # The first element of "neg_corrcoef" is the first zero crossing.

    # now calculate nstar
    # this isn't coded very well, but I think it works
    try: 
        if neg_corrcoef[0]:
            n_prime = neg_corrcoef[0]
            rsum = 0
            # the following line is to more closely replicate the matlab results
            del r_corrcoef[0]
            for i in range(n_prime):
                rsum = rsum + (npts-i+1) * r_corrcoef[i]
            if rsum <= 0:
                # seriel correlation is too high or something else is wrong
                nstar = 1
            else:
                nstar = 1/((1/float(npts)) + ((2/float(npts*npts)) * rsum))
        else:
            # occasionally the cross correlation does not cross zero, this means that the spatial correlation
            # is very high, set nstar to 1 
            nstar = 1
    except:
        nstar = 1

    return(nstar)
