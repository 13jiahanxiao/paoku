//
//  TapjoyManagerBinding.m
//
//  Created by Josh Noble on 03/15/13.
//  Copyright 2013 Disney Mobile. All rights reserved.
//

#import "TapjoyManager.h"


// Converts C style string to NSString
#define GetStringParam( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]


void _initTapjoyWithDeviceId( const char* deviceId )
{
    [[TapjoyManager sharedManager] initWithDeviceId:GetStringParam( deviceId )];
}

void _showTapjoyOffers()
{
     [[TapjoyManager sharedManager] showOffers];
}

void _deductTapjoyCurrency( int amount )
{
    [[TapjoyManager sharedManager] deductTapjoyCurrency:amount];
}
