//
//  AirshipManagerBinding.m
//
//  Created by Josh Noble on 05/17/13.
//  Copyright 2013 Disney Mobile. All rights reserved.
//

#import "AirshipManager.h"


// Converts C style string to NSString
#define GetStringParam( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]


const char* _getDestinationScreen()
{
    NSString* destinationScreen = [[AirshipManager sharedManager] getDestinationScreen];
    if ( destinationScreen != nil )
    {
        return strdup( [destinationScreen UTF8String] );
    }

    return nil;
}

/*
void _setDestinationScreen( const char* destinationScreen )
{
    [[AirshipManager sharedManager] setDestinationScreen:GetStringParam( destinationScreen )];
}
*/

const char* _getPayload()
{
    NSString* payload = [[AirshipManager sharedManager] getPayload];
    if ( payload != nil )
    {
        return strdup( [payload UTF8String] );
    }
    
    return nil;
}

void _setPayload( const char* payload )
{
    [[AirshipManager sharedManager] setPayload:GetStringParam( payload )];
}

void _showRemoteNotificationAlert( const char* title, const char* message, const char* viewButtonText, const char* cancelButtonText )
{
    [[AirshipManager sharedManager] showAlert:[title UTF8String] message:[message UTF8String] viewButtonText:[viewButtonText UTF8String] cancelButtonText:[cancelButtonText UTF8String]];
}
