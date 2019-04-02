//
//  AirshipManager.mm
//
//  Created by Josh Noble on 05/17/13.
//  Copyright 2013 Disney Mobile. All rights reserved.
//

#import "AirshipManager.h"


@implementation AirshipManager


+ (AirshipManager*)sharedManager
{
	static AirshipManager* sharedSingleton;
	
	if ( !sharedSingleton )
    {
		sharedSingleton = [[AirshipManager alloc] init];
	}
    
	return sharedSingleton;
}

- (NSString*)getDestinationScreen
{
    return mDestinationScreen;
}

- (void)setDestinationScreen:(NSString*)destinationScreen
{
    if ( destinationScreen != nil )
    {
        [self _releaseDestinationScreen];
        mDestinationScreen = [[NSString alloc] initWithString:destinationScreen];
    }
}

- (void)setPayload:(NSString*)payload
{
    if ( payload != nil )
    {
        [self _releasePayload];
        mPayload = [[NSString alloc] initWithString:payload];
    }
}

- (NSString*)getPayload
{
    return mPayload;
}

- (void)_releaseDestinationScreen
{
    if ( mDestinationScreen != nil )
    {
        [mDestinationScreen release];
        mDestinationScreen = nil;
    }
}

- (void)_releasePayload
{
    if ( mPayload != nil )
    {
        [mPayload release];
        mPayload = nil;
    }
}

- (void)showAlert:(NSString*)title message:(NSString*)message viewButtonText:(NSString*)viewButtonText cancelButtonText:(NSString*)cancelButtonText
{
    UIAlertView* alert = [[[UIAlertView alloc] initWithTitle:title message:message delegate:self cancelButtonTitle:cancelButtonText otherButtonTitles:viewButtonText,nil] autorelease];
    [alert show];
}

- (void)alertView:(UIAlertView*)alertView didDismissWithButtonIndex:(NSInteger)buttonIndex
{
    if ( buttonIndex == 1 )
    {
        // The user clicked the "View" button
        UnitySendMessage( "SharingManagerBinding", "ReceivedRemoteNotification", [mDestinationScreen UTF8String] );
    }
}

- (void)dealloc
{
    [self _releaseDestinationScreen];
    [self _releasePayload];
    
    [super dealloc];
}

@end
