
#import "BurstlyBannerAdView.h"
#import "BurstlyInterstitial.h"
#import "Burstly.h"
#import "BurstlyAdUtils.h"


@interface BurstlyManager : NSObject <BurstlyBannerViewDelegate,BurstlyInterstitialDelegate>
{
    NSString*               mAppId;
    NSMutableDictionary*    mBanners;
    NSMutableDictionary*    mInterstitials;
}

+ (BurstlyManager*)sharedManager;

- (void)setAppId:(NSString*)appId;
- (void)addInterstitialWithZoneName:(NSString*)zoneName zoneId:(NSString*)zoneId;
- (void)showInterstitial:(NSString*)zoneName;
- (void)addBannerWithZoneName:(NSString*)zoneName zoneId:(NSString*)zoneId;
- (void)showBannerAd:(NSString*)zoneName show:(bool)show;
//- (void)decreaseRewardsBalance:(int)amount;

@end
