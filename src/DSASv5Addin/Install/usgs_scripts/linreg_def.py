# this code calcualtes a linear regresion of shoreline change and a weighted linear regression
# afarris@usgs.gov 2017July26
# afarris@usgs.gov 2017Sep26 handles case when no bias data is passed correctly
# afarris@usgs.gov 2017Sep28 added exceptions
# afarris@usgs.gov 2017Sep29 fixed divisino by zero problem
# afarris@usgs.gov 2017Oct02 fixed very minor bug in one exception
# afarris@usgs.gov 2017Dec15 now if all shoreliens are HWL,and bias_flag =1 , then LRR = NB_LRR, WLR = NB_WLR
# afarris@usgs.gov 2018Feb12 now if all shoreliens are MHW, and bias = 0 and bias_flag =1 , then LRR = NB_LRR, WLR = NB_WLR
# afarris@usgs.gov 2018Feb13 the previous fix had forgotten WLR

def linreg(data):
    from math import sqrt
    from xalglib import invstudenttdistribution
    mean = lambda nums: sum(nums, 0.0) / len(nums)

    shoreNoBias = data.get('shoreNoBias')
    shoreBias = data.get('shoreBias')
    dates = data.get('dates')
    conf = data.get('CI')
    bias = data.get('bias')
    bias_flag = data.get('bias_flag')
    type = data.get('type')

    # this code is based on the m-file "regression_significance.m" written by
    # kweber.  It is based on equations in the book "Biostatistical Analysis"
    # by Jerold Zar.   

    # if no bias data is availae, it has been set to 0

    # do 'no bias' first
    X = dates
    Y = shoreNoBias
    n = len(X)
    df = n-2
    alpha =  1 - (float(conf)*.01);
    if df <= 0:
        #results wil be crazy, set output to NaN
        LRR = None
        LR2 = None
        LCI = None
        LSE = None
        if bias:
            rates = {'LRR': LRR, 'LR2': LR2, 'LSE': LSE, 'LCI': LCI,'NB_LRR': LRR, 'NB_LR2': LR2, 'NB_LCI': LCI, 'NB_LSE': LSE}
        else:
            rates = {'LRR': LRR, 'LR2': LR2, 'LSE': LSE, 'LCI': LCI}
    else:
         # there are enough shorelines to do the calculation
        Xbar = mean(X)
        Ybar = mean(Y)
        SS = 0
        SCP = 0
        totalSS = 0
        for x,y in zip(X,Y):
            SS = SS + (x - Xbar)*(x - Xbar)
            SCP = SCP + (x - Xbar)*(y - Ybar)
            totalSS = totalSS + (y - Ybar)*(y - Ybar)

        if SS == 0:
            #throw exception, data is odd and code will crash
            raise Exception('IPY: There is something wrong with the distances from the shorelines to the reference line ')
        if totalSS == 0:
            #throw exception, data is odd and code will crash
            raise Exception('IPY: There is something wrong with the dates  ')

        # calcualte the regression coeficient , which is also the slope of the regression   
        # Eqn 17.4
        b = SCP/SS
        LRR = b
        # calculate r2 , Eqn 17.12
        regressionSS = b * SCP
        # Eqn 17.13
        residualSS = totalSS - regressionSS
        # Eqn 17.16, r squared
        LR2 = regressionSS / totalSS
        # now calc residual mean square error
        rmse = residualSS/df
        # calc standard error of the regression
        SE = sqrt(rmse)
        # now calc confidence intervals
        # first calc standard error of the regression coefficient, Eqn 17.20
        SERC = sqrt(rmse/SS);        
        #test statistic  for the two tailed test
        t = invstudenttdistribution(df,1-alpha/2);
        LCI = t*SERC
        if bias_flag:
            # bias is generally applied, save these rates at NB_*
            rates = {'NB_LRR': LRR, 'NB_LR2': LR2, 'NB_LCI': LCI, 'NB_LSE': SE}
            if bias ==0:
                # bias was genearally applied, but no bias data for this transect
                # need to check...if all shorelines are all HWL or all MHW, then bias is not needed and 
                # rates can be used for both
                flag = 0
                for t in type:
                    if t != 'HWL':
                       flag = flag + 1
                if flag == 0 or flag == len(type):
                    # all shoreline are HWL or all are (MHW or WDL)
                    rates.update ( {'LRR': LRR, 'LR2': LR2, 'LCI': LCI, 'LSE': SE})

        else:
            # if no bias, save these rates with the standard name
            rates = {'LRR': LRR, 'LR2': LR2, 'LCI': LCI, 'LSE': SE}

    # now check if bias exists, if so, and if there are enogh surveys,
    # do calcs on bias shifted data
    if bias != 0 and df > 0:
        Y = shoreBias
        Xbar = mean(X)
        Ybar = mean(Y)
        SS = 0
        SCP = 0
        totalSS = 0
        for x,y in zip(X,Y):
            SS = SS + (x - Xbar)*(x - Xbar)
            SCP = SCP + (x - Xbar)*(y - Ybar)
            totalSS = totalSS + (y - Ybar)*(y - Ybar)

        if SS == 0:
            #throw exception, data is odd and code will crash
            raise Exception('IPY: There is something wrong with the distances from the shorelines to the reference line ')
        if totalSS == 0:
            #throw exception, data is odd and code will crash
            raise Exception('IPY: There is something wrong with the dates ' )
        # calcualte the regression coeficient , which is also the slope of the regression   
        # Eqn 17.4
        b = SCP/SS
        LRR = b
        # calculate r2 , Eqn 17.12
        regressionSS = b * SCP
        # Eqn 17.13
        residualSS = totalSS - regressionSS
        # Eqn 17.16, r squared
        LR2 = regressionSS / totalSS
        # now calc residual mean square error
        rmse = residualSS/df
        # calc standard error of the regression
        SE = sqrt(rmse)
        # now calc confidence intervals
        # first calc standard error of the regression coefficient, Eqn 17.20
        SERC = sqrt(rmse/SS);        
        LCI = t*SERC
        rates.update ( {'LRR': LRR, 'LR2': LR2, 'LCI': LCI, 'LSE': SE})


    return rates

