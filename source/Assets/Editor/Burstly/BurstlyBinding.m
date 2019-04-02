//
//  BurstlyBinding.m
//
//  Created by Josh Noble on 01/30/13.
//  Copyright 2013 Disney Mobile. All rights reserved.
//

#import "BurstlyManager.h"

// Converts C style string to NSString
#define GetStringParam( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]


void _setAppId( const char* appId )
{
    [[BurstlyManager sharedManager] setAppId:GetStringParam( appId )];
}

void _addInterstitialWithZoneName( const char* zoneName, const char* zoneId )
{
    [[BurstlyManager sharedManager] addInterstitialWithZoneName:GetStringParam( zoneName ) zoneId:GetStringParam( zoneId )];
}

void _showInterstitial( const char* zoneId )
{
    [[BurstlyManager sharedManager] showInterstitial:GetStringParam( zoneId )];
}

void _addBannerWithZoneName( const char* zoneName, const char* zoneId )
{
    [[BurstlyManager sharedManager] addBannerWithZoneName:GetStringParam( zoneName ) zoneId:GetStringParam( zoneId )];
}

void _showBannerAd( const char* zoneId, bool show )
{
    [[BurstlyManager sharedManager] showBannerAd:GetStringParam( zoneId ) show:show];
}
/*
void _decreaseRewardsBalance( int amount )
{
    [[BurstlyManager sharedManager] decreaseRewardsBalance:amount];
}
*/
