from math import sqrt
from DSAS_kalmanfilter import multiply_mat
from DSAS_kalmanfilter import multiply_colvec_mat 
from DSAS_kalmanfilter import matrixTranspose
from DSAS_kalmanfilter import sub_mats
from DSAS_kalmanfilter import add_mats
from DSAS_kalmanfilter import kalman



# afarris@usgs.gov 2017nov22 my first attempt at integrating kalman forcasting into DSAS
# afarris@usgs.gov 2017nov28 minor changes
# afarris@usgs.gov 2017dec12 added another excetption, LSE and LCI cannot be 0 (but LRR can)
# afarris@usgs.gov 2017dec18 added another excetption, check if all shore distances are zero
# afarris@usgs.gov 2018jan10 added an excetption for 'z', b/c old way was not catching that z was empty 
# afarris@usgs.gov 2018jan10 added excetptions to catch DBNull 
# afarris@usgs.gov 2018jan24 relaxed excetption that compares re-calculated rate to old rate 
# afarris@usgs.gov 2018may21 time step is now 0.1 of a year 
# afarris@usgs.gov 2018jun01 made some changes suggested by code review 

# if running code on it's own to test:
# sample data (tr #966)
#params = {
#    "dates": [ 1905.4, 1918.5, 1934.4, 1957.9, 1981.5, 2001.5],
#    "shore":  [ 213.8,  157.4,  130.2,  126.2,  162.6,  58.3],
#    "uncy" : [ 10.8, 5.10, 5.100, 3.2300,  2.6300, 1.3000],
#    "LSE":  37.1 ,
#    "LCI":  0.9,
#    "LRR" : -1.1,
#    "forecast_length": [10,20]}

#transect NC 102
params = {
    "dates":  [1842, 1946, 1980, 1997],
    "shore": [-122, -357, -510, -552],
    "uncy": [10.8, 5.10, 5.100, 1.3000],
    "LRR": -2.77,
    "LCI": 0.8,
    "LSE": 34.1,
    "forecast_length": [20]}

params = {
"dates": [1859.9,   1924.5,   1937.6,   1956.8,   1970.6,   1973.8,   1977.6,   1999.8,   2006,   2016.3,   2017.3],
"shore": [120.3, 389.4,   101.1,   59.1,   39.7,  -42.8,   45.3,  -95.4,   56.1,   12.2,  -5.6],
"uncy":  [11.35,  11.35,  11.35,  11.35,   4.7,   6.19,   4.73,   5.43,   3.80,   0.5,   0.5],
"LRR": -1.53,
"LCI": 1.33,
"LSE": 107.83,
"forecast_length": [20]}

# test with bad data:
#params = {
#    "dates":  [1842, 1946, 1980,],
#    "shore": [-122, -357, -510, ],
#    "uncy": [10.8, 5.10, 5.100, ],
#    "LRR": -2.77,
#    "LCI": 0.8,
#    "LSE": 34.1,
#    "forecast_length": [20]}

