//
//  DisneySharingManagerBinding.m
//
//  Created by Josh Noble on 02/10/13.
//  Copyright 2013 Disney Mobile. All rights reserved.
//

#import <AdSupport/ASIdentifierManager.h>
#import "DisneySharingManager.h"

#define MakeStringCopy( _x_ ) ( _x_ != NULL && [_x_ isKindOfClass:[NSString class]] ) ? strdup( [_x_ UTF8String] ) : NULL

void _showActivityIndicator()
{
    [[DisneySharingManager sharedManager] showActivityIndicator];
}

void _hideActivityIndicator()
{
    [[DisneySharingManager sharedManager] hideActivityIndicator];
}

const char* _getLocale()
{
    NSString* localeStr = [[NSLocale currentLocale] objectForKey:NSLocaleIdentifier];
    return MakeStringCopy( localeStr );
}

const char* _getBundleVersion()
{
    NSString* versionStr = [[NSBundle mainBundle] objectForInfoDictionaryKey:(NSString *)kCFBundleVersionKey];
    return MakeStringCopy( versionStr );
}

const char* _getAdvertisingIdentifier()
{
    NSString* advertisingIdentifier = nil;

    if ( [[UIDevice currentDevice] respondsToSelector:@selector(identifierForVendor)] == YES )
    {
        // IDFV (vendor-specific identifier)
        advertisingIdentifier = [[[UIDevice currentDevice] identifierForVendor] UUIDString];
    }
    /*
    if ( NSClassFromString( @"ASIdentifierManager" ) )
    {
        // iOS 6+ IDFA (app-specific identifier)
        advertisingIdentifier = [[[ASIdentifierManager sharedManager] advertisingIdentifier] UUIDString];
    }
     */

    return MakeStringCopy( advertisingIdentifier );
}
