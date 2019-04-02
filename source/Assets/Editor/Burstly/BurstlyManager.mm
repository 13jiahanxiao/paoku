//
//  BurstlyManager.mm
//
//  Created by Josh Noble on 01/30/13.
//  Copyright 2013 Disney Mobile. All rights reserved.
//

#import "BurstlyManager.h"

// Converts C style string to NSString
#define GetStringParam( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]

void UnitySendMessage( const char* className, const char* methodName, const char* param );
void UnityPause( bool pause );
UIViewController *UnityGetGLViewController();

#define kServerUrl @"http://mercury.appads.com"


@implementation BurstlyManager

+ (BurstlyManager*)sharedManager
{
	static BurstlyManager* sharedSingleton;
	
	if ( !sharedSingleton )
    {
		sharedSingleton = [[BurstlyManager alloc] init];
	}
    
	return sharedSingleton;
}

- (id)init
{
    self = [super init];
    
    mBanners = [[NSMutableDictionary alloc] init];
    mInterstitials = [[NSMutableDictionary alloc] init];
    
    NSURL* serverUrl = [NSURL URLWithString:kServerUrl];
    [Burstly setServerUrl:serverUrl];
#if DEBUG
    [Burstly setLogLevel:AS_LOG_LEVEL_DEBUG];
#else
    [Burstly setLogLevel:AS_LOG_LEVEL_DEBUG];
     //[Burstly setLogLevel:AS_LOG_LEVEL_WARN];
#endif
    
    return self;
}

- (void)setAppId:(NSString*)appId
{
    if ( mAppId != nil )
    {
        [mAppId release];
        mAppId = nil;
    }
    mAppId = [[NSString alloc] initWithString:appId];
    
    //BurstlyCurrency* currencyManager = [BurstlyCurrency sharedCurrencyManager];
    //[currencyManager setPublisherId:mAppId];
    //currencyManager.delegate = self;
}

- (void)addInterstitialWithZoneName:(NSString*)zoneName zoneId:(NSString*)zoneId
{
    BurstlyInterstitial* interstitial = [mInterstitials objectForKey:zoneId];
    if ( interstitial == nil )
    {
        interstitial = [[[BurstlyInterstitial alloc] initAppId:mAppId zoneId:zoneId delegate:self] autorelease];
        [mInterstitials setObject:interstitial forKey:zoneName];
    }
    else
    {
        NSLog( @"[BurstlyManager] Error: This interstitial has already been added");
    }
}

- (void)showInterstitial:(NSString*)zoneName
{
    BurstlyInterstitial* interstitial = [mInterstitials objectForKey:zoneName];
    if ( interstitial != nil )
    {
        [interstitial showAd];
    }
    else
    {
        NSLog(@"[BurstlyManager] Error: No interstitial zone exists with that name");
    }
}

- (void)addBannerWithZoneName:(NSString*)zoneName zoneId:(NSString*)zoneId
{
    BurstlyBannerAdView* banner = [mBanners objectForKey:zoneId];
    if ( banner == nil )
    {
        CGSize screenSize = UnityGetGLViewController().view.frame.size;

        // Default to "iPhone 4" dimensions
        float bannerSize = 57; // iPhone app icon size
        CGPoint bannerPosition = CGPointMake( 236, 333 );
       
        if ( screenSize.width > 320 )
        {
            // Screen is wider than an iPhone 4, so we assume it's an iPad
            bannerSize = 72; // iPad app icon size
            bannerPosition = CGPointMake( 572, 734 );
        }
        else if ( screenSize.height > 480 )
        {
            // Screen is taller than an iPhone 4, so we assume it's an iPhone 5
            bannerSize = 57; // iPhone app icon size
            bannerPosition = CGPointMake( 246, 398 );
        }
 
        CGRect bannerFrame = CGRectMake( bannerPosition.x, bannerPosition.y, bannerSize, bannerSize );
        
        // Set root view controller for modal display of banner landing pages.
        // The anchor defines what edge of the banner will be used to anchor the banner within it's frame. kBurstlyAnchorBottom specifies that the bottom/middle of the banner will anchor to the bottom/middle of it's frame.
        // Set the delegate as the object that receive messages about the status of the banner.
        banner = [[[BurstlyBannerAdView alloc] initWithAppId:mAppId zoneId:zoneId frame:bannerFrame anchor:kBurstlyAnchorTop rootViewController:UnityGetGLViewController() delegate:self] autorelease];
        [banner cacheAd];
        
        // Optionally, add a background color to ensure you have correctly positioned your banner
        [banner setBackgroundColor:[UIColor clearColor]];
        banner.hidden = YES;
        
        [UnityGetGLViewController().view addSubview:banner];
        
        [mBanners setObject:banner forKey:zoneName];
    }
    else
    {
        NSLog( @"[BurstlyManager] Error: This banner has already been added" );
    }
}

- (void)showBannerAd:(NSString*)zoneName show:(bool)show
{
    NSLog( @"[BurstlyManager] showBannerAd %@", zoneName );
    
    BurstlyBannerAdView* banner = [mBanners objectForKey:zoneName];
    if ( banner != nil )
    {
        if ( show == true )
        {
            // Show
            [banner showAd];
            banner.hidden = NO;
        }
        else
        {
            // Hide (and get a new ad ready to show next time)
            banner.hidden = YES;
            [banner cacheAd];
        }
    }
    else
    {
        NSLog( @"[BurstlyManager] Error: No banner zone exists with that name");
    }
}
/*
- (void)decreaseRewardsBalance:(int)amount
{
    NSLog( @"[BurstlyManager] decreaseRewardsBalance by %d coins", amount );
    BurstlyCurrency* currencyManager = [BurstlyCurrency sharedCurrencyManager];
    [currencyManager decreaseBalance:amount forCurrency:@"coin"];
}
*/