def weightlinreg(data):

    from math import sqrt
    from xalglib import invstudenttdistribution
    mean = lambda nums: sum(nums, 0.0) / len(nums)

    # this code is based on the m-file "regression_significance.m" written by
    # kweber.  It is based on equations in the book "Biostatistical Analysis"
    # by Jerold Zar.   
    # The weighted stuff is from visual basic code written by Jess Zickichi
    # and Ayhan Ergul
    
    shoreNoBias = data.get('shoreNoBias')
    shoreBias = data.get('shoreBias')
    dates = data.get('dates')
    conf = data.get('CI')
    uncyNoBias = data.get('uncyNoBias')
    uncyWBias = data.get('uncyWBias')
    bias = data.get('bias')
    bias_flag = data.get('bias_flag')
    type = data.get('type')


        
    # do 'no bias' first
    # convert uncertainty to a weight "W" that will be mulitplied with each shoreline
    W = []

    for i in range(len(uncyNoBias)):
        if uncyNoBias[i] == 0:
            raise Exception('IPY: cannot do WLR if weight = 0 ' )
        W.append ( 1 / float( uncyNoBias[i] * uncyNoBias[i] ) )

    X = dates
    Y = shoreNoBias
    n = len(X)
    df = n-2
    alpha =  1 - (float(conf)*.01);

    if df <= 0:
        #results wil be crazy, set output to NaN
        WLR = None
        WR2 = None
        WCI = None
        SE = None
        if bias:
            rates = {'WLR': WLR, 'WR2': WR2, 'WCI': WCI, 'WSE': SE,'NB_WLR': WLR, 'NB_WR2': WR2, 'NB_WCI': WCI, 'NB_WSE': SE}
        else:
            rates = {'WLR': WLR, 'WR2': WR2, 'WCI': WCI, 'WSE': SE}
    else:
        # there are enough surveys to do the calculation
        Xbar = mean(X)
        Ybar = mean(Y)

        sumW = 0
        sumWX = 0
        sumWY = 0
        sumWXY = 0
        sumWYY = 0
        sumWXX = 0

        for x,y,w in zip(X,Y,W):
            # these are the Visual Basic eqns:
            sumW =  sumW + w;
            sumWY = sumWY + (w * y);
            sumWX = sumWX + (w * x);
            sumWXX = sumWXX + (w * x * x);
            sumWYY = sumWYY + (w * y * y);
            sumWXY = sumWXY + (w * x * y);
        
        if sumW == 0:
            #throw exception, data is odd and code will crash
            raise Exception('IPY: There is something wrong with the uncertainties, are they all zero? ')
        SS = sumWXX - (sumWX * sumWX/sumW);
        SCP = sumWXY -(sumWX * sumWY/sumW);
        totalSS = sumWYY - (sumWY * sumWY / sumW);

        if SS == 0:
            #throw exception, data is odd and code will crash
            raise Exception('IPY: There is something wrong with the uncertanties and/or distances  ')
        if totalSS == 0:
            #throw exception, data is odd and code will crash
            raise Exception('IPY: There is something wrong with the dates and/or uncertanties ')
        # calcualte the regression coeficient , which is also the slope of the regression   
        # Eqn 17.4
        b = SCP/SS
        WLR = b

        # calculate r2,  Eqn 17.12
        regressionSS = b * SCP
        # Eqn 17.13
        residualSS = totalSS - regressionSS
        # Eqn 17.16, r squared
        WR2 = regressionSS / totalSS

        # now calc residual mean square error
        rmse = residualSS/df
        # calc standard error of the regression
        SE = sqrt(rmse)

        # now calc confidence intervals
        # first calc standard error of the regression coefficient, Eqn 17.20
        SERC = sqrt(rmse/SS);        
        #test statistic  for the two tailed test
        t = invstudenttdistribution(df,1-alpha/2);
        WCI = t*SERC
        if bias_flag:
            # bias is generally applied save these data as NB_*
            rates = {'NB_WLR': WLR, 'NB_WR2': WR2, 'NB_WCI': WCI, 'NB_WSE': SE}
            if bias ==0:
                # bias was genearally applied, but no bias data for this transect
                # need to check...if all shorelines are HWL, then bias is not needed and 
                # rates can be used for both
                flag = 0
                for t in type:
                    if t != 'HWL':
                       # one of the shorelines usese a difference datum, can't use rates
                       flag = flag + 1
                if flag == 0 or flag == len(type):
                    # all shoreline are HWL or all are (MHW or WDL)
                    rates.update( {'WLR': WLR, 'WR2': WR2, 'WCI': WCI, 'WSE': SE})
        else:
            # no bias, save these data with the standard names
            rates = {'WLR': WLR, 'WR2': WR2, 'WCI': WCI, 'WSE': SE}


    # now check if bias exists, if so, and if thre are enogh surveys,
    # do calcs on bias shifted data
    if bias != 0 and df > 0:

        # convert uncertainty to a weight "W" that will be mulitplied with each shoreline
        W = []
        for i in range(len(uncyNoBias)):
            if uncyNoBias[i] == 0:
                raise Exception('IPY: cannot do WLR if weight = 0 ')
            W.append ( 1 / float( uncyNoBias[i] * uncyNoBias[i] ) )

        Y = shoreBias
        Xbar = mean(X)
        Ybar = mean(Y)

        sumW = 0
        sumWX = 0
        sumWY = 0
        sumWXY = 0
        sumWYY = 0
        sumWXX = 0

        for x,y,w in zip(X,Y,W):
            # these are the Visual Basic eqns:
            sumW =  sumW + w;
            sumWY = sumWY + (w * y);
            sumWX = sumWX + (w * x);
            sumWXX = sumWXX + (w * x * x);
            sumWYY = sumWYY + (w * y * y);
            sumWXY = sumWXY + (w * x * y);
        
        if sumW == 0:
            #throw exception, data is odd and code will crash
            raise Exception('IPY: There is something wrong with the uncertainties, are they all zero?  ')
        SS = sumWXX - (sumWX * sumWX/sumW);
        SCP = sumWXY -(sumWX * sumWY/sumW);
        totalSS = sumWYY - (sumWY * sumWY / sumW);

        if SS == 0:
            #throw exception, data is odd and code will crash
            raise Exception('IPY: There is something wrong with the distances and/or uncertainties ' )
        if totalSS == 0:
            #throw exception, data is odd and code will crash
            raise Exception('IPY: There is something wrong with the dates and/or uncertanties ')
        # calcualte the regression coeficient , which is also the slope of the regression   
        # Eqn 17.4
        b = SCP/SS
        WLR = b

        # calculate r2,  Eqn 17.12
        regressionSS = b * SCP
        # Eqn 17.13
        residualSS = totalSS - regressionSS
        # Eqn 17.16, r squared
        WR2 = regressionSS / totalSS

        # now calc residual mean square error
        rmse = residualSS/df
        # calc standard error of the regression
        SE = sqrt(rmse)

        # now calc confidence intervals
        # first calc standard error of the regression coefficient, Eqn 17.20
        SERC = sqrt(rmse/SS);        
        WCI = t*SERC

        rates.update ( {'WLR': WLR, 'WR2': WR2, 'WCI': WCI, 'WSE': SE})

    return rates