def calc_forecast(params):

    # data will be passed in transect by transect
    # dates are the dates of each intersect
    # shore is bias_distance if available, if not, distance
    # uncy is bias_uncertainty if available, if not, uncertainty
    # all three are stored in 'intersects' attribute table
    dates = params.get("dates")
    test = sorted(dates)
    if not dates == test:
        # throw exception
        raise BaseException('IPY: main_forecasting.py:  dates need to be in chronological order')

    z = params.get("shore")
    uncy = params.get("uncy")
    # the rest are output from the linear regression code
    LCI = params.get("LCI")
    LRR = params.get("LRR")
    LSE = params.get("LSE")
    # user also picked forcast length
    prediction_term = params.get("forecast_length")


    # check the data that was passed in
    if not dates or not z or not LSE or not uncy or not LCI :
           # throw exception, input data not passed in
           raise Exception('IPY: A least one of the following is empty: dates, shore, uncy, LCI ')
    # the above does not work if the lists contain DBNull which are defined to be 'True'
    # I tried very hard to test for DBNull, but gave up.  The following works b/c you can't "sum" a 
    # list containing 'DBNull'.
    # This is less than ideal also b/c it will throw out data with only one DBNull, but there are limits
    # to what I can achieve.
    try:
        foo = sum(z)
        if sum(z) == 0 :
            # throw exception, 
           raise Exception('IPY: All shores are zero ')
    except:
        # z can't be summed if it contains DBNull
        raise Exception('IPY: Something is wrong with the shorelines ')
    try:
        sum(uncy)
    except:
        # z can't be summed if it contains DBNull
        raise Exception('IPY: Something is wrong with the uncertainty ')
    try:
        sum(dates)
    except:
        # z can't be summed if it contains DBNull
        raise Exception('IPY: Something is wrong with the dates ')
       
    if LSE == 0 or  LCI == 0 :
           # throw exception, 
           raise Exception('IPY: LSE and LCI  cannot be zero')
    if len(dates) != len(z) or len(dates) != len(uncy) :
           # throw exception
           raise Exception('IPY: all these variables should be the same length: dates, shore, uncy. ' )
    if sum(dates) <= 0 :
           # throw exception, 
           raise Exception('IPY: Something is wrong with the dates ')
    if len(dates) < 4:
           # throw exception, 
           raise Exception('IPY: At least four surveys are needed to do shoreline prediction ')
    if not prediction_term:
           # throw exception, 
           raise Exception('IPY: you need to pass in the predition term ')


    # Define inputs to the Kalman Filter that will be hard-coded in current release
    # now 0.1 of a year
    dt_model = 0.1
    # Process noise estimate
    # a low value here means we believe our model that the shoreline should
    # follow a more or less linear trend line
    qf = 0.1

    # get forcast length in right format to pass into code
    # I add the "1" below b/c otherwise code calculstes 19 years instead of 20
    # (or 9 instead of 10), I don't know why
    forecast_length = max(prediction_term) + 1

    # dates need to be integers, convert from float:
    t =[]
    for temp in dates:
        tempB = round(temp,1)
        t.append(tempB)

    # I need to convert the scalers LCI, LRR and LSE to vectors the same size as t, z and uncy.
    # I will call these new vectors lci, lrr and lse
    # I think my nameing convention is a bit backwards, captial letters are traditionally matricies, 
    # but this is just how the code evolved.
    lse = [LSE] * len(t)
    lrr = [LRR] * len(t)
    lci = [LCI] * len(t)

    # We need y-intercept, I'll calculate it
    mean = lambda nums: sum(nums, 0.0) / len(nums)
    Xbar = mean(t)
    Ybar = mean(z)
    SS = 0
    SCP = 0
    totalSS = 0
    for x,y in zip(t,z):
        SS = SS + (x - Xbar)*(x - Xbar)
        SCP = SCP + (x - Xbar)*(y - Ybar)
        totalSS = totalSS + (y - Ybar)*(y - Ybar)
    if SS == 0:
       #throw exception, data is odd and code will crash
       raise Exception('IPY: There is something wrong with the distances from the shorelines to the reference line ')
    # calcualte the regression coeficient , which is also the slope of the regression   
    b = SCP/SS
    # the following is stupid, LRR is a list (tho' a scaler) and I can do no math on it, so 
    # I have to trick python. If I understood python better I could have a better solution
    if abs(b - max([LRR])) > 4:
       #throw exception, rates should match
       raise Exception('IPY: There is something wrong, recalcuated rate is different  ')
    # my y-intercept
    a = Ybar - (b*Xbar)
    # joe defines 'y-intercept' as y value at the first date:
    yo = a + b * t[0]
    # Joe needs a variable called xo, it containts: [joe's y-intercept, LRR]
    xo = [yo, b]


    #--------------------------------------------------------
    # finally, run the code to forcast the position
    [Xc, Pc, T] = kalman(dt_model,forecast_length,qf,t,z,lse,uncy,lci,xo)

    # now get needed data out of matricies that were passed back
    n = len(T)

    # figure out what was calculated
    if  max(prediction_term) == 20:
        # both 20 and 10 year forecasts were done
        # get 20 year predictions out of returned lists
        date20 = T[n-1]
        temp = Xc[n-1]
        shore20 = temp[0]
        temp = Pc[n-1]
        tempB = temp[0]
        u = tempB[0]
        shore20p = shore20 + sqrt(u)
        shore20m = shore20 - sqrt(u)
        # get 10 year predictions out of returned lists
        # use '11' if time step is yearly, use '101' if time step is 0.1 year
        date10 = T[n-101]
        temp = Xc[n-101]
        shore10 = temp[0]
        temp = Pc[n-101]
        tempB = temp[0]
        u = tempB[0]
        shore10p = shore10 + sqrt(u)
        shore10m = shore10 - sqrt(u)
    else:
        # just get 10 year prediction out of returned lists 
        # 20 was not calculateed, so just get the last point
        date10 = T[n-1]
        temp = Xc[n-1]
        shore10 = temp[0]
        temp = Pc[n-1]
        tempB = temp[0]
        u = tempB[0]
        shore10p = shore10 + sqrt(u)
        shore10m = shore10 - sqrt(u)


    # shore10 is shoreline prediction in 10 years (distance to baseline origin)
    # shore20 is shoreline prediction in 20 years
    # date10 is date in 10 years (reduntant, probably, but just to be clear)
    # date20 is date in 20 years (reduntant, probably)
    # shore10p is shoreline predictin in 10 years, plus uncertainty
    # shore10m is shoreline predictin in 10 years, minus uncertainty

    # now create output variable 'forecast'
    # see what the user asked for
    if len(prediction_term) == 2:
        # user wants both 10 and 20 year
        forecast = [[10, date10, shore10m, shore10, shore10p], [20, date20, shore20m, shore20, shore20p]]
    elif len(prediction_term) == 1 and min(prediction_term) == 10:
        # user just wants 10 year
        forecast = [[10, date10, shore10m, shore10, shore10p]]
    elif len(prediction_term) == 1 and min(prediction_term) == 20:
        # user just wants 20 year
        forecast = [[20, date20, shore20m, shore20, shore20p]]
    else:
       #throw exception, 
       raise Exception('IPY: There is something wrong with prediction_term  ')

    return (forecast)

