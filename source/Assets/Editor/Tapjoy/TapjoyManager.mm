//
//  TapjoyManager.mm
//
//  Created by Josh Noble on 03/15/13.
//  Copyright 2013 Disney Mobile. All rights reserved.
//

#import "TapjoyManager.h"

#define kTapjoyAppID        @"fe380fc4-f58a-4a8b-9776-e1e851d02269"
#define kTapjoyAppSecretKey @"DyjjBqyoqWH4ZVDeKZkM"

UIViewController *UnityGetGLViewController();
void UnityPause( bool pause );


@implementation TapjoyManager


+ (TapjoyManager*)sharedManager
{
	static TapjoyManager* sharedSingleton;
	
	if ( !sharedSingleton )
    {
		sharedSingleton = [[TapjoyManager alloc] init];
	}
    
	return sharedSingleton;
}

- (void)initWithDeviceId:(NSString*)deviceId
{
    NSLog( @"[TapjoyManager] initWithDeviceId: %@", deviceId );
    
    [TapjoyConnect requestTapjoyConnect:kTapjoyAppID secretKey:kTapjoyAppSecretKey options:@{ TJC_OPTION_ENABLE_LOGGING : @(YES)}];
    
        [TapjoyConnect setUserID:deviceId];
    
    mNumOzCoinsToBeAwarded = 0;
    mNumTapjoyPointsToDeduct = 0;
    
    // Notifications for Tap Points related callbacks.
    [[NSNotificationCenter defaultCenter] addObserver:self
                                             selector:@selector(tapjoyOfferWallClosed:)
                                                 name:TJC_VIEW_CLOSED_NOTIFICATION
                                               object:nil];
	[[NSNotificationCenter defaultCenter] addObserver:self
											 selector:@selector(getUpdatedPoints:)
												 name:TJC_TAP_POINTS_RESPONSE_NOTIFICATION
											   object:nil];
	[[NSNotificationCenter defaultCenter] addObserver:self
											 selector:@selector(spendTapjoyPoints:)
												 name:TJC_SPEND_TAP_POINTS_RESPONSE_NOTIFICATION
											   object:nil];
	[[NSNotificationCenter defaultCenter] addObserver:self
											 selector:@selector(getUpdatedPoints:)
												 name:TJC_AWARD_TAP_POINTS_RESPONSE_NOTIFICATION
											   object:nil];
	[[NSNotificationCenter defaultCenter] addObserver:self
											 selector:@selector(getUpdatedPointsError:)
												 name:TJC_TAP_POINTS_RESPONSE_NOTIFICATION_ERROR
											   object:nil];
	[[NSNotificationCenter defaultCenter] addObserver:self
											 selector:@selector(spendTapPointsError:)
												 name:TJC_SPEND_TAP_POINTS_RESPONSE_NOTIFICATION_ERROR
											   object:nil];
	[[NSNotificationCenter defaultCenter] addObserver:self
											 selector:@selector(awardTapPointsError:)
												 name:TJC_AWARD_TAP_POINTS_RESPONSE_NOTIFICATION_ERROR
											   object:nil];
	//[TapjoyConnect getTapPoints];
    
    
    
    // Set the notification observer for earned-currency-notification. It's recommended that this be placed within the applicationDidBecomeActive method.
    // (jonoble: However, Tapjoy doesn't get initialized until AFTER applicationDidBecomeActive gets called, so I'm adding it here too)
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(showEarnedCurrencyAlert:) name:TJC_TAPPOINTS_EARNED_NOTIFICATION object:nil];
}

- (void)showOffers
{
    mOfferWallIsVisible = true;
    
    UnityPause( true );
    
    [TapjoyConnect showOffersWithViewController:UnityGetGLViewController()];
}

- (BOOL)getOfferWallIsVisible
{
    return mOfferWallIsVisible;
}

- (void)applicationDidBecomeActive
{
    NSLog( @"[TapjoyManager] applicationDidBecomeActive" );
    
    // Set the notification observer for earned-currency-notification. It's recommended that this be placed within the applicationDidBecomeActive method.
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(showEarnedCurrencyAlert:) name:TJC_TAPPOINTS_EARNED_NOTIFICATION object:nil];
}

- (void)applicationWillResignActive
{
     NSLog( @"[TapjoyManager] applicationWillResignActive" );
 
    // Remove this to prevent the possibility of multiple redundant notifications.
	[[NSNotificationCenter defaultCenter] removeObserver:self name:TJC_TAPPOINTS_EARNED_NOTIFICATION object:nil];
}

- (void)deductTapjoyCurrency:(int)amount
{
    mNumTapjoyPointsToDeduct = amount;
    
    [TapjoyConnect spendTapPoints:mNumTapjoyPointsToDeduct];
}

// --- Notification handlers ---------------------------------------------------

- (void)getUpdatedPoints:(NSNotification*)notifyObj
{
    NSNumber *tapPoints = notifyObj.object;
    if ( tapPoints != nil )
    {
        int currentBalance = [tapPoints intValue];
        NSLog( @"[Tapjoy Manager] getUpdatedPoints: %d", currentBalance );
        if ( currentBalance > 0 )
        {
            // Deduct the balance from the server, and THEN award the coins to the user
            [self deductTapjoyCurrency:currentBalance];
        }
    }
}

- (void)tapjoyOfferWallClosed:(NSNotification*)notifyObj
{
    NSLog( @"[TapjoyManager] Offer wall closed" );
    
    mOfferWallIsVisible = false;
    
    UnityPause( false );
    
    // Now that Unity is running again, we can convert the Tapjoy points to Oz coins
    NSString* currencyName = @"(currency name not used in iOS)";
    NSString* parametersStr = [NSString stringWithFormat:@"%@,%d", currencyName, mNumOzCoinsToBeAwarded];
    UnitySendMessage( "SharingManagerBinding", "ConvertTapjoyPointsToOzCoins", [parametersStr UTF8String] );
    
    mNumOzCoinsToBeAwarded = 0;
}

- (void)getUpdatedPointsError:(NSNotification*)notifyObj
{
	NSLog( @"[TapjoyManager] getUpdatedPointsError" );
}

- (void)spendTapPointsError:(NSNotification*)notifyObj
{
	NSLog( @"[TapjoyManager] spendTapPointsError" );
}

- (void)awardTapPointsError:(NSNotification*)notifyObj
{
	NSLog( @"[TapjoyManager] awardTapPointsError" );
}

- (void)showEarnedCurrencyAlert:(NSNotification*)notifyObj
{
    NSNumber* tapPointsEarned = notifyObj.object;
    int earnedNum = [tapPointsEarned intValue];
 
    NSLog( @"[TapjoyManager] showEarnedCurrencyAlert: %d", earnedNum );

    // Pops up a UIAlert notifying the user that they have successfully earned some currency.
    // This is the default alert, so you may place a custom alert here if you choose to do so.
    [TapjoyConnect showDefaultEarnedCurrencyAlert];
    
    // This is a good place to remove this notification since it is undesirable to have a pop-up alert more than once per app run.
    [[NSNotificationCenter defaultCenter] removeObserver:self name:TJC_TAPPOINTS_EARNED_NOTIFICATION object:nil];
}

- (void)spendTapjoyPoints:(NSNotification*)notifyObj
{
    NSLog( @"[TapjoyManager] spendTapjoyPoints" );
    
    // Now that the points have been successfully deducted from Tapjoy, we can add them to the user's Offer Wall coin count
    mNumOzCoinsToBeAwarded += mNumTapjoyPointsToDeduct;
}

@end
