# afarris@usgs.gov 2017Sep29 added exceptions


def moveShoreline(data):
   from math import atan
   from math import sin
   from math import cos

   XsAll = data.get('shore_x')
   YsAll = data.get('shore_y')
   Xo = data.get('origin_x')
   Yo = data.get('origin_y')
   d2rAll = data.get('d2r')
   bias = data.get('bias')
   type = data.get('type')

   XnAll = []
   YnAll = []
   d2rNewAll = []

   if not XsAll or not YsAll or not d2rAll:
       raise Exception('IPY: XsAll or YsAll or d2rAll is missing for transect: ' + str(transectId))
   if not Xo or not Yo :
       raise Exception('IPY: Xo or Yo is missing for transect: ' + str(transectId))
   if not bias or not type :
       raise Exception('IPY: bias or type is missing for transect: ' + str(transectId))

   # there may be several shorelines on this transect, look at one at a time,
   # but only shft HWL shorelines
   for ii in range(len(type)):   
       if type[ii] == "HWL":
           Xs = XsAll[ii]
           Ys = YsAll[ii]
           d2r = d2rAll[ii]
           
           # step 1: calculate theta, the angle of the transect
           if (Ys-Yo) == 0:
             theta = 1.5707
           else:
             theta = abs(atan(float(Xs - Xo)/float(Ys-Yo)))

           # step 2: get signs
           if (Xs-Xo) < 0:
             Xsign = -1
           else:
             Xsign = 1

           if (Ys-Yo) < 0:
             Ysign = -1
           else:
             Ysign = 1

           # step 3: calcualte how far to move shoreline
           deltaYn = (d2r + bias) * cos(theta)
           deltaXn = (d2r + bias) * sin(theta)

           # step 4: move the shoreline! New position is Xn,Yn
           if d2r < 0 and Xsign < 0 and Ysign > 0:
                # case 1
                Xn = Xo + deltaXn
                Yn = Yo - deltaYn
           elif d2r < 0 and Xsign > 0 and Ysign < 0:
                # case 1b
                Xn = Xo - deltaXn
                Yn = Yo + deltaYn
           elif d2r > 0 and Xsign > 0 and Ysign < 0:
                # case 2
                Xn = Xo + deltaXn
                Yn = Yo - deltaYn
           elif d2r > 0 and Xsign < 0 and Ysign > 0:
                # case 2b
                Xn = Xo - deltaXn
                Yn = Yo + deltaYn
           elif d2r < 0 and Xsign < 0 and Ysign < 0:
                # case 5
                Xn = Xo + deltaXn
                Yn = Yo + deltaYn
           elif d2r > 0 and Xsign > 0 and Ysign > 0:
                # case 6
                Xn = Xo + deltaXn
                Yn = Yo + deltaYn
           else:
                # default 
                Xn = Xo - deltaXn
                Yn = Yo - deltaYn

           # done with calculation for this shoreline, calc d2r
           d2rNew =  d2r + bias
       else:
           # shoreline is MHW, doesn't need to be shifted
           Xn = XsAll[ii]
           Yn = YsAll[ii]
           d2rNew = d2rAll[ii]

       # save data for this shoreline
       XnAll.append( Xn ) 
       YnAll.append( Yn ) 
       d2rNewAll.append( d2rNew ) 

   # done stepping through each shoreline
   # collect data together to pass out
   if not XnAll or not YnAll or not d2rNewAll:
       raise Exception('IPY: unable to shift shoreline for transect: ' + str(transectId))
   else:
       rates = {'bias_x': XnAll, 'bias_y': YnAll, 'bias': bias, 'bias_distance': d2rNewAll}
   
   return rates