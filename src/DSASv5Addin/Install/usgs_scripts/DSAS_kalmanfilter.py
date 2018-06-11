# -*- coding: utf-8 -*-
# KALMAN FILTER FUNCTION
# Python version 2.7

# afarris@usgs.gov 2017nov22 my first attempt at integrating this code into DSAS

"""
Created on Wed Aug 23 16:03:11 2017
@author: Brittany Combs, USGS; bcombs@usgs.gov
@author: Joseph Long, USGS; jwlong@usgs.gov

VARIABLES:
       A = state transition matrix 
       H = observation matrix
       t = observation time vector
       T = model time vector
       dt_model = model time step
       forecast_length = forecast time period
       qf = process noise
       t = observation time vector
       z = observations
       Q = estimate process error covariance
       P = covariance matrix
       R = measurement error covariance (assumed Gaussian)
       xo = initial conditions for the state vector

OUTPUTS:
       Xc = corrected/updated state vector
       Pc = corrected/updated covariance matrix 
       Xp = predicted state vector
       Pp = predicted covariance matrix 

Code adapted from Kalman Fiter MATLAB function; Author: Joe Long, USGS; jwlong@usgs.gov
"""

def multiply_mat(X, Y):
    return [[sum(X*Y for X,Y in zip(X_row,Y_col)) for Y_col in zip(*Y)] for X_row in X]
       
def multiply_colvec_mat(v, X):
    return [[sum(v*X for v,X in zip(v_row,X_col)) for X_col in X] for v_row in v]

def add_mats(X,Y): # ** TypeError: 'int' object is not subscriptable
    result = [[X[i][j] + Y[i][j]  for j in range(len(X[0]))] for i in range(len(X))]    
    return result
 
def sub_mats(X,Y): # ** TypeError: 'int' object is not subscriptable
    result = [[X[i][j] - Y[i][j]  for j in range(len(X[0]))] for i in range(len(X))]    
    return result

def matrixTranspose(anArray):
    transposed = [None]*len(anArray[0])
    for t in range(len(anArray)):
        transposed[t] = [None]*len(anArray)
        for tt in range(len(anArray[t])):
            transposed[t][tt] = anArray[tt][t]
    return transposed

###############################################################################

# Define inputs to the Kalman Filter that will be hard-coded in current release
dt_model = 1

# Define the forecast length (e.g., 10, 20 years)
forecast_length = 9

# Process noise estimate
# a low value here means we believe our model that the shoreline should
# follow a more or less linear trend line
qf = 0.1

##############################################################################

import datetime