- (void)dealloc
{
    if ( mAppId != nil )
    {
        [mAppId release];
        mAppId = nil;
    }
    
    if ( mBanners != nil )
    {
        [mBanners release];
        mBanners = nil;
    }
    
    if ( mInterstitials != nil )
    {
        [mInterstitials release];
        mInterstitials = nil;
    }
    
    [super dealloc];
}

- (NSString*)_getZoneNameForBannerAd:(BurstlyBannerAdView*)bannerAd
{
    NSString* zoneName = nil;
    
    for ( NSString* key in mBanners )
    {
        BurstlyBannerAdView* dictionaryBanner = [mBanners objectForKey:key];
        if ( dictionaryBanner == bannerAd )
        {
            zoneName = key;
            break;
        }
    }
    
    return zoneName;
}

// =================================================================================================
// Delegate methods
// =================================================================================================

- (UIViewController*) viewControllerForModalPresentation:(BurstlyInterstitial *)interstitial
{
    return UnityGetGLViewController();
}

- (void)burstlyInterstitial:(BurstlyInterstitial *)ad didHide:(NSString*)lastViewedNetwork
{
    NSLog(@"[BurstlyManager] burstlyInterstitial didHide");
}

- (void)burstlyInterstitial:(BurstlyInterstitial *)ad didShow:(NSString*)adNetwork
{
    NSLog(@"[BurstlyManager] burstlyInterstitial didShow");
}

- (void)burstlyInterstitial:(BurstlyInterstitial *)ad didCache:(NSString*)adNetwork
{
    NSLog(@"[BurstlyManager] burstlyInterstitial didCache");
}

- (void)burstlyInterstitial:(BurstlyInterstitial *)ad wasClicked:(NSString*)adNetwork
{
   NSLog(@"[BurstlyManager] burstlyInterstitial wasClicked");
}

- (void)burstlyInterstitial:(BurstlyInterstitial *)ad didFailWithError:(NSError*)error
{
    NSLog( @"[BurstlyManager] burstlyInterstitial didFailWithError: %@", error.localizedDescription );
    
    UnitySendMessage( "SharingManagerBinding", "BurstlyFullScreenInterstitialDismissed", "message" ); // Tell Unity to hide the busy indicator (if any)
}

- (void)burstlyInterstitial:(BurstlyInterstitial *)ad willTakeOverFullScreen:(NSString*)adNetwork
{
    UnityPause( true );
}

- (void)burstlyInterstitial:(BurstlyInterstitial *)ad willDismissFullScreen:(NSString*)adNetwork
{
    //[[BurstlyCurrency sharedCurrencyManager] checkForUpdate];
    
    UnityPause( false );
    
    UnitySendMessage( "SharingManagerBinding", "BurstlyFullScreenInterstitialDismissed", "message" ); // Tell Unity to hide the busy indicator (if any)
    
    // Load a new ad that we can display next time
    [ad cacheAd];
}

- (void)burstlyBannerAdView:(BurstlyBannerAdView *)view willTakeOverFullScreen:(NSString*)adNetwork
{
    UnityPause( true );
    
    // Banner will take over screen.
    // You should pause any app activity to improve performance.
    // Pause banner. It will no longer auto-refresh.
    [view setAdPaused:YES];
}

- (void)burstlyBannerAdView:(BurstlyBannerAdView *)view willDismissFullScreen:(NSString*)adNetwork
{
    UnityPause( false );
    
    // Banner will dismiss from screen take over.
    // You should resume any paused activity within your app.
    // Resume banner. It will begin auto-refreshing (if you have a default refresh interval defined via [banner setDefaultRefreshInterval:]).
    [view setAdPaused:NO];
}

- (void)burstlyBannerAdView:(BurstlyBannerAdView *)view didShow:(NSString*)adNetwork
{
    NSLog( @"[BurstlyManager] banner ad didShow" );
    
    NSString* zoneName = [self _getZoneNameForBannerAd:view];
    if ( zoneName != nil )
    {
        UnitySendMessage( "SharingManagerBinding", "BurstlyBannerAdShown", [zoneName UTF8String] );
    }
    else
    {
        NSLog( @"[BurstlyManager] ERROR: Couldn't find a zone name for the banner ad" );
    }
}

- (void)burstlyBannerAdView:(BurstlyBannerAdView *)view didHide:(NSString*)lastViewedNetwork
{
    NSLog( @"[BurstlyManager] banner ad didShow" );
}

- (void)burstlyBannerAdView:(BurstlyBannerAdView *)view didCache:(NSString*)lastViewedNetwork
{
    NSLog( @"[BurstlyManager] banner ad didCache" );
}

- (void)burstlyBannerAdView:(BurstlyBannerAdView *)view didFailWithError:(NSError*)error
{
     NSLog( @"[BurstlyManager] banner ad didFailWithError %@", error.localizedDescription );
}

@end