def kalman(dt_model,forecast_length,qf,t,z,lse,uncy,lci90,xo):

    
    if not t or not z or not lse or not uncy or not lci90 or not xo:
       # throw exception, input data not passed in
       raise Exception('IPY: A least one of the following is empty: dates, shore, uncy, CI, group ')
    if len(t) != len(z) or len(t) != len(uncy) or len(t) != len(lci90) :
       # throw exception
       raise Exception('IPY: all these variables should be the same length: dates, shore, uncy. ' )
    if sum(t) <= 0 :
       # throw exception, 
       raise Exception('IPY: Something is wrong with the dates ')
    if len(t) < 4:
       # throw exception, 
       raise Exception('IPY: At least four surveys are needed to do shoreline prediction ')


    # Initialize time vector time step
    now = datetime.datetime.now()

    startT = t[:1][0]

    # hard-code a 10 year forecast or mak[Xce this an input variable
    T = list(range(startT,now.year+forecast_length))

    # Create model matrix
    A = []

    for i in range(2):

        A.append([0] * 2)

    A[0][0]=1

    A[0][1]=dt_model

    A[1][1]=1

    # Create process noise matrix
    Q= []

    for i in range(2):

        Q.append([0] * 2)


    Q[0][0]=round((qf**2)*(dt_model**3)/3,8)

    Q[0][1]=round((qf**2)*(dt_model**2)/2,8)

    Q[1][0]=round((qf**2)*(dt_model**2)/2,8)

    Q[1][1]=round((qf**2)*dt_model,8)

    # Single number or vector the same size as t
    P = []

    for i in range(2):

        P.append([0] * 2)

    P[0][0] = (lse[0])**2 + (uncy[0])**2 

    P[1][1] = (lci90[0])**2

    # Create observation equation
    H = [1,0]
    
    # Make the measurement error vector
    R = []
    for index in range(len(lse)):
        l = lse[index]*lse[index] + uncy[index]*uncy[index] 
        R.append(l)
        
    ###############################################################################
    # Define the length of simulation time (T) and number of states being forecasted
    # 2 states (sd=2):  shoreline position and shoreline change rate

    numpts = len(T)
    sd = len(A[:][:])

    # Initialize matricies for results
    # Predicted state (Xp)
    Xp = [[0] * sd for _ in range(numpts)] 

    # Corrected state (Xc)
    Xc = [[0] * sd for _ in range(numpts)]

    # Predicted covariance (Pp)
    Pp = [[ [0] * sd for _ in range(sd) ] for _ in range(numpts)]
    
    Pp[0][0][0] = P[0][0] ## Pp = sd*sd 
    Pp[0][1][0] = P[1][0]
    Pp[0][0][1] = P[0][1]
    Pp[0][1][1] = P[1][1]
        
    # Corrected covariance (Pc)
    Pc = [[ [0] * sd for _ in range(sd) ] for _ in range(numpts)]
    
    Pc[0][0][0] = P[0][0] ## Pc 
    Pc[0][1][0] = P[1][0]
    Pc[0][0][1] = P[0][1]
    Pc[0][1][1] = P[1][1]
    
    
    # KALMAN GAIN
    K = [ [0] * sd for _ in range(numpts) ]
    
    # Residual
    resid = [ 0 for _ in range(numpts)]
    
    
    # Insert user defined initial conditions
    Xp[0][0] = xo[0]
    Xp[0][1] = xo[1]
    
    Xc[0][0] = xo[0]
    Xc[0][1] = xo[1]

    # Begin iteration in time
    
    for j in range(1,numpts):
        
        # Compute predicted state
        #Xp = A*Xc
        
        try:
            Xp1 = multiply_colvec_mat([Xc[j-1][:]],A[:][:])
        
            Xp[j][0] = Xp1[0][0]
            Xp[j][1] = Xp1[0][1]
        except:
            raise Exception('IPY: THere was a problem with the matrix algebra  ' )

        try:
            # Compute A PRIORI error covariance 
            # Pp = A[j-1] * Pc[j-1] * A[j-1] + Q[j-1]
        
            Pp1 = multiply_mat(A[:][:],Pc[j-1][:][:]) 
        
            At = matrixTranspose(A[:][:])
        
            Pp2 = multiply_mat(Pp1,At)
        
            Pp3 = add_mats(Q[:][:],Pp2)
        except:
            raise Exception('IPY: there was a problem with the matrix algebra ' )

            
        # this is clunky but necessary as far as I can tell
        Pp[j][0][0] = Pp3[0][0] 
        Pp[j][1][0] = Pp3[1][0]
        Pp[j][0][1] = Pp3[0][1]
        Pp[j][1][1] = Pp3[1][1]

         
        # Compute Kalman gain if observations are available
        # K = Pp * H' * invert(H * P * H' + R)
                      

        if T[j] in t:

            i = t.index(T[j])
             
            K1 = [[Pp[j][0][0] * H[0] + Pp[j][1][0] * H[1]] , [Pp[j][0][1] * H[0] + Pp[j][1][1] * H[1]]]

            K2 = H[0] * K1[0][0] + H[1] * K1[1][0] + R[i]
            
            K3 = 1/K2
            
            K4 = [[K1[0][0] * K3],[K1[1][0] * K3]]

            K[j][0] = K4[0][0]
            K[j][1] = K4[1][0]

            # Compute the innovation/residual
            # resid[j] = obs - H * Xp)
            try:
                res1 = [H[0] * Xp[j][0] + H[1] * Xp[j][1]]                    
                resid[j] = z[i] - res1[0]
            except:
                raise Exception('IPY: there was a problem with the matrix algebra ' )

        
        ##  POSTERIORI (corrected) state
        #Xc = Xp + K*resid
        try:
            Xc[j][:] = [Xp[j][0] + K[j][0]*resid[j], Xp[j][1] + K[j][1]*resid[j]]
        except:
            raise Exception('IPY: there was a problem with the matrix algebra ' )
        
        # Update the error covariance
        ## Pc = (I - K*H)*Pp

              
        eyesd = [[1,0],[0,1]]
        
        try:
            Pc1 = [[K[j][0]*H[0],K[j][0]*H[1]],[K[j][1]*H[0],K[j][1]*H[1]]]

            Pc2 = sub_mats(eyesd,Pc1)

            Pc3 = multiply_mat(Pc2,Pp[j][:][:])
        except:
            raise Exception('IPY: there was a problem with the matrix algebra ' )
     
        Pc[j][0][0] = Pc3[0][0] 
        Pc[j][1][0] = Pc3[1][0]
        Pc[j][0][1] = Pc3[0][1]
        Pc[j][1][1] = Pc3[1][1]  


                
    return(Xc, Pc,T)
